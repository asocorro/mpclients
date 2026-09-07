using System;
using System.Web;
using System.Configuration;
using System.Web.Hosting;
using System.Diagnostics;

using NHibernate;
using NHibernate.Cfg;

namespace MPClients.DataAccess.NHibernate
{
    /// <summary> 
    ///        Provides access to a single NHibernate session on a per request basis. 
    /// </summary> 
    /// <remarks> 
    ///        NHibernate requires mapping files to be loaded. These can be stored as 
    ///        embedded resources in the assemblies alongside the classes that they are for or 
    ///        in separate XML files. 
    ///        <br /><br /> 
    ///        As the number of assemblies containing mapping resources may change and 
    ///        these have to be referenced by the NHibernate config before the SessionFactory 
    ///        is created, the configuration is stored in a separate config file to allow 
    ///        new references to be added. This would also enable the use of external mapping 
    ///        files if required. 
    ///        <br /><br /> 
    ///        The name of the NHibernate configuration file is set in the appSettings section 
    ///        with the name "nhibernate.config". If not specified the the default value of 
    ///        ~/nhibernate.config is used. 
    ///        <br /><br /> 
    ///        Note that the default config name is not used because the .xml extension is 
    ///        public and could be downloaded from a website whereas access to .config files is 
    ///        automatically restricted by ASP.NET. 
    /// </remarks> 

    public sealed class UnitOfWork
    {
        /// <summary> 
        /// Key used to identify NHibernate session in context items collection 
        /// </summary> 
        private const string sessionKey = "MPClients.DataAccess.NHibernate.Db";

        /// <summary> 
        /// NHibernate Configuration 
        /// </summary> 
        private static Configuration configuration;

        /// <summary> 
        /// NHibernate SessionFactory 
        /// </summary> 
        private static ISessionFactory sessionFactory;

        /// <summary> 
        /// NHibernate Session. This is only used for NUnit testing (ie. when HttpContext 
        /// is not available. 
        /// </summary> 
        private static ISession session;

        private static readonly object factoryLock = new object();
        private static bool initialized = false;

        /// <summary> 
        /// None public constructor. Prevent direct creation of this object.  
        /// </summary> 
        private UnitOfWork() { }

        /// <summary> 
        /// See beforefieldinit 
        /// </summary> 
        // Static constructor left empty to avoid HttpContext usage at type init.
        static UnitOfWork() { }

        /// <summary>
        /// Initialize NHibernate configuration and build the SessionFactory in a thread-safe way.
        /// Call this from Application_Start to pre-warm mappings and avoid concurrent initialization.
        /// </summary>
        public static void Initialize()
        {
            if (initialized) return;
            lock (factoryLock)
            {
                if (initialized) return;
                string nhConfig = ConfigurationManager.AppSettings["nhibernate.config"] ?? "~/nhibernate.config";
                string configFile = HostingEnvironment.MapPath(nhConfig) ?? nhConfig;
                Log("UnitOfWork.Initialize: nhibernate config file=" + configFile);

                configuration = new Configuration();
                configuration.Configure(configFile);

                // Try building the SessionFactory, with one retry if there is an XPathException (intermittent mapping parse race)
                int attempts = 0;
                while (true)
                {
                    attempts++;
                    try
                    {
                        Log("UnitOfWork.Initialize: Starting BuildSessionFactory attempt=" + attempts);
                        sessionFactory = configuration.BuildSessionFactory();
                        session = sessionFactory.OpenSession();
                        initialized = true;
                        Log("UnitOfWork.Initialize: BuildSessionFactory completed");
                        break;
                    }
                    catch (System.Xml.XPath.XPathException xpe)
                    {
                        Log("UnitOfWork.Initialize: XPathException on BuildSessionFactory: " + xpe.Message);
                        if (attempts >= 2)
                        {
                            Log("UnitOfWork.Initialize: Giving up after " + attempts + " attempts");
                            throw;
                        }
                        // brief backoff then retry
                        System.Threading.Thread.Sleep(200);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Log("UnitOfWork.Initialize: Exception building SessionFactory: " + ex);
                        throw;
                    }
                }
            }
        }

        private static readonly object logLock = new object();
        private static void Log(string message)
        {
            try
            {
                Trace.WriteLine(message);

                // If Trace listeners are not configured in host, fallback to writing a small log in App_Data
                if (Trace.Listeners.Count == 0)
                {
                    string basePath = HostingEnvironment.MapPath("~");
                    if (!string.IsNullOrEmpty(basePath))
                    {
                        string logDir = System.IO.Path.Combine(basePath, "App_Data", "Logs");
                        try
                        {
                            lock (logLock)
                            {
                                System.IO.Directory.CreateDirectory(logDir);
                                string path = System.IO.Path.Combine(logDir, "nhibernate_init.log");
                                string line = DateTime.UtcNow.ToString("o") + " " + message + Environment.NewLine;
                                System.IO.File.AppendAllText(path, line);
                            }
                        }
                        catch { /* swallow - best effort logging */ }
                    }
                }
            }
            catch { /* swallow logging errors */ }
        }

        /// <summary> 
        /// Creates a new NHibernate Session object if one does not already exist. 
        /// When running in the context of a web application the session object is 
        /// stored in HttpContext items and has 'per request' lifetime. For client apps 
        /// and testing with NUnit a normal singleton is used. 
        /// </summary> 
        /// <returns>NHibernate Session object.</returns> 
        [CLSCompliant(false)]
        public static ISession Session
        {
            get
            {
                if (!initialized) Initialize();

                ISession session;
                if (HttpContext.Current == null)
                {
                    session = UnitOfWork.session;
                }
                else
                {
                    if (HttpContext.Current.Items.Contains(sessionKey))
                    {
                        session = (ISession)HttpContext.Current.Items[sessionKey];
                    }
                    else
                    {
                        session = UnitOfWork.sessionFactory.OpenSession();
                        HttpContext.Current.Items[sessionKey] = session;
                    }
                }
                return session;
            }
        }

        /// <summary> 
        /// Closes any open NHibernate session if one has been used in this request.<br /> 
        /// This is called from the EndRequest event. 
        /// </summary> 
        [CLSCompliant(false)]
        public static void CloseSession()
        {
            if (HttpContext.Current == null)
            {
                UnitOfWork.session.Close();
            }
            else
            {
                if (HttpContext.Current.Items.Contains(sessionKey))
                {
                    ISession session = (ISession)HttpContext.Current.Items[sessionKey];
                    session.Close();
                    HttpContext.Current.Items.Remove(sessionKey);
                }
            }

        }

        /// <summary> 
        /// Returns the NHibernate Configuration object. 
        /// </summary> 
        /// <returns>NHibernate Configuration object.</returns> 
        [CLSCompliant(false)]
        public static Configuration Configuration
        {
            get { return UnitOfWork.configuration; }
        }

        /// <summary> 
        /// Returns the NHibernate SessionFactory object. 
        /// </summary> 
        /// <returns>NHibernate SessionFactory object.</returns> 
        [CLSCompliant(false)]
        public static ISessionFactory SessionFactory
        {
            get { return UnitOfWork.sessionFactory; }
        }

        /// <summary> 
        /// Loads the specified object. 
        /// </summary> 
        /// <param name="type">Type.</param> 
        /// <param name="id">Id.</param> 
        public static void Load(System.Type type, object id)
        {
            UnitOfWork.Session.Load(type, id);
        }

        /// <summary> 
        /// Gets the specified object. 
        /// </summary> 
        /// <param name="type">Type.</param> 
        /// <param name="id">Id.</param> 
        public static object Get(System.Type type, object id)
        {
            return UnitOfWork.Session.Get(type, id);
        }

        /// <summary> 
        /// Save object item using NHibernate. 
        /// </summary> 
        /// <param name="item">Object to save</param> 
        public static void Save(object item)
        {
            ITransaction transaction = UnitOfWork.Session.BeginTransaction();

            try
            {
                UnitOfWork.Session.Save(item);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary> 
        /// Save object item using NHibernate. 
        /// </summary> 
        /// <param name="item">Object to delete</param> 
        public static void Delete(object item)
        {
            ITransaction transaction = UnitOfWork.Session.BeginTransaction();

            try
            {
                UnitOfWork.Session.Delete(item);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public static ISession GetIsolatedSession()
        {
            string configFile = HttpContext.Current.Request.MapPath(ConfigurationSettings.AppSettings["nhibernate.config"]);
            ISessionFactory  factory = new Configuration().Configure(configFile).BuildSessionFactory();
            return factory.OpenSession();
        }

    }
}
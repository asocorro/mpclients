using System;
using System.Web.UI;
using MPClients.DataAccess.NHibernate;

namespace MPClients.Web
{
    public partial class DebugInit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Diagnostic logging: write request context to App_Data/Logs/debuginit.log to help determine why requests may be redirected
            try
            {
                string basePath = Server.MapPath("~");
                string logDir = System.IO.Path.Combine(basePath ?? "", "App_Data", "Logs");
                System.IO.Directory.CreateDirectory(logDir);
                string logPath = System.IO.Path.Combine(logDir, "debuginit.log");
                string log = DateTime.UtcNow.ToString("o") + " | RequestIsLocal=" + Request.IsLocal
                    + " | UserHostAddress=" + Request.UserHostAddress
                    + " | Remote_Addr=" + Request.ServerVariables["REMOTE_ADDR"]
                    + " | Local_Addr=" + Request.ServerVariables["LOCAL_ADDR"]
                    + " | Url=" + Request.Url
                    + " | RawUrl=" + Request.RawUrl
                    + " | Host=" + Request.Url.Host
                    + Environment.NewLine;
                System.IO.File.AppendAllText(logPath, log);
            }
            catch { /* best-effort logging, swallow exceptions */ }

            // Protect this endpoint: allow only local requests or a valid init token from config
            string initToken = GetInitTokenFromConfig();
            string provided = Request.QueryString["token"];

            bool allowed = Request.IsLocal || (!string.IsNullOrEmpty(initToken) && initToken == provided);

            if (!allowed)
            {
                Response.StatusCode = 403;
                Response.Write("Forbidden");
                return;
            }

            try
            {
                UnitOfWork.Initialize();
                Response.Write("OK");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                    Response.Write("ERROR: " + ex.ToString());
                    // also append exception to debug log for easier capture
                    try
                    {
                        string basePath = Server.MapPath("~");
                        string logDir = System.IO.Path.Combine(basePath ?? "", "App_Data", "Logs");
                        System.IO.Directory.CreateDirectory(logDir);
                        string logPath = System.IO.Path.Combine(logDir, "debuginit.log");
                        string log = DateTime.UtcNow.ToString("o") + " | EXCEPTION: " + ex + Environment.NewLine;
                        System.IO.File.AppendAllText(logPath, log);
                    }
                    catch { }
            }
        }

        private string GetInitTokenFromConfig()
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings["InitToken"];
            }
            catch
            {
                try
                {
                    // fallback to environment variable
                    return System.Environment.GetEnvironmentVariable("InitToken");
                }
                catch { return null; }
            }
        }
    }
}

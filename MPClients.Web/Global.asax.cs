using System;
using System.Web;
using System.IO;
using System.Threading;
using System.Reflection;
using MPClients.DataAccess.NHibernate;

namespace MPClients
{
    public class Global : HttpApplication
    {
        [ThreadStatic]
        private static bool _firstChanceLoggingInProgress;
        protected void Application_Start(object sender, EventArgs e)
        {
            // Pre-warm NHibernate mappings and build SessionFactory once to avoid concurrent build issues
            try
            {
                UnitOfWork.Initialize();
                // Register a safer FirstChanceException logger to capture thrown exceptions for diagnosis.
                // Use a reentrancy guard and minimal operations to avoid recursion or StackOverflow.
                AppDomain.CurrentDomain.FirstChanceException += (s, ev) =>
                {
                    // prevent re-entrancy which can lead to StackOverflow
                    if (_firstChanceLoggingInProgress) return;
                    _firstChanceLoggingInProgress = true;
                    try
                    {
                        var ex = ev.Exception;
                        string basePath = AppDomain.CurrentDomain.BaseDirectory;
                        string logDir = Path.Combine(basePath, "App_Data", "Logs");
                        try { Directory.CreateDirectory(logDir); } catch { }
                        try
                        {
                            string path = Path.Combine(logDir, "first_chance_exceptions.log");
                            // Log full exception.ToString() to capture inner exceptions and loader details
                            string requestInfo = "";
                            try
                            {
                                var ctx = HttpContext.Current;
                                if (ctx != null && ctx.Request != null)
                                {
                                    requestInfo = " | URL=" + ctx.Request.RawUrl;
                                }

        private void DiagnosticLogDuplicateTypeOwners(string logDir)
        {
            try
            {
                string path = Path.Combine(logDir, "duplicate_type_owners.log");
                var map = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>(StringComparer.OrdinalIgnoreCase);
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        var types = asm.GetTypes();
                        foreach (var t in types)
                        {
                            string key = t.FullName ?? "<null>";
                            if (!map.TryGetValue(key, out var list)) { list = new System.Collections.Generic.List<string>(); map[key] = list; }
                            if (!list.Contains(asm.FullName)) list.Add(asm.FullName);
                        }
                    }
                    catch { }
                }
                foreach (var kv in map)
                {
                    if (kv.Value.Count > 1)
                    {
                        try { File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | TYPE=" + kv.Key + " | ASMS=" + string.Join(";", kv.Value) + System.Environment.NewLine); } catch { }
                    }
                }
            }
            catch { }
        }
                            }
                            catch { }

                            string msg = DateTime.UtcNow.ToString("o") + " | THREAD=" + Thread.CurrentThread.ManagedThreadId
                                + requestInfo
                                + " | EX=" + (ex != null ? ex.ToString() : "<null>")
                                + Environment.NewLine;
                            File.AppendAllText(path, msg);
                            // If we observed an AmbiguousMatchException, dump a summary of types that appear in multiple loaded assemblies.
                            try
                            {
                                Exception probeEx = ex;
                                while (probeEx != null)
                                {
                                    if (probeEx.GetType().Name == "AmbiguousMatchException")
                                    {
                                        try { DiagnosticLogDuplicateTypeOwners(logDir); } catch { }
                                        break;
                                    }
                                    probeEx = probeEx.InnerException;
                                }
                            }
                            catch { }


                            // If this is a ReflectionTypeLoadException, log each LoaderException explicitly
                            var rtlex = ex as ReflectionTypeLoadException;
                            if (rtlex != null && rtlex.LoaderExceptions != null)
                            {
                                foreach (var le in rtlex.LoaderExceptions)
                                {
                                    try
                                    {
                                        File.AppendAllText(path, "LOADER: " + (le != null ? le.ToString() : "<null>") + Environment.NewLine);
                                    }
                                    catch { }
                                }
                            }
                        }
                        catch { /* swallow to avoid cascading failures */ }
                    }
                    finally
                    {
                        _firstChanceLoggingInProgress = false;
                    }
                };
                // Probe assemblies in the bin folder and loaded assemblies for ReflectionTypeLoadException
                try
                {
                    DiagnosticProbeAssemblies();
                }
                catch { }
            }
            catch (Exception ex)
            {
                // Let startup fail loudly in staging; in production ensure monitoring/alerts are in place
                System.Diagnostics.Trace.WriteLine("Global.Application_Start: UnitOfWork.Initialize failed: " + ex);
                throw;
            }
        }

        private void DiagnosticProbeAssemblies()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string binPath = Path.Combine(basePath, "bin");
                string logDir = Path.Combine(basePath, "App_Data", "Logs");
                Directory.CreateDirectory(logDir);
                string path = Path.Combine(logDir, "loader_exceptions.log");

                // Check already loaded assemblies
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        // Force type load check
                        var types = asm.GetTypes();
                    }
                    catch (ReflectionTypeLoadException rtlex)
                    {
                        File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | LoadedAssembly=" + (asm.FullName ?? "<null>") + "\n");
                        foreach (var le in rtlex.LoaderExceptions)
                        {
                            File.AppendAllText(path, "LoaderException: " + (le?.Message ?? "<null>") + "\n" + (le?.StackTrace ?? "") + "\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | AssemblyGetTypesFailed=" + (asm.FullName ?? "<null>") + " | " + ex.Message + "\n");
                    }
                }

                // Also attempt to load assemblies from bin (discover problems with assemblies not yet loaded)
                if (Directory.Exists(binPath))
                {
                    foreach (var file in Directory.GetFiles(binPath, "*.dll"))
                    {
                        try
                        {
                            // Load into reflection-only context to avoid executing code
                            Assembly.ReflectionOnlyLoadFrom(file);
                        }
                        catch (FileLoadException flex)
                        {
                            File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | ReflectionOnlyLoadFromFailed=" + file + " | " + flex.Message + "\n");
                        }
                        catch (BadImageFormatException bife)
                        {
                            File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | BadImageFormat=" + file + " | " + bife.Message + "\n");
                        }
                        catch (Exception ex)
                        {
                            File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | LoadFromFailed=" + file + " | " + ex.Message + "\n");
                        }
                    }
                }
            }
            catch { /* best effort */ }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception ex = Server.GetLastError();
                if (ex != null)
                {
                    string basePath = Server.MapPath("~") ?? AppDomain.CurrentDomain.BaseDirectory;
                    string logDir = System.IO.Path.Combine(basePath, "App_Data", "Logs");
                    System.IO.Directory.CreateDirectory(logDir);
                    string path = System.IO.Path.Combine(logDir, "unhandled_exceptions.log");
                    string text = DateTime.UtcNow.ToString("o") + " | " + ex.ToString() + System.Environment.NewLine;
                    System.IO.File.AppendAllText(path, text);
                }
            }
            catch { /* best-effort logging */ }
        }
    }
}

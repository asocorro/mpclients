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

                // Register FirstChance exception logger
                AppDomain.CurrentDomain.FirstChanceException += FirstChanceHandler;

                // Probe assemblies and log diagnostics
                try { DiagnosticProbeAssemblies(); } catch { }
                try { DiagnosticLogOverloadedMethods(); } catch { }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Global.Application_Start: UnitOfWork.Initialize failed: " + ex);
                throw;
            }
        }

        private void FirstChanceHandler(object s, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs ev)
        {
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
                    string requestInfo = "";
                    try
                    {
                        var ctx = HttpContext.Current;
                        if (ctx != null && ctx.Request != null) requestInfo = " | URL=" + ctx.Request.RawUrl;
                    }
                    catch { }

                    string msg = DateTime.UtcNow.ToString("o") + " | THREAD=" + Thread.CurrentThread.ManagedThreadId + requestInfo + " | EX=" + (ex != null ? ex.ToString() : "<null>") + System.Environment.NewLine;
                    File.AppendAllText(path, msg);

                    // Append full stack traces and inner exception chain for better diagnostics
                    try
                    {
                        try { File.AppendAllText(path, "STACK: " + (ex?.StackTrace ?? "<null>") + System.Environment.NewLine); } catch { }
                        try { File.AppendAllText(path, "ENVSTACK: " + Environment.StackTrace + System.Environment.NewLine); } catch { }
                        try
                        {
                            Exception ie = ex?.InnerException;
                            while (ie != null)
                            {
                                try { File.AppendAllText(path, "INNER: " + ie.GetType().FullName + " - " + (ie.Message ?? "<null>") + System.Environment.NewLine + (ie.StackTrace ?? "") + System.Environment.NewLine); } catch { }
                                ie = ie.InnerException;
                            }
                        }
                        catch { }
                    }
                    catch { }

                    // If this looks like the ReturnUrl/login enum failure, collect request/session/items for deeper context
                    try
                    {
                        bool isEnumArgNull = ex is ArgumentNullException || (ex != null && ex.ToString().IndexOf("Enum", StringComparison.OrdinalIgnoreCase) >= 0);
                        var ctx = HttpContext.Current;
                        bool urlHasReturn = false;
                        try { if (ctx != null && ctx.Request != null && ctx.Request.RawUrl != null && ctx.Request.RawUrl.IndexOf("ReturnUrl", StringComparison.OrdinalIgnoreCase) >= 0) urlHasReturn = true; } catch { }

                        if (isEnumArgNull || urlHasReturn)
                        {
                            try
                            {
                                string diagPath = Path.Combine(logDir, "login_failure_diagnostics.log");
                                var sb2 = new System.Text.StringBuilder();
                                sb2.AppendLine("---- LOGIN FAILURE DIAGNOSTICS " + DateTime.UtcNow.ToString("o") + " ----");
                                try { sb2.AppendLine("EXTYPE=" + (ex != null ? ex.GetType().FullName : "<null>")); } catch { }
                                try { sb2.AppendLine("EX=" + (ex != null ? ex.ToString() : "<null>")); } catch { }
                                try { sb2.AppendLine("RawUrl=" + (ctx?.Request?.RawUrl ?? "<null>")); } catch { }
                                try { sb2.AppendLine("QueryString:"); if (ctx != null) { foreach (string k in ctx.Request.QueryString) sb2.AppendLine("  " + k + "=" + ctx.Request.QueryString[k]); } } catch { }
                                try { sb2.AppendLine("Form:"); if (ctx != null) { foreach (string k in ctx.Request.Form) sb2.AppendLine("  " + k + "=" + ctx.Request.Form[k]); } } catch { }
                                try { sb2.AppendLine("Headers:"); if (ctx != null) { foreach (string k in ctx.Request.Headers) sb2.AppendLine("  " + k + "=" + ctx.Request.Headers[k]); } } catch { }
                                try { sb2.AppendLine("Cookies:"); if (ctx != null) { foreach (string k in ctx.Request.Cookies) sb2.AppendLine("  " + k + "=" + ctx.Request.Cookies[k]?.Value); } } catch { }
                                try { sb2.AppendLine("--- HttpContext.Items ---"); if (ctx != null) { foreach (object k in ctx.Items.Keys) { try { sb2.AppendLine("  " + k + "=" + (ctx.Items[k] ?? "<null>")); } catch { sb2.AppendLine("  " + k + "=<unreadable>"); } } } } catch { }
                                try { sb2.AppendLine("--- Session keys ---"); if (ctx != null && ctx.Session != null) { foreach (string k in ctx.Session.Keys) { try { sb2.AppendLine("  " + k + "=" + (ctx.Session[k] ?? "<null>")); } catch { sb2.AppendLine("  " + k + "=<unreadable>"); } } } } catch { }
                                try { sb2.AppendLine("STACK: " + (ex?.StackTrace ?? "<null>")); } catch { }
                                try { sb2.AppendLine("ENVSTACK: " + Environment.StackTrace); } catch { }
                                try { File.AppendAllText(diagPath, sb2.ToString() + System.Environment.NewLine); } catch { }
                            }
                            catch { }
                        }
                    }
                    catch { }

                    // If we see an ArgumentNullException related to enum parsing, capture request details for diagnostics
                    try
                    {
                        if (ex is ArgumentNullException || (ex != null && ex.ToString().IndexOf("Enum", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            try
                            {
                                var ctx = HttpContext.Current;
                                if (ctx != null && ctx.Request != null)
                                {
                                    var sb = new System.Text.StringBuilder();
                                    sb.AppendLine("--- Request parameters ---");
                                    try
                                    {
                                        foreach (string k in ctx.Request.QueryString)
                                        {
                                            sb.AppendLine("QS: " + k + "=" + ctx.Request.QueryString[k]);
                                        }
                                    }
                                    catch { }
                                    try
                                    {
                                        foreach (string k in ctx.Request.Form)
                                        {
                                            sb.AppendLine("FORM: " + k + "=" + ctx.Request.Form[k]);
                                        }
                                    }
                                    catch { }
                                    try { File.AppendAllText(path, sb.ToString()); } catch { }
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }

                    // If AmbiguousMatch observed, log duplicate type owners
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

                    var rtlex = ex as ReflectionTypeLoadException;
                    if (rtlex != null && rtlex.LoaderExceptions != null)
                    {
                        foreach (var le in rtlex.LoaderExceptions)
                        {
                            try { File.AppendAllText(path, "LOADER: " + (le != null ? le.ToString() : "<null>") + System.Environment.NewLine); } catch { }
                        }
                    }
                }
                catch { }
            }
            finally
            {
                _firstChanceLoggingInProgress = false;
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

                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try { var types = asm.GetTypes(); }
                    catch (ReflectionTypeLoadException rtlex)
                    {
                        File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | LoadedAssembly=" + (asm.FullName ?? "<null>") + "\n");
                        foreach (var le in rtlex.LoaderExceptions) { File.AppendAllText(path, "LoaderException: " + (le?.Message ?? "<null>") + "\n" + (le?.StackTrace ?? "") + "\n"); }
                    }
                    catch (Exception ex) { File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | AssemblyGetTypesFailed=" + (asm.FullName ?? "<null>") + " | " + ex.Message + "\n"); }
                }

                if (Directory.Exists(binPath))
                {
                    foreach (var file in Directory.GetFiles(binPath, "*.dll"))
                    {
                        try { Assembly.ReflectionOnlyLoadFrom(file); }
                        catch (FileLoadException flex) { File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | ReflectionOnlyLoadFromFailed=" + file + " | " + flex.Message + "\n"); }
                        catch (BadImageFormatException bife) { File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | BadImageFormat=" + file + " | " + bife.Message + "\n"); }
                        catch (Exception ex) { File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | LoadFromFailed=" + file + " | " + ex.Message + "\n"); }
                    }
                }
            }
            catch { }
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
                foreach (var kv in map) { if (kv.Value.Count > 1) { try { File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | TYPE=" + kv.Key + " | ASMS=" + string.Join(";", kv.Value) + System.Environment.NewLine); } catch { } } }
            }
            catch { }
        }
        private void DiagnosticLogOverloadedMethods()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string logDir = Path.Combine(basePath, "App_Data", "Logs");
                Directory.CreateDirectory(logDir);
                string path = Path.Combine(logDir, "overloaded_methods.log");
                var typesToCheck = new[] { typeof(MPClients.DataAccess.Domain.Client), typeof(MPClients.DataAccess.Domain.MembershipUsers) };
                foreach (var t in typesToCheck)
                {
                    try
                    {
                        var methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                        var groups = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();
                        foreach (var m in methods) { string name = m.Name; if (!groups.TryGetValue(name, out var list)) { list = new System.Collections.Generic.List<string>(); groups[name] = list; } list.Add(m.ToString()); }
                        foreach (var kv in groups) { if (kv.Value.Count > 1) { try { File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | TYPE=" + t.FullName + " | METHOD=" + kv.Key + " | COUNT=" + kv.Value.Count + " | SIGS=" + string.Join("; ", kv.Value) + System.Environment.NewLine); } catch { } } }
                    }
                    catch (Exception ex)
                    {
                        try { File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " | ERROR inspecting " + t.FullName + " | " + ex + System.Environment.NewLine); } catch { }
                    }
                }
            }
            catch { }
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
            catch { }
        }
    }
}

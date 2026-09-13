using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace MPClients.Web
{
    public partial class DiagAssemblies : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // protect page: allow only local or token
            try
            {
                string token = System.Configuration.ConfigurationManager.AppSettings["InitToken"];
                string provided = Request.QueryString["token"];
                if (!(Request.IsLocal || (!string.IsNullOrEmpty(token) && token == provided)))
                {
                    Response.StatusCode = 403;
                    Response.Write("Forbidden");
                    return;
                }
            }
            catch { }

            var sb = new StringBuilder();
            var asm = AppDomain.CurrentDomain.GetAssemblies().OrderBy(a => a.FullName);
            foreach (var a in asm)
            {
                try
                {
                    sb.AppendLine(a.FullName);
                    sb.AppendLine("  Location: " + (a.IsDynamic ? "<dynamic>" : a.Location));
                    Type[] types = null;
                    try { types = a.GetTypes(); } catch (Exception ex) { sb.AppendLine("  (failed to enumerate types: " + ex.Message + ")"); }
                    if (types != null)
                    {
                        var matches = types.Where(t => t.FullName != null && t.FullName.StartsWith("MPClients", StringComparison.OrdinalIgnoreCase)).Select(t => t.FullName).ToArray();
                        if (matches.Length > 0)
                        {
                            sb.AppendLine("  Types:");
                            foreach (var t in matches) sb.AppendLine("    " + t);
                        }
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine("ERROR enumerating assembly: " + ex.Message);
                }
                sb.AppendLine();
            }

            Response.ContentType = "text/html";
            Response.Write("<pre>" + HttpUtility.HtmlEncode(sb.ToString()) + "</pre>");
        }
    }
}

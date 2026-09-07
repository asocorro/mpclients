using System;
using System.Web.UI;
using MPClients.DataAccess.NHibernate;

namespace MPClients.Web
{
    public partial class DebugInit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Protect this endpoint: allow only local requests or a valid init token from config
            string initToken = System.Configuration.ConfigurationManager.AppSettings["InitToken"];
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
            }
        }
    }
}

using System;
using System.Web;
using MPClients.DataAccess.NHibernate;

namespace MPClients
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Pre-warm NHibernate mappings and build SessionFactory once to avoid concurrent build issues
            try
            {
                UnitOfWork.Initialize();
            }
            catch (Exception ex)
            {
                // Let startup fail loudly in staging; in production ensure monitoring/alerts are in place
                System.Diagnostics.Trace.WriteLine("Global.Application_Start: UnitOfWork.Initialize failed: " + ex);
                throw;
            }
        }
    }
}

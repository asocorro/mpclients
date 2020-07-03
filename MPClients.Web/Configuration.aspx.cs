using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MPClients.PageControllers;
using NHibernate;
using MPClients.DataAccess.NHibernate;

namespace MPClients.Web
{
    public partial class Configuration : BasePage
    {
        private const string  ConfigId = "FF6C1844-8872-438C-9336-9B16957402B7";

        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                DoLoad(ConfigId);
            }
        }
     

        protected override void OnInit(EventArgs e)
        {
            uxSave.Click += new EventHandler(uxSave_Click);
            base.OnInit(e);
        }

        void uxSave_Click(object sender, EventArgs e)
        {
            Guid id = new Guid(ConfigId);

            ISession session = UnitOfWork.Session;
            ITransaction transaction = session.BeginTransaction();
            MPClients.DataAccess.Domain.Configuration config = session.Get<MPClients.DataAccess.Domain.Configuration>(id);

            config.MaxOrderProductsForPickup = int.Parse(uxMaxOrderProductsForPickup.Text);
            config.RestrictOrderProductsForPickup = int.Parse(chkRestrictOrderProductsForPickup.Text);

            session.Save(config);
            transaction.Commit();
        }

        public void DoLoad(string id)
        {
            uxID.Value = id;

            MPClients.DataAccess.Domain.Configuration config
                = UnitOfWork.Session.Get<MPClients.DataAccess.Domain.Configuration>(new Guid(id));

            if (config == null)
            {
                base.MasterPage.DisplayMessage("Error Loading the News");
            }
            else
            {
                chkRestrictOrderProductsForPickup.Checked = Convert.ToBoolean(config.RestrictOrderProductsForPickup);

                uxMaxOrderProductsForPickup.Text = config.MaxOrderProductsForPickup.ToString();
                uxMaxOrderProductsForPickup.Enabled = chkRestrictOrderProductsForPickup.Checked;
            }
        }
    }
}
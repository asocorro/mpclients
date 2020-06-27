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
using MPClients.DataAccess.Domain;

namespace MPClients.Web
{
    public partial class OrderConfirmation : BasePage
    {
        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                if (!MetaSapiens.PageMethods.PageMethodsEngine.InvokeMethod(this, true))
                    throw new Exception("Page method not found!");

            }
        }

        protected override void OnInit(EventArgs e)
        {
            //uxViewAll.Click += new EventHandler(uxViewAll_Click);
            base.OnInit(e);
        }

        [MetaSapiens.PageMethods.PageMethod(true)]
        public void DoLoad(string id)
        {
            ISession session = UnitOfWork.GetIsolatedSession();
            Orders orders;
            orders = session.Get<Orders>(new Guid(id));
            uxOrderId.Value = id;
            if (orders != null)
            {
                uxConfirmationNumber.Text = orders.OrderNo; // orders.ConfirmationNumber.ToUpper();
            }
            else
            {
                base.MasterPage.DisplayMessage("There was an error loading your order.  Please contact the system administrator.");
            }
        }
    }
}

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
using System.Collections.Generic;
using NHibernate.Expression;

namespace MPClients.Web
{
    public partial class OrdersResume : BasePage
    {
        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                BindDataGrid();
            }
        }


        protected override void OnInit(EventArgs e)
        {
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            base.OnInit(e);
        }

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            try
            {
                ICriterion expression = Expression.Eq("Status", 2);

                IList<MPClients.DataAccess.Domain.VWOrders> orders;
                using (ISession isolatedSession = UnitOfWork.GetIsolatedSession())
                {
                    ICriteria criteria = isolatedSession.
                        CreateCriteria(typeof(MPClients.DataAccess.Domain.VWOrders)).Add(expression);
                    criteria.AddOrder(new Order("OrderDate", false));
                    orders = criteria.List<MPClients.DataAccess.Domain.VWOrders>();
                }
                uxGrid.DataSource = orders;
                uxGrid.DataBind();
            }
            catch
            {

            }
        }


    }
}

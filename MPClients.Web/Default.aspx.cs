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
using NHibernate;
using NHibernate.Expression;
using MPClients.DataAccess.NHibernate;
using MPClients.PageControllers;

namespace MPClients
{
    public partial class Default : BasePage
    {
        protected override void PageLoad()
        {
            BindDataGrid();
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
            ISession session = UnitOfWork.GetIsolatedSession();
            //ICriterion expression = Expression.Ge("FromDate", new DateTime(2000, 1, 1));
            ICriterion expression = Expression.And(Expression.Le("FromDate", DateTime.Now), Expression.Ge("ToDate", DateTime.Now));

            ICriteria criteria = UnitOfWork.GetIsolatedSession().
                    CreateCriteria(typeof(MPClients.DataAccess.Domain.News)).Add(expression);
            criteria.AddOrder(new Order("FromDate", false));
            uxGrid.DataSource = criteria.List<MPClients.DataAccess.Domain.News>();
            uxGrid.DataBind();
        }
    }
}

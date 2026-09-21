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
using MPClients.DataAccess.NHibernate;
using NHibernate;
using NHibernate.Expression;

namespace MPClients.Web
{
    public partial class NewsSearch : BasePage
    {

        protected override void PageLoad()
        {
            BindDataGrid();
        }

        protected override void OnInit(EventArgs e)
        {
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            uxGrid.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(uxGrid_ItemCommand);
            uxAddNews.Click += new EventHandler(uxAddNews_Click);
            base.OnInit(e);
        }

        void uxGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string id = string.Empty; // e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString();

            if (e.CommandName == "Remove")
            {
                id = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString();
                ISession session = UnitOfWork.Session;
                ITransaction transaction = session.BeginTransaction();
                MPClients.DataAccess.Domain.News entity =
                    UnitOfWork.Session.Load<MPClients.DataAccess.Domain.News>(new Guid(id));
                session.Delete(entity);
                transaction.Commit();
                uxGrid.Rebind();
                uxGrid.StatusBarSettings.ReadyText = "News Removed.";
            }
            else if (e.CommandName == "Edit")
            {
                id = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString();
                Response.Redirect(MPClients.PageMethods.NewsEdit.DoLoad(id));
            }
        }

        void uxAddNews_Click(object sender, EventArgs e)
        {
            Response.Redirect(MPClients.PageMethods.NewsEdit.DoCreate());
        }

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            using (ISession session = UnitOfWork.GetIsolatedSession())
            {
                ICriterion expression = Expression.Ge("FromDate", new DateTime(2000, 1, 1));

                ICriteria criteria = session.
                        CreateCriteria(typeof(MPClients.DataAccess.Domain.News)).Add(expression);
                criteria.AddOrder(new Order("FromDate", false));
                uxGrid.DataSource = criteria.List<MPClients.DataAccess.Domain.News>();
                uxGrid.DataBind();
            }
        }
    }
}

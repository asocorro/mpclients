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
using NHibernate.Expression;
using NHibernate;
using MPClients.DataAccess.NHibernate;
using System.Collections.Generic;
using MPClients.Common;

namespace MPClients.Web
{
    public partial class OrderSearch : BasePage
    {

        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                uxTDSpecialOrdes.Visible = false;// System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            uxSearch.Click += new EventHandler(uxSearch_Click);
            uxViewSpecialOrders.Click += new EventHandler(uxViewSpecialOrders_Click);
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            base.OnInit(e);
        }

        void uxViewSpecialOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrdersResume.aspx");
        }

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindDataGrid();
        }

        void uxSearch_Click(object sender, EventArgs e)
        {
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            try
            {
                DateTime fromDate = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00,00, 00);
                DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                switch (uxDates.SelectedValue)
                {
                    case "0":
                        //fromDate = toDate;
                        break;
                    case "1":
                        fromDate = fromDate.AddDays(-1);
                        toDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 23, 59, 59);
                        break;
                    case "2":
                        fromDate = fromDate.AddDays(-15);
                        break;
                    case "3":
                        fromDate = fromDate.AddDays(-30);
                        break;
                    case "4":
                        fromDate = new DateTime(2008, 1, 1);
                        //toDate = fromDate.AddDays(-30);
                        break;
                }


                ICriterion expression = Expression.Between("OrderDate", fromDate, toDate );
                if (!System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE))
                {
                    System.Web.Security.MembershipUser MembershipUser =
                        System.Web.Security.Membership.GetUser();

                    expression = Expression.And(
                             expression,
                             Expression.Eq("UserId", new Guid(MembershipUser.ProviderUserKey.ToString()))
                             );
                }
                //else
                //{
                //expression = Expression.And(
                //         expression,
                //         Expression.Eq("Status", 1)
                //         );
                //}
                expression = Expression.And(
                         expression,
                         Expression.Eq("Status", 1)
                         );
                ICriteria criteria = UnitOfWork.GetIsolatedSession().
                    CreateCriteria(typeof(MPClients.DataAccess.Domain.Orders)).Add(expression);
                criteria.AddOrder(new Order("OrderDate", false));
                IList<MPClients.DataAccess.Domain.Orders> orders = criteria.List<MPClients.DataAccess.Domain.Orders>();
                uxGrid.DataSource = orders;
                uxGrid.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
    }
}

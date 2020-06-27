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
using MPClients.DataAccess.Domain;
using Telerik.Web.UI;
using MPClients.DataAccess.NHibernate;
using NHibernate.Expression;
using System.Collections.Generic;

namespace MPClients.Web
{
    public partial class PrintOrder : MPClients.PageControllers.BasePage
    {

        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                uxID.Value = Request.QueryString["id"];
                LoadData();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            uxGrid.ItemDataBound += new GridItemEventHandler(uxGrid_ItemDataBound);
            base.OnInit(e);
        }

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }


        private void LoadData()
        {
            ISession session = UnitOfWork.GetIsolatedSession();
            Orders orders;
            orders = session.Get<Orders>(new Guid(uxID.Value ));

            MembershipUser User; 
            MPClients.DataAccess.Domain.Client userClient;

            User = Membership.GetUser(new Guid(orders.UserId.ToString()));

            if (User != null)
            {
                if (!string.IsNullOrEmpty(orders.OnBehalfOf))
                {
                    userClient = session.Get<MPClients.DataAccess.Domain.Client>(orders.OnBehalfOf);
                }
                else
                {
                    MPClients.DataAccess.Domain.MembershipUsers userProfile = session.Get<MPClients.DataAccess.Domain.MembershipUsers>(new Guid(User.ProviderUserKey.ToString()));
                    userClient = session.Get<MPClients.DataAccess.Domain.Client>(userProfile.ClientID);
                }

                uxRequestedBy.Text = "Requester User Name: " + User.UserName;
                uxCompany.Text = userClient != null ? userClient.Fullname : "";
                uxContactPerson.Text = orders.ContactPerson;
                uiPurchaseOrder.Text = orders.PONumber;
                uiOrderDate.Text = orders.OrderDate.ToString();
                if (orders.Delivery.HasValue)
                {
                    uiPickupMethod.Text = orders.Delivery.Value ? "Delivery" : "Pickup at warehouse";
                }
            }
        }
        private void BindGrid()
        {
            ISession session = UnitOfWork.GetIsolatedSession();
            ICriterion expression = Expression.Eq("OrderID", new Guid(uxID.Value));
            ICriteria criteria =
                session.CreateCriteria(typeof(MPClients.DataAccess.Domain.OrderDetail)).Add(expression);
            criteria.AddOrder(new Order("ProductID", true));
            IList<MPClients.DataAccess.Domain.OrderDetail> orderDetails
                = criteria.List<MPClients.DataAccess.Domain.OrderDetail>();

            uxGrid.DataSource = orderDetails;
        }

        double sum = 0;
        void uxGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                Label lbl = dataItem["Template1"].FindControl("uxPrice") as Label;
                sum += double.Parse(lbl.Text);

                lbl.Text = Convert.ToDecimal(lbl.Text).ToString("C");
            }
            else if (e.Item is GridFooterItem)
            {
                GridFooterItem footer = (GridFooterItem)e.Item;
                (footer["Template1"].FindControl("uxTotalPrice") as Label).Text = sum.ToString("C");
            }
        }

    }
}

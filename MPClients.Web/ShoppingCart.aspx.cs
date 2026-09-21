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
using System.Collections.Generic;
using NHibernate.Expression;
using Telerik.Web;
using Telerik.Web.UI;
using System.Data.SqlClient;

namespace MPClients
{
    public partial class ShoppingCart : BasePage
    {

        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                uxResetShopping.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to empty your shopping cart?')== false) return false;");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            uxGrid.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(uxGrid_ItemCommand);
            uxGrid.ItemDataBound += new GridItemEventHandler(uxGrid_ItemDataBound);
            uxCreateOrder.Click += new EventHandler(uxCreateOrder_Click);
            uxAddProducts.Click += new EventHandler(uxAddProducts_Click);
            uxResetShopping.Click += new EventHandler(uxResetShopping_Click);
            uxUpdateShopping.Click += new EventHandler(uxUpdateShopping_Click);
            base.OnInit(e);
        }

        void uxUpdateShopping_Click(object sender, EventArgs e)
        {
            using (ISession session = UnitOfWork.GetIsolatedSession())
            {
            MPClients.DataAccess.Domain.Orders order =
                session.Get<MPClients.DataAccess.Domain.Orders>(ShoppingCartId);
            ITransaction tran = session.BeginTransaction();

            ICriterion expression = Expression.Eq("OrderID", ShoppingCartId);
            // Fetch only the fields we need to avoid NHibernate creating proxies for OrderDetail
            ISession _session = session;
            IQuery q = _session.CreateQuery("select od.ID, od.ProductID, od.Quantity, od.NetPrice from OrderDetail od where od.OrderID = :orderId");
            q.SetParameter("orderId", ShoppingCartId);
            IList<object[]> rows = q.List<object[]>();
            // Materialize lightweight DTO list to update quantities
            var orders = new List<MPClients.DataAccess.Domain.OrderDetail>();
            foreach (var r in rows)
            {
                var od = new MPClients.DataAccess.Domain.OrderDetail((Guid)r[0]);
                od.ProductID = (string)r[1];
                od.Quantity = Convert.ToInt32(r[2]);
                od.NetPrice = r[3] == null ? (decimal?)null : Convert.ToDecimal(r[3]);
                orders.Add(od);
            }

            foreach (object item in this.uxGrid.Items)
            {
                if (item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)item;
                    int value = int.Parse((dataItem["QuantityDesired"].FindControl("QuantityDesiredTextBox") as RadNumericTextBox).Text);
                    string productID = dataItem["ProductID"].Text;
                    foreach (MPClients.DataAccess.Domain.OrderDetail dataOrder in orders)
                    {
                        if (dataOrder.ProductID == productID)
                        {
                            dataOrder.Quantity = value;
                            session.Update(dataOrder);
                            //dataOrder.ExtendedPrice = value * dataOrder.NetPrice;
                        }
                    }

                }
            }

            tran.Commit();
            }
            BindGrid(true);
        }

        void uxResetShopping_Click(object sender, EventArgs e)
        {
 
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("DELETE from OrderDetail where OrderID = '" + ShoppingCartId + "'; DELETE from Orders where OrderId = '" + ShoppingCartId + "';", conn);
            command.ExecuteNonQuery();
            conn.Close();
            MembershipUser currentUser = Membership.GetUser();
            MPClients.PageControllers.BasePage.CleanCurrentShopping(currentUser.ProviderUserKey.ToString());
            BindGrid(true);
        }

        void uxAddProducts_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductsSearch.aspx");
        }

        void uxCreateOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect(MPClients.PageMethods.OrderEdit.DoLoad(ShoppingCartId.ToString(), true));
        }

        double sum = 0;
        void uxGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                Label lbl = dataItem["Template1"].FindControl("uxPrice") as Label ;
                Label lblIsFriend = dataItem["Template1"].FindControl("uxIsFriend") as Label;
                RadNumericTextBox ctlQuantityDesiredTextBox = dataItem["Template1"].FindControl("QuantityDesiredTextBox") as RadNumericTextBox;

                sum += double.Parse(lbl.Text);
                lbl.Text = Convert.ToDecimal(lbl.Text).ToString("c");
                bool isFriend = lblIsFriend.Text == "True";

                if (isFriend)
                {
                    ctlQuantityDesiredTextBox.Enabled = false;
                    dataItem["RemoveFromCart"].Enabled = false;
                }
            }
            else if (e.Item is GridFooterItem)
            {
                GridFooterItem footer = (GridFooterItem)e.Item;
                (footer["Template1"].FindControl("uxTotalPrice") as Label).Text = sum.ToString("C");
            }
        }

        void uxGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "RemoveFromCart")
            {
                string id = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString();
                ISession session = UnitOfWork.Session;
                ITransaction transaction = session.BeginTransaction();
                // Use Load to get a reference for deletion without requiring an eager existence check
                MPClients.DataAccess.Domain.OrderDetail entity =
                    UnitOfWork.Session.Load<MPClients.DataAccess.Domain.OrderDetail>(new Guid(id));
                session.Delete(entity);
                transaction.Commit();
                uxGrid.Rebind();
                uxGrid.StatusBarSettings.ReadyText = "Product Removed."; 
            }
        }

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid(false);
        }

        private void BindGrid(bool rebind)
        {
            try
            {
                ICriterion expression = Expression.Eq("OrderID", ShoppingCartId);
                // Avoid creating NHibernate proxies for OrderDetail by selecting required fields
                IList<object[]> rows;
                using (ISession _session = UnitOfWork.GetIsolatedSession())
                {
                    IQuery q = _session.CreateQuery("select od.ID, od.ProductID, od.Quantity, od.NetPrice, od.ProductName from OrderDetail od where od.OrderID = :orderId order by od.ProductName, od.ProductID");
                    q.SetParameter("orderId", ShoppingCartId);
                    rows = q.List<object[]>();
                }
                var userShoppingCart = new List<MPClients.DataAccess.Domain.OrderDetail>();
                foreach (var r in rows)
                {
                    var od = new MPClients.DataAccess.Domain.OrderDetail((Guid)r[0]);
                    od.ProductID = (string)r[1];
                    od.Quantity = Convert.ToInt32(r[2]);
                    od.NetPrice = r[3] == null ? (decimal?)null : Convert.ToDecimal(r[3]);
                    od.ProductName = (string)r[4];
                    userShoppingCart.Add(od);
                }
                if (userShoppingCart.Count == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Your shopping cart is empty.";
                    uxCreateOrder.Enabled = false;
                }
                uxGrid.DataSource = userShoppingCart;
                if (rebind)
                {
                    uxGrid.DataBind();
                }


            }
            catch
            {

            }
        }
    }
}

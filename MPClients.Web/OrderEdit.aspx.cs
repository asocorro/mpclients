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
using MPClients.DataAccess.Domain;
using NHibernate;
using NHibernate.Expression;
using System.Collections.Generic;
using MPClients.DataAccess.NHibernate;
using Telerik.Web.UI;
using MPClients.Common;
using System.Text;

namespace MPClients.Web
{
    public partial class OrderEdit : BasePage
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
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            uxGrid.ItemDataBound += new GridItemEventHandler(uxGrid_ItemDataBound);
            uxSendOrder.Click += new EventHandler(uxSendOrder_Click);
            uxSendLink.Click += new EventHandler(uxSendOrder_Click);
            base.OnInit(e);
        }



        void uxSendOrder_Click(object sender, EventArgs e)
        {
            if (!uxPickupWhse.Checked && !uxPickupDelivery.Checked)
            {
                base.MasterPage.DisplayMessage("Please enter the pickup method.");
            }
            else if (uxContactPerson.Text.Contains(","))
                base.MasterPage.DisplayMessage("Please remove commas from the 'Contact Person' field.");
            else if (uxPONumber.Text.Contains(","))
                base.MasterPage.DisplayMessage("Please remove commas from the 'PO Number' field.");
            else
            {
                ISession session = UnitOfWork.GetIsolatedSession();
                ITransaction transaccion = session.BeginTransaction();

                Orders orders;
                orders = session.Get<Orders>(new Guid(uxID.Value));

                orders.ContactPerson = uxContactPerson.Text;
                orders.PONumber = uxPONumber.Text;
                orders.Note = uxComments.Text;
                orders.OrderDate = DateTime.UtcNow.AddHours(Convert.ToDouble(MPClients.Web.Properties.Settings.Default.TimeZoneOffset));
                orders.Status = 1;
                orders.StatusDate = DateTime.UtcNow.AddHours(Convert.ToDouble(MPClients.Web.Properties.Settings.Default.TimeZoneOffset));
                orders.Delivery = uxPickupDelivery.Checked;
                

                string[] value = uxID.Value.ToString().Split('-');
                orders.ConfirmationNumber = value[0] + value[1];

                //Send order to WS
                MPClients.Web.OrderWS.OrderWS orderWS = new MPClients.Web.OrderWS.OrderWS();
                orderWS.Url = MPClients.Web.Properties.Settings.Default.MPClients_Web_OrderWS_OrderWS;

                MPClients.Web.OrderWS.Order sendOrder = new MPClients.Web.OrderWS.Order();

                MembershipUser currentUser = Membership.GetUser();

                sendOrder.ClientID = string.IsNullOrEmpty(orders.OnBehalfOf) ? UserClientID : orders.OnBehalfOf;
                sendOrder.OrderDate = DateTime.Now;
                sendOrder.SendDate = DateTime.Now;

                sendOrder.OrderNo = orders.OrderNo.Replace(" ","").Trim() + "-" + orders.AutoNum;

                sendOrder.PONumber = orders.PONumber;
                sendOrder.SalespersonID = MPClients.Web.Properties.Settings.Default.SalespersonID;
                string strPickupMethod = orders.Delivery.Value ? "--Entregar por Ruta--" : "--Recogerá en Almacén--";
                sendOrder.Note = strPickupMethod + orders.Note;
                sendOrder.Status = 1;
                sendOrder.ID = orders.AutoNum;

                ICriterion expression = Expression.Eq("OrderID", ShoppingCartId);
                ICriteria criteria =
                    UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MPClients.DataAccess.Domain.OrderDetail)).Add(expression);
                IList<MPClients.DataAccess.Domain.OrderDetail> ordersDetail
                      = criteria.List<MPClients.DataAccess.Domain.OrderDetail>();

                sendOrder.OrderDetails = new MPClients.Web.OrderWS.OrderDetail[ordersDetail.Count];

                int i = 0;
                foreach (MPClients.DataAccess.Domain.OrderDetail orderDetail in ordersDetail)
                {
                    MPClients.Web.OrderWS.OrderDetail details = new MPClients.Web.OrderWS.OrderDetail();
                    details.NetPrice = orderDetail.NetPrice;
                    details.Quantity = orderDetail.Quantity;
                    details.ProductID = orderDetail.ProductID;
                    details.OrderID = orders.AutoNum;
                  
                    sendOrder.OrderDetails[i] = details;
                    i += 1;
                }
                bool success = false;
                try
                {
                    orderWS.SendOrders(MPClients.Web.Properties.Settings.Default.SalespersonID, new MPClients.Web.OrderWS.Order[] { sendOrder });
                    session.Save(orders);
                    MPClients.PageControllers.BasePage.CleanCurrentShopping(currentUser.ProviderUserKey.ToString());
                    transaccion.Commit();
                    success = true;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();

                    //
                    try
                    {
                        transaccion = session.BeginTransaction();
                        orderWS.Url = MPClients.Web.Properties.Settings.Default.MPClients_Web_OrderWS_OrderWS_Alternate;
                        orderWS.SendOrders(MPClients.Web.Properties.Settings.Default.SalespersonID, new MPClients.Web.OrderWS.Order[] { sendOrder });
                        session.Save(orders);
                        MPClients.PageControllers.BasePage.CleanCurrentShopping(currentUser.ProviderUserKey.ToString());
                        transaccion.Commit();
                        success = true;
                    }
                    catch (Exception ex2)
                    {
                        MasterPage.DisplayMessage("We are currently encountering communication problems.  Please try later to submit your order, or call us for help.");
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("El cliente {0} ha tenido problemas sometiendo la orden con número {1}.  Error: {2}", 
                            currentUser.UserName, sendOrder.OrderNo.Trim(), ex2.Message);
                    }
                    //

                    //try
                    //{
                    //    Helper.SendEmail(sb.ToString(), "Error tratando de enviar la orden desde mpclients.com");
                    //}
                    //catch (Exception ex2)
                    //{
                    //    MasterPage.DisplayMessage("Email error: " + ex2.Message + ".  Original error: " + ex.Message);
                    
                    //}
                }
                if (success)
                {
                    Response.Redirect(MPClients.PageMethods.OrderConfirmation.DoLoad(uxID.Value));
                }
            }
        }

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
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

        private void BindGrid()
        {
            ISession session = UnitOfWork.GetIsolatedSession();
            ICriterion expression = Expression.Eq("OrderID", new Guid(uxID.Value));
            ICriteria criteria =
                session.CreateCriteria(typeof(MPClients.DataAccess.Domain.OrderDetail)).Add(expression);
            criteria.AddOrder(new Order("ProductID", true));
            IList<MPClients.DataAccess.Domain.OrderDetail> orderDetails
                = criteria.List<MPClients.DataAccess.Domain.OrderDetail>();

            int intNumberOfProducts = 0;
            foreach (var item in orderDetails)
            {
                if (!item.IsFriend)
                {
                    intNumberOfProducts += 1;
                }
            }

            uxNumberOfProducts.Text = intNumberOfProducts.ToString();

            bool boolRestrictOrderProductsForPickup = Convert.ToBoolean(Convert.ToInt32(ConfigurationManager.AppSettings["RestrictOrderProductsForPickup"]));
            int intMaxOrderProductsForPickup = Convert.ToInt32(ConfigurationManager.AppSettings["MaxOrderProductsForPickup"].ToString());

            uxPickupWhse.Visible = !boolRestrictOrderProductsForPickup || intNumberOfProducts <= intMaxOrderProductsForPickup;
            uxPickupDeliveryMessage.Visible = !uxPickupWhse.Visible;

            if (uxPickupDeliveryMessage.Visible) {
                uxPickupDeliveryMessage.InnerHtml = "Por el momento no estamos aceptando órdenes de más de " + intMaxOrderProductsForPickup.ToString();
                uxPickupDeliveryMessage.InnerHtml += " artículos para recoger en el almacén. <br/> Para activar la opción de recogido,";
                uxPickupDeliveryMessage.InnerHtml += " por favor edite su carrito de compras y reduzca a " + intMaxOrderProductsForPickup.ToString() + " ó menos el número de artículos.";
            }

            uxGrid.DataSource = orderDetails;
        }

        [MetaSapiens.PageMethods.PageMethod(true)]
        public void DoLoad(string id, bool fromsc)
        {
            uxID.Value = id;
            //create the order
            ISession session = UnitOfWork.GetIsolatedSession();
            Orders orders;
            orders = session.Get<Orders>(new Guid(id));

            MembershipUser UserAccount; 
            MPClients.DataAccess.Domain.Client userClient;

            UserAccount = Membership.GetUser(new Guid(orders.UserId.ToString()));

            if (UserAccount != null)
            {

                if (!string.IsNullOrEmpty(orders.OnBehalfOf))
                {
                    userClient = session.Get<MPClients.DataAccess.Domain.Client>(orders.OnBehalfOf);
                }
                else
                {
                    MPClients.DataAccess.Domain.MembershipUsers userProfile = session.Get<MPClients.DataAccess.Domain.MembershipUsers>(new Guid(UserAccount.ProviderUserKey.ToString()));
                    userClient = session.Get<MPClients.DataAccess.Domain.Client>(userProfile.ClientID);
                }

                uxCurrentTime.Text = DateTime.UtcNow.AddHours(Convert.ToDouble(MPClients.Web.Properties.Settings.Default.TimeZoneOffset)).ToString();
                uxRequestedBy.Text = UserAccount.UserName;
                uxClientName.Text = userClient != null ? userClient.Fullname : "";
                uxContactPerson.Text = orders.ContactPerson;
                uxPONumber.Text = orders.PONumber;
                if (orders.Delivery.HasValue)
                {
                    uxPickupWhse.Checked = !orders.Delivery.Value;
                    uxPickupDelivery.Checked = orders.Delivery.Value;
                }
                uxComments.Text = orders.Note;
                uxOrderStatus.Text = orders.Status == 1 ? "Placed" : "Not Placed";
                // poder enviar sólo si está Not Placed y el usuario no es Admin
                uxSendOrder.Enabled = orders.Status == 2 && !System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE);
                uxContactPerson.ReadOnly = orders.Status == 1;
                uxPONumber.ReadOnly = orders.Status == 1;
                uxPickupWhse.Enabled = orders.Status == 2;
                uxPickupDelivery.Enabled = orders.Status == 2;
                uxComments.ReadOnly = orders.Status == 1;
                // imprimir si el status es Placed o si el usuario es Admin
                uxPrint.Enabled = orders.Status == 1 || System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE);
                uxPrintLink.Visible = false; // orders.Status == 1;
            }

        }

    }
}

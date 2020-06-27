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
using MPClients.Common;
using System.Data.SqlClient;

namespace MPClients
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MPClients.PageControllers.BasePage page = this.Page as MPClients.PageControllers.BasePage;
            if (page == null)
                page = new Login();

            uiClientTableInfo.Visible = !string.IsNullOrEmpty(page.Territory);

            //Si es vendedor lo obligamos a elegir un cliente para continuar....
            if (!string.IsNullOrEmpty(page.Territory) && (String.IsNullOrEmpty(page.UserClientID)))
            {
                if (!(this.Page is MPClients.Login)
                    && !(this.Page is MPClients.Default)
                    && !(this.Page is MPClients.Web.ChooseClient)
                    && !(this.Page is MPClients.PasswordRetrieval)
                    && !(this.Page is MPClients.ResetPassword)
                    && !(this.Page is MPClients.TermsOfUse) )
                {
                    Response.Redirect("ChooseClient.aspx");
                }
            }

            uiClientInfo.InnerText = string.IsNullOrEmpty(page.ClientName) ? "[SELECT CLIENT]" : page.ClientName;

            uiChangeClient.Visible = !string.IsNullOrEmpty(page.Territory);
            uiReset.Visible = uiChangeClient.Visible;

            lblActiveShoppingCarts.Visible = System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE);
            lblNewsSearch.Visible = System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE)
                    || System.Web.Security.Roles.IsUserInRole(Helper.NEWS_EDITOR_ROLE);
            lblUserSearch.Visible = System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE);
            lblClients.Visible = System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE);
            lblSearchConfiguration.Visible = System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE);

            lblAdministration.Visible = System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE);

            ViewShoppingCart.Visible = !System.Web.Security.Roles.IsUserInRole(Helper.ADMINISTRATOR_ROLE)
                && !System.Web.Security.Roles.IsUserInRole(Helper.SEARCH_ONLY_ROLE)
                && Context.Request.IsAuthenticated;

            //ProductSearch.PostBackUrl = "ProductsSearch.aspx?PageMethod=DoLoad";

            OrderSearch.PostBackUrl =  "OrderSearch.aspx";

            ViewShoppingCart.PostBackUrl = "ShoppingCart.aspx";

            if (!this.IsPostBack)
            {
                userName.InnerHtml = page.DefaultClientName;
                ProductInfoLabel.InnerHtml = Session["ProductUpdateInfo"] != null ? "Inventory as of " + Session["ProductUpdateInfo"].ToString() : "";
            }

            lblOrderSearch.Visible = !System.Web.Security.Roles.IsUserInRole(Helper.SEARCH_ONLY_ROLE) && Context.Request.IsAuthenticated;
            contactseparator.Visible = Context.Request.IsAuthenticated;
            uiproductslink.Visible = Context.Request.IsAuthenticated;
            tblOtherResources.Visible = Context.Request.IsAuthenticated;
        }

        internal void DisplayMessage(string message)
        {
            uxMessageBox.NavigateUrl = MPClients.PageMethods.Message.MsgLoad(message); ;
            uxMessageBox.VisibleOnPageLoad = true;
        }
        internal void DisplayMessageById(int messageId)
        {
            uxMessageBox.NavigateUrl = MPClients.PageMethods.Message.LoadById(messageId); ;
            uxMessageBox.VisibleOnPageLoad = true;
        }

        protected override void OnInit(EventArgs e)
        {
            //uxLogOff.Click += new EventHandler(uxLogOff_Click);
            LoginStatus1.LoggedOut += new EventHandler(LoginStatus1_LoggedOut);
            uiChangeClient.Click += new EventHandler(uiChangeClient_Click);
            uiReset.Click += new EventHandler(uiReset_Click);
            ProductSearch.Click +=new EventHandler(ProductSearch_Click);
            base.OnInit(e);
        }

        void ProductSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductsSearch.aspx");
        }

        void uiReset_Click(object sender, EventArgs e)
        {
            MPClients.PageControllers.BasePage page = this.Page as MPClients.PageControllers.BasePage;
            if (page == null)
                page = new Login();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("Select count(*) from Orders where OrderID = '" + page.ShoppingCartId.ToString() + "'", conn);
                object total = command.ExecuteScalar();

                int i;
                if (total != null && int.TryParse(total.ToString(), out i))
                {
                    if (i > 0)
                    {
                        uxMessageBox.NavigateUrl = MPClients.PageMethods.Message.MsgLoad("Actualmente existe un Shopping Cart activo, no puede cambiar el cliente mientras tenga productos en el carrito de compras."); ;
                        uxMessageBox.VisibleOnPageLoad = true;

                        return;
                    }
                }
            }

            MembershipUser currentUser = Membership.GetUser();
            MPClients.PageControllers.BasePage.UpdateCurrentClientId(currentUser.ProviderUserKey.ToString(), page.DefaultClientID);
            MPClients.PageControllers.BasePage.UpdateCurrentClientName(currentUser.ProviderUserKey.ToString(), page.DefaultClientName);

            Response.Redirect(this.Page.Request.RawUrl);


        }

        void uiChangeClient_Click(object sender, EventArgs e)
        {
            MPClients.PageControllers.BasePage page = this.Page as MPClients.PageControllers.BasePage;
            if (page == null)
                page = new Login();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("Select count(*) from Orders where OrderID = '" + page.ShoppingCartId.ToString() + "'", conn);
                object total = command.ExecuteScalar();

                int i;
                if (total != null && int.TryParse(total.ToString(), out i))
                {
                    if (i > 0)
                    {
                        uxMessageBox.NavigateUrl = MPClients.PageMethods.Message.MsgLoad("Actualmente existe un Shopping Cart activo, no puede cambiar el cliente mientras tenga productos en el carrito de compras."); ;
                        uxMessageBox.VisibleOnPageLoad = true;
                        return;
                    }
                }

            }

            Response.Redirect("ChooseClient.aspx");
        }

        void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            Session["ProductUpdateInfo"] = null;
            Session["ChangePassword"] = null;

            FormsAuthentication.SignOut();
            Session.Abandon();

            Response.Redirect("Default.aspx");
        }

        void uxLogOff_Click(object sender, EventArgs e)
        {
             
        }

      
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MPClients.DataAccess.NHibernate;
using NHibernate;
using NHibernate.Expression;
using System.Collections.Generic;
using MPClients.DataAccess.Domain;
using System.Data.SqlClient;

namespace MPClients.PageControllers
{
    public abstract class BasePage : Page
    {

        //-----------------------------------------------

        /// <summary>
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            MembershipUser currentUser = Membership.GetUser();

            if (currentUser != null)
            {
                //ICriterion expression = Expression.Eq("UserName", currentUser.UserName);
                //ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MembershipUsers)).Add(expression);
                //IList<MPClients.DataAccess.Domain.MembershipUsers> users = criteria.List<MembershipUsers>();

                MembershipUsers membershipUser;
                membershipUser = UnitOfWork.GetIsolatedSession().Get<MembershipUsers>(new Guid(currentUser.ProviderUserKey.ToString()));

                if (membershipUser != null)
                {
                    Territory = membershipUser.Territory;
                    DefaultClientID = membershipUser.ClientID;
                    DefaultClientName = membershipUser.ClientName;
                    UserClientID = membershipUser.CurrentClientID;
                    ClientName = membershipUser.CurrentClientName;
                    ShoppingCartId = membershipUser.ShoppingCartId;

                    if (membershipUser.ChangePassword && ! (this is ChangePassword))
                    {
                        Response.Redirect("ChangePassword.aspx");
                    }
                }
            }
         
            PageLoad();
        }

        //-----------------------------------------------

        protected abstract void PageLoad();

        //-----------------------------------------------

        protected virtual void LoadDropDownControls()
        {
        }

        //-----------------------------------------------

        public string UserID
        {
            get
            {
                return Convert.ToString(
                    System.Web.Security.Membership.GetUser(
                        HttpContext.Current.User.Identity.Name).ProviderUserKey);
            }
        }

        //-----------------------------------------------

        public string Territory
        {
            get
            {
                try
                {

                    return ViewState["Territory"].ToString();
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                ViewState["Territory"] = value;
            }
        }

        //-----------------------------------------------

        public string DefaultClientID
        {
            get
            {
                try
                {

                    return ViewState["DefaultClientID"].ToString();
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                ViewState["DefaultClientID"] = value;
            }
        }

        //-----------------------------------------------

        public string ClientName
        {
            get
            {
                try
                {

                    return ViewState["CurrentClientName"].ToString();
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                ViewState["CurrentClientName"] = value;
            }
        }

        //-----------------------------------------------

        public string DefaultClientName
        {
            get
            {
                try
                {

                    return ViewState["DefaultClientName"].ToString();
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                ViewState["DefaultClientName"] = value;
            }
        }
        //-----------------------------------------------

        public string UserClientID
        {
            get
            {
                try
                {

                    return ViewState["UserClientID"].ToString();
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                ViewState["UserClientID"] = value;
            }
        }

        public MPClients.Site  MasterPage
        {
            get
            {
                return base.Master as MPClients.Site ;
            }
        }


        public Guid ShoppingCartId
        {
            get
            {
                try
                {
                    Guid sh = new Guid(ViewState["ShoppingCartId"].ToString());

                    if (sh == Guid.Empty)
                    {
                        MembershipUser currentUser = Membership.GetUser();

                        Guid val = Guid.NewGuid();
                        UpdateCurrentShopping(val, currentUser.ProviderUserKey.ToString());

                        ViewState["ShoppingCartId"] = val;
                        return val;
                    }
                    else
                    {
                        return sh;
                    }
                }
                catch
                {
                    MembershipUser currentUser = Membership.GetUser();

                    Guid val = Guid.NewGuid();
                    UpdateCurrentShopping(val, currentUser.ProviderUserKey.ToString());

                    ViewState["ShoppingCartId"] = val;
                    return val;
                }
            }
            set
            {
                ViewState["ShoppingCartId"] = value;
            }
            
        }

        public static void UpdateCurrentClientId(String userId, string newClientId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(String.Format("Update MembershipUsers set CurrentClientID='{0}' where UserId='{1}'", newClientId, userId), conn);
                command.ExecuteNonQuery();

            }
        }

        public static void UpdateCurrentClientName(String userId, string newClientName)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(String.Format("Update MembershipUsers set CurrentClientName='{0}' where UserId='{1}'", newClientName, userId), conn);
                command.ExecuteNonQuery();

            }
        }

        public static void CleanCurrentShopping(string userId)
        {
            MembershipUser currentUser = Membership.GetUser(); 
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(String.Format("Update MembershipUsers set ShoppingCartId=NULL where UserId='{0}'", userId), conn);
                command.ExecuteNonQuery();

            }
        }

        public static void UpdateCurrentShopping(Guid shopingCartId, string userId)
        {
        

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(String.Format("Update MembershipUsers set ShoppingCartId='{0}' where UserId='{1}'", shopingCartId, userId), conn);
                command.ExecuteNonQuery();
            }
        }
    }
}

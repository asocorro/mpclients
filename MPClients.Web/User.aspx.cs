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
using MPClients.DataAccess.NHibernate;
using MPClients.Common;
using NHibernate;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MPClients
{
    public partial class User : BasePage
    {

        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                LoadCategories();

                if (!MetaSapiens.PageMethods.PageMethodsEngine.InvokeMethod(this, true))
                    throw new Exception("Page method not found!");

                //uxCategories
            }
        }

        private void LoadCategories()
        {
            ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(Category));
            criteria.AddOrder(new NHibernate.Expression.Order("ID", true));
            IList<Category> categories = criteria.List<Category>();

            foreach (Category category in categories)
            {
                uxCategories.Items.Add(new ListItem(category.ID + "- " + category.Description, category.ID));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            uxSave.Click += new EventHandler(uxSave_Click);
            uxClearAll.Click += new EventHandler(uxClearAll_Click);
            uxSelectAll.Click += new EventHandler(uxSelectAll_Click);
            base.OnInit(e);
        }

        void uxSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in uxCategories.Items)
            {
                item.Selected = true;
            }
        }

        void uxClearAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in uxCategories.Items)
            {
                item.Selected = false;
            }
        }


        protected void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            Helper.LoadClients(e, uxClients);
        }

        void uxSave_Click(object sender, EventArgs e)
        {
            //try
            //{

            ISession session = MPClients.DataAccess.NHibernate.UnitOfWork.Session;
            ITransaction transaction = session.BeginTransaction();
            System.Web.Security.MembershipUser SecurityMembershipUser =
                    System.Web.Security.Membership.GetUser(new Guid(uxID.Value));

            
            SecurityMembershipUser.IsApproved = uxActive.Checked;
            Membership.UpdateUser(SecurityMembershipUser);
            if (!(string.IsNullOrEmpty(uxPassword.Text)))
            {
                SecurityMembershipUser.UnlockUser();

                string password = SecurityMembershipUser.ResetPassword("test");
                SecurityMembershipUser.ChangePassword(password, uxPassword.Text);
            }
            if (uxIsAdministrator.Checked
                && !System.Web.Security.Roles.IsUserInRole(SecurityMembershipUser.UserName, Helper.ADMINISTRATOR_ROLE))
            {
                System.Web.Security.Roles.AddUserToRole(SecurityMembershipUser.UserName, Helper.ADMINISTRATOR_ROLE);
            }

            if (uxSearchOnly.Checked && !System.Web.Security.Roles.IsUserInRole(SecurityMembershipUser.UserName, Helper.SEARCH_ONLY_ROLE))
            {
                System.Web.Security.Roles.AddUserToRole(SecurityMembershipUser.UserName, Helper.SEARCH_ONLY_ROLE);
            }
            else if (! uxSearchOnly.Checked && System.Web.Security.Roles.IsUserInRole(SecurityMembershipUser.UserName, Helper.SEARCH_ONLY_ROLE))
            {
                System.Web.Security.Roles.RemoveUserFromRole(SecurityMembershipUser.UserName, Helper.SEARCH_ONLY_ROLE);
            }

            if (uxIsNewsEditor.Checked && !System.Web.Security.Roles.IsUserInRole(SecurityMembershipUser.UserName, Helper.NEWS_EDITOR_ROLE))
            {
                System.Web.Security.Roles.AddUserToRole(SecurityMembershipUser.UserName, Helper.NEWS_EDITOR_ROLE);
            }
            else if (!uxIsNewsEditor.Checked && System.Web.Security.Roles.IsUserInRole(SecurityMembershipUser.UserName, Helper.NEWS_EDITOR_ROLE))
            {
                System.Web.Security.Roles.RemoveUserFromRole(SecurityMembershipUser.UserName, Helper.NEWS_EDITOR_ROLE);
            }

            MembershipUsers membershipUser;
            membershipUser = session.Get<MembershipUsers>(new Guid(uxID.Value));

            try
            {
                membershipUser.UserId = new Guid(uxID.Value);
            }
            catch
            {
                membershipUser = new MembershipUsers(new Guid(uxID.Value));
            }
            membershipUser.ChangePassword = uxChangePassword.Checked;
            membershipUser.ClientID = uxClients.SelectedValue;
           
            membershipUser.ClientName = uxClients.Text;
            membershipUser.LastPriceQueryDate = !membershipUser.LastPriceQueryDate.HasValue ||  membershipUser.LastPriceQueryDate < new DateTime(2000, 1, 1) ? DateTime.Now : membershipUser.LastPriceQueryDate;
            membershipUser.Territory = uiTerritory.Text;
            //membershipUser.LastActivityDate = DateTime.Now;
            //membershipUser.PriceQuerys = 0;

            membershipUser.AllowDelivery = uxAllowDelivery.Checked;
            membershipUser.AllowPickup = uxAllowPickup.Checked;


            IList<MPClients.DataAccess.Domain.UserCategory> Categories = Helper.GetUserCategories(uxID.Value);
            //delete 
            foreach (UserCategory var in Categories)
            {
                bool delete = false;
                foreach (ListItem item in uxCategories.Items)
                {
                    if (!item.Selected)
                    {
                        delete = delete || ( item.Value == var.Category);
                    }
                }
                if (delete)
                    session.Delete(var);
            }

            //Agregamos
            foreach (ListItem item in uxCategories.Items)
            {
                bool add = item.Selected;
                if (item.Selected)
                {
                    foreach (UserCategory var in Categories)
                    {
                        add &= item.Value != var.Category;
                    }
                }
                if (add)
                {
                    UserCategory userCategory = new UserCategory(Guid.NewGuid());
                    userCategory.UserId = new Guid(uxID.Value);
                    userCategory.Category = item.Value;
                    session.Save(userCategory);
                }
            }

            session.Save(membershipUser);

            transaction.Commit();

            string connString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();

            string command = String.Format("Update Client Set Active={1} where Client_id='{0}'", membershipUser.ClientID, !uxInHold.Checked ? 1 : 0);

            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand cmd = new SqlCommand(command, sqlConn))
                {
                    cmd.ExecuteNonQuery();
                }
                sqlConn.Close();
            }

            object clientId = null;

            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand cmd = new SqlCommand(String.Format("Select Client_id from ClientHoldDate where Client_id='{0}'", membershipUser.ClientID), sqlConn))
                {
                    clientId = cmd.ExecuteScalar();
                    if (clientId != null)
                    {
                        string update = String.Format("Update ClientHoldDate Set HoldFromDate={1} where Client_id='{0}'", membershipUser.ClientID, uxHoldFromDate.SelectedDate.HasValue ? "'" + uxHoldFromDate.SelectedDate + "'" : "null");
                        SqlCommand cmdUpdate = new SqlCommand(update, sqlConn);
                        cmdUpdate.ExecuteNonQuery();
                    }
                    else
                    {
                        string insert = String.Format("INSERT INTO [ClientHoldDate]([Client_ID],[HoldFromDate]) VALUES ('{0}', {1})", membershipUser.ClientID, uxHoldFromDate.SelectedDate.HasValue ? "'" + uxHoldFromDate.SelectedDate + "'" : "null");

                        SqlCommand cmdInsert = new SqlCommand(insert, sqlConn);
                        cmdInsert.ExecuteNonQuery();
                    }
                }
                sqlConn.Close();
            }

            MasterPage.DisplayMessage("User information updated.");
            //}
            //catch (Exception ex)
            //{
            //    MasterPage.DisplayMessage(ex.ToString());
            //}
        }
       

        [MetaSapiens.PageMethods.PageMethod(true)]
        public void DoLoad(string user)
        {
            //uxUserName.Value = userName;
            uxID.Value = user;
            System.Web.Security.MembershipUser SecurityMembershipUser =
                System.Web.Security.Membership.GetUser(new Guid(user));


            uxActive.Checked = SecurityMembershipUser.IsApproved;
            uxUserName.Text = SecurityMembershipUser.UserName;
            uxIsAdministrator.Checked = System.Web.Security.Roles.IsUserInRole(SecurityMembershipUser.UserName, Helper.ADMINISTRATOR_ROLE);
            uxIsNewsEditor.Checked = System.Web.Security.Roles.IsUserInRole(SecurityMembershipUser.UserName, Helper.NEWS_EDITOR_ROLE);
            uxSearchOnly.Checked = System.Web.Security.Roles.IsUserInRole(SecurityMembershipUser.UserName, Helper.SEARCH_ONLY_ROLE);
           
            uxLastLogon.Text = SecurityMembershipUser.LastLoginDate.ToString();

            uxLockedOut.Checked = SecurityMembershipUser.IsLockedOut;

            MembershipUsers membershipUser;
            membershipUser = UnitOfWork.GetIsolatedSession().Get<MembershipUsers>(new Guid(user));

            if (membershipUser != null)
            {
                uxClients.SelectedValue = membershipUser.ClientID;
                uxClients.Text = membershipUser.ClientName;
                uxChangePassword.Checked = membershipUser.ChangePassword;
                uxLastPriceQueryDate.Text = membershipUser.LastPriceQueryDate.ToString();
                uxPriceQuerys.Text = membershipUser.PriceQuerys.ToString();
                //uxInHold.Checked =  membershipUser.IsActive.HasValue ? ! membershipUsers.IsActive.Value : true;

                uxAllowDelivery.Checked = membershipUser.AllowDelivery.HasValue ? membershipUser.AllowDelivery.Value : false ;
                uxAllowPickup.Checked = membershipUser.AllowPickup.HasValue ? membershipUser.AllowPickup.Value : false;

                if (!string.IsNullOrEmpty(membershipUser.Territory))
                {
                    string[] territory = membershipUser.Territory.Trim().Split(',');
                    foreach (var s in territory)
                    {
                        var item = uiTerritory.Items.FindItemByText(s.TrimStart(), true);
                        if (item != null)
                        {
                            item.Checked = true;
                        }
                    }
                }

                Client client = UnitOfWork.GetIsolatedSession().Load<Client>(membershipUser.ClientID);
                try
                {
                    //Shit de nhibernate no devuelve nulo si  no esta...devuelve cualquier cosa
                    uxInHold.Checked = client != null && client.Active.HasValue ? !client.Active.Value : false; ;
                }
                catch
                {

                }

                ClientHoldDate clientHoldDate = UnitOfWork.GetIsolatedSession().Load<ClientHoldDate>(membershipUser.ClientID);
                try
                {
                    uxHoldFromDate.SelectedDate = clientHoldDate != null && clientHoldDate.HoldFromDate.HasValue ? clientHoldDate.HoldFromDate : new Nullable<DateTime>();
                }
                catch
                {
                    //uxHoldFromDate.SelectedDate = new Nullable<DateTime>();
                }
            }
        
            //uxCategories
            IList<MPClients.DataAccess.Domain.UserCategory> Categories = Helper.GetUserCategories(uxID.Value);
            foreach (UserCategory var in Categories)
            {
                foreach (ListItem item in uxCategories.Items)
                {
                    if (item.Value == var.Category )
                    {
                        item.Selected = true;
                    }
                }
            }
        }

      

    }
}

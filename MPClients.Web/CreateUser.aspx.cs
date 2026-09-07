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
using MPClients.Common;
using System.Data.SqlClient;
using Telerik.Web.UI;

namespace MPClients
{
    public partial class CreateUser : BasePage
    {

        protected override void PageLoad()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            uxCreateUser.Click += new EventHandler(uxCreateUser_Click);
            base.OnInit(e);
        }

        protected void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            Helper.LoadClients(e, uxClients);
        }

        void uxCreateUser_Click(object sender, EventArgs e)
        {
            try
            {
                MembershipCreateStatus status;
                Membership.CreateUser(
                    uxUserName.Text, uxPassword.Text, uxPassword.Text + "1@mpclients.com", "test", "test", true, out status);


                if (status == MembershipCreateStatus.Success)
                {

                    MembershipUser user = Membership.GetUser(uxUserName.Text);

                    if (uxIsAdministrator.Checked)
                    {
                        System.Web.Security.Roles.AddUserToRole(uxUserName.Text, Helper.ADMINISTRATOR_ROLE);
                    }

                    if (uxSearchOnly.Checked && !System.Web.Security.Roles.IsUserInRole(uxUserName.Text, Helper.SEARCH_ONLY_ROLE))
                    {
                        System.Web.Security.Roles.AddUserToRole(uxUserName.Text, Helper.SEARCH_ONLY_ROLE);
                    }
                    else if (!uxSearchOnly.Checked && System.Web.Security.Roles.IsUserInRole(uxUserName.Text, Helper.SEARCH_ONLY_ROLE))
                    {
                        System.Web.Security.Roles.RemoveUserFromRole(uxUserName.Text, Helper.SEARCH_ONLY_ROLE);
                    }

                    if (uxIsNewsEditor.Checked && !System.Web.Security.Roles.IsUserInRole(uxUserName.Text, Helper.NEWS_EDITOR_ROLE))
                    {
                        System.Web.Security.Roles.AddUserToRole(uxUserName.Text, Helper.NEWS_EDITOR_ROLE);
                    }
                    else if (!uxIsNewsEditor.Checked && System.Web.Security.Roles.IsUserInRole(uxUserName.Text, Helper.NEWS_EDITOR_ROLE))
                    {
                        System.Web.Security.Roles.RemoveUserFromRole(uxUserName.Text, Helper.NEWS_EDITOR_ROLE);
                    }

                    string insert = @"INSERT INTO [MembershipUsers]
                                                   ([UserId]
                                                   ,[Email]
                                                   ,[UserName]
                                                   ,[ClientID]
                                                   ,[PriceQuerys]
                                                   ,[ChangePassword]
                                                   ,[ClientName]
                                                    , Territory
                                                    , AllowDelivery
                                                    , AllowPickup)
                                             VALUES
                                                   ('{0}'
                                                   ,'{1}'
                                                   ,'{2}'
                                                   ,'{3}'
                                                   , 0
                                                   , {5}
                                                   ,'{4}'
                                                   ,'{6}'
                                                   ,'{7}'
                                                   ,'{8}'
                                                   )";
                    insert = String.Format(insert, new Guid(user.ProviderUserKey.ToString()), uxPassword.Text + "1@mpclients.com", uxUserName.Text, uxClients.SelectedValue, uxClients.Text, uxChangePassword.Checked ? 1 : 0, uiTerritory.Text
                            , uxAllowDelivery.Checked, uxAllowPickup.Checked);

                    SqlConnection sqlConn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                    sqlConn1.Open();
                    SqlCommand sqlCommand = new SqlCommand(insert, sqlConn1);
                    sqlCommand.ExecuteNonQuery();
                    sqlConn1.Close();

                    MasterPage.DisplayMessage("New user created successfully.");
                    uxCreateUser.Enabled = false;
                }
                else
                {
                    string outputMessage = "";
                    switch (status)
                    {
                        case MembershipCreateStatus.InvalidPassword:
                            outputMessage = "Invalid Password";
                            break;
                        case MembershipCreateStatus.DuplicateEmail:
                            outputMessage = "Duplicate Email";
                            break;
                        case MembershipCreateStatus.DuplicateProviderUserKey:
                            outputMessage = "Invalid Provider User Key";
                            break;
                        case MembershipCreateStatus.DuplicateUserName:
                            outputMessage = "Duplicate User Name";
                            break;
                        case MembershipCreateStatus.InvalidUserName:
                            outputMessage = "Invalid User Name";
                            break;
                        case MembershipCreateStatus.UserRejected:
                            outputMessage = "User Rejected";
                            break;

                    }
                    MasterPage.DisplayMessage(outputMessage);
                }
            }
            catch (Exception ex)
            {
                MasterPage.DisplayMessage(ex.ToString());
            }
        }
    }
}

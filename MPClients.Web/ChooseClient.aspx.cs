using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using NHibernate.Expression;
using NHibernate;
using MPClients.DataAccess.NHibernate;
using MPClients.DataAccess.Domain;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;

namespace MPClients.Web
{
    public partial class ChooseClient : MPClients.PageControllers.BasePage
    {
        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                //ISession session = MPClients.DataAccess.NHibernate.UnitOfWork.Session;
                //MembershipUser currentUser = Membership.GetUser();
                //MembershipUsers membershipUser;
                //membershipUser = session.Get<MembershipUsers>(new Guid(currentUser.ProviderUserKey.ToString()));

                //if (membershipUser != null && ! string.IsNullOrEmpty(membershipUser.ClientID))
                //{
                //    uxClients.SelectedValue = membershipUser.ClientID;
                //    uxClients.Text = membershipUser.ClientName;
                //}


                ////Cuando se cambia el cliente se resetea el shopping cart
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("Select count(*) from Orders where OrderID = '" + ShoppingCartId + "'", conn);
                    object total = command.ExecuteScalar();

                    int i;
                    if (total != null && int.TryParse(total.ToString(), out i))
                    {
                        uiAlertPanel.Visible = i > 0;
                        uiNewClientPanel.Visible = i == 0;
                    }
                    else
                    {
                        uiAlertPanel.Visible = false;
                        uiNewClientPanel.Visible = true;
                    }

                    conn.Close();
                }

            }
        }

        protected override void OnInit(EventArgs e)
        {
            uxSave.Click += new EventHandler(uxSave_Click);
            base.OnInit(e);
        }

        void uxSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(uxClients.SelectedValue))
            {
                //ISession session = MPClients.DataAccess.NHibernate.UnitOfWork.Session;
                //ITransaction transaction = session.BeginTransaction();

                //MembershipUser currentUser = Membership.GetUser();

                //MembershipUsers membershipUser;
                //membershipUser = session.Get<MembershipUsers>(new Guid(currentUser.ProviderUserKey.ToString()));
                //membershipUser.ClientID = uxClients.SelectedValue;
                //membershipUser.ClientName = uxClients.Text;
                //membershipUser.LastPriceQueryDate = membershipUser.LastPriceQueryDate.HasValue ? membershipUser.LastPriceQueryDate : DateTime.Now;
                //session.Save(membershipUser);
                //transaction.Commit();

                //Session["UserClientID"] = uxClients.SelectedValue;

                MembershipUser currentUser = Membership.GetUser();
                MPClients.PageControllers.BasePage.UpdateCurrentClientId(currentUser.ProviderUserKey.ToString(), uxClients.SelectedValue);
                MPClients.PageControllers.BasePage.UpdateCurrentClientName(currentUser.ProviderUserKey.ToString(), uxClients.Text);

                Response.Redirect(this.Page.Request.RawUrl);
        

                //MasterPage.DisplayMessage("Client assigned successfully.");
            }
        }

        protected void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Text) && e.Text.Length > 1)
            {
                try
                {
                    ICriterion expression =
                        Expression.Or(
                        Expression.Like("ID", e.Text, MatchMode.Start)
                        , Expression.Like("Name", e.Text, MatchMode.Start));
                    ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MPClients.DataAccess.Domain.Client)).Add(expression);
                    criteria.AddOrder(new Order("Name", true));
                    IList<MPClients.DataAccess.Domain.Client> allClients = criteria.List<MPClients.DataAccess.Domain.Client>();

                    int itemsPerRequest = 10;
                    int itemOffset = e.NumberOfItems;
                    int endOffset = itemOffset + itemsPerRequest;

                    if (endOffset > allClients.Count)
                    {
                        endOffset = allClients.Count;
                    }

                    if (! string.IsNullOrEmpty(Territory))
                    {
                        var list = Territory.ToString().Replace(" ", "").Split(',').ToList();

                        foreach (MPClients.DataAccess.Domain.Client client in allClients)
                        {
                            if (list.Contains(client.Territory))
                            {
                                uxClients.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(
                                    client.ID + "-" + client.Name,
                                    client.ID));
                            }
                        }
                    }

                    uxClients.SelectedIndex = 0;

                    if (allClients.Count > 0)
                    {
                        e.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset, allClients.Count);
                    }
                    else
                    {
                        e.Message = "No matches";
                    }
                }
                catch
                {
                    e.Message = "No matches";
                }
            }
        }
    }
}
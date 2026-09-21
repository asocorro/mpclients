using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic ;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MPClients.DataAccess;
using MPClients.DataAccess.Domain;
using MPClients.DataAccess.NHibernate;
using NHibernate.Expression;
using NHibernate;
using System.Data.SqlClient;
using MPClients.PageControllers;
using MPClients.Common;
using Telerik.Web.UI;

namespace MPClients
{
    public partial class Users : BasePage
    {


        protected override void PageLoad()
        {

        }

        //----------------------------------------------------------------------------------

        protected override void OnInit(EventArgs e)
        {
            uxSearch.Click += new EventHandler(uxSearch_Click);
            uxAddUser.Click += new EventHandler(uxAddUser_Click);
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            uxGrid.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(uxGrid_ItemCommand);
            uiExportResult.Click += new EventHandler(uiExportResult_Click);
            base.OnInit(e);
        }

        void uxGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                string id = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString();
                Response.Redirect(MPClients.PageMethods.User.DoLoad(id));
            }
        }


        protected void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            Helper.LoadClients(e, uxClients);
        }
 
        void uxAddUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("Createuser.aspx");
        }

        //----------------------------------------------------------------------------------

        void uxSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindDataGrid();
            }
            catch { }
        }

        //----------------------------------------------------------------------------------

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                BindDataGrid();
            }
            catch { }
        }

        void uiExportResult_Click(object sender, EventArgs e)
        {
            uxGrid.MasterTableView.ExportToExcel();
        }

        private void BindDataGrid()
        {
            try
            {
                string client = "";
                string userName = "";
                if (!String.IsNullOrEmpty(uxClients.SelectedValue))
                {
                    client = uxClients.SelectedValue;
                }
                if (!String.IsNullOrEmpty(uxTextToSearch.Text))
                {
                    userName = uxTextToSearch.Text;
                }
                
                ICriterion expression = Expression.Eq("UserName", "^%$%");
                if (userName == "" && client == "")
                {
                    expression = Expression.Like("UserName", "%%", MatchMode.Anywhere);
                }
                else if (userName != "" && client != "")
                {
                    expression = Expression.And(
                        Expression.Like("UserName", userName, MatchMode.Anywhere),
                        Expression.Like("ClientID", client, MatchMode.Anywhere)
                        );
                }
                else if (userName == "" && client != "")
                {
                    expression = Expression.Like("ClientID", client, MatchMode.Anywhere);
                }
                else if (userName != "" && client == "")
                {
                    expression = Expression.Like("UserName", userName, MatchMode.Anywhere);
                }
                if (chkOnlyActive.Checked)
                {
                    expression = Expression.And(expression,Expression.Eq("IsActive",true));
                }
                if (chkOnlyLockedOut.Checked)
                {
                    expression = Expression.And(expression, Expression.Eq("IsLockedOut", true));
                }
                IList<MPClients.DataAccess.Domain.MembershipUsers> users;
                using (ISession isolatedSession = UnitOfWork.GetIsolatedSession())
                {
                    ICriteria criteria = isolatedSession.CreateCriteria(typeof(MembershipUsers)).Add(expression);
                    criteria.AddOrder(new Order("UserName", true));
                    users = criteria.List<MembershipUsers>();
                }

                uxGrid.DataSource = users;
                uxGrid.Rebind();
            }
            catch (Exception ex)
            {

            }
        }
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
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

namespace MPClients.Web
{
    public partial class Clients : System.Web.UI.Page
    {
        SqlConnection _conn;
        SqlDataReader _reader;

        string _SelectedClient;

        public string SelectedClient
        {
            get { return _SelectedClient; }
            set { _SelectedClient = value; } 
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            uxSearch.Click += new EventHandler(uxSearch_Click);
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            uxGrid.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(uxGrid_ItemCommand);
            base.OnInit(e);
        }

        void uxGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                SelectedClient = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClientId"].ToString();
                //Response.Redirect(MPClients.PageMethods.User.DoLoad(id));
                //Response.Redirect("ClientEdit.aspx?"); //ClientId=" + id);
                Server.Transfer("ClientEdit.aspx"); 
            }
        }

        protected void uxClients_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            Helper.LoadClients(e, uxClients);
        }

        void uxSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindDataGrid();
            }
            catch { }
        }

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                //BindDataGrid();
            }
            catch { }
        }

        protected void uxGrid_DataBound(object sender, EventArgs e)
        {
            _reader.Close();
            _conn.Close();
        }

        private void BindDataGrid()
        {
            try
            {
                string client = "";
                if (!String.IsNullOrEmpty(uxClients.SelectedValue))
                {
                    client = uxClients.SelectedValue;
                }

                ICriterion expression = Expression.Eq("UserName", "^%$%");

                if (client != string.Empty)
                {
                    expression = Expression.Like("ClientID", client, MatchMode.Anywhere);
                }

                if (chkInSearchResults.Checked)
                {
                    if (client != string.Empty)
                    {
                        expression = Expression.And(expression, Expression.Eq("InSearchResults", true));
                    }
                    else
                    {
                        expression = Expression.Eq("InSearchResults", true);
                    }
                }

                _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

                SqlCommand command = new SqlCommand("spGetClients", _conn);
                command.Parameters.AddWithValue("@ClientId",client);
                command.Parameters.AddWithValue("@InSearchResults", chkInSearchResults.Checked);
                command.CommandType = CommandType.StoredProcedure;

                _conn.Open();
                _reader = command.ExecuteReader();

                uxGrid.DataSource = _reader;
                uxGrid.DataBind();


                // todo: el query es XXX
                //ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MembershipUsers)).Add(expression);
                //criteria.AddOrder(new Order("ClientID", true));
                //IList<MPClients.DataAccess.Domain.MembershipUsers> users = criteria.List<MembershipUsers>();


            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
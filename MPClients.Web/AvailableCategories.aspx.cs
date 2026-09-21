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
    public partial class AvailableCategories : BasePage
    {
        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                LoadAllCategories();
                MarkAvailableCategories();
            }
        }

        private void LoadAllCategories()
        {
            IList<Category> categories;
            using (ISession isolatedSession = UnitOfWork.GetIsolatedSession())
            {
                ICriteria criteria = isolatedSession.CreateCriteria(typeof(Category));
                criteria.AddOrder(new NHibernate.Expression.Order("ID", true));
                categories = criteria.List<Category>();
            }

            foreach (Category category in categories)
            {
                uxCategories.Items.Add(new ListItem(category.ID + "- " + category.Description, category.ID.Trim()));
            }
        }

        public void MarkAvailableCategories()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            SqlCommand command = new SqlCommand("spGetAvailableCategories", conn);
            command.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    foreach (ListItem item in uxCategories.Items)
                    {
                        if (item.Value == reader.GetSqlString(0))
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            reader.Close();
            conn.Close();
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

        void uxSave_Click(object sender, EventArgs e)
        {
            string strSelectedIndexes = string.Empty;
            foreach (ListItem item in uxCategories.Items)
            {
                if (item.Selected)
                {
                    strSelectedIndexes += "," + Convert.ToInt32(item.Value).ToString();
                }
            }

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            SqlCommand command = new SqlCommand("spSaveAvailableCategories", conn);
            command.Parameters.AddWithValue("@SelectedIndexes", strSelectedIndexes);
            command.CommandType = CommandType.StoredProcedure;

            conn.Open();
            command.ExecuteNonQuery();

            MasterPage.DisplayMessage("Information updated.");
        }
    }
}
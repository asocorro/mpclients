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
    public partial class OtherSearchSettings : BasePage
    {
        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                LoadData();
            }
        }



        public void LoadData()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            SqlCommand command = new SqlCommand("spGetConfiguration", conn);
            command.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                txtMaxDistance.Text = reader["MaxDistance"].ToString();
                txtMaxDistanceExpanded.Text = reader["MaxDistanceExpanded"].ToString();
            }
            reader.Close();
            conn.Close();
        }

        protected override void OnInit(EventArgs e)
        {
            uxSave.Click += new EventHandler(uxSave_Click);
            base.OnInit(e);
        }

        void uxSave_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            SqlCommand command = new SqlCommand("spSaveConfiguration", conn);
            command.Parameters.AddWithValue("@MaxDistanceExpanded", txtMaxDistanceExpanded.Text);
            command.Parameters.AddWithValue("@MaxDistance", txtMaxDistance.Text);
            command.CommandType = CommandType.StoredProcedure;

            conn.Open();
            command.ExecuteNonQuery();

            MasterPage.DisplayMessage("Information updated.");
        }
    }
}
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
    public partial class ClientEdit : BasePage
    {
        int _Client_ID_index = 0;

        // for client local
        int _BusinessName_index = 1;
        int _Name_index = 2;
        int _Contact_person_index = 3;
        int _Address1_index = 4;
        int _Address2_index = 5;
        int _Town_index = 6;
        int _Country_index = 7;
        int _Telephone_number_index = 8;
        int _Active_index = 9;
        int _Class_index = 10;
        int _HoldFromDate_index = 11;
        int _Territory_index = 12;
        int _ZipCode_index = 13;
        int _WebAddress_index = 14;
        int _MapLink_index = 15;
        int _ExtraColumn1_index = 16;
        int _ExtraColumn2_index = 17;
        int _ExtraColumn3_index = 18;
        int _ExtraColumn4_index = 19;
        int _ExtraColumn5_index = 20;
        int _ExtraColumn6_index = 21;
        int _Label1_index = 22;
        int _Label2_index = 23;
        int _Label3_index = 24;
        int _Label4_index = 25;
        int _Label5_index = 26;
        int _Label6_index = 27;
        int _IsMapAddress_index = 28;

        // for client

        int _c_ClientId_index = 0;
        int _c_Name_index = 1;
        int _c_contact_person_index = 2;
        int _c_Address1_index = 3;
        int _c_Address2_index = 4;
        int c_Town_index = 5;
        int c_Country_index = 6;
        int c_Telephone_number_index = 7;
        int c_ZipCode_index = 12;

        string _BaseMapURL = "https://maps.google.com/?q=";

        SqlConnection _conn;
        SqlDataReader _reader;

        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                uxClientId.Value = PreviousPage.SelectedClient;
                LoadLocalData();
                LoadDynamcicsData();
                LoadLogo();
                LoadAdditionalZipCodes();
                BindDataGrid();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            uxSave.Click += new EventHandler(uxSave_Click);
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            uxEditClientOrdering.Click += new EventHandler(uxEditClientOrdering_Click);
            base.OnInit(e);
        }

        private void LoadDynamcicsData()
        {
            radioDynamicsAddress.Checked = !radioLocalAddress.Checked;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            SqlCommand command = new SqlCommand("spGetClient", conn);
            command.Parameters.AddWithValue("@ClientId", uxClientId.Value);
            command.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                lblClientName.Text = uxClientId.Value + "-" + reader[_c_contact_person_index].ToString();
                lblAddressLine1.Text = reader[_c_Address1_index].ToString().Trim();
                lblAddressLine2.Text = reader[_c_Address2_index].ToString().Trim();
                lblCityStateZipCode.Text = reader[c_Town_index].ToString().Trim() + ", "
                    + reader[c_Country_index].ToString().Trim() + "  "
                    + reader[c_ZipCode_index].ToString().Trim();
                lblTelephoneNumber.Text = reader[c_Telephone_number_index].ToString();
            }
            reader.Close();
            conn.Close();

            // build map URL
            uxClientDynamicsMap.HRef = lblAddressLine1.Text + ", " + lblAddressLine2.Text + ", " + lblCityStateZipCode.Text;
            uxClientDynamicsMap.HRef = _BaseMapURL + uxClientDynamicsMap.HRef.Replace(" ", "+");
        }

        private void LoadLocalData()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            SqlCommand command = new SqlCommand("spGetClientLocal", conn);
            command.Parameters.AddWithValue("@ClientId", uxClientId.Value);
            command.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                txtBusinessName.Text = reader[_BusinessName_index].ToString();
                txtAddressLine1.Text = reader[_Address1_index].ToString();
                txtAddressLine2.Text = reader[_Address2_index].ToString();
                txtCity.Text = reader[_Town_index].ToString();
                txtState.Text = reader[_Country_index].ToString();
                txtZipCode.Text = reader[_ZipCode_index].ToString();
                txtWebAddress.Text = reader[_WebAddress_index].ToString();
                txtMapLink.Text = reader[_MapLink_index].ToString();
                txtExtra1.Text = reader[_ExtraColumn1_index].ToString();
                txtExtra2.Text = reader[_ExtraColumn2_index].ToString();
                txtExtra3.Text = reader[_ExtraColumn3_index].ToString();
                txtExtra4.Text = reader[_ExtraColumn4_index].ToString();
                txtExtra5.Text = reader[_ExtraColumn5_index].ToString();
                txtExtra6.Text = reader[_ExtraColumn6_index].ToString();
                txtLabel1.Text = reader[_Label1_index].ToString();
                txtLabel2.Text = reader[_Label2_index].ToString();
                txtLabel3.Text = reader[_Label3_index].ToString();
                txtLabel4.Text = reader[_Label4_index].ToString();
                txtLabel5.Text = reader[_Label5_index].ToString();
                txtLabel6.Text = reader[_Label6_index].ToString();
                radioLocalAddress.Checked = Convert.ToBoolean(reader[_IsMapAddress_index]);
            }
            reader.Close();
            conn.Close();

            // build map URL
            if (txtMapLink.Text != null && txtMapLink.Text != string.Empty)
            {
                uxClientLocalMap.HRef = txtMapLink.Text;
            }
            else
            {
                uxClientLocalMap.HRef = txtAddressLine1.Text + ", " + txtAddressLine2.Text + ", " + txtCity.Text + ", " + txtState.Text + ", " + txtZipCode.Text;
                uxClientLocalMap.HRef = _BaseMapURL + uxClientLocalMap.HRef.Replace(" ", "+");
            }
        }

        private void LoadLogo()
        {
            uxClientLogo.ImageUrl = "ClientLogo.aspx?ClientId=" + uxClientId.Value;
        }

        private void LoadAdditionalZipCodes()
        {
            radioDynamicsAddress.Checked = !radioLocalAddress.Checked;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            SqlCommand command = new SqlCommand("spGetClientAdditionalZipCodeCSV", conn);
            command.Parameters.AddWithValue("@ClientId", uxClientId.Value);
            command.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                txtAdditionalZipCodes.Text = reader[0].ToString();
            }
            reader.Close();
            conn.Close();
        }

        void uxSave_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            // update logo
            UploadedFile file;
            SqlCommand cmdClientLogo = new SqlCommand();
            cmdClientLogo.Connection = conn;
            cmdClientLogo.CommandType = CommandType.StoredProcedure;

            if (chkRemoveLogo.Checked)
            {
                cmdClientLogo.CommandText = "spDeleteClientLogo";
                cmdClientLogo.Parameters.AddWithValue("@ClientId", uxClientId.Value);
            }
            else if (uxLogoFile.UploadedFiles.Count > 0)
            {
                file = uxLogoFile.UploadedFiles[0];
                byte[] filedata = new byte[file.InputStream.Length];
                file.InputStream.Read(filedata, 0, (int)file.InputStream.Length);
                string FileName = file.FileName;
                string FileExtension = file.GetExtension();

                int intThumbnailSize = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailSize"].ToString());
                byte[] thumb = Helper.CreateThumbnail(filedata, intThumbnailSize);

                cmdClientLogo.CommandText = "spSaveClientLogo";
                # region REGION parameters
                cmdClientLogo.Parameters.AddWithValue("@ClientId", uxClientId.Value);
                cmdClientLogo.Parameters.AddWithValue("@FileName", FileName);
                cmdClientLogo.Parameters.AddWithValue("@DateUploaded", DateTime.Now);
                cmdClientLogo.Parameters.AddWithValue("@MimeType", FileExtension);
                cmdClientLogo.Parameters.AddWithValue("@FileLength", file.InputStream.Length);
                cmdClientLogo.Parameters.AddWithValue("@FileContent", filedata);
                cmdClientLogo.Parameters.AddWithValue("@ThumbnailFileContent", thumb);
                #endregion
            }

            // update local data
            SqlCommand cmdClientLocal = new SqlCommand("spSaveClientLocal", conn);
            cmdClientLocal.CommandType = CommandType.StoredProcedure;

            # region REGION parameters
            cmdClientLocal.Parameters.AddWithValue("@ClientId", uxClientId.Value);
            cmdClientLocal.Parameters.AddWithValue("@BusinessName", txtBusinessName.Text);
            cmdClientLocal.Parameters.AddWithValue("@Address1",txtAddressLine1.Text);
            cmdClientLocal.Parameters.AddWithValue("@Address2",txtAddressLine2.Text);
            cmdClientLocal.Parameters.AddWithValue("@Town",txtCity.Text);
            cmdClientLocal.Parameters.AddWithValue("@Country",txtState.Text);
            cmdClientLocal.Parameters.AddWithValue("@ZipCode",txtZipCode.Text);
            cmdClientLocal.Parameters.AddWithValue("@WebAddress",txtWebAddress.Text);
            cmdClientLocal.Parameters.AddWithValue("@MapLink", txtMapLink.Text);
            cmdClientLocal.Parameters.AddWithValue("@ExtraColumn1",txtExtra1.Text);
            cmdClientLocal.Parameters.AddWithValue("@ExtraColumn2",txtExtra2.Text);
            cmdClientLocal.Parameters.AddWithValue("@ExtraColumn3",txtExtra3.Text);
            cmdClientLocal.Parameters.AddWithValue("@ExtraColumn4", txtExtra4.Text);
            cmdClientLocal.Parameters.AddWithValue("@ExtraColumn5", txtExtra5.Text);
            cmdClientLocal.Parameters.AddWithValue("@ExtraColumn6", txtExtra6.Text);
            cmdClientLocal.Parameters.AddWithValue("@Label1", txtLabel1.Text);
            cmdClientLocal.Parameters.AddWithValue("@Label2", txtLabel2.Text);
            cmdClientLocal.Parameters.AddWithValue("@Label3", txtLabel3.Text);
            cmdClientLocal.Parameters.AddWithValue("@Label4", txtLabel4.Text);
            cmdClientLocal.Parameters.AddWithValue("@Label5", txtLabel5.Text);
            cmdClientLocal.Parameters.AddWithValue("@Label6", txtLabel6.Text);
            cmdClientLocal.Parameters.AddWithValue("@IsMapAddress",radioLocalAddress.Checked);
            #endregion

            // update additional zip codes
            SqlCommand cmdAdditionalZipCode = new SqlCommand("spSaveClientAdditionalZipCode", conn);
            cmdAdditionalZipCode.CommandType = CommandType.StoredProcedure;

            # region REGION parameters
            cmdAdditionalZipCode.Parameters.AddWithValue("@ClientId", uxClientId.Value);
            cmdAdditionalZipCode.Parameters.AddWithValue("@ZipCodeCSV", txtAdditionalZipCodes.Text);
            #endregion

            conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                cmdClientLocal.Transaction = transaction;

                cmdClientLocal.ExecuteNonQuery();
                if (uxLogoFile.UploadedFiles.Count > 0 || chkRemoveLogo.Checked)
                {
                    cmdClientLogo.Transaction = transaction;
                    cmdClientLogo.ExecuteNonQuery();
                    chkRemoveLogo.Checked = false;
                }
                cmdAdditionalZipCode.Transaction = transaction;
                cmdAdditionalZipCode.ExecuteNonQuery();

                transaction.Commit();

                // rebuild map URL
                if (txtMapLink.Text != null && txtMapLink.Text != string.Empty)
                {
                    uxClientLocalMap.HRef = txtMapLink.Text;
                }
                else
                {
                    uxClientLocalMap.HRef = txtAddressLine1.Text + ", " + txtAddressLine2.Text + ", " + txtCity.Text + ", " + txtState.Text + ", " + txtZipCode.Text;
                    uxClientLocalMap.HRef = _BaseMapURL + uxClientLocalMap.HRef.Replace(" ", "+");
                }

                MasterPage.DisplayMessage("Information updated.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MasterPage.DisplayMessage(ex.Message);
            }
            finally
            {
                conn.Close();
            }
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
                _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

                SqlCommand command = new SqlCommand("spGetResultOrderByClient", _conn);
                command.Parameters.AddWithValue("@ClientId", uxClientId.Value);
                command.CommandType = CommandType.StoredProcedure;

                _conn.Open();
                _reader = command.ExecuteReader();

                uxGrid.DataSource = _reader;
                uxGrid.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        void uxEditClientOrdering_Click(object sender, EventArgs e)
        {
            Response.Redirect("ClientOrderingForResults.aspx");
        }
    }
}
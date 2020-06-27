using MPClients.DataAccess.Domain;
using MPClients.DataAccess.NHibernate;
using NHibernate.Expression;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MPClients.Web
{
    public class ClientHoldDate
    {
        public String Client_ID { get; set; }
        public String HoldFromDate { get; set; }
        public String HoldDbFromDate { get; set; }
        public String Message { get; set; }

        public bool Update { get; set; }

    }
    public partial class LoadHoldsFile : PageControllers.BasePage
    {
        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
            }
         
        }

        protected override void OnInit(EventArgs e)
        {
            btnUpload.Click += BtnUpload_Click;
            btnBack.Click += BtnBack_Click;
            btnImport.Click += BtnImport_Click;
            base.OnInit(e);
        }

  
        private void BtnBack_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            var list = new List<ClientHoldDate>();
            btnImport.Visible = true;

            try
            {
                if (filMyFile.PostedFile != null && !String.IsNullOrEmpty(filMyFile.PostedFile.FileName))
                {
                    fileName.Value = filMyFile.PostedFile.FileName;

                    var dbList = new List<ClientHoldDate>();

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                    SqlCommand command = new SqlCommand("Select Client_ID, HoldFromDate from ClientHoldDate", conn);

                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                if (!reader.IsDBNull(1))
                                {
                                    dbList.Add(new ClientHoldDate
                                    {
                                        Client_ID = reader.GetString(0),
                                        HoldDbFromDate = reader.GetDateTime(1).ToShortDateString()
                                    });
                                }
                                else
                                {
                                    dbList.Add(new ClientHoldDate
                                    {
                                        Client_ID = reader.GetString(0),
                                        HoldDbFromDate = string.Empty
                                    });
                                }
                            }

                        }

                        reader.Close();
                    }
                    conn.Close();


                    DataTable dt = new DataTable();
                    dt.Columns.Add("Client_ID");
                    dt.Columns.Add("HoldFromDate");
                    dt.Columns.Add("HoldDbFromDate");
                    dt.Columns.Add("Message");

                    int lineNumber = 0;
                    var fileReader = new StreamReader(filMyFile.PostedFile.InputStream);
                    string line;
                    while ((line = fileReader.ReadLine()) != null)
                    {
                        lineNumber += 1;
                        // skip the first line
                        if (lineNumber > 1)
                        {
                            var lineData = line.Split(',');

                            ClientHoldDate clientHoldDate = new ClientHoldDate();

                            clientHoldDate.Client_ID = lineData[0];

                            // rellenar con ceros hasta hacer el largo de 5
                            if (clientHoldDate.Client_ID.Length < 5)
                            {
                                clientHoldDate.Client_ID = new String('0', 5 - clientHoldDate.Client_ID.Length) 
                                    + clientHoldDate.Client_ID;
                            }

                            clientHoldDate.HoldFromDate = lineData[1];

                            DateTime holdFromDate;
                            if (DateTime.TryParse(clientHoldDate.HoldFromDate, out holdFromDate))
                            {
                                clientHoldDate.HoldFromDate = holdFromDate.ToShortDateString();

                                var row = dbList.FirstOrDefault(r => r.Client_ID == clientHoldDate.Client_ID);
                                if (row != null)
                                {
                                    clientHoldDate.Message = "OK";
                                    clientHoldDate.HoldDbFromDate = row.HoldDbFromDate;
                                    clientHoldDate.Update = true;
                                }
                                else
                                {
                                    clientHoldDate.Message = "First time this client will be on hold.";
                                    clientHoldDate.Update = false;
                                }
                            }
                            else
                            {

                                clientHoldDate.Message = "Invalid Date";
                                btnImport.Visible = false;
                            }
                            list.Add(clientHoldDate);

                            dt.Rows.Add(clientHoldDate.Client_ID, clientHoldDate.HoldFromDate, clientHoldDate.HoldDbFromDate, clientHoldDate.Message);
                        }
                    }
                    fileReader.Close();

                    rptUpload.DataSource = dt;
                    rptUpload.DataBind();

                    panel1.Visible = false;
                    panel2.Visible = true;

                    Session["ClientHoldDate"] = list;
                }
            }
            catch
            {
            }
          
             
        }
        private void BtnImport_Click(object sender, EventArgs e)
        {

            var list = (List < ClientHoldDate > )Session["ClientHoldDate"] ;
            if(list != null)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
                {
                    conn.Open();
                    MembershipUser currentUser = Membership.GetUser();

                    foreach (var item in list)
                    {
                        var pClientId = new SqlParameter("ClientId", item.Client_ID);
                        var pHoldFromDate = new SqlParameter("HoldFromDate", DateTime.Parse(item.HoldFromDate));
                        var pFileName = new SqlParameter("FileName", fileName.Value);
                        var pModifyOn = new SqlParameter("ModifyOn", DateTime.Now);
                        var pModifyBy = new SqlParameter("ModifyBy", currentUser.UserName);
                        var pCreatedOn = new SqlParameter("CreatedOn", DateTime.Now);
                        var pCreatedBy = new SqlParameter("CreatedBy", currentUser.UserName);


                        if (item.Update)
                        {
                            SqlCommand cmd = new SqlCommand("UPDATE ClientHoldDate set HoldFromDate=@HoldFromDate, FileName = @FileName,  ModifiedOn = @ModifyOn, ModifiedBy = @ModifyBy WHERE  Client_ID=@ClientId");
                            cmd.Connection = conn;
                            cmd.Parameters.Add(pClientId);
                            cmd.Parameters.Add(pHoldFromDate);
                            cmd.Parameters.Add(pFileName);
                            cmd.Parameters.Add(pModifyOn);
                            cmd.Parameters.Add(pModifyBy);

                            cmd.ExecuteNonQuery();

                        }
                        else
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO ClientHoldDate (Client_ID, HoldFromDate , FileName, CreatedOn, CreatedBy) Values(@ClientId, @HoldFromDate, @FileName, @CreatedOn, @CreatedBy)");
                            cmd.Connection = conn;
                            cmd.Parameters.Add(pClientId);
                            cmd.Parameters.Add(pHoldFromDate);
                            cmd.Parameters.Add(pFileName);
                            cmd.Parameters.Add(pCreatedOn);
                            cmd.Parameters.Add(pCreatedBy);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    conn.Close();
                }
            }

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
        }

    }
}
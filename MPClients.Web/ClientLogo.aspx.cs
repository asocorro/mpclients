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

namespace MPClients.Web
{
    public partial class ClientLogo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int _ClientLogoId_index = 0;
            int _ClientId_index = 1;
            int _FileName_index = 2;
            int _MimeType_index = 4;
            int _FileContent_index = 6;

            if (Request.QueryString["ClientId"] != null)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

                SqlCommand command = new SqlCommand("spGetClientLogo", conn);
                command.Parameters.AddWithValue("@ClientId", Request.QueryString["ClientId"]);
                command.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    Byte[] bytes = (Byte[])reader[_FileContent_index];
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = reader[_MimeType_index].ToString();
                    Response.AddHeader("content-disposition", "attachment;filename=" + reader[_FileName_index].ToString());
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
   
                }
                reader.Close();
                conn.Close();
            }
        }
    }
}
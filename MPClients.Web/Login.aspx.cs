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
using MPClients.DataAccess.NHibernate;
using NHibernate.Expression;
using NHibernate;
using MPClients.DataAccess.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MPClients
{
    public partial class Login : MPClients.PageControllers.BasePage
    {

        protected override void PageLoad()
        {
            //adolfo/adolfo1
            //MembershipUser CurrentUser = Membership.GetUser("adolfo");
            //CurrentUser.UnlockUser();
            //string password = Membership.Provider.ResetPassword("adolfo", "test");
            //CurrentUser.ChangePassword(password, "adolfo1");
            ////mascaro/porter123$

            Page.Form.DefaultFocus = uiLogin.FindControl("Username").ClientID;
        }

        protected override void OnInit(EventArgs e)
        {
            uiLogin.LoggingIn += new LoginCancelEventHandler(uiLogin_LoggingIn);
            uiLogin.LoggedIn += new EventHandler(uiLogin_LoggedIn);
            uiLogin.LoginError += new EventHandler(uiLogin_LoginError);
       
            base.OnInit(e);
        }

        void uiLogin_LoginError(object sender, EventArgs e)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString()))
            {
                sqlConn.Open();
                string sqlcommand = String.Format(@"select failedpasswordattemptcount 
                from aspnet_Membership left join aspnet_Users ON aspnet_Membership.UserId = aspnet_Users.UserId
                where aspnet_Membership.ApplicationId = 'D7EAB50C-F113-4A93-947D-0F05F7B09163' AND UserName = '{0}'", uiLogin.UserName);
                using (SqlCommand cmd = new SqlCommand(sqlcommand, sqlConn))
                {
                    object value = cmd.ExecuteScalar();
                    if (value != null)
                    {
                        uxAlert.InnerHtml = String.Format("<p><strong>Intento #{0}, Después de 5 intentos erróneos perderá acceso a la página</strong> </p>", value);
                        uxAlert.Visible = true;
                    }

                }
                sqlConn.Close();
            }

        }

        void uiLogin_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            // Diagnostic: capture request context and username to help trace ArgumentNullException from enum parsing
            try
            {
                string basePath = System.Web.Hosting.HostingEnvironment.MapPath("~") ?? AppDomain.CurrentDomain.BaseDirectory;
                string logDir = System.IO.Path.Combine(basePath, "App_Data", "Logs");
                System.IO.Directory.CreateDirectory(logDir);
                string path = System.IO.Path.Combine(logDir, "login_request_context.log");
                var sb = new System.Text.StringBuilder();
                sb.AppendLine(DateTime.UtcNow.ToString("o") + " | THREAD=" + System.Threading.Thread.CurrentThread.ManagedThreadId + " | ACTION=Login.LoggingIn");
                try { sb.AppendLine("UserName=" + (uiLogin.UserName ?? "<null>")); } catch { }
                try { sb.AppendLine("RawUrl=" + (HttpContext.Current?.Request?.RawUrl ?? "<null>")); } catch { }
                try { sb.AppendLine("QueryString:"); foreach (string k in HttpContext.Current.Request.QueryString) sb.AppendLine("  " + k + "=" + HttpContext.Current.Request.QueryString[k]); } catch { }
                try { sb.AppendLine("Form:"); foreach (string k in HttpContext.Current.Request.Form) sb.AppendLine("  " + k + "=" + HttpContext.Current.Request.Form[k]); } catch { }
                try { sb.AppendLine("Headers:"); foreach (string k in HttpContext.Current.Request.Headers) sb.AppendLine("  " + k + "=" + HttpContext.Current.Request.Headers[k]); } catch { }
                try { sb.AppendLine("Cookies:"); foreach (string k in HttpContext.Current.Request.Cookies) sb.AppendLine("  " + k + "=" + HttpContext.Current.Request.Cookies[k]?.Value); } catch { }
                try { System.IO.File.AppendAllText(path, sb.ToString() + System.Environment.NewLine); } catch { }
            }
            catch { }

            try
            {
                ICriterion expression = Expression.Eq("UserName", uiLogin.UserName);
                ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MembershipUsers)).Add(expression);
                IList<MPClients.DataAccess.Domain.MembershipUsers> users = criteria.List<MembershipUsers>();

                if (users.Count > 0)
                {
                    ISession isolated = null;
                    Client client = null;
                    bool isInHold = false;
                    try
                    {
                        isolated = UnitOfWork.GetIsolatedSession();
                        client = isolated.Load<Client>(users[0].ClientID);
                        isInHold = client != null && client.Active.HasValue ? !client.Active.Value : false;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            string basePath = System.Web.Hosting.HostingEnvironment.MapPath("~") ?? AppDomain.CurrentDomain.BaseDirectory;
                            string logDir = System.IO.Path.Combine(basePath, "App_Data", "Logs");
                            System.IO.Directory.CreateDirectory(logDir);
                            string path = System.IO.Path.Combine(logDir, "nhibernate_proxy_exceptions.log");
                            string req = "";
                            try { if (HttpContext.Current != null && HttpContext.Current.Request != null) req = " | URL=" + HttpContext.Current.Request.RawUrl; } catch { }
                            string line = DateTime.UtcNow.ToString("o") + " | THREAD=" + System.Threading.Thread.CurrentThread.ManagedThreadId + req + " | ACTION=GetIsolatedSession.Load | TYPE=Client | ID=" + users[0].ClientID + " | EX=" + ex.ToString() + System.Environment.NewLine;
                            System.IO.File.AppendAllText(path, line);
                        }
                        catch { }
                        finally
                        {
                            try { if (isolated != null) isolated.Close(); } catch { }
                        }
                        throw;
                    }
                    finally
                    {
                        try { if (isolated != null) isolated.Close(); } catch { }
                    }

                    if (isInHold)
                    {
                        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString()))
                        {
                            sqlConn.Open();
                            using (SqlCommand cmd = new SqlCommand(String.Format("Select HoldFromDate from ClientHoldDate where Client_id='{0}'", users[0].ClientID), sqlConn))
                            {
                                object date = cmd.ExecuteScalar();
                                if (date == null)
                                {
                                    uxAlert.InnerHtml = "<p><strong>Su cuenta ha sido suspendida, contacte al Administrador del sistema al 787-782-4121.</strong> </p>";
                                    uxAlert.Visible = true;
                                    e.Cancel = isInHold;
                                }
                                else
                                {
                                    DateTime holdDate;
                                    if (DateTime.TryParse(date.ToString(), out holdDate))
                                    {
                                        if (holdDate <= DateTime.Now)
                                        {
                                            uxAlert.InnerHtml = "<p><strong>Su cuenta ha sido suspendida, contacte al Administrador del sistema al 787-782-4121.</strong> </p>";
                                            uxAlert.Visible = true;
                                            e.Cancel = isInHold;
                                        }
                                    }
                                    else
                                    {
                                        uxAlert.InnerHtml = "<p><strong>Su cuenta ha sido suspendida, contacte al Administrador del sistema al 787-782-4121.</strong> </p>";
                                        uxAlert.Visible = true;
                                        e.Cancel = isInHold;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string basePath = System.Web.Hosting.HostingEnvironment.MapPath("~") ?? AppDomain.CurrentDomain.BaseDirectory;
                    string logDir = System.IO.Path.Combine(basePath, "App_Data", "Logs");
                    System.IO.Directory.CreateDirectory(logDir);
                    string path = System.IO.Path.Combine(logDir, "login_error_context.log");
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine(DateTime.UtcNow.ToString("o") + " | THREAD=" + System.Threading.Thread.CurrentThread.ManagedThreadId + " | ACTION=Login.LoggingIn.Exception");
                    try { sb.AppendLine("EX=" + ex.ToString()); } catch { }
                    try { sb.AppendLine("UserName=" + (uiLogin.UserName ?? "<null>")); } catch { }
                    try { sb.AppendLine("RawUrl=" + (HttpContext.Current?.Request?.RawUrl ?? "<null>")); } catch { }
                    try
                    {
                        sb.AppendLine("--- Session keys ---");
                        var sess = HttpContext.Current?.Session;
                        if (sess != null)
                        {
                            foreach (string k in sess.Keys)
                            {
                                try { sb.AppendLine(k + "=" + (sess[k] != null ? sess[k].ToString() : "<null>")); } catch { sb.AppendLine(k + "=<unreadable>"); }
                            }
                        }
                    }
                    catch { }
                    try
                    {
                        sb.AppendLine("--- ViewState keys ---");
                        foreach (string k in ViewState.Keys)
                        {
                            try { sb.AppendLine(k + "=" + (ViewState[k] != null ? ViewState[k].ToString() : "<null>")); } catch { sb.AppendLine(k + "=<unreadable>"); }
                        }
                    }
                    catch { }
                    try { System.IO.File.AppendAllText(path, sb.ToString() + System.Environment.NewLine); } catch { }
                }
                catch { }
                throw;
            }
        }

        void uiLogin_LoggedIn(object sender, EventArgs e)
        {
            MembershipUser CurrentUser = Membership.GetUser(uiLogin.UserName);

            MPClients.DataAccess.Domain.MembershipUsers membershipUser = null;
            ICriterion expression = Expression.Eq("UserName", uiLogin.UserName);
            ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MembershipUsers)).Add(expression);
            IList<MPClients.DataAccess.Domain.MembershipUsers> users = criteria.List<MembershipUsers>();

            HttpCookie cookie = new HttpCookie("Preferences");
            cookie["Id"] = CurrentUser.ProviderUserKey.ToString();
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);
            
            string connString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT [ProductUpdated] FROM [Configuration]", sqlConn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Session["ProductUpdateInfo"] = reader.GetDateTime(0).ToShortDateString() + " " + reader.GetDateTime(0).ToLongTimeString();
                    }
                    reader.Close();
                }
                sqlConn.Close();
            }

          

            if (users.Count > 0)
            {
                membershipUser = users[0];

                string newClientid = users[0].ClientID;
                string newClientName = users[0].ClientName;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("select top 1  T1.OnBehalfOf, T2.Name FROM Orders T1 LEFT JOIN dbo.Client T2 ON T1.OnBehalfOf = T2.Client_ID where T1.UserId = '" + users[0].ID + "'", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            newClientid = reader.IsDBNull(0) ? newClientid : reader.GetString(0);
                            newClientName = reader.IsDBNull(1) ? newClientName : reader.GetString(1);

                        }
                        reader.Close();
                    }
                    conn.Close();
                }

                MPClients.PageControllers.BasePage.UpdateCurrentClientId(users[0].ID.ToString(), newClientid);
                MPClients.PageControllers.BasePage.UpdateCurrentClientName(users[0].ID.ToString(), newClientName);

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("select OrderId FROM ORDERS WHERE Status=2 AND UserId='" + users[0].ID + "'", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MPClients.PageControllers.BasePage.UpdateCurrentShopping(reader.GetGuid(0), users[0].ID.ToString());
                        }
                        else
                        {
                            MPClients.PageControllers.BasePage.CleanCurrentShopping(users[0].ID.ToString());
                        }
                        reader.Close();
                    }
                    conn.Close();
                }


                if (membershipUser.ChangePassword)
                {
                    Response.Redirect("ChangePassword.aspx");
                }
            }

            Response.Redirect("Default.aspx");
        }


    }
}

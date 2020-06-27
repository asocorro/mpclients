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

namespace MPClients
{
    public partial class PasswordRetrieval : MPClients.PageControllers.BasePage
    {

        protected override void PageLoad()
        {

        }

        protected override void OnInit(EventArgs e)
        {
            btnSend.Click += new EventHandler(btnSend_Click);
            base.OnInit(e);
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    Utils utils = new Utils();

            //    SqlDataReader dr = utils.GetUserByEmail(txbEmail.Text);

            //    if (dr.HasRows)
            //    {

            //        dr.Read();


            //        StringBuilder sbBody = new StringBuilder();

            //        sbBody.Append("Your username is: \r\n");

            //        sbBody.Append(dr["userName"].ToString());

            //        sbBody.Append(" \r\n");

            //        sbBody.Append(" \r\n");

            //        sbBody.Append("Your password is:  \r\n");

            //        sbBody.Append(dr["password"].ToString());

            //        sbBody.Append(" \r\n");

            //        sbBody.Append(" \n\r");

            //        sbBody.Append("Please let us know if this does not work for you. \n\r");


            //        MailMessage message = new MailMessage();

            //        message.From = new MailAddress("From<From@MyDomain.com>");

            //        message.To.Add("User<" + txbEmail.Text + ">");

            //        message.Bcc.Add("Me<me@mydomain.com>");

            //        message.Subject = "Your Login Information";

            //        message.IsBodyHtml = false;

            //        message.Body = sbBody.ToString();


            //        SmtpClient mailClient = new SmtpClient("mail.MyDomain.com");

            //        mailClient.Credentials = new System.Net.NetworkCredential("MyUserName", "MyPassword");

            //        mailClient.Send(message);


            //        litMessage.Text = "Mail Sent Successfully";

            //    }

            //    else
            //    {

            //        litMessage.Text = "This email was not found. Please try again";

            //    }

            //    dr.Close();

            //}

            //catch (Exception ex)
            //{

            //    litMessage.Text += "Error getting email information:" + ex.Message;

            //}
        }
    }
}

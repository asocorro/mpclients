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
using System.Text;
using System.Web.Mail;

namespace MPClients
{
    public partial class Contactus : MPClients.PageControllers.BasePage
    {

        protected override void PageLoad()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            //uxSubmit.ServerClick += new EventHandler(uxSubmit_ServerClick);
            base.OnInit(e);
        }

        //void uxSubmit_ServerClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SmtpMail.SmtpServer = MPClients.Web.Properties.Settings.Default.SMTPServer;
        //        string notifyEmail = MPClients.Web.Properties.Settings.Default.SMTPSendUserName;
        //        string tomail = MPClients.Web.Properties.Settings.Default.MailTo;
        //        string subject = string.Format("Contact Us message from {0}", this.uxName.Text);
        //        string body = "The following message was received at www.mpclients.com: <br><br>";
        //        body  += string.Format("From: {0} <br>", this.uxName.Text) ;

        //        body = body + string.Format("Email: {0} <br>", this.uxEmail.Text) + string.Format("Message: {0}", this.uComments.Value );
        //        MailMessage message = new MailMessage();
        //        message.To = tomail;
        //        message.From = notifyEmail;
        //        message.Subject = subject;
        //        message.Body = body;
        //        message.BodyFormat = MailFormat.Html;
        //        SmtpMail.SmtpServer = MPClients.Web.Properties.Settings.Default.SMTPServer;
        //        SmtpMail.Send(message);
        //        base.Response.Redirect("SendMessageConfirmation.aspx");
        //    }
        //    catch 
        //    {
               
        //    }
        //}
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NHibernate.Expression;
using NHibernate;
using System.Collections.Generic;
using MPClients.DataAccess.NHibernate;
using System.Web.Mail;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

namespace MPClients.Common
{
    public class Helper
    {
        internal const string ADMINISTRATOR_ROLE = "Administrator";
        internal const string SEARCH_ONLY_ROLE = "SearchOnly";
        internal const string NEWS_EDITOR_ROLE = "NewsEditor";

        internal static void LoadClients(Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e, global::Telerik.Web.UI.RadComboBox radComboBox)
        {
            if (!String.IsNullOrEmpty(e.Text) && e.Text.Length > 3)
            {
                try
                {
                    ICriterion expression =
                        Expression.Or(
                        Expression.Like("ID", e.Text, MatchMode.Start)
                        , Expression.Like("Name", e.Text, MatchMode.Start));
                    ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MPClients.DataAccess.Domain.Client)).Add(expression);
                    criteria.AddOrder(new Order("Name", true));
                    IList<MPClients.DataAccess.Domain.Client> allClients = criteria.List<MPClients.DataAccess.Domain.Client>();

                    int itemsPerRequest = 10;
                    int itemOffset = e.NumberOfItems;
                    int endOffset = itemOffset + itemsPerRequest;

                    if (endOffset > allClients.Count)
                    {
                        endOffset = allClients.Count;
                    }

                    foreach (MPClients.DataAccess.Domain.Client client in allClients)
                    {
                        radComboBox.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(
                            client.ID + "-" + client.Name,
                            client.ID));
                        radComboBox.SelectedIndex = 0;

                    }

                    if (allClients.Count > 0)
                    {
                        e.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset, allClients.Count);
                    }
                    else
                    {
                        e.Message = "No matches";
                    }
                }
                catch
                {
                    e.Message = "No matches";
                }
            }
        }

        internal static void LoadCategories(Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e, global::Telerik.Web.UI.RadComboBox radComboBox)
        {
            if (!String.IsNullOrEmpty(e.Text) && e.Text.Length > 1)
            {
                try
                {
                    ICriterion expression =
                        Expression.Or(
                        Expression.Like("ID", e.Text, MatchMode.Start)
                        , Expression.Like("Description", e.Text, MatchMode.Start));
                    ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MPClients.DataAccess.Domain.Category)).Add(expression);
                    criteria.AddOrder(new Order("ID", true));
                    IList<MPClients.DataAccess.Domain.Category> allCategories = criteria.List<MPClients.DataAccess.Domain.Category>();

                    int itemsPerRequest = 10;
                    int itemOffset = e.NumberOfItems;
                    int endOffset = itemOffset + itemsPerRequest;

                    if (endOffset > allCategories.Count)
                    {
                        endOffset = allCategories.Count;
                    }

                    foreach (MPClients.DataAccess.Domain.Category cat in allCategories)
                    {
                        radComboBox.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(
                            cat.ID.Trim() + " - " + cat.Description,
                            cat.ID));
                        radComboBox.SelectedIndex = 0;
                    }

                    if (allCategories.Count > 0)
                    {
                        e.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset, allCategories.Count);
                    }
                    else
                    {
                        e.Message = "No matches";
                    }
                }
                catch
                {
                    e.Message = "No matches";
                }
            }
        }


        internal static string NormalizeText(string inputText)
        {
            string value = HttpUtility.HtmlEncode(inputText);
            value = value.Replace("-", "");
            value = value.Replace("/", "");
            value = value.Replace(".", "");
            value = value.Replace("'", "");
            value = value.Replace("\"", "");
            value = value.Replace("á","a");
            value = value.Replace("é","e");
            value = value.Replace("í","i");
            value = value.Replace("ó","o");
            value = value.Replace("ú","u");
            value = value.Replace("ñ", "n");
            // reemplazar múltiples espacios por uno sólo
            value = Regex.Replace(value, @"\s+", " ");
            value = value.Trim();
            return value;
        }

        internal static void SendEmail(string body, string subject)
        {
            System.Web.Mail.MailMessage eMail = new System.Web.Mail.MailMessage();

            eMail.Fields["http://schemas.microsoft.com/cdo/configuration/smtsperver"]
                = MPClients.Web.Properties.Settings.Default.SMTPServer;
            eMail.Fields[
                "http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = MPClients.Web.Properties.Settings.Default.SMTPPort;
            eMail.Fields[
                "http://schemas.microsoft.com/cdo/configuration/sendusing"] = MPClients.Web.Properties.Settings.Default.SMTPSendUsing;

            //sendusing: cdoSendUsingPort, value 2, for sending the message using 
            //the network.
            //smtpauthenticate: Specifies the mechanism used when authenticating 
            //to an SMTP 
            //service over the network. Possible values are:
            //- cdoAnonymous, value 0. Do not authenticate.
            //- cdoBasic, value 1. Use basic clear-text authentication. 
            //When using this option you have to provide the user name and password 
            //through the sendusername and sendpassword fields.
            //- cdoNTLM, value 2. The current process security context is used to 
            // authenticate with the service.
            eMail.Fields[
                    "http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
            eMail.Fields[
                "http://schemas.microsoft.com/cdo/configuration/sendusername"] =
                MPClients.Web.Properties.Settings.Default.SMTPSendUserName;
            eMail.Fields[
                "http://schemas.microsoft.com/cdo/configuration/sendpassword"] =
                MPClients.Web.Properties.Settings.Default.SMTPSendPassword;
            eMail.Fields.Add
               ("http://schemas.microsoft.com/cdo/configuration/smtpusessl",
                    "true");

            eMail.From = MPClients.Web.Properties.Settings.Default.SMTPSendUserName;
            eMail.To = MPClients.Web.Properties.Settings.Default.MailTo;
            eMail.Subject = subject;
            eMail.BodyFormat = MailFormat.Html;

            eMail.Body = body.ToString();

            System.Web.Mail.SmtpMail.SmtpServer = MPClients.Web.Properties.Settings.Default.SMTPServer;
            System.Web.Mail.SmtpMail.Send(eMail);
        }

        internal static void SendMail()
        {
        

        }

        //using System.Net.Mail;
        //using System.Net.Mime;
        //using System.Web.Mail;

        //public static void SendMailTo(string sTo, string sNotify, string sFrom, string sSubject, string sBody, string sOriginatingProcedure)
        //{
        //    string sEmailServer;
        //    int iEmailServerPort;
        //    string sEmailServerUsername;
        //    string sEmailServerPassword;

        //    string sStyle = "<style>td, tr, table {font-family:calibri; font-size:11pt}</style>";

        //    string IsTestConfig = ConfigurationManager.AppSettings["IsTestconfig"];
        //    if (IsTestConfig == "1")
        //    {
        //        //sBody = "-- THIS IS A TEST --" + CommonLibConstants.CrLf
        //        //    + "Original sender: " + sFrom.ToString() + CommonLibConstants.CrLf
        //        //    + "Original recipients: " + sTo.ToString() + CommonLibConstants.CrLf
        //        //    + "Original CC: " + sNotify.ToString() + CommonLibConstants.CrLf + CommonLibConstants.CrLf
        //        //    + "[" + sOriginatingProcedure +"]" + CommonLibConstants.CrLf
        //        //    + "-- END OF TEST SECTION --" + CommonLibConstants.CrLf + CommonLibConstants.CrLf
        //        //    + sBody;

        //        sBody =
        //            sStyle
        //            + sBody
        //            + "<br>"
        //            + "<table>"
        //            + "<tr><td>"
        //                + "-- TEST INFORMATION --"
        //            + "</tr></td>"
        //            + "<tr><td>"
        //                + "Original sender: " + sFrom.ToString()
        //            + "</tr></td>"
        //            + "<tr><td>"
        //                + "Original recipients: " + sTo.ToString()
        //            + "</tr></td>"
        //            + "<tr><td>"
        //                + "Original CC: " + sNotify.ToString()
        //            + "</tr></td>"
        //            + "<tr><td>"
        //                + "[" + sOriginatingProcedure + "]"
        //            + "</tr></td>"
        //            + "<tr><td>"
        //                + "-- END OF TEST INFORMATION --"
        //            + "</tr></td>"
        //            + "</table>"
        //            ;

        //        sFrom = ConfigurationManager.AppSettings["TestEmailSender"];
        //        sTo = ConfigurationManager.AppSettings["TestTargetEmail"];
        //        sNotify = string.Empty;
        //        sEmailServer = ConfigurationManager.AppSettings["TestEmailServer"];
        //        iEmailServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["TestEmailServerPort"]);
        //        sEmailServerUsername = ConfigurationManager.AppSettings["TestEmailServerUsername"];
        //        sEmailServerPassword = ConfigurationManager.AppSettings["TestEmailServerPassword"];
        //    }
        //    else
        //    {
        //        if (String.IsNullOrWhiteSpace(sFrom))
        //            sFrom = ConfigurationManager.AppSettings["ProductionEmailSender"];
        //        sEmailServer = ConfigurationManager.AppSettings["ProductionEmailServer"];
        //        iEmailServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ProductionEmailServerPort"]);
        //        sEmailServerUsername = ConfigurationManager.AppSettings["ProductionEmailServerUsername"];
        //        sEmailServerPassword = ConfigurationManager.AppSettings["ProductionEmailServerPassword"];
        //        sBody = sStyle + sBody;
        //    }

        //    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        //    message.From = new System.Net.Mail.MailAddress(sFrom);
        //    message.To.Add(sTo);
        //    message.Subject = sSubject;
        //    message.Body = sBody;
        //    message.IsBodyHtml = true;
        //    if (!String.IsNullOrWhiteSpace(sNotify))
        //        message.Bcc.Add(sNotify);

        //    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(sEmailServer, iEmailServerPort);
        //    System.Net.NetworkCredential netwrkCrd = new System.Net.NetworkCredential();
        //    smtp.UseDefaultCredentials = false;
        //    netwrkCrd.UserName = sEmailServerUsername;
        //    netwrkCrd.Password = sEmailServerPassword;
        //    smtp.Credentials = netwrkCrd;
        //    smtp.Send(message);
        //}

        internal static IList<MPClients.DataAccess.Domain.UserCategory> GetUserCategories(string userID)
        {
            ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MPClients.DataAccess.Domain.UserCategory));
            criteria.Add(NHibernate.Expression.Expression.Eq("UserId", new Guid(userID)));
            IList<MPClients.DataAccess.Domain.UserCategory> categories = criteria.List<MPClients.DataAccess.Domain.UserCategory>();

            return categories;
        }

        public static byte[] CreateThumbnail(byte[] PassedImage, int LargestSide)
        {
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(),
                                NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }

        // Resize a Bitmap  
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        } 
    }
}

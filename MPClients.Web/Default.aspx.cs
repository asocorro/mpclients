using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NHibernate;
using NHibernate.Expression;
using MPClients.DataAccess.NHibernate;
using MPClients.PageControllers;

namespace MPClients
{
    public partial class Default : BasePage
    {
        private const string PasswordAnswer = "test";
        private const string ExportTriggerValue = "YES";
        private const string ExportFileName = "membership-password-export-plain.txt";

        protected override void PageLoad()
        {
            BindDataGrid();

            /*
             * TEMPORARY LOCAL PASSWORD EXPORT
             *
             * To run the export locally, browse to:
             *   Default.aspx?exportPasswords=YES
             *
             * Output file:
             *   App_Data\membership-password-export-plain.txt
             *
             * Remove this block and the helper methods immediately after the export.
             */
            if (ShouldExportPasswords())
            {
                PasswordExportResult result = ExportMembershipPasswordsToPlainTextFile();

                lblPassword.Text =
                    "Password export completed. " +
                    "Successful exports: " + result.SuccessCount +
                    "; Failed exports: " + result.FailureCount +
                    "; Total records: " + result.TotalRecords +
                    "; File: " + Server.HtmlEncode(result.ExportFilePath);

                return;
            }

            lblPassword.Text = "Password export is not running. Add ?exportPasswords=YES to the local URL to run it.";
        }

        protected override void OnInit(EventArgs e)
        {
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            base.OnInit(e);
        }

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindDataGrid();
        }

        private bool ShouldExportPasswords()
        {
            string requestedValue = Request.QueryString["exportPasswords"];

            return string.Equals(
                requestedValue,
                ExportTriggerValue,
                StringComparison.OrdinalIgnoreCase);
        }

        private PasswordExportResult ExportMembershipPasswordsToPlainTextFile()
        {
            string exportFilePath = Server.MapPath("~/App_Data/" + ExportFileName);

            int pageIndex = 0;
            int pageSize = 100;
            int totalRecords;

            int successCount = 0;
            int failureCount = 0;

            List<string> outputLines = new List<string>();

            outputLines.Add("Username\tPassword");
            outputLines.Add("");

            do
            {
                MembershipUserCollection users =
                    Membership.GetAllUsers(pageIndex, pageSize, out totalRecords);

                foreach (MembershipUser user in users)
                {
                    try
                    {
                        string username = user.UserName;
                        string plainPassword = user.GetPassword(PasswordAnswer);

                        outputLines.Add(username + "\t" + plainPassword);

                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        failureCount++;

                        outputLines.Add(
                            user.UserName +
                            "\t" +
                            "ERROR: " + ex.Message);
                    }
                }

                pageIndex++;

            } while (pageIndex * pageSize < totalRecords);

            outputLines.Add("");
            outputLines.Add("Summary");
            outputLines.Add("Successful exports: " + successCount);
            outputLines.Add("Failed exports: " + failureCount);
            outputLines.Add("Total records: " + totalRecords);
            outputLines.Add("Created UTC: " + DateTime.UtcNow.ToString("o"));

            File.WriteAllLines(exportFilePath, outputLines.ToArray(), Encoding.UTF8);

            return new PasswordExportResult
            {
                ExportFilePath = exportFilePath,
                SuccessCount = successCount,
                FailureCount = failureCount,
                TotalRecords = totalRecords
            };
        }

        private void BindDataGrid()
        {
            ISession session = UnitOfWork.GetIsolatedSession();
            //ICriterion expression = Expression.Ge("FromDate", new DateTime(2000, 1, 1));
            ICriterion expression = Expression.And(Expression.Le("FromDate", DateTime.Now), Expression.Ge("ToDate", DateTime.Now));

            ICriteria criteria = UnitOfWork.GetIsolatedSession().
                    CreateCriteria(typeof(MPClients.DataAccess.Domain.News)).Add(expression);
            criteria.AddOrder(new Order("FromDate", false));
            uxGrid.DataSource = criteria.List<MPClients.DataAccess.Domain.News>();
            uxGrid.DataBind();
        }

        private class PasswordExportResult
        {
            public string ExportFilePath { get; set; }
            public int SuccessCount { get; set; }
            public int FailureCount { get; set; }
            public int TotalRecords { get; set; }
        }
    }
}

<%@ Page Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.Security" %>

<!DOCTYPE html>

<script runat="server">

    private const string Token = "migrate-9F3D7B21-local-only";
    private const string ApplicationName = "/MPClients";
    private const string SharedQuestion = "test";
    private const string SharedAnswer = "test";
    private const string ExportFilePath = "~/App_Data/membership-password-export-plain.txt";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["token"] != Token)
        {
            Response.StatusCode = 403;
            lblResult.Text = "Forbidden - bad or missing token.";
            return;
        }

        bool applyChanges =
            string.Equals(Request.QueryString["apply"], "YES", StringComparison.OrdinalIgnoreCase);

        try
        {
            ImportPasswords(applyChanges);
        }
        catch (Exception ex)
        {
            lblResult.Text =
                "<h2>ERROR</h2><pre>" +
                Server.HtmlEncode(ex.ToString()) +
                "</pre>";
        }
    }

    private void ImportPasswords(bool applyChanges)
    {
        string physicalPath = Server.MapPath(ExportFilePath);

        if (!File.Exists(physicalPath))
        {
            throw new FileNotFoundException("Export file not found.", physicalPath);
        }

        string connectionString =
            ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        MethodInfo encodePasswordMethod = GetEncodePasswordMethod();

        List<string> lines = new List<string>();

        lines.Add("<h2>Membership Password Import</h2>");
        lines.Add("<p><strong>Mode:</strong> " + (applyChanges ? "APPLY CHANGES" : "DRY RUN") + "</p>");
        lines.Add("<p><strong>Application:</strong> " + Server.HtmlEncode(ApplicationName) + "</p>");
        lines.Add("<p><strong>File:</strong> " + Server.HtmlEncode(physicalPath) + "</p>");

        string only = Request.QueryString["only"];

        if (string.IsNullOrWhiteSpace(only))
        {
            lines.Add("<p><strong>User filter:</strong> none - all users in the export file will be processed.</p>");
        }
        else
        {
            lines.Add("<p><strong>User filter:</strong> " + Server.HtmlEncode(only) + "</p>");
        }

        lines.Add("<hr />");
        lines.Add("<pre>");

        int successCount = 0;
        int failureCount = 0;
        int skippedCount = 0;
        int lineNumber = 0;

        string[] fileLines = File.ReadAllLines(physicalPath, Encoding.UTF8);

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            foreach (string rawLine in fileLines)
            {
                lineNumber++;

                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    skippedCount++;
                    continue;
                }

                if (rawLine.StartsWith("Username\tPassword", StringComparison.OrdinalIgnoreCase))
                {
                    skippedCount++;
                    continue;
                }

                if (rawLine.StartsWith("Summary", StringComparison.OrdinalIgnoreCase) ||
                    rawLine.StartsWith("Successful exports:", StringComparison.OrdinalIgnoreCase) ||
                    rawLine.StartsWith("Failed exports:", StringComparison.OrdinalIgnoreCase) ||
                    rawLine.StartsWith("Total records:", StringComparison.OrdinalIgnoreCase) ||
                    rawLine.StartsWith("Created UTC:", StringComparison.OrdinalIgnoreCase))
                {
                    skippedCount++;
                    continue;
                }

                int tabIndex = rawLine.IndexOf('\t');

                if (tabIndex <= 0)
                {
                    failureCount++;
                    lines.Add("Line " + lineNumber + ": FAILED - no tab separator found.");
                    continue;
                }

                string username = rawLine.Substring(0, tabIndex);
                string plainPassword = rawLine.Substring(tabIndex + 1);

                if (!ShouldProcessUser(username))
                {
                    skippedCount++;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(username))
                {
                    failureCount++;
                    lines.Add("Line " + lineNumber + ": FAILED - blank username.");
                    continue;
                }

                if (string.IsNullOrEmpty(plainPassword))
                {
                    failureCount++;
                    lines.Add("Line " + lineNumber + ": FAILED - blank password for " + username + ".");
                    continue;
                }

                try
                {
                    bool userExists = UserExists(conn, username);

                    if (!userExists)
                    {
                        failureCount++;
                        lines.Add(username + ": FAILED - user not found in application " + ApplicationName + ".");
                        continue;
                    }

                    string newSalt = GenerateSalt();

                    string encodedPassword = EncodePassword(
                        encodePasswordMethod,
                        plainPassword,
                        newSalt
                    );

                    string encodedAnswer = EncodePassword(
                        encodePasswordMethod,
                        SharedAnswer.ToLowerInvariant(),
                        newSalt
                    );

                    if (applyChanges)
                    {
                        UpdateMembershipPassword(
                            conn,
                            username,
                            encodedPassword,
                            encodedAnswer,
                            newSalt
                        );
                    }

                    successCount++;
                    lines.Add(username + ": OK" + (applyChanges ? " - updated" : " - dry run only"));
                }
                catch (Exception ex)
                {
                    failureCount++;
                    lines.Add(username + ": FAILED - " + ex.Message);
                }
            }
        }

        lines.Add("");
        lines.Add("Summary");
        lines.Add("-------");
        lines.Add("Successful: " + successCount);
        lines.Add("Failed:     " + failureCount);
        lines.Add("Skipped:    " + skippedCount);
        lines.Add("Applied:    " + applyChanges);
        lines.Add("</pre>");

        if (!applyChanges)
        {
            lines.Add("<p><strong>No database changes were made.</strong></p>");
            lines.Add("<p>To apply changes for the same users, add <code>&amp;apply=YES</code> to the URL.</p>");
            lines.Add("<p>Example for selected users:</p>");
            lines.Add("<pre>ImportMembershipPasswords.aspx?token=" + Token + "&amp;only=adolfo,john&amp;apply=YES</pre>");
        }
        else
        {
            lines.Add("<p><strong>Database changes were applied.</strong></p>");
            lines.Add("<p>Now test login for the updated user or users.</p>");
        }

        lblResult.Text = string.Join(Environment.NewLine, lines.ToArray());
    }

    private bool ShouldProcessUser(string username)
    {
        string only = Request.QueryString["only"];

        if (string.IsNullOrWhiteSpace(only))
        {
            return true;
        }

        string[] usernames = only.Split(',');

        foreach (string item in usernames)
        {
            if (string.Equals(
                item.Trim(),
                username,
                StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private MethodInfo GetEncodePasswordMethod()
    {
        Type providerType = Membership.Provider.GetType();

        MethodInfo method = providerType.GetMethod(
            "EncodePassword",
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { typeof(string), typeof(int), typeof(string) },
            null
        );

        if (method == null)
        {
            throw new Exception(
                "Could not find SqlMembershipProvider.EncodePassword(string, int, string). " +
                "Provider type is: " + providerType.FullName
            );
        }

        return method;
    }

    private string EncodePassword(MethodInfo encodePasswordMethod, string plainText, string salt)
    {
        /*
         * PasswordFormat:
         * 0 = Clear
         * 1 = Hashed
         * 2 = Encrypted
         */
        object encoded = encodePasswordMethod.Invoke(
            Membership.Provider,
            new object[] { plainText, 2, salt }
        );

        return (string)encoded;
    }

    private string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];

        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }

    private bool UserExists(SqlConnection conn, string username)
    {
        using (SqlCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
SELECT COUNT(*)
FROM aspnet_Users u
JOIN aspnet_Applications a
    ON u.ApplicationId = a.ApplicationId
WHERE u.UserName = @UserName
  AND a.ApplicationName = @ApplicationName;
";

            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;
            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = ApplicationName;

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }
    }

    private void UpdateMembershipPassword(
        SqlConnection conn,
        string username,
        string encodedPassword,
        string encodedAnswer,
        string newSalt)
    {
        using (SqlCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
UPDATE m
SET
    m.Password = @Password,
    m.PasswordFormat = 2,
    m.PasswordSalt = @PasswordSalt,
    m.PasswordQuestion = @PasswordQuestion,
    m.PasswordAnswer = @PasswordAnswer,
    m.IsApproved = 1,
    m.IsLockedOut = 0,
    m.LastLockoutDate = @ResetDate,
    m.FailedPasswordAttemptCount = 0,
    m.FailedPasswordAttemptWindowStart = @ResetDate,
    m.FailedPasswordAnswerAttemptCount = 0,
    m.FailedPasswordAnswerAttemptWindowStart = @ResetDate,
    m.LastPasswordChangedDate = @NowUtc
FROM aspnet_Membership m
JOIN aspnet_Users u
    ON m.UserId = u.UserId
JOIN aspnet_Applications a
    ON u.ApplicationId = a.ApplicationId
WHERE u.UserName = @UserName
  AND a.ApplicationName = @ApplicationName;
";

            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 128).Value = encodedPassword;
            cmd.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 128).Value = newSalt;
            cmd.Parameters.Add("@PasswordQuestion", SqlDbType.NVarChar, 256).Value = SharedQuestion;
            cmd.Parameters.Add("@PasswordAnswer", SqlDbType.NVarChar, 128).Value = encodedAnswer;
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;
            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = ApplicationName;
            cmd.Parameters.Add("@ResetDate", SqlDbType.DateTime).Value = new DateTime(1754, 1, 1);
            cmd.Parameters.Add("@NowUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;

            int rows = cmd.ExecuteNonQuery();

            if (rows != 1)
            {
                throw new Exception("Expected to update 1 row, but updated " + rows + " rows.");
            }
        }
    }

</script>

<html>
<head runat="server">
    <title>Import Membership Passwords</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal ID="lblResult" runat="server" />
    </form>
</body>
</html>
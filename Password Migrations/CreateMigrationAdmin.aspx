<%@ Page Language="C#" ClassName="CreateMigrationAdminPage" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web.Security" %>

<!DOCTYPE html>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        string token = Request.QueryString["token"];

        if (token != "migrate-9F3D7B21-local-only")
        {
            Response.StatusCode = 403;
            Response.Write("Forbidden");
            Response.End();
            return;
        }

        CreateTemporaryAdmin();

        Response.Write("<h2>Temporary admin created or verified.</h2>");
        Response.Write("<p>You can now delete this page.</p>");
        Response.End();
    }

    private void CreateTemporaryAdmin()
    {
        string username = "migration_admin";
        string password = "ChangeThisTempPassword123!";
        string email = "migration_admin@example.com";
        string question = "test";
        string answer = "test";

        string adminRole = "Administrator"; // Change if your role has a different name

        MembershipUser user = Membership.GetUser(username);

        if (user == null)
        {
            MembershipCreateStatus status;

            user = Membership.CreateUser(
                username,
                password,
                email,
                question,
                answer,
                true,
                out status
            );

            if (status != MembershipCreateStatus.Success)
            {
                throw new Exception("Could not create user. Status: " + status);
            }
        }

        if (user.IsLockedOut)
        {
            user.UnlockUser();
        }

        user.IsApproved = true;
        Membership.UpdateUser(user);

        if (!Roles.RoleExists(adminRole))
        {
            throw new Exception("Admin role does not exist: " + adminRole);
        }

        if (!Roles.IsUserInRole(username, adminRole))
        {
            Roles.AddUserToRole(username, adminRole);
        }
    }

</script>

<html>
<head runat="server">
    <title>Create Migration Admin</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal ID="lblResult" runat="server" />
    </form>
</body>
</html>

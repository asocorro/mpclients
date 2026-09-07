<%@ Page Language="C#" ClassName="DeleteMigrationAdminPage" %>
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

        string username = "migration_admin";

        bool deleted = Membership.DeleteUser(username, true);

        Response.Write("<h2>Delete migration user</h2>");
        Response.Write("<p>User: " + username + "</p>");
        Response.Write("<p>Deleted: " + deleted + "</p>");
        Response.End();
    }

</script>

<html>
<head runat="server">
    <title>Delete Migration Admin</title>
</head>
<body>
    <form id="form1" runat="server"></form>
</body>
</html>
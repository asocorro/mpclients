<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Error.aspx.cs" Inherits="MPClients.Web.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <br />
    <table border="0" cellpadding="2" cellspacing="2" width="75%" class="bodycopy">
        <tr>
            <td>
                <img src="/images/warning.png" alt="Warning" />
            </td>
            <td>
                Wow, we have encountered an unexpected error. Please accept our apologies and notifiy
                us immediately about it so that we may investigate it further. In the meantime,
                try these steps:
                <ul>
                    <li>Go back to the page you were at and refresh it. Check if the problem persists.</li>
                    <li>If the problem has not gone away, try closing your browser and opening it again.</li>
                </ul>
            </td>
        </tr>
    </table>
</asp:Content>

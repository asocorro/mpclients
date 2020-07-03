<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Administration.aspx.cs" Inherits="MPClients.Web.Administration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" class="tdBody">
                <table style="border: 1px solid rgb(204, 204, 204);" border="0" cellpadding="0" cellspacing="0"
                    width="100%">
                    <tbody>
                        <tr>
                            <td class="mainheading" height="30">Administration
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" class="tdBody">
                <table border="0" cellpadding="4" cellspacing="4" width="100%" style="border: 1px solid rgb(204, 204, 204);">
                    <tr>
                        <td style="width: 60px">
                            <a href="LoadHoldsFile.aspx">
                                <asp:Image ImageUrl="~/Images/Upload.jpg" runat="server" />
                            </a>
                        </td>
                        <td class="bodycopy">
                            <span style="font-size: 13pt; font-weight: 600; color: Gray; font-family: segoe ui">Load Holds File</span>
                            <br />
                            <br />
                            Load a file with clients and hold dates.
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="4" cellspacing="4" width="100%" style="border: 1px solid rgb(204, 204, 204);">
                    <tr>
                        <td style="width: 60px">
                            <a href="Configuration.aspx">
                                <asp:Image ImageUrl="~/Images/Settings.png" runat="server" />
                            </a>
                        </td>
                        <td class="bodycopy">
                            <span style="font-size: 13pt; font-weight: 600; color: Gray; font-family: segoe ui">General Settings</span>
                            <br />
                            <br />
                            General configuration settings.
                        </td>
                    </tr>


                </table>
            </td>
        </tr>
    </table>
</asp:Content>

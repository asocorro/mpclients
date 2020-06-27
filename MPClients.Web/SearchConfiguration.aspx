<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SearchConfiguration.aspx.cs" Inherits="MPClients.Web.SearchConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" class="tdBody">
                <table style="border: 1px solid rgb(204, 204, 204);" border="0" cellpadding="0" cellspacing="0"
                    width="100%">
                    <tbody>
                        <tr>
                            <td class="mainheading" height="30">
                                Search Configuration
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
                            <a href="EditClientOrderingForResults.aspx">
                                <asp:Image ImageUrl="~/Images/ResultOrder.png" runat="server" />
                            </a>
                        </td>
                        <td class="bodycopy">
                            <span style="font-size: 13pt; font-weight: 600; color: Gray; font-family: segoe ui">
                                Results Order</span>
                            <br />
                            Establish the order in which clients appear when searching in MascaroPorter.com.
                        </td>
                    </tr>
                    <tr style="height: 10px">
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="AvailableCategories.aspx">
                                <asp:Image ID="Image1" ImageUrl="~/Images/AvailableCategories.png" runat="server" />
                            </a>
                        </td>
                        <td class="bodycopy">
                            <span style="font-size: 13pt; font-weight: 600; color: Gray; font-family: segoe ui">
                                Available Categories</span>
                            <br />
                            Indicate which product categories will be searchable in MascaroPorter.com.
                        </td>
                    </tr>

                     <tr style="height: 10px">
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="OtherSearchSettings.aspx">
                                <asp:Image ID="Image2" ImageUrl="~/Images/Settings.png" runat="server" />
                            </a>
                        </td>
                        <td class="bodycopy">
                            <span style="font-size: 13pt; font-weight: 600; color: Gray; font-family: 'segoe ui'">
                                Other Settings</span>
                            <br />
                            Manage other configuration settings.
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</asp:Content>

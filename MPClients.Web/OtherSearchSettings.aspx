<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OtherSearchSettings.aspx.cs"
    Inherits="MPClients.OtherSearchSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr valign="top">
                <td align="left" class="tdBody">
                    <table style="border: 1px solid rgb(204, 204, 204);" border="0" cellpadding="0" cellspacing="0"
                        width="100%">
                        <tbody>
                            <tr>
                                <td class="mainheading" height="30">
                                    Other Search Settings
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22" colspan="2">
                                <div class="bodyTitle">
                                    Values
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" width="30%">
                                Maximum distance for store finder (in miles):
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtMaxDistance" MinValue="0" runat="server" Width="75px"
                                    NumberFormat-DecimalDigits="0">
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy">
                                Maximum <u>expanded</u> distance for store finder (in miles):
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtMaxDistanceExpanded" MinValue="0" runat="server"
                                    Width="75px" NumberFormat-DecimalDigits="0">
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" colspan="2">
                                <asp:Button SkinID="Button" runat="server" Text="Save" Width="100" ID="uxSave" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

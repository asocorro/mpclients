<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChooseClient.aspx.cs" Inherits="MPClients.Web.ChooseClient" %>
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
                                    Client Selection
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Select Default Client for Orders and Prices Queries</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top" id="uiAlertPanel" runat="server">
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            Actualmente existe un Shopping Cart activo, no puede cambiar el cliente mientras tenga productos en el carrito de compras.
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="bodycopy" valign="top" id="uiNewClientPanel" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            Client:
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="uxClients" runat="server" AllowCustomText="True" ShowToggleImage="True"
                                                ShowMoreResultsBox="true" EnableLoadOnDemand="True" MarkFirstMatch="True" OnItemsRequested="RadComboBox1_ItemsRequested"
                                                EnableVirtualScrolling="true">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Button SkinID="Button" runat="server" Text="Save" Width="120" ID="uxSave"
                                                ValidationGroup="RequieredFields" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>

</asp:Content>

<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="Configuration.aspx.cs"
    Inherits="MPClients.Web.Configuration" Title="MPClients::Config" %>

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
                                    Configuration Edit
                                    <input type="hidden" runat="server" id="uxID" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Configuration Information</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table>
                                   
                                    <tr>
                                        <td>
                                            Restrict Order Products for Pickup:</td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkRestrictOrderProductsForPickup" Width="300"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            Maximum Order Products for Pickup:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="uxMaxOrderProductsForPickup" Width="400" MaxLength=200></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button SkinID="Button"  runat="server" ID="uxSave" Text="Save" Width="80"  />
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

 

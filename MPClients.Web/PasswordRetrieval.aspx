<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="PasswordRetrieval.aspx.cs"
    Inherits="MPClients.PasswordRetrieval" Title="Untitled Page" %>

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
                                    Products Search
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
                                    Enter your email address to receive username and password</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="litMessage" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txbEmail" runat="server" Width="250px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Your login information will be sent to the email address specified above.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button SkinID="Button"  ID="btnSend" runat="server" Text="Request Login Info">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

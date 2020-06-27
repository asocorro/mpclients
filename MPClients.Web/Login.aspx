<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="MPClients.Login" Title="Mascaro Porter::Login" %>

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
                                    Log On
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Logon Form</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <p>
                                    Please enter your username and password to access the privileged areas of this site.
                                </p>
                                <span runat='server' id='uxAlert' visible='false' style='color:Red'>
                                <p><strong>Your account is on Hold, please contact us at 787-782-4121.</strong> </p>
                                </span>
                                <p>
                                    <asp:Login InstructionTextStyle-BorderWidth="0" InstructionText=" " runat="server" 
                                        ID="uiLogin" LoginButtonStyle-CssClass="loginButton" SkinID="CoreLogin" DestinationPageUrl="Default.aspx"
                                        LoginButtonText="Logon" LoginButtonType="Button" TitleText="" RememberMeText="Remember me next time."
                                        CreateUserText=" " CreateUserIconUrl="" DisplayRememberMe="false" TextBoxStyle-Width="200" />
                                </p>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

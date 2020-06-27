<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="ResetPassword.aspx.cs"
    Inherits="MPClients.ResetPassword" Title="MPMobile::Reset Password" %>

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
                                    Log On</td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                   CHANGE PASSWORD</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <p>
                                    Change Password Area
                                </p>
                                <p>
                                    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server">
                                        <UserNameTemplate>
                                            <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                                                <tr>
                                                    <td>
                                                        <table border="0" cellpadding="0">
                                                            <tr>
                                                                <td align="center" colspan="2" class="LoginHeader">
                                                                    Forgot Your Password?</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2" class="Normal">
                                                                    Enter your User Name to receive your password.</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="Normal">
                                        User Name:</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2" style="color: Red;">
                                                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="2">
                                                                    <asp:Button SkinID="Button"  ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" ValidationGroup="PasswordRecovery1" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </UserNameTemplate>
                                    </asp:PasswordRecovery>
                                    <asp:Label ID="lblLegendStatus" runat="server" EnableViewState="false" Text="" />
                                </p>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

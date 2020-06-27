<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs"
    Inherits="MPClients.CreateUser" Title="MPClients::Create User" %>

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
                                    Create User
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
                                    User Information</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
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
                                            User Name:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="uxUserName" Width="300"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="uxUserName"
                                                ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="RequieredFields">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="NewPasswordLabel" runat="server" CssClass="Normal" AssociatedControlID="uxPassword">New Password:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="uxPassword" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="uxPassword"
                                                ErrorMessage="New Password is required." ToolTip="New Password is required."
                                                ValidationGroup="RequieredFields">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="ConfirmNewPasswordLabel" runat="server" CssClass="Normal" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                                ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                                ValidationGroup="RequieredFields">* 
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="uxPassword"
                                                ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                                ValidationGroup="RequieredFields"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="color: Red;">
                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Request password change:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="uxChangePassword" SkinID="CheckBoxSkin" Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Active:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="uxActive" SkinID="CheckBoxSkin" Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                             If this user is sales person, <br />indicate the route(s):
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="uiTerritory" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="false" 
                                                Width="300px">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text=" " />
                                                    <telerik:RadComboBoxItem Text="01" />
                                                    <telerik:RadComboBoxItem Text="02" />
                                                    <telerik:RadComboBoxItem Text="03" />
                                                    <telerik:RadComboBoxItem Text="04" />
                                                    <telerik:RadComboBoxItem Text="05" />
                                                    <telerik:RadComboBoxItem Text="06" />
                                                    <telerik:RadComboBoxItem Text="07" />
                                                    <telerik:RadComboBoxItem Text="08" />
                                                    <telerik:RadComboBoxItem Text="09" />
                                                    <telerik:RadComboBoxItem Text="10" />
                                                    <telerik:RadComboBoxItem Text="11" />
                                                    <telerik:RadComboBoxItem Text="12" />
                                                    <telerik:RadComboBoxItem Text="13" />
                                                    <telerik:RadComboBoxItem Text="14" />
                                                    <telerik:RadComboBoxItem Text="15" />
                                                    <telerik:RadComboBoxItem Text="16" />
                                                    <telerik:RadComboBoxItem Text="17" />
                                                    <telerik:RadComboBoxItem Text="50" />
                                                    <telerik:RadComboBoxItem Text="60" />
                                                    <telerik:RadComboBoxItem Text="70" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr style="visibility: hidden;">
                                        <td>
                                            Is Administrator:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="uxIsAdministrator" SkinID="CheckBoxSkin" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            Can Edit News:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="uxIsNewsEditor" SkinID="CheckBoxSkin" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>

                                    <tr>
                                        <td>
                                            Search Only:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="uxSearchOnly" SkinID="CheckBoxSkin" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Button SkinID="Button" runat="server" Text="Create User" Width="120" ID="uxCreateUser"
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

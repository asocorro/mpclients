<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="MPClients.ChangePassword" Title="MPClients::Change Password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">

    <script>
    function ValidateTandCs(source, args){
        args.IsValid = document.getElementById('<%= uxAccept.ClientID %>').checked;
    }
    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr valign="top">
                <td align="left" class="tdBody">
                    <table style="border: 1px solid rgb(204, 204, 204);" border="0" cellpadding="0" cellspacing="0"
                        width="100%">
                        <tbody>
                            <tr>
                                <td class="mainheading" height="30">
                                    Account Activation
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Terms of Agreement and Password Change</div>
                            </td>
                        </tr>
                        <tr >
                            <td class="bodycopy" valign="top">
                            Please review the terms of use for this site and proceed to select a new password
                                    for your account.<br /><br />
                                <div style="border-left: 1px gray solid; padding-left: 10px; width: 700px; height: 200px; overflow: auto; margin-left: 20px">
                                    Al utilizar el web site www.mpclients.com acepto los siguientes términos y condiciones:<br /><br />

1.  Utilizar este site solo para beneficio del negocio al cual represento.<br /><br />
2.  No divulgar el user name y password asignado.<br /><br />
3.  Informar a Mascaro-Porter & Co., Inc. si algún empleado con conocimiento del user name y password renuncia para así poder asignar unos nuevos.<br /><br />
4.  Mantener mi cuenta al día con Mascaró-Porter & Co., Inc.<br /><br />
5.  Mantener su nivel de compras acostumbrado.<br /><br />
6.  Mascaró-Porter & Co., Inc. se propone subir al site el inventario y los precios actuales una vez al día.  Debido a que puede surgir
alguna variación entre el precio indicado en el site y el facturado, el usuario acepta que el precio indicado en la factura será el correcto.  
Cuando el web site indique pocas cantidades disponibles de algún artículo, el usuario podrá llamar a Mascaró-Porter & Co., Inc. 
para confirmar que lo haya.<br /><br />
7.  El web site indicará la fecha y la hora de la última actualización de inventario y precios.<br /><br />
8.  El web site desactivará aquellos usuarios que no utilizen la página en un período de 30 días.<br /><br />
9.	Búsqueda utilizando intercambio de otra marca es para referencia solamente.  Verificar el catálogo para asegurar la aplicación correcta para el vehículo.  Cualquier uso del intercambio es a riesgo del instalador.<br /><br />
                                </div>
                            </td>
                        </tr>
                        <tr align="center">
                            <td class="bodycopy" valign="top">
                                <asp:CheckBox runat="server" ID="uxAccept" Text="I have read and agree to abide by the terms of use of this site." />
                                <br />
                                <asp:CustomValidator ID="valTandCs" ClientValidationFunction="ValidateTandCs" runat="server"
                                    ErrorMessage="Please accept the Terms and Conditions before continuing." ValidationGroup="ChangePassword1"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr align="center">
                            <td class="bodycopy" valign="top" align=center>
                                <asp:ChangePassword    ID="uxChangePassword" runat="server" ContinueDestinationPageUrl="~/productssearch.aspx">
                                    <ChangePasswordTemplate>
                                        <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                                            <tr>
                                                <td>
                                                    <table border="0" cellpadding="0">
                                                        <tr>
                                                            <td align="center" colspan="2" class="LoginHeader">
                                                                Select Your New Password
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="CurrentPasswordLabel" runat="server" CssClass="Normal" AssociatedControlID="CurrentPassword">Current Password:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                                                    ErrorMessage="Current password is required." ToolTip="Current password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="NewPasswordLabel" runat="server" CssClass="Normal" AssociatedControlID="NewPassword">New Password:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                                                    ErrorMessage="New Password is required." ToolTip="New Password is required."
                                                                    ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="ConfirmNewPasswordLabel" runat="server" CssClass="Normal" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                                                    ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                                                    ValidationGroup="ChangePassword1">* 
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="2">
                                                                <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                                                    ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                                                    ValidationGroup="ChangePassword1"></asp:CompareValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="2" style="color: Red;">
                                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button SkinID="Button" ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword"
                                                                    Text="Change Password" ValidationGroup="ChangePassword1" Width="140" />
                                                            </td>
                                                            <td>
                                                                <asp:Button SkinID="Button" ID="CancelPushButton" runat="server" CausesValidation="False"
                                                                    CommandName="Cancel" Text="Cancel" Width="140" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ChangePasswordTemplate>
                                </asp:ChangePassword>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

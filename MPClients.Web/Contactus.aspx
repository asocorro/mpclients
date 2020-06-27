<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="Contactus.aspx.cs"
    Inherits="MPClients.Contactus" Title="Mascaro Porter::Contact us" %>

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
                                    Contact Us</td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Contact Details</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table cellpadding="0" cellspacing="0">
                                    <tr valign="TOP">
                                        <td>
                                            <ul>
                                                <li><b>Visit Us:</b>
                                                    <br />
                                                    Mascaró-Porter & Co., Inc.
                                                    <br />
                                                    Inc. Calle Segarra
                                                    <br />
                                                    Esq. Blay Reparto Industrial Bechara
                                                    <br />
                                                    Pueblo Viejo, PR 00920 </li>
                                            </ul>
                                        </td>
                                        <td>
                                            <ul>
                                                <li><b>Write Us: </b>
                                                    <br />
                                                    Mascaró-Porter & Co., Inc.
                                                    <br />
                                                    P.O. Box 9024236
                                                    <br />
                                                    San Juan, PR 00902-4236</li>
                                            </ul>
                                        </td>
                                        <td>
                                            <ul>
                                                <li><b>Phone, Fax: </b>
                                                    <br />
                                                    Store: (787) 782-2845
                                                    <br />
                                                    Office: (787) 782-4121
                                                    <br />
                                                    Fax: (787) 792-6750</li>
                                            </ul>
                                        </td>
                                        <td>
                                            <ul>
                                                <li><b>Email us: </b>
                                                    <br />
                                                    mpcinc@MascaroPorter.com</li>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td colspan="2" class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Contact Form</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top" colspan="2">
                                You may also contact us by completing the following form. Hope to hear from you
                                soon!
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 15%">
                                            <strong>Your name:</strong></td>
                                        <td>
                                            <asp:TextBox runat="server" ID="uxName"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Your e-mail address:</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="uxEmail"></asp:TextBox>
                                            We need it in order to respond to you.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1" valign="top">
                                            <strong>How may we help you?</strong>
                                        </td>
                                    
                                        <td colspan="2">
                                            <textarea style="font-family:arial; font-size: 9pt" runat="server" id="uComments" rows="10" SkinID="MultiLine" cols=75></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1" align="left">
                                            <div style="padding-top: 5px; padding-right: 45px" >
                                                <input style="font-family:arial; font-size: 9pt" type="button" value="Submit" id="uxSubmit" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

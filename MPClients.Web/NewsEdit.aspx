<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="NewsEdit.aspx.cs"
    Inherits="MPClients.Web.NewsEdit" Title="MPClients::News Edit" %>

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
                                    News Edit
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
                                    News Information</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            From date:</td>
                                        <td>
                                            <telerik:RadDatePicker ID="uxFromDate" Width="200px" MinDate="2006-02-01" runat="server"
                                                MaxDate="2099-12-16">
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            To date:</td>
                                        <td>
                                             <telerik:RadDatePicker ID="uxToDate" Width="200px" MinDate="2006-02-01" runat="server"
                                                MaxDate="2099-12-16">
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Title:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="uxTitle" Width="400" MaxLength=200></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr valign=top >
                                        <td>
                                            Description:</td>
                                        <td> 
                                            <telerik:RadEditor id="uxDescriptionEditor" runat="server">
                                                <ImageManager UploadPaths="~/Images" ViewPaths="~/Images"  />
                                                <DocumentManager UploadPaths="~/Documents" ViewPaths="~/Documents" MaxUploadFileSize="1000000" />
                                                    <CssFiles>
                                                        <telerik:EditorCssFile Value="~/Styles/EditorContentArea.css" />
                                                    </CssFiles>
                                            </telerik:RadEditor>
                                            <br />
                                             <textarea runat=server id=uxDescription rows=10 style="display:none; font-size: 9pt; font-family: Arial; border-color:#7b9ebd; border-width: 1px; border-style:solid; width:400px;" ></textarea> 
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

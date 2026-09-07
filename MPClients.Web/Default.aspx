<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="MPClients.Default" Title="Mascaro Porter::Home" %>

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
                                    Home Page
                                    <asp:Label runat="server" ID="lblPassword" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Latest News</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                             <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="True"
                                    AllowSorting="True" PageSize="10" ShowFooter="True" AllowMultiRowSelection="False" ClientSettings-Scrolling-AllowScroll="false"
                                    GridLines="Both" EnableAJAX="True" ShowStatusBar="True" Width="100%" StatusBarSettings-ReadyText=" ">
                                    <PagerStyle Mode="NumericPages" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="News" UniqueName="TemplateColumn4" Groupable="False">
                                                <HeaderStyle Width="20px"></HeaderStyle>
                                                <ItemStyle Height="35px" Width="700px"></ItemStyle>
                                                <ItemTemplate>
                                                    <table cellpadding="2" cellspacing="0" width="700px">
                                                        <tr valign="top">
                                                            <td style="width: 5%;display:none">
                                                                Title:
                                                            </td>
                                                            <td style="font-weight:bold" >
                                                                <%# DataBinder.Eval(Container.DataItem, "Title") %>
                                                            </td>
                                                        </tr>
                                                        <tr valign="top">
                                                            <td style="display:none">
                                                            </td>
                                                            <td colspan=2>
                                                                <div style="color: rgb(44,44,44);">
                                                                    <%# DataBinder.Eval(Container.DataItem, "Description") %>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

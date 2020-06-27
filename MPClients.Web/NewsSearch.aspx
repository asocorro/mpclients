<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="NewsSearch.aspx.cs"
    Inherits="MPClients.Web.NewsSearch" Title="MPClients::News" %>

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
                                    News Search
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0" id="uxResultsGrid" runat="server">
                        <tr>
                            <td class="bodyTitleBak">
                                <div class="bodyTitle">
                                    All the News</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                            Use this page to manage the news that will appear on the home page.<br /><br />
                                <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="True" AllowSorting="True"
                                    PageSize="10" ShowFooter="True" AllowMultiRowSelection="False" GridLines="Both"
                                    EnableAJAX="True" ShowStatusBar="True" Width="100%">
                                    <PagerStyle Mode="NumericPages" />
                                    
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="FromDate" HeaderText="From Date" DataFormatString="{0:MM/dd/yy}" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="120">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ToDate" HeaderText="To Date" DataFormatString="{0:MM/dd/yy}" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="120">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Title" HeaderText="Title"  HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle">
                                            </telerik:GridBoundColumn>
                                            
                                            <telerik:GridButtonColumn CommandName="Remove" Text="Remove" UniqueName="Remove" ItemStyle-Width=100>
                                            </telerik:GridButtonColumn>
                                             
                                            <telerik:GridButtonColumn CommandName="Edit" Text="Edit" UniqueName="Edit"  ItemStyle-Width=100>
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top" align="right">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                
                                            </td>
                                            <td>
                                                <asp:Button SkinID="Button"  runat="server" ID="uxAddNews" Text="Add News..." />
                                            </td>
                                        </tr>
                                       
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

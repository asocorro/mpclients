<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Clients.aspx.cs" Inherits="MPClients.Web.Clients" %>

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
                                    Client Search
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Search Criteria
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" class="bodycopy">
                                Search for clients.
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
                                                ShowMoreResultsBox="true" EnableLoadOnDemand="True" MarkFirstMatch="True" OnItemsRequested="uxClients_ItemsRequested"
                                                EnableVirtualScrolling="true">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            In search results:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkInSearchResults" Width="300"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Button SkinID="Button" runat="server" Text="Search" Width="120" ID="uxSearch" />&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak">
                                <div class="bodyTitle">
                                    Results</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="True"
                                    AllowSorting="True" PageSize="50" ShowFooter="True" AllowMultiRowSelection="False"
                                    GridLines="Both" EnableAJAX="True" ShowStatusBar="True" Width="100%">
                                    <PagerStyle Mode="NumericPages" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="ClientId">
                                        <Columns>
                                            <telerik:GridBoundColumn Groupable="false" DataField="Client" HeaderText="Client"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="180"
                                                ItemStyle-Wrap="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="BusinessName" HeaderText="Business Name"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="180">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="InSearchResults" HeaderText="In Search Results"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="180">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn CommandName="Edit" Text="Edit" UniqueName="Edit" ItemStyle-Width="100">
                                            </telerik:GridButtonColumn>
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

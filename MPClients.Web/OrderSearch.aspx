<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderSearch.aspx.cs"
    Inherits="MPClients.Web.OrderSearch" Title="MPClients::Order Search" %>

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
                                    Order Search
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
                                This page allows you to review the orders you have sent to us.
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            Show orders from:
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="uxDates">
                                                <asp:ListItem Selected="true" Text="Today" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yesterday" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Last 15 days" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Last 30 days" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Older than 30 days" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
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
                                    GridLines="Both" EnableAJAX="True" ShowStatusBar="False" Width="100%" GroupPanel-ToolTip="toooool tip"
                                    ClientSettings-AllowDragToGroup="true" ShowGroupPanel="true" GroupingEnabled="true"
                                    GroupPanel-Enabled="true">
                                    <PagerStyle Mode="NumericPages" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
                                        <Columns>
                                            <telerik:GridBoundColumn Groupable="false" DataField="OrderNo" HeaderText="Order Number"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="100">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserIdLookup.ClientNameOnly" HeaderText="Client Name"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Wrap="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserIdLookup.Username" HeaderText="User Name"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="100">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="OrderDate" HeaderText="Order Date"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="180">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="OrderTotal" HeaderText="Total"
                                                DataFormatString="{0:C}" HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle"
                                                ItemStyle-Width="100">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="StatusOfOrder.Description" HeaderText="Status"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="100">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridHyperLinkColumn Groupable="false" DataNavigateUrlFields="ID" Text="View"
                                                DataNavigateUrlFormatString="OrderEdit.aspx?PageMethod=DoLoad&id={0}&fromsc=False"
                                                HeaderText="View Details" HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle"
                                                ItemStyle-Width="200">
                                            </telerik:GridHyperLinkColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr runat="server" id="uxTDSpecialOrdes">
                            <td class="bodycopy" valign="top" align="right">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                To view Shopping Carts and Unplaced Orders:&nbsp;&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:Button SkinID="Button" runat="server" ID="uxViewSpecialOrders" Text="View" Width="120" />
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

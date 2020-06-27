<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdersResume.aspx.cs"
    Inherits="MPClients.Web.OrdersResume" Title="MPClients::Active Shopping Carts" %>

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
                                    Active Shopping Carts
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0" id="uxResultsGrid" runat="server">
                        <tr>
                            <td class="bodyTitleBak">
                                <div class="bodyTitle">
                                    Active Carts</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                The following clients have shopping carts which have not been converted to orders.<br />
                                <br />
                                <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="True"
                                    AllowSorting="True" PageSize="50" ShowFooter="True" AllowMultiRowSelection="False"
                                    GridLines="Both" EnableAJAX="True" ShowStatusBar="True" Width="100%">
                                    <PagerStyle Mode="NumericPages" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="OrderNo" HeaderText="Order Number" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="100">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Name" HeaderText="Client Name" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="220">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="100">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ClientId" HeaderText="Client Id" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="80">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Telephonenumber" HeaderText="Telephone" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="140">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="OrderDate" HeaderText="Cart Created On" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="220">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="StatusDate" HeaderText="Cart Last Updated On"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="220">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="DetailsCount" HeaderText="Items" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="40">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridHyperLinkColumn DataNavigateUrlFields="OrderId" Text="View" DataNavigateUrlFormatString="OrderEdit.aspx?PageMethod=DoLoad&id={0}&fromsc=False"
                                                HeaderText="" HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle"
                                                ItemStyle-Width="70">
                                            </telerik:GridHyperLinkColumn>
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

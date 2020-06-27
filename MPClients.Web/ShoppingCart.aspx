<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs"
    Inherits="MPClients.ShoppingCart" Title="MPClients::Shopping Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        <img src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
            alt="" style="border: 0px;" />
    </telerik:RadAjaxLoadingPanel>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr valign="top">
                <td align="left" class="tdBody">
                    <table style="border: 1px solid rgb(204, 204, 204);" border="0" cellpadding="0" cellspacing="0"
                        width="100%">
                        <tbody>
                            <tr>
                                <td class="mainheading" height="30">
                                    Shopping Cart
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0" id="uxResultsGrid" runat="server">
                        <tr>
                            <td class="bodyTitleBak">
                                <div class="bodyTitle">
                                    Shopping Cart Details</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <asp:Label runat="server" ID="lblMessage" Visible="false" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <asp:Button SkinID="Button" runat="server" ID="uxUpdateShopping" Text="Update Shopping Cart"
                                    Width="200" />
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                                    <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="True"
                                        AllowSorting="True" PageSize="100" ShowFooter="True" AllowMultiRowSelection="False"
                                        GridLines="Both" EnableAJAX="True" ShowStatusBar="True" Width="100%">
                                        <PagerStyle Mode="NumericPages" />
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="True"></Selecting>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
                                            <Columns>
                                                <telerik:GridTemplateColumn DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity"
                                                    UniqueName="QuantityDesired" ItemStyle-Width="70" ItemStyle-VerticalAlign="top">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox SkinID="Office2007" ID="QuantityDesiredTextBox" ShowSpinButtons="true"
                                                            Type="Number" ButtonsPosition="Right" runat="server" Text='<%# Eval("Quantity")%>'
                                                            Width="50" MinValue="1" MaxValue="10000" ItemStyle-VerticalAlign="top">
                                                            <NumberFormat DecimalDigits="0" />
                                                        </telerik:RadNumericTextBox>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="ProductID" HeaderText="Product ID" HeaderStyle-VerticalAlign="Middle"
                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-Width="120" UniqueName="ProductID">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Product.Description1" HeaderText="Short Description"
                                                    HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Product.Name" HeaderText="Description" HeaderStyle-VerticalAlign="Middle"
                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-Width="220">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="NetPrice" DataFormatString="{0:C}" HeaderText="Your Net Unit Price"
                                                    HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="120">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="Template1" HeaderText="Extended Price" ItemStyle-Width="120">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="uxPrice" Text='<% #Eval("ExtendedPrice") %>'>
                                                        </asp:Label>
                                                        <asp:Label runat="server" Visible='false' ID="uxIsFriend" Text='<% #Eval("IsFriend") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label Font-Bold="true" runat="server" ID="uxTotalPrice">
                                                        </asp:Label>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridButtonColumn CommandName="RemoveFromCart" Text="Remove From Cart" UniqueName="RemoveFromCart">
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </telerik:RadAjaxPanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tbody>
                                        <tr style="height: 50px">
                                            <td style="width: 25%">
                                                To add more products to your cart, press here:&nbsp;&nbsp;
                                            </td>
                                            <td style="width: 25%">
                                                <asp:Button SkinID="Button" runat="server" ID="uxAddProducts" Text="Search for More Products..."
                                                    Width="200" />
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr style="height: 50px">
                                            <td>
                                                To create an order with the products in your shopping cart, press here. You will
                                                then be able to review your order before placing it:&nbsp;&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:Button SkinID="Button" runat="server" ID="uxCreateOrder" Text="Create Order..."
                                                    Width="200" />
                                            </td>
                                            <td align="right" style="font-weight: bold">*Prices and order totals shown on this web site do not include the IVU tax.</td>
                                        </tr>
                                        <tr style="height: 50px">
                                            <td>
                                                To remove all of the contents from your shopping cart, press here:&nbsp;&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:Button SkinID="Button" runat="server" ID="uxResetShopping" Text="Reset Shopping Cart"
                                                    Width="200" />
                                            </td>
                                            <td>&nbsp;</td>
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

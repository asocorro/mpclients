<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintOrder.aspx.cs" Inherits="MPClients.Web.PrintOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Order</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" LoadScriptsBeforeUI="true" runat="server">
    </asp:ScriptManager>
    <br />
    <div>
        <div id="TextLayer">
            <table width="100%" cellspacing="0" cellpadding="0" border="0" style="padding-left: 10px;
                font-family: arial; font-size: 9pt">
                <tr>
                    <td valign="top" colspan="2">
                        <table class="Title">
                            <tr>
                                <td class="Head">
                                    <b>PRODUCT ORDER</b>
                                </td>
                                <td align="right" class="RightHead">&nbsp;
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </td>
                </tr>
                <tr style="padding-right: 10px;">
                    <td>
                        <input type="hidden" runat="server" id="uxID" />
                        <asp:Label runat="server" CssClass="input" ID="uxRequestedBy"></asp:Label>
                    </td>
                    <td align="right" class="input" width="200px">
                        Mascaró-Porter Co.<br>
                        Phone: 787-782-4121<br>
                        Fax: 787-792-6750
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="300px" cellspacing="2" cellpadding="2" border="0" style="padding-left: 0px;
                            font-family: arial; font-size: 9pt">
                            <tr>
                                <td>
                                    Client Name:
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="uxCompany"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    P.O. Number:
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="uiPurchaseOrder"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>
                                    Contact Information:
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="uxContactPerson"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Pickup Method:
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="uiPickupMethod"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" class="input" style="padding-right: 10px;">
                        <asp:Label runat="server" ID="uiOrderDate"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" border="0" style="padding-left: 10px;">
                <tr width="100%" style="padding-right: 2px;">
                    <td valign="top" colspan="3" class="input" >
                        <br />
                        <br />
                        <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="True"
                            AllowSorting="True" PageSize="1000" ShowFooter="True" AllowMultiRowSelection="False"
                            GridLines="Both" EnableAJAX="True" ShowStatusBar="false" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <ClientSettings>
                                <Selecting AllowRowSelect="True"></Selecting>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Quantity" HeaderText="Quantity" HeaderStyle-VerticalAlign="Middle"
                                        ItemStyle-VerticalAlign="Middle" ItemStyle-Width="40">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ProductID" HeaderText="Product ID" HeaderStyle-VerticalAlign="Middle"
                                        ItemStyle-VerticalAlign="Middle" ItemStyle-Width="80">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ShortDescription" HeaderText="Short Description"
                                        HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="110">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ProductName" HeaderText="Name" HeaderStyle-VerticalAlign="Middle"
                                        ItemStyle-VerticalAlign="Middle" ItemStyle-Width="160">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NetPrice" DataFormatString="{0:C}" HeaderText="Your Net<br>Unit Price"
                                        HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="60">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn UniqueName="Template1" HeaderText="Extended<br>Price"
                                        ItemStyle-Width="60">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="uxPrice" Text='<% #Eval("ExtendedPrice") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label Font-Bold="true" runat="server" ID="uxTotalPrice">
                                            </asp:Label>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-left: 10px; padding-left: 0px; font-family: arial; font-size: 9pt"
            class="texttable">
            <br>
            The prices and available quantities shown may vary at invoicing. The user accepts
            that the invoiced price will be the correct one.
            <br>
            You may contact us to confirm prices or quantities.
        </div>
    </div>
    </form>
</body>
</html>

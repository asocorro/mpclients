<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderEdit.aspx.cs"
    Inherits="MPClients.Web.OrderEdit" Title="MPClients::Order" %>

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
                                    Order Edit
                                    <input type="hidden" runat="server" id="uxID" />
                                </td>
                                <td align="right">
                                    <!--
                                <asp:Button SkinID="Button" Text="Send Order" runat="server" /> <asp:Button ID="Button1" SkinID="Button" Text="Print Order" runat="server" />
                                -->
                                    <asp:LinkButton Visible=false runat="server" ID="uxSendLink" title="Send us your order so that we may process it"
                                        Text="<img src='images/file-up.png' style='vertical-align: middle; padding-right: 2px' />Send
                                        Order"></asp:LinkButton>
                                    &nbsp;&nbsp; <span runat="server" id="uxPrintLink" width="120" style="color: Blue; text-decoration: underline; cursor: hand" onclick="javascript:PrintOrder()">
                                        <img src='images/print.png' style='vertical-align: middle; padding-right: 2px' />Print
                                        Order</span>&nbsp;&nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    General Information</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <table>
                                                 <tr style="display:none">
                                                    <td>
                                                        Current Time:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="uxCurrentTime" Width="300" ForeColor="gray" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Requested by:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="uxRequestedBy" Width="300" ForeColor="gray" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Client name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="uxClientName" Width="300" ForeColor="gray" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Contact person:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="uxContactPerson" Width="300"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        PO Number:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="uxPONumber" Width="300"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Pickup method: <asp:TextBox runat="server" ID="uxNumberOfProducts" Visible="false"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="uxPickupWhse" GroupName="Pickup" Text="Pickup at warehouse (Recoger en almacén)" />
                                                        <br />
                                                        <asp:RadioButton runat="server" ID="uxPickupDelivery" GroupName="Pickup" Text="Delivery (Enviar por ruta)" Checked="true" />
                                                        <br />
                                                        <i runat="server" id="uxPickupDeliveryMessage">Placeholder for text.</i>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        Order status:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="uxOrderStatus" Width="100" ForeColor="gray" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        Comments:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox Rows="5" runat="server" ID="uxComments" Width="300" TextMode="MultiLine"
                                                            SkinID="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br>
                    <table class="bodyTable" cellpadding="0" cellspacing="0" id="uxResultsGrid" runat="server">
                        <tr>
                            <td class="bodyTitleBak">
                                <div class="bodyTitle">
                                    Products</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="True"
                                    AllowSorting="True" PageSize="100" ShowFooter="True" AllowMultiRowSelection="False"
                                    GridLines="Both" EnableAJAX="True" ShowStatusBar="True" Width="100%">
                                    <PagerStyle Mode="NumericPages" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="Quantity" HeaderText="Quantity" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="50">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ProductID" HeaderText="Product ID" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="120">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ShortDescription" HeaderText="Short Description"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ProductName" HeaderText="Name" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="220">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="NetPrice" DataFormatString="{0:C}" HeaderText="Your Net Unit Price"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="120">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="Template1" HeaderText="Extended Price" ItemStyle-Width="120">
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
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tbody>
                                        <tr style="height: 50px">
                                            <td style="width: 25%">
                                                To send us your order so that we may process it, press here:&nbsp;&nbsp;
                                            </td>
                                            <td style="width: 25%">
                                                <asp:Button SkinID="Button" runat="server" ID="uxSendOrder" Text="Send Order" Width="120" />
                                            </td>
                                            <td align="right" style="font-weight: bold">*Prices and order totals shown on this web site do not include the IVU tax.</td>
                                        </tr>
                                        <tr style="height: 50px">
                                            <td>
                                                To obtain a printed copy of this order, press here:&nbsp;&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:Button SkinID="Button" runat="server" ID="uxPrint" Text="Print Order" Width="120"
                                                    OnClientClick="javascript:PrintOrder()" />
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

    <script language="javascript">
    
	function PrintOrder(){
		
	   var url = "PrintOrder.aspx?ID=" + document.getElementById('<%= uxID.ClientID %>').value ;
	   window.open(url ,"newwin","dialogHeight=520px, dialogWidth=400px, width=800px, center=yes, menubar=no, scrollbars=yes, toolbar=yes");
	}
    </script>

</asp:Content>

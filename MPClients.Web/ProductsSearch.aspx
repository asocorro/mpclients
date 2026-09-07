<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductsSearch.aspx.cs"
    Inherits="MPClients.ProductsSearch" Title="MPClients::Product Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">


        <style type='text/css'>
            .RadInput A.riDown 
            {
                margin-top:0px;
            }
            .RadTreeView_Windows7 .rtPlus, 
.RadTreeView_Windows7 .rtMinus {
	background-image: url(images/PlusMinus.png)
}
        </style>
    <script type="text/javascript">
function rowMouseOver(sender, eventArgs)
{ 
     //you can take the number of the row here
    //alert("Mouse is over row: " + eventArgs.get_itemIndexHierarchical());
    //make a postback and transfer the value; there are several ways, choose the one which fits best your logic
    //__doPostBack("","");
}


    </script>
    <script type="text/jscript">
        function KeyPress(sender, args) {

            if (args.get_keyCode() == 13) {
                var btn = document.getElementById(sender.get_emptyMessage());
                if (btn != null) {
                    alert('enter');
                    btn.click();
                }
                args.set_cancel(true);
            }
        }

        function RowClick(index) {
            alert(index);
        }
    </script> 
    <script type"text/javascript">
            function doClick(buttonName, e) {
                //the purpose of this function is to allow the enter key to 
                //point to the correct button to click.
                var key;

                if (window.event)
                    key = window.event.keyCode;     //IE
                else
                    key = e.which;     //firefox

                if (key == 13) {
                    //Get the button the user wants to have clicked
                    var btn = document.getElementById(buttonName);
                    if (btn != null) { //If we find the button click it
                        btn.click();
                        event.keyCode = 0
                    }
                }
            }
    </script>
    <div>
        <telerik:RadToolTipManager AutoTooltipify="false" Title="<i>More Details</i>" 
            Sticky="true" ShowDelay="0" ID="RadToolTipManager1" runat="server" OnAjaxUpdate="OnAjaxUpdate"
            Position="BottomRight" ShowEvent="OnClick">
        </telerik:RadToolTipManager>
    </div>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr valign="top">
                <td align="left" class="tdBody">
                    <table style="border: 1px solid rgb(204, 204, 204);" border="0" cellpadding="0" cellspacing="0"
                        width="100%">
                        <tbody>
                            <tr>
                                <td class="mainheading" height="30">
                                    Product Search
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0" id="uiSearchCriteria" runat="server" border="0">
                        <tr>
                            <td colspan="2" class="bodyTitleBak" height="22">   
                                <div class="bodyTitle">
                                    Search Criteria</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table border="0">
                                    <tr>
                                        <td colspan="2" align="left">
                                            Indicate any details about the products you wish to find. Use separate lines for
                                            each search text.
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <table cellpadding="0" cellspacing="4" width="200">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox4"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox6"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox7"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox8"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox9"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="TextBox10"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <i>Further filter your search by choosing categories and sub-categories below:</i>
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <telerik:RadTreeView ID="uxCategories" runat="server" Height="350px" Width="450px"
                                                            OnNodeExpand="RadTreeView_NodeExpand" CheckBoxes="True" />
                                                            
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                  
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                </table>
                            </td>

                            <td class="bodycopy" valign="top">
                              <table border="0" runat="server" id="uiTireSearchTable">
                                    <tr>
                                        <td colspan="2" align="left">
                                            Perform a direct search for tires:
                                            <telerik:RadToolTip runat="server" ID="RadToolTip1" RelativeTo="Element" ShowEvent="OnClick"
                                                HideEvent="ManualClose" ShowCallout="true" TargetControlID="TireHelp" IsClientID="true"
                                                Animation="Fade" Position="TopRight">
                                                <img src="images/TireSizeExample.png" alt="&nbsp;" />
                                            </telerik:RadToolTip>
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <%-- first set of combo boxes --%>
                                    <tr valign="top">
                                        <td width="5%" nowrap>
                                            <asp:UpdatePanel ID="upSetSession" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList runat="server" ID="cmbSize1" AutoPostBack="true" OnSelectedIndexChanged="cmbSize1_SelectedIndexChanged"
                                                        Width="100px" />
                                                    <asp:DropDownList runat="server" ID="cmbSize2" AutoPostBack="true" OnSelectedIndexChanged="cmbSize2_SelectedIndexChanged"
                                                        Width="100px" />
                                                    <asp:DropDownList runat="server" ID="cmbSize3" Width="100px" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSize1" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSize2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="upSetSession">
                                                <ProgressTemplate>
                                                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 50%;
                                                        right: 0; left: 0; z-index: 9999999;">
                                                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/images/loader.gif" AlternateText="Loading..."
                                                            ToolTip="Loading..." Style="padding: 10px;" />
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>
                                    <tr style="height: 4px">
                                        <td></td>
                                    </tr>

                                    <%-- second set of combo boxes --%>
                                    <tr valign="top">
                                        <td width="5%" nowrap>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList runat="server" ID="cmbSize1_2" AutoPostBack="true" OnSelectedIndexChanged="cmbSize1_2_SelectedIndexChanged"
                                                        Width="100px" />
                                                    <asp:DropDownList runat="server" ID="cmbSize2_2" AutoPostBack="true" OnSelectedIndexChanged="cmbSize2_2_SelectedIndexChanged"
                                                        Width="100px" />
                                                    <asp:DropDownList runat="server" ID="cmbSize3_2" Width="100px" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSize1_2" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSize2_2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="updateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                                                <ProgressTemplate>
                                                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 50%;
                                                        right: 0; left: 0; z-index: 9999999;">
                                                        <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/images/loader.gif" AlternateText="Loading..."
                                                            ToolTip="Loading..." Style="padding: 10px;" />
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>
                                    <tr style="height: 4px">
                                        <td></td>
                                    </tr>

                                    <%-- third set of combo boxes --%>
                                    <tr valign="top">
                                        <td width="5%" nowrap>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList runat="server" ID="cmbSize1_3" AutoPostBack="true" OnSelectedIndexChanged="cmbSize1_3_SelectedIndexChanged"
                                                        Width="100px" />
                                                    <asp:DropDownList runat="server" ID="cmbSize2_3" AutoPostBack="true" OnSelectedIndexChanged="cmbSize2_3_SelectedIndexChanged"
                                                        Width="100px" />
                                                    <asp:DropDownList runat="server" ID="cmbSize3_3" Width="100px" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSize1_3" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSize2_3" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="updateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
                                                <ProgressTemplate>
                                                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 50%;
                                                        right: 0; left: 0; z-index: 9999999;">
                                                        <asp:Image ID="imgUpdateProgress3" runat="server" ImageUrl="~/images/loader.gif" AlternateText="Loading..."
                                                            ToolTip="Loading..." Style="padding: 10px;" />
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>
                                    <tr style="height: 4px">
                                        <td></td>
                                    </tr>

                                    <%-- fourth set of combo boxes --%>
                                    <tr valign="top">
                                        <td width="5%" nowrap>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList runat="server" ID="cmbSize1_4" AutoPostBack="true" OnSelectedIndexChanged="cmbSize1_4_SelectedIndexChanged"
                                                        Width="100px" />
                                                    <asp:DropDownList runat="server" ID="cmbSize2_4" AutoPostBack="true" OnSelectedIndexChanged="cmbSize2_4_SelectedIndexChanged"
                                                        Width="100px" />
                                                    <asp:DropDownList runat="server" ID="cmbSize3_4" Width="100px" />
                                                    <br />
                                                    <br />
                                                    <a href="http://mascaroporter.com/gomas/" target="_blank" style="font-size: 8pt">Click here to see tire models.</a>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSize1_4" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSize2_4" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="updateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel4">
                                                <ProgressTemplate>
                                                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 50%;
                                                        right: 0; left: 0; z-index: 9999999;">
                                                        <asp:Image ID="imgUpdateProgress4" runat="server" ImageUrl="~/images/loader.gif" AlternateText="Loading..."
                                                            ToolTip="Loading..." Style="padding: 10px;" />
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>                                                                                                            
                                </table>
                            </td>
                        </tr>

                          <tr>
                                        <td colspan="2">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:Button SkinID="Button" runat="server" Text="Search" Width="120" ID="uxSearchButton" />
                                            <asp:Button SkinID="Button" runat="server" Text="Clear" Width="120" ID="uxClear" />
                                        </td>
                                    </tr>

                    </table>
                    <table class="bodyTable" cellpadding="0" cellspacing="0" id="uxResultsMessages" runat="server">
                        <tr>
                            <td class="bodyTitleBak">
                                <div class="bodyTitle">
                                    Results Messages</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="uxResultsLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Button SkinID="Button" runat="server" Text="Revise Search Criteria" ID="uxReviseSearchCriteria" />
                                            &nbsp;
                                            <asp:Button SkinID="Button" runat="server" Text="View Results Anyway" ID="uxViewResults" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table class="bodyTable" cellpadding="0" cellspacing="0" id="uxResultsGrid" runat="server">
                        <tr>
                            <td class="bodyTitleBak">
                                <div class="bodyTitle">
                                    Search Results&nbsp;&nbsp;<span style="font-weight: normal; font-size: smaller; color: #880808">Resultados son para referencia solamente. Para la aplicación correcta, debe verificar el catálogo del suplidor.</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <asp:Label runat="server" ID="uxProductAddedMsg"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopyShorter" valign="top">
                                <table width=" 100%" border="0">
                                    <tr>
                                        <td align="left" style="width: 50%">
                                            <asp:Button SkinID="Button" Width="130px" runat="server" Text="Search Again" ID="uxSearchAgain2" />
                                            <asp:Button SkinID="Button" Width="130px" runat="server" Text="View Shopping Cart"
                                                ID="uxViewShoppingCart2" />
                                            <asp:Button SkinID="Button" runat="server" Text="Add Selected Products To Cart" ID="uxAddSelectedTop" />
                                        </td>
                                        <td align="left">
                                            <asp:Label runat="server" ID="uxResultsLabel1"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="True"
                                    AllowSorting="True" PageSize="25" ShowFooter="True" AllowMultiRowSelection="True" 
                                    GridLines="Both" EnableAJAX="True" ShowStatusBar="True" Width="100%">
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                    <ClientSettings  >
                                        <Selecting AllowRowSelect="true"   EnableDragToSelectRows="false"  ></Selecting>
                                        <ClientEvents OnRowMouseOver="rowMouseOver" />
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false"  DataKeyNames="ID">
                                        <Columns>
                                             <telerik:GridClientSelectColumn UniqueName="CheckboxSelectColumn" HeaderText="Select Item" ItemStyle-Width=24 >
                                             
                                             </telerik:GridClientSelectColumn>
                                            <telerik:GridTemplateColumn UniqueName="AddProducts" ItemStyle-Width="10" Visible="false">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="#" onclick="return false;"
                                                        Text="<img src='../Images/zoom.png'>" ToolTip="Click for more details">
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="FromQuery"  HeaderText="Search Text" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="top" ItemStyle-Width="80">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ID" HeaderText="Product ID" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="top" ItemStyle-Width="100">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Description1" HeaderText="Short<br>Description"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="top" ItemStyle-Width="100">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn Visible="false" DataField="Name" HeaderText="Description"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="top" ItemStyle-Width="220">
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn  DataField="DescriptionExt" HeaderText="Description"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="top" ItemStyle-Width="240">
                                            </telerik:GridBoundColumn>
                                     
                                            <telerik:GridBoundColumn DataField="ClientProductPrice" HeaderText="Your Net<br>Unit Price"
                                                DataFormatString="{0:C}" HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="top"
                                                ItemStyle-Width="80">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="StockquantityText" HeaderText="Qty.<br>Available"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="top" ItemStyle-Width="70">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn DataField="IssueDetail" HeaderText="Qty.<br>Desired"
                                                SortExpression="QuantityDesired" UniqueName="QuantityDesired" ItemStyle-Width="70"
                                                ItemStyle-VerticalAlign="top">
                                                <ItemTemplate>
                                                    <telerik:RadMaskedTextBox SkinID="Office2007" ID="QuantityDesiredTextBox" runat="server"
                                                        Text='<%# Eval("QuantityDesired")%>' SelectionOnFocus="SelectAll" Mask="#####" Width="40" 
                                                        DisplayPromptChar=" " PromptChar=" " >
                                                    
                                                    </telerik:RadMaskedTextBox>
                                                    <input type="hidden" runat="server" value='<%# Eval("ID")%>' id="productid" />
                                                    <input type="hidden" runat="server" value='<%# Eval("ClientProductPrice")%>' id="uxClientProductPrice" />
                                                    <input type="hidden" runat="server" value='<%# Eval("Name")%>' id="uxProductName" />
                                                    <input type="hidden" runat="server" value='<%# Eval("Description1")%>' id="uxDescription1" />
                                                    
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridButtonColumn ItemStyle-Width="80"  CommandName="AddToChart" Text="Add to Cart" 
                                                ItemStyle-VerticalAlign="top" UniqueName="AddToChart">
                                            </telerik:GridButtonColumn>

                                            <telerik:GridTemplateColumn UniqueName="TemplateColumn"
                                                ItemStyle-VerticalAlign="top" ItemStyle-Width="80">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="MoreInfoURL" runat="server"
                                                        NavigateUrl='<%# Eval("MoreInfoURL") %>'
                                                        Target="_blank"
                                                        Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("MoreInfoURL"))) %>'>
                                                        Click for More Info <img src="images/external.png" height="12" width="12" />
                                                    </asp:HyperLink>
                                                    
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table>
                                    <tr>
                                        <td align="right" nowrap='nowrap'>
                                            <asp:Button SkinID="Button" Width="130px" runat="server" Text="Search Again" ID="uxSearchAgain" />
                                       
                                            <asp:Button SkinID="Button" Width="130px" runat="server" Text="View Shopping Cart"
                                                ID="uxViewShoppingCart" />
                                                <asp:Button SkinID="Button" runat="server" Text="Add Selected Products To Cart" ID="uxAddSelectedBottom" />
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
    <input type=hidden runat='server' id='txtIncludedCategories' />
    <input type=hidden runat='server' id='uiTotal' />
</asp:Content>

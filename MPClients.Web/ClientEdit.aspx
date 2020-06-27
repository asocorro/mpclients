<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ClientEdit.aspx.cs" Inherits="MPClients.ClientEdit" %>

<%@ PreviousPageType VirtualPath="Clients.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <style>
        .ruFileInput
        {
            font: 11px/10px "Segoe UI" , Arial, sans-serif !important;
        }
    </style>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr valign="top">
                <td align="left" class="tdBody">
                    <table style="border: 1px solid rgb(204, 204, 204);" border="0" cellpadding="0" cellspacing="0"
                        width="100%">
                        <tbody>
                            <tr>
                                <td class="mainheading" height="30">
                                    Client Edit
                                    <input type="hidden" runat="server" id="uxClientId" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td class="bodyTitleBak" height="22" colspan="3">
                                <div class="bodyTitle">
                                    Client Details
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="bodycopy">
                                <asp:Label runat="server" ID="lblClientName" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 30%">
                                <table class="bodycopy" border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td style="font-size: larger; height: 25px">
                                            Address from Dynamics:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="radioDynamicsAddress" runat="server" GroupName="AddressGroup"
                                                Text="Use this address for the map" />
                                        </td>
                                    </tr>
                                    <tr style="height: 2px">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAddressLine1" Text="line 1" runat="server" Width="250px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAddressLine2" Text="line 2" runat="server" Width="250px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>
                                            <asp:Label ID="lblCityStateZipCode" Text="City" runat="server" Width="250px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTelephoneNumber" Text="phone" runat="server" Width="250px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a id="uxClientDynamicsMap" runat="server" href="map.aspx" target="_blank">View map</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top" style="width: 30%">
                                <table class="bodycopy" border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                      <td style="font-size: larger; height: 25px">
                                            Address in Local Database:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="radioLocalAddress" runat="server" GroupName="AddressGroup" Text="Use this address for the map" />
                                        </td>
                                    </tr>
                                    <tr style="height: 2px">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        Business name:<br />
                                            <asp:TextBox ID="txtBusinessName" MaxLength="250" Text="business name" runat="server"
                                                Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 8px">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        Address:<br />
                                            <asp:TextBox ID="txtAddressLine1" MaxLength="250" Text="line 1" runat="server" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtAddressLine2" MaxLength="250" Text="line 2" runat="server" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>
                                            <asp:TextBox ID="txtCity" MaxLength="100" Text="City" runat="server" Width="250px"></asp:TextBox>
                                            <asp:TextBox ID="txtState" MaxLength="100" Text="State" runat="server" Width="100px"></asp:TextBox>
                                            <asp:TextBox ID="txtZipCode" MaxLength="5" Text="Zip Code" runat="server" Width="75px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 8px">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        Web address:<br />
                                            <asp:TextBox ID="txtWebAddress" MaxLength="100" Text="web address" runat="server"
                                                Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        Map link:<br />
                                            <asp:TextBox CssClass="clsMultiLine" TextMode="MultiLine" Rows="3" ID="txtMapLink"
                                                MaxLength="250" Text="web address" runat="server" Width="246px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        Extra fields:<br />
                                            <asp:TextBox ID="txtLabel1" MaxLength="100" Text="label 1" runat="server" Width="115px"></asp:TextBox>
                                            <asp:TextBox ID="txtExtra1" MaxLength="100" Text="extra 1" runat="server" Width="235px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLabel2" MaxLength="100" Text="label 2" runat="server" Width="115px"></asp:TextBox>
                                            <asp:TextBox ID="txtExtra2" MaxLength="100" Text="extra 2" runat="server" Width="235px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLabel3" MaxLength="100" Text="label 3" runat="server" Width="115px"></asp:TextBox>
                                            <asp:TextBox ID="txtExtra3" MaxLength="100" Text="extra 3" runat="server" Width="235px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLabel4" MaxLength="100" Text="label 4" runat="server" Width="115px"></asp:TextBox>
                                            <asp:TextBox ID="txtExtra4" MaxLength="100" Text="extra 4" runat="server" Width="235px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLabel5" MaxLength="100" Text="label 5" runat="server" Width="115px"></asp:TextBox>
                                            <asp:TextBox ID="txtExtra5" MaxLength="100" Text="extra 5" runat="server" Width="235px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLabel6" MaxLength="100" Text="label 6" runat="server" Width="115px"></asp:TextBox>
                                            <asp:TextBox ID="txtExtra6" MaxLength="100" Text="extra 6" runat="server" Width="235px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a id="uxClientLocalMap" href="" target="_blank" runat="server">View map</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table class="bodycopy" border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td style="padding-left: 4px; padding-bottom: 4px">
                                            <asp:Image GenerateEmptyAlternateText="true" AlternateText="Logo not found" runat="server"
                                                ID="uxClientLogo" alt="Logo not found" ImageAlign="Middle" /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 8px;">
                                            <telerik:RadAsyncUpload Width="200px" ID="uxLogoFile" runat="server" AllowedFileExtensions="gif,png,jpg,jpeg"
                                                EnableInlineProgress="true" MaxFileInputsCount="1" MaxFileSize="102400" OnClientValidationFailed="validationFailed">
                                            </telerik:RadAsyncUpload>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 0px">
                                            <asp:CheckBox CssClass="bodycopy" ID="chkRemoveLogo" runat="server" Text="Remove logo" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" colspan="3">
                                Additional Zip Codes:
                                <asp:TextBox ID="txtAdditionalZipCodes"  MaxLength="100" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" colspan="3">
                                <asp:Button SkinID="Button" runat="server" Text="Save" Width="100" ID="uxSave" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Search Categories
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" class="bodycopy">
                                These are the categories in which users of MascaroPorter.com will find this client
                                and the order in which the client will appear in search results.
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
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="Client_Id">
                                        <Columns>
                                            <telerik:GridBoundColumn Groupable="false" DataField="Client_Id" HeaderText="Client ID"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="100"
                                                Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="CategoryIDandName" HeaderText="Category"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="180">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="OrderPosition" HeaderText="Position"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="180"
                                                Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridCalculatedColumn UniqueName="CalculatedColumn" HeaderText="Position"
                                                DataFields="OrderPosition" Expression='{0} + 1' ItemStyle-Width="180">
                                            </telerik:GridCalculatedColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="HasExpandedRange" HeaderText="Expanded Range"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="180">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr style="visibility: hidden">
                            <td class="bodycopy" colspan="3">
                                <asp:Button SkinID="Button" runat="server" Text="Edit..." Width="100" ID="uxEditClientOrdering" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    <script type="text/javascript">
        function showClientLocalMap() {
            alert(txtAddress1.valueOf);
            uxClientLocalMap.href = txtAddress1.valueOf;
            return true;
            // https://maps.google.com/?q=1200+Pennsylvania+Ave+SE,+Washington,+District+of+Columbia,+20003
        }
    </script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            //<![CDATA[
            var $ = $telerik.$;
            function validationFailed(radAsyncUpload, args) {
                var $row = $(args.get_row());
                var erorMessage = getErrorMessage(radAsyncUpload, args);
                var span = createError(erorMessage);
                $row.addClass("ruError");
                $row.append(span);
            }

            function getErrorMessage(sender, args) {
                var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
                if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
                    if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
                        return ("This file type is not supported.");
                    }
                    else {
                        return ("This file exceeds the maximum allowed size of 100 KB.");
                    }
                }
                else {
                    return ("not correct extension.");
                }
            }

            function createError(erorMessage) {
                var input = '<div class="ruErrorMessage">' + erorMessage + ' </div>';
                return input;
            }

         //]]>
        </script>
    </telerik:RadScriptBlock>
</asp:Content>

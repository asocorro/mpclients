<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs"
    Inherits="MPClients.Users" Title="MPClients::Users" %>

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
                                    User Search
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Search Criteria</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            User Name:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="uxTextToSearch" Width="300"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Client:
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="uxClients" runat="server" AllowCustomText="True" ShowToggleImage="True"
                                                ShowMoreResultsBox="true" EnableLoadOnDemand="True" MarkFirstMatch="True" OnItemsRequested="RadComboBox1_ItemsRequested"
                                                EnableVirtualScrolling="true">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Show only active users:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkOnlyActive" Width="300"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Show only locked out users:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkOnlyLockedOut" Width="300"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Button SkinID="Button" runat="server" Text="Search" Width="120" ID="uxSearch" />&nbsp;&nbsp;
                                            <asp:Button SkinID="Button" runat="server" Text="Add User..." Width="120" ID="uxAddUser" />
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
                            <td align="right" style="padding-right: 10px; padding-top: 2px">
                                <telerik:RadButton runat="server" ID="uiExportResult" Text="Export to Excel" ToolTip="Export Results to Excel">
                                    <Icon PrimaryIconCssClass="rbDownload" />
                                </telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="True"
                                    AllowSorting="True" PageSize="50" ShowFooter="True" AllowMultiRowSelection="False"
                                    GridLines="Both" EnableAJAX="True" ShowStatusBar="True" Width="100%" AllowFilteringByColumn="false">
                                    <PagerStyle Mode="NumericPages" />
                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="120">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ClientID" HeaderText="Client ID" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="120">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ClientNameOnly" HeaderText="Client Name" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle" ItemStyle-Width="220">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="LastPriceQueryDate" HeaderText="Last Price Query Date"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="PriceQuerys" HeaderText="Price Queries" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridCheckBoxColumn DataField="IsActive" HeaderText="Is Active" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="IsLockedOut" HeaderText="Is Locked Out" HeaderStyle-VerticalAlign="Middle"
                                                ItemStyle-VerticalAlign="Middle">
                                            </telerik:GridCheckBoxColumn>
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

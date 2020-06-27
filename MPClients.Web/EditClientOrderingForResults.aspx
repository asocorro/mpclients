<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditClientOrderingForResults.aspx.cs" Inherits="MPClients.EditClientOrderingForResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function rowDblClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }
        </script>
    </telerik:RadCodeBlock>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr valign="top">
                <td align="left" class="tdBody">
                    <table style="border: 1px solid rgb(204, 204, 204);" border="0" cellpadding="0" cellspacing="0"
                        width="100%">
                        <tbody>
                            <tr>
                                <td class="mainheading" height="30">
                                    Client Ordering for Results
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td class="bodyTitleBak" height="22" colspan="2">
                                <div class="bodyTitle">
                                    Ordering
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" class="bodycopy">
                                Establish the order in which clients appear in search results, per category. Use
                                drag-and-drop to reorder.
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top" style="width: 10%">
                                Category:
                            </td>
                            <td>
                                <telerik:RadComboBox ID="uxCategories" runat="server" AllowCustomText="True" ShowToggleImage="True"
                                    ShowMoreResultsBox="true" EnableLoadOnDemand="True" MarkFirstMatch="True" OnItemsRequested="uxCategories_ItemsRequested"
                                    AutoPostBack="true" EnableVirtualScrolling="true" OnSelectedIndexChanged="uxCategories_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top">
                                Client:
                            </td>
                            <td>
                                <telerik:RadComboBox ID="uxClients" runat="server" AllowCustomText="True" ShowToggleImage="True"
                                    ShowMoreResultsBox="true" EnableLoadOnDemand="True" MarkFirstMatch="True" OnItemsRequested="uxClients_ItemsRequested"
                                    EnableVirtualScrolling="true">
                                </telerik:RadComboBox>
                                &nbsp;
                                <asp:Button  SkinID="Button" runat="server" Text="Add" Width="100" ID="uxAdd" />
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" valign="top" colspan="2">
                                <telerik:RadGrid ID="uxGrid" SkinID="Office2007" runat="server" AllowPaging="false"
                                    AllowSorting="false" PageSize="50" ShowFooter="True" AllowMultiRowSelection="False"
                                    GridLines="Both" EnableAJAX="True" ShowStatusBar="false" Width="100%" Visible="true"
                                    OnRowDrop="uxGrid_RowDrop" OnDeleteCommand="uxGrid_DeleteCommand" OnUpdateCommand="uxGrid_UpdateCommand"
                                    OnEditCommand="uxGrid_EditCommand" OnCancelCommand="uxGrid_CancelCommand">
                             
                                    <ClientSettings AllowRowsDragDrop="true">
                                        <ClientEvents OnRowDblClick="rowDblClick" />
                                        <Selecting AllowRowSelect="True"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="ClientId" EditMode="InPlace">
                                        <Columns>
                                            <%--<telerik:GridDragDropColumn HeaderStyle-Width="18px" />--%>
                                            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                            </telerik:GridEditCommandColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="CategoryName" HeaderText="Category"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Wrap="false"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="ClientId" HeaderText="Client ID"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ReadOnly="true" Visible="false">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn Groupable="false" DataField="Client" HeaderText="Client"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ReadOnly="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn Groupable="false" DataField="BusinessName" HeaderText="Business Name"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" ItemStyle-Wrap="false"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn Groupable="false" DataField="OrderPosition" HeaderText="Order Position"
                                                HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" Visible="false"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridCheckBoxColumn UniqueName="BoolField" ReadOnly="false" HeaderText="Expanded Range"
                                                DataField="HasExpandedRange">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridButtonColumn HeaderText="Delete" Text="Delete" CommandName="Delete"
                                                ButtonType="ImageButton" ConfirmText="Delete this record?" ConfirmDialogType="RadWindow"
                                                UniqueName="DeleteColumn" />
                                        </Columns>
                                        <EditFormSettings>
                                            <EditColumn ButtonType="ImageButton" />
                                        </EditFormSettings>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy" colspan="2">
                                <asp:Button SkinID="Button" runat="server" Text="Save" Width="100" ID="uxSave" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <telerik:RadListBox runat="server" ID="SavedChangesList" Width="200px" Height="100px"
                                    Visible="false">
                                </telerik:RadListBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

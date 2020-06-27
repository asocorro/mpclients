<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AvailableCategories.aspx.cs" Inherits="MPClients.AvailableCategories" %>

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
                                    Select Search Categories
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table class="bodyTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="bodyTitleBak" height="22">
                                <div class="bodyTitle">
                                    Selection
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" class="bodycopy">
                                Indicate which categories will be available to users in MascaroPorter.com to perform
                                product searches.
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style='overflow: auto; align: left; padding-top: 0px; padding-bottom: 0px;'>
                                    <asp:CheckBoxList runat='server' ID='uxCategories' RepeatColumns="4" RepeatDirection="Horizontal" CellPadding="2" CellSpacing="0">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td  class="bodycopy">
                                <asp:Button SkinID="Button" runat='server' ID='uxClearAll' Text='Clear All' />
                                <asp:Button SkinID="Button" runat='server' ID='uxSelectAll' Text='Select All' />
                            </td>
                        </tr>
                        <tr>
                            <td class="bodycopy">
                                <asp:Button SkinID="Button" runat="server" Text="Save" Width="100" ID="uxSave" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="LoadHoldsFile.aspx.cs" Inherits="MPClients.Web.LoadHoldsFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="server">

    <style>
        table.confirmation-table {
            border-collapse: collapse;
        }

            table.confirmation-table td, th {
                border: 1px solid rgb(200,200,200);
            }

            table.confirmation-table td, th {
                padding: 15px;
            }
    </style>

    <asp:HiddenField runat="server" ID="fileName" />

    <asp:Panel runat="server" ID="panel1" Visible="true">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" class="tdBody">
                    <table style="border: 1px solid rgb(204, 204, 204);" border="0" cellpadding="0" cellspacing="0"
                        width="100%">
                        <tbody>
                            <tr>
                                <td class="mainheading" height="30">Load Holds File
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>

        <table class="bodyTable" cellpadding="2" cellspacing="2" border="0">
            <tr>
                <td class="bodycopy" valign="top">
                    Please choose a file to upload.  It must be in .CSV format, with the first two columns being the ID of the client and the date of the hold.
                <br /><br />
                    We will assume the first line is the header and will skip it.
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <input id="filMyFile" type="file" runat="server" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
               <td><asp:Button runat="server" Text="Next >>" ID="btnUpload" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="panel2" Visible="false">

        <table border="0" cellpadding="2" cellspacing="2" width="100%">
            <tr>
            <td class="bodycopy">
            This is the data in the file:
            </td>
        </tr>
            <tr>
                <td class="bodycopy" colspan="2">
                    <asp:Repeater ID="rptUpload" runat="server">
                        <HeaderTemplate>
                            <table class="confirmation-table">
                                <tr>
                                    <th>Client ID</th>
                                    <th>Hold From Date (current)</th>
                                    <th>Hold From Date (from file)</th>
                                    <th>Messages</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lblClient_ID" runat="server" Text='<%# Eval("Client_ID") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblHoldDbFromDate" runat="server" Text='<%# Eval("HoldDbFromDate") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblHoldFromDate" runat="server" Text='<%# Eval("HoldFromDate") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" Text='<%# Eval("Message") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>

            </tr>
              <tr>
                <td><asp:Button runat="server" Text="<< Back" ID="btnBack"  Width="120"/> &nbsp;<asp:Button runat="server" Text="Import" ID="btnImport" Width="120" /></td>
            </tr>
        </table>
    </asp:Panel>


    <asp:Panel runat="server" ID="panel3" Visible="false">
        <table border="0" cellpadding="2" cellspacing="2" width="100%">
            <tr>
            <td class="bodycopy">
            The data was successfully loaded.
            </td>
        </tr>
        </table>
    </asp:Panel>
</asp:Content>

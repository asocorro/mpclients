<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Message.aspx.cs" Inherits="MPClients.Message" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MPClients::Message</title>

    <script type="text/javascript">  
        function CloseMessage()   
        {   
            GetRadWindow().Close();   
        }   
           
        function GetRadWindow()   
        {   
            var oWindow = null;   
            if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog   
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;//IE (and Moz az well)   
                   
            return oWindow;   
        }   

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="bodycopy">
            <asp:Label runat="server" ID="uxMessage"></asp:Label>
            <br />
            <br />
            <input type="button" onclick="javascript:CloseMessage();" title="Close" value="Close" />
        </div>
    </form>
</body>
</html>

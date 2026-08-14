<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="Vendor_Portal.Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        setTimeout('Redirect()', 2000);
        function Redirect() {
            location.href = 'Login.aspx';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; text-align: center; min-height: 400px; height: auto;">
            <span style="color: Black; font-family: 'Times New Roman'; font-size: 22px;">You have
            been logged out.</span>
        </div>
    </form>
</body>
</html>

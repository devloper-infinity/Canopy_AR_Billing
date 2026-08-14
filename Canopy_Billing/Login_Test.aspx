<%@ Page Language="C#" AutoEventWireup="true" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Infinity IPS Login</title>
    <style>
        *{margin:0;padding:0;box-sizing:border-box;font-family:'Segoe UI',sans-serif;}
        body{
            min-height:100vh;
            display:flex;
            justify-content:center;
            align-items:center;
            background:linear-gradient(135deg,#1e3a8a,#2563eb,#22c1dc);
        }
        .wrapper{
            width:1100px;
            height:620px;
            background:#fff;
            border-radius:24px;
            overflow:hidden;
            display:grid;
            grid-template-columns: 62% 38%;
            box-shadow:0 20px 50px rgba(0,0,0,.2);
        }
        .left{
            color:#fff;
            padding:42px;
            background:linear-gradient(135deg,#233b8f,#3568e8,#2ab7d6);
            position:relative;
        }
        .badge{
            display:inline-block;
            padding:10px 18px;
            border-radius:30px;
            background:rgba(255,255,255,.15);
        }
        h1{font-size:56px;margin:25px 0 15px;}
        .desc{max-width:520px;line-height:1.8;}
        .features{
            position:absolute;
            bottom:40px;
            display:flex;
            gap:14px;
        }
        .card{
            width:180px;
            padding:20px;
            border-radius:18px;
            background:rgba(255,255,255,.12);
            border:1px solid rgba(255,255,255,.18);
        }
        .right{
            padding:60px 35px;
            display:flex;
            align-items:center;
            justify-content:center;
        }
        .login{width:100%;max-width:320px;}
        .logo{text-align:center;margin-bottom:15px;font-weight:bold;}
        .title{text-align:center;font-size:36px;margin-bottom:8px;}
        .sub{text-align:center;color:#666;margin-bottom:25px;}
        .input{
            width:100%;
            padding:14px;
            margin-bottom:14px;
            border:1px solid #cfd8e3;
            border-radius:14px;
        }
        .btn{
            width:100%;
            padding:14px;
            border:none;
            border-radius:14px;
            color:#fff;
            background:linear-gradient(90deg,#2563eb,#11b6d8);
            cursor:pointer;
        }
    </style>

      <script>
        function login_submit() {
            var login_username = document.getElementById("login_username").value;
            var login_password = document.getElementById("login_password").value;
            if (login_username == "") {
                alert("Please enter username");
                return false;
            }
            if (login_password == "") {
                alert("Please enter password");
                return false;
            }

            __doPostBack("<%= btnLogin.UniqueID %>", '');
            return false;
        }
</script>
</head>
<body>
<form runat="server">
<div class="wrapper">
    <div class="left">
        <div class="badge">Secure Access Portal</div>
        <h1>Welcome to Infinity IPS</h1>
        <p class="desc">
            Access your enterprise dashboard, employee services,
            assets, reports and helpdesk tools from one secure workspace.
        </p>

       <%-- <div class="features">
            <div class="card">Dashboard & Productivity Insights</div>
            <div class="card">Secure Sign-in for Authorized Users</div>
            <div class="card">Service Desk & Reporting Access</div>
        </div>--%>
    </div>

    <div class="right">
        <div class="login">
            <div class="logo">INFINITY IPS</div>
            <div class="title">Sign In</div>
            <div class="sub">Use your account credentials to continue</div>

             <input type="text" id="login_username" placeholder="Username" name="login_username" class="form-control form-icon-input" style="width: 250px; text-transform: uppercase;" required />
            <input type="password" id="login_password" placeholder="Password" name="login_password" class="form-control form-icon-input" style="width: 250px;" required />
            <button id="login_btnsubmit" CssClass="btn" onclick="return login_submit();">Sign In</button>
             <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Style="display: none;" />
        </div>
    </div>
</div>
</form>
</body>
</html>
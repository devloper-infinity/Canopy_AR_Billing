<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Vendor_Portal.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome to Infinity IPS</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css" />
    <!-- icheck bootstrap -->
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <style>
        @import url('https://fonts.googleapis.com/css?family=Exo:400,700');

        * {
            margin: 0px;
            padding: 0px;
        }

        body {
        }


        .context {
            width: 100%;
            position: absolute;
            /* top:50vh!important;*/
            padding-left: 35%;
        }

            .context h1 {
                text-align: center;
                color: #fff;
                font-size: 50px;
            }


        .area {
            /*  background: #4e54c8;  
    background: -webkit-linear-gradient(to left, #8f94fb, #4e54c8);  */
            width: 100%;
            height: 100vh;
        }

        .circles {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            overflow: hidden;
        }

            .circles li {
                position: absolute;
                display: block;
                list-style: none;
                width: 20px;
                height: 20px;
                background: rgba(255, 255, 255, 0.5);
                animation: animate 25s linear infinite;
                bottom: -150px;
            }

                .circles li:nth-child(1) {
                    left: 25%;
                    width: 80px;
                    height: 80px;
                    animation-delay: 0s;
                }


                .circles li:nth-child(2) {
                    left: 10%;
                    width: 20px;
                    height: 20px;
                    animation-delay: 2s;
                    animation-duration: 12s;
                }

                .circles li:nth-child(3) {
                    left: 70%;
                    width: 20px;
                    height: 20px;
                    animation-delay: 4s;
                }

                .circles li:nth-child(4) {
                    left: 40%;
                    width: 60px;
                    height: 60px;
                    animation-delay: 0s;
                    animation-duration: 18s;
                }

                .circles li:nth-child(5) {
                    left: 65%;
                    width: 20px;
                    height: 20px;
                    animation-delay: 0s;
                }

                .circles li:nth-child(6) {
                    left: 75%;
                    width: 110px;
                    height: 110px;
                    animation-delay: 3s;
                }

                .circles li:nth-child(7) {
                    left: 35%;
                    width: 150px;
                    height: 150px;
                    animation-delay: 7s;
                }

                .circles li:nth-child(8) {
                    left: 50%;
                    width: 25px;
                    height: 25px;
                    animation-delay: 15s;
                    animation-duration: 45s;
                }

                .circles li:nth-child(9) {
                    left: 20%;
                    width: 15px;
                    height: 15px;
                    animation-delay: 2s;
                    animation-duration: 35s;
                }

                .circles li:nth-child(10) {
                    left: 85%;
                    width: 150px;
                    height: 150px;
                    animation-delay: 0s;
                    animation-duration: 11s;
                }



        @keyframes animate {

            0% {
                transform: translateY(0) rotate(0deg);
                opacity: 1;
                border-radius: 0;
            }

            100% {
                transform: translateY(-1000px) rotate(720deg);
                opacity: 0;
                border-radius: 50%;
            }
        }
    </style>

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
<body class="login-page" style="background-color: #ffffff; background: url('images/111.jpeg') no-repeat;">
    <div class="context">

           <div class="login-box">
            <div class="login-logo">
            </div>
            <div class="card">
                <div class="card-body login-card-body">
                    <p class="login-box-msg"><b>Sign in to start your session</b></p>
                    <div id="dvError" runat="server" role="alert"></div>
                    <form id="form1" runat="server">
                        <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Style="display: none;" />
                        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
                            <scripts>
                                <asp:ScriptReference Path="~/Scripts/Functions/Login.js" />

                            </scripts>
                        </asp:ToolkitScriptManager>
                        <div class="input-group mb-3" style="z-index: 1000;">
                            <input type="text" id="login_username" placeholder="Username" name="login_username" class="form-control form-icon-input" style="width: 250px; text-transform: uppercase;" required />

                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-envelope"></span>
                                </div>
                            </div>
                        </div>
                        <div class="input-group mb-3" style="z-index: 1000;">
                            <input type="password" id="login_password" placeholder="Password" name="login_password" class="form-control form-icon-input" style="width: 250px;" required />
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-lock"></span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-4">
                                <div class="icheck-primary">
                                    <input type="checkbox" id="chkRemember" runat="server" class="custom-checkbox" style="display: none;" />
                                    <label for="remember" style="display: none;">
                                        Remember Me             
                                    </label>
                                </div>
                            </div>
                            <!-- /.col -->
                            <div class="col-8" style="z-index: 1000;">
                                <button id="login_btnsubmit" name="login_btnsubmit" class="btn btn-primary" onclick="return login_submit();">Sign In</button>


                            </div>
                            <!-- /.col -->
                        </div>
                    </form>


                    <!-- /.social-auth-links -->

                    <p class="mb-1">
                        <br />
                        <br />
                    </p>
                    <p class="mb-0">
                    </p>
                </div>
                <!-- /.login-card-body -->
            </div>
        </div>

    </div>

 



    <div class="area">
        <ul class="circles">
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
        </ul>
    </div>
    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>

</body>
</html>


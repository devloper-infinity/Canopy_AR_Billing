<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Vendor_Portal.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Canopy Billing Sign In</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css" />
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <link rel="stylesheet" href="dist/css/canopy-modern.css?v=20260619" />
    <style>
        /* MFA QR Code */
        .mfa-qr-wrapper {
            display: flex;
            justify-content: center;
            margin: 1rem 0;
        }

            .mfa-qr-wrapper img,
            .mfa-qr-wrapper canvas {
                border: 6px solid #fff;
                border-radius: 8px;
                box-shadow: 0 2px 8px rgba(0,0,0,0.15);
            }

        /* Manual key fallback collapsible */
        .mfa-manual-fallback {
            margin-top: 0.75rem;
            text-align: center;
        }

            .mfa-manual-fallback summary {
                cursor: pointer;
                font-size: 0.85rem;
                color: #6c757d;
                user-select: none;
            }

                .mfa-manual-fallback summary:hover {
                    color: #343a40;
                }

        .mt-2 {
            margin-top: 0.5rem;
        }
    </style>
</head>
<body class="canopy-login-page">
    <form id="form1" runat="server" class="login-shell" autocomplete="off">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
            <Scripts>
                <asp:ScriptReference Path="~/Scriptms/Functions/canopy-common.js" />
                <asp:ScriptReference Path="~/Scripts/Functions/Login.js" />
            </Scripts>
        </asp:ToolkitScriptManager>
        <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Style="display: none;" UseSubmitBehavior="false" />
        <input type="hidden" id="login_stage" name="login_stage" value="<%= IsMfaStep ? "mfa" : "password" %>" />
        <input type="hidden" id="login_postback_target" value="<%= btnLogin.UniqueID %>" />

        <section class="login-brand-panel" aria-label="Canopy Billing">
            <div class="login-brand-copy">
                <p class="login-brand-kicker">Canopy Financial Technology Partners</p>
                <h1 class="login-brand-title">Billing operations, secured.</h1>
                <div class="login-brand-meta" aria-label="Application areas">
                    <span>Billing</span>
                    <span>Invoices</span>
                    <span>Reports</span>
                    <span>MFA</span>
                </div>
            </div>
        </section>

        <section class="login-card-panel">
            <div class="login-card">
                <img src="images/logo-canopy-1.png" class="login-logo-img" alt="Canopy" />
                <h1><%= IsMfaStep ? "Verify your sign in" : "Sign in" %></h1>
                <p><%= Server.HtmlEncode(LoginSubtitle) %></p>

                <div id="dvError" runat="server" role="alert" style="display: none;"></div>

                <% if (!IsMfaStep)
                    { %>
                <div class="login-field">
                    <label for="login_username">Username</label>
                    <div class="login-input-group">
                        <span class="field-icon fas fa-user" aria-hidden="true"></span>
                        <input type="text" id="login_username" placeholder="Username" name="login_username" class="form-control" autocomplete="username" required autofocus />
                    </div>
                </div>

                <div class="login-field">
                    <label for="login_password">Password</label>
                    <div class="login-input-group">
                        <span class="field-icon fas fa-lock" aria-hidden="true"></span>
                        <input type="password" id="login_password" placeholder="Password" name="login_password" class="form-control" autocomplete="current-password" required />
                    </div>
                </div>

                <div class="login-actions">
                    <label class="remember-option" for="chkRemember">
                        <input type="checkbox" id="chkRemember" runat="server" />
                        <span>Remember this session</span>
                    </label>
                    <button id="login_btnsubmit" name="login_btnsubmit" type="button" class="btn btn-primary" onclick="return login_submit();">
                        <span class="fas fa-arrow-right mr-2" aria-hidden="true"></span>Sign In
                   
                    </button>
                </div>
                <% }
                    else
                    { %>
                <% if (ShowMfaSetup)
                    { %>
                <div class="mfa-setup-panel">
                    <p class="mfa-note"><strong>Set up multi-factor authentication</strong></p>
                    <p class="mfa-note">Scan the QR code with your authenticator app, then enter the 6-digit code it generates.</p>

                    <!-- QR Code -->
                    <div class="mfa-qr-wrapper">
                        <div id="mfa_qrcode"></div>
                    </div>

                    <!-- Manual fallback -->
                    <details class="mfa-manual-fallback">
                        <summary>Can't scan? Enter the key manually</summary>
                        <p class="mfa-note mt-2">Type this key into your authenticator app:</p>
                        <code class="mfa-manual-key"><%= Server.HtmlEncode(MfaManualKey) %></code>
                    </details>

                    <!-- Hidden OTP URI for JS to read -->
                    <input type="hidden" id="mfa_otp_uri" value="<%= Server.HtmlEncode(MfaOtpUri) %>" />
                </div>
                <div class="mfa-setup-panel" style="display: none;">
                    <p class="mfa-note"><strong>Set up multi-factor authentication</strong></p>
                    <p class="mfa-note">Add this setup key in your authenticator app, then enter the 6-digit code it generates.</p>
                    <code class="mfa-manual-key"><%= Server.HtmlEncode(MfaManualKey) %></code>
                    <%--<a href="<%= Server.HtmlEncode(MfaOtpUri) %>">Open authenticator setup link</a>--%>
                </div>
                <% } %>

                <div class="login-field">
                    <label for="mfa_code">Verification code</label>
                    <div class="login-input-group">
                        <span class="field-icon fas fa-shield-alt" aria-hidden="true"></span>
                        <input type="text" id="mfa_code" placeholder="6-digit code" name="mfa_code" class="form-control" inputmode="numeric" pattern="[0-9]*" maxlength="6" autocomplete="one-time-code" required autofocus />
                    </div>
                </div>

                <div class="login-actions">
                    <a href="Login.aspx" class="btn btn-default">Cancel</a>
                    <button id="login_btnsubmit" name="login_btnsubmit" type="button" class="btn btn-primary" onclick="return login_submit();">
                        <span class="fas fa-check mr-2" aria-hidden="true"></span>Verify
                   
                    </button>
                </div>
                <% } %>
            </div>
        </section>
    </form>

    <script src="plugins/jquery/jquery.min.js"></script>
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="dist/js/adminlte.min.js"></script>
    <% if (IsMfaStep && ShowMfaSetup)
        { %>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
    <script>
        (function () {
            var uri = document.getElementById('mfa_otp_uri');
            if (uri && uri.value) {
                new QRCode(document.getElementById('mfa_qrcode'), {
                    text: uri.value,
                    width: 180,
                    height: 180,
                    colorDark: '#000000',
                    colorLight: '#ffffff',
                    correctLevel: QRCode.CorrectLevel.M
                });
            }
        })();
</script>
    <% } %>
</body>
</html>

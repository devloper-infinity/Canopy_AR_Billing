using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using Vendor_Portal.App_Code.BLL;
using Vendor_Portal.App_Code.EL;

namespace Vendor_Portal
{
    public partial class Login : System.Web.UI.Page
    {
        private const string PendingLoginSessionKey = "CanopyPendingMfaLogin";
        private const string PendingMfaSecretSessionKey = "CanopyPendingMfaSecret";
        private readonly bllLogin loginService = new bllLogin();

        protected bool IsMfaStep { get; private set; }
        protected bool ShowMfaSetup { get; private set; }
        protected string MfaManualKey { get; private set; }
        protected string MfaOtpUri { get; private set; }
        protected string LoginSubtitle { get; private set; }

        private string returnUrl
        {
            get
            {
                if (ViewState["returnUrl"] == null)
                {
                    ViewState["returnUrl"] = string.Empty;
                }

                return (string)ViewState["returnUrl"];
            }
            set
            {
                ViewState["returnUrl"] = value ?? string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ResetPresentation();
            if (!IsPostBack || Request.QueryString["ReturnUrl"] != null)
            {
                returnUrl = Request.QueryString["ReturnUrl"];
            }

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                RedirectAuthenticatedUser();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                HideError();
                string stage = Convert.ToString(Request.Form["login_stage"]);

                if (string.Equals(stage, "mfa", StringComparison.OrdinalIgnoreCase))
                {
                    CompleteMfaChallenge();
                }
                else
                {
                    StartPasswordChallenge();
                }
            }
            catch
            {
                PendingLogin pendingLogin = GetPendingLogin();
                if (pendingLogin != null)
                {
                    ShowMfaChallenge(pendingLogin, false, null);
                }

                ShowError("Unable to complete sign in. Please try again or contact your administrator.");
            }
        }

        private void StartPasswordChallenge()
        {
            string userID = Convert.ToString(Request.Form["login_username"]).Trim();
            string pwd = Convert.ToString(Request.Form["login_password"]);

            if (string.IsNullOrWhiteSpace(userID))
            {
                ShowError("Please enter username.");
                return;
            }

            if (string.IsNullOrEmpty(pwd))
            {
                ShowError("Please enter password.");
                return;
            }

            UpdateRememberedUser(userID);

            DataTable blockedUser = loginService.BlockUserLogin(userID);
            string encPassword = loginService.Encrypt(pwd);
            int returnValue = ValidatePassword(userID, pwd, encPassword);

            if (returnValue == -1)
            {
                ShowError("User does not exist.");
                return;
            }

            if (returnValue == 0)
            {
                ShowError("Invalid password.");
                return;
            }

            if (blockedUser != null && blockedUser.Rows.Count > 0)
            {
                ShowError("Your login has been blocked. Please contact your reporting manager.");
                return;
            }

            DataTable user = loginService.GetUserById(returnValue, Filter.SQLInjectionFilter(userID), Filter.SQLInjectionFilter(encPassword));
            if (user == null || user.Rows.Count <= 0)
            {
                user = loginService.GetUserById(returnValue, Filter.SQLInjectionFilter(userID), Filter.SQLInjectionFilter(pwd));
            }

            if (user == null || user.Rows.Count <= 0)
            {
                ShowError("Unable to load your user profile. Please contact your administrator.");
                return;
            }

            PendingLogin pendingLogin = new PendingLogin
            {
                EmployeeId = Convert.ToInt32(user.Rows[0]["EmployeeId"]),
                Role = Convert.ToString(user.Rows[0]["Role"]),
                UserName = userID,
                IsPersistent = chkRemember.Checked,
                RequiresPasswordReset = PasswordRequiresReset(pwd),
                ReturnUrl = GetSafeReturnUrl(returnUrl)
            };
#if DEBUG
            CompleteLogin(pendingLogin);
#else
            Session[PendingLoginSessionKey] = pendingLogin;
            BeginMfaChallenge(pendingLogin);
#endif
        }

        private void BeginMfaChallenge(PendingLogin pendingLogin)
        {
            DataTable mfaSetting = loginService.GetUserMfaSetting(pendingLogin.EmployeeId);
            if (mfaSetting != null && mfaSetting.Rows.Count > 0)
            {
                Session.Remove(PendingMfaSecretSessionKey);
                ShowMfaChallenge(pendingLogin, false, null);
                return;
            }

            string newSecret = MfaService.GenerateSecret();
            Session[PendingMfaSecretSessionKey] = newSecret;
            ShowMfaChallenge(pendingLogin, true, newSecret);
        }

        private void CompleteMfaChallenge()
        {
            PendingLogin pendingLogin = GetPendingLogin();
            if (pendingLogin == null)
            {
                ShowError("Your sign-in session expired. Please sign in again.");
                return;
            }

            string code = Convert.ToString(Request.Form["mfa_code"]);
            string enrollmentSecret = Convert.ToString(Session[PendingMfaSecretSessionKey]);

            if (!string.IsNullOrWhiteSpace(enrollmentSecret))
            {
                if (!MfaService.ValidateCode(enrollmentSecret, code))
                {
                    ShowMfaChallenge(pendingLogin, true, enrollmentSecret);
                    ShowError("Invalid verification code.");
                    return;
                }

                string protectedSecret = MfaService.ProtectSecret(enrollmentSecret);
                loginService.SaveUserMfaSetting(pendingLogin.EmployeeId, pendingLogin.UserName, protectedSecret);
                loginService.MarkUserMfaVerified(pendingLogin.EmployeeId);
                CompleteLogin(pendingLogin);
                return;
            }

            DataTable mfaSetting = loginService.GetUserMfaSetting(pendingLogin.EmployeeId);
            if (mfaSetting == null || mfaSetting.Rows.Count <= 0)
            {
                ShowError("MFA enrollment was not found. Please sign in again.");
                Session.Remove(PendingLoginSessionKey);
                return;
            }

            string secret = MfaService.UnprotectSecret(Convert.ToString(mfaSetting.Rows[0]["ProtectedSecret"]));
            if (!MfaService.ValidateCode(secret, code))
            {
                ShowMfaChallenge(pendingLogin, false, null);
                ShowError("Invalid verification code.");
                return;
            }

            loginService.MarkUserMfaVerified(pendingLogin.EmployeeId);
            CompleteLogin(pendingLogin);
        }

        private int ValidatePassword(string userID, string pwd, string encPassword)
        {
            int encryptedPasswordResult = loginService.ValidateUser(
                Filter.SQLInjectionFilter(userID),
                Filter.SQLInjectionFilter(encPassword));

            if (encryptedPasswordResult != 0)
            {
                return encryptedPasswordResult;
            }

            return loginService.ValidateUser(
                Filter.SQLInjectionFilter(userID),
                Filter.SQLInjectionFilter(pwd));
        }

        private void CompleteLogin(PendingLogin pendingLogin)
        {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                Convert.ToString(pendingLogin.EmployeeId),
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                pendingLogin.IsPersistent,
                pendingLogin.Role,
                FormsAuthentication.FormsCookiePath);

            string hash = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash)
            {
                HttpOnly = true,
                Secure = Request.IsSecureConnection
            };

            if (authTicket.IsPersistent)
            {
                authCookie.Expires = authTicket.Expiration;
            }

            Response.Cookies.Add(authCookie);
            Session.Remove(PendingLoginSessionKey);
            Session.Remove(PendingMfaSecretSessionKey);

            if (string.Equals(pendingLogin.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                Session["User"] = "Admin";
            }

            if (pendingLogin.RequiresPasswordReset && File.Exists(Server.MapPath("~/ResetPassword.aspx")))
            {
                RedirectTo("~/ResetPassword.aspx");
                return;
            }

            if (!string.IsNullOrWhiteSpace(pendingLogin.ReturnUrl))
            {
                RedirectTo(pendingLogin.ReturnUrl);
                return;
            }

            RedirectTo("~/BDM/Dashboard.aspx");
        }

        private void ShowMfaChallenge(PendingLogin pendingLogin, bool setupRequired, string secret)
        {
            IsMfaStep = true;
            ShowMfaSetup = setupRequired;
            LoginSubtitle = setupRequired
                ? "Pair your authenticator app before continuing."
                : "Enter the current code from your authenticator app.";

            if (setupRequired && !string.IsNullOrWhiteSpace(secret))
            {
                MfaManualKey = MfaService.FormatSecretForDisplay(secret);
                MfaOtpUri = MfaService.BuildOtpAuthUri("Canopy Billing", pendingLogin.UserName, secret);
            }
        }

        private void UpdateRememberedUser(string userID)
        {
            if (chkRemember.Checked)
            {
                Response.Cookies["userid"].Value = userID;
                Response.Cookies["userid"].Expires = DateTime.Now.AddMinutes(30);
            }
            else
            {
                Response.Cookies["userid"].Expires = DateTime.Now.AddMinutes(-1);
            }

            Response.Cookies["pwd"].Expires = DateTime.Now.AddMinutes(-1);
        }

        private void RedirectAuthenticatedUser()
        {
            string safeReturnUrl = GetSafeReturnUrl(returnUrl);
            if (!string.IsNullOrWhiteSpace(safeReturnUrl))
            {
                RedirectTo(safeReturnUrl);
                return;
            }

            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                Session["User"] = "Admin";
            }

            RedirectTo("~/BDM/Dashboard.aspx");
        }

        private string GetSafeReturnUrl(string requestedReturnUrl)
        {
            if (string.IsNullOrWhiteSpace(requestedReturnUrl))
            {
                return string.Empty;
            }

            if (requestedReturnUrl.StartsWith("//") || requestedReturnUrl.StartsWith("\\"))
            {
                return string.Empty;
            }

            Uri parsed;
            return Uri.TryCreate(requestedReturnUrl, UriKind.Relative, out parsed) ? requestedReturnUrl : string.Empty;
        }

        private bool PasswordRequiresReset(string password)
        {
            return !string.IsNullOrEmpty(password) &&
                   password.IndexOf("INFINITY", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private PendingLogin GetPendingLogin()
        {
            return Session[PendingLoginSessionKey] as PendingLogin;
        }

        private void ResetPresentation()
        {
            IsMfaStep = false;
            ShowMfaSetup = false;
            MfaManualKey = string.Empty;
            MfaOtpUri = string.Empty;
            LoginSubtitle = "Use your Canopy Billing credentials to continue.";
            HideError();
        }

        private void ShowError(string message)
        {
            dvError.Style["display"] = "block";
            dvError.Attributes["class"] = "alert alert-danger";
            dvError.InnerText = message;
        }

        private void HideError()
        {
            if (dvError == null)
            {
                return;
            }

            dvError.Style["display"] = "none";
            dvError.Attributes["class"] = string.Empty;
            dvError.InnerHtml = string.Empty;
        }

        private void RedirectTo(string url)
        {
            Response.Redirect(url, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        [Serializable]
        private sealed class PendingLogin
        {
            public int EmployeeId { get; set; }
            public string UserName { get; set; }
            public string Role { get; set; }
            public bool IsPersistent { get; set; }
            public bool RequiresPasswordReset { get; set; }
            public string ReturnUrl { get; set; }
        }
    }
}

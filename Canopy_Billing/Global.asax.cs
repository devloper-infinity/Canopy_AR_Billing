using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Vendor_Portal.App_Code.BLL;

namespace Canopy_Billing
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id =
                            (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        string userData = ticket.UserData;
                        string[] roles = userData.Split(',');
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);
                    }
                }
            }
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null) return;
            string file = VirtualPathUtility.GetFileName(context.Request.AppRelativeCurrentExecutionFilePath).ToLowerInvariant();
            bool protectedCosting = file == "projectdetails.aspx" || file == "priceconfiguration.aspx" || file == "raterevisionconfiguration.aspx";
            if (!protectedCosting) return;
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            if (CostingSecurityService.HasActiveAccess()) return;

            if (context.Request.HttpMethod == "POST")
            {
                context.Response.StatusCode = 403;
                context.Response.TrySkipIisCustomErrors = true;
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"Message\":\"Costing Email OTP verification is required.\"}");
                context.ApplicationInstance.CompleteRequest();
                return;
            }

            context.Response.Redirect("~/BDM/ProjectDetailsMaster.aspx", false);
            context.ApplicationInstance.CompleteRequest();
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}

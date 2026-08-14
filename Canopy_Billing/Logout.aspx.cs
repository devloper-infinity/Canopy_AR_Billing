using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vendor_Portal
{
    public partial class Logout : System.Web.UI.Page
    {
        public string localIP;
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddYears(-1);
            try
            {

                if (Convert.ToString(Request.QueryString["var"]) == "s")
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch { }
        }

        public string GetLocalIPaddress()
        {
            localIP = "";
            string strIP = String.Empty;
            try
            {
                string lhostname = string.Empty;
                try
                {
                    lhostname = Dns.GetHostName();
                    IPHostEntry iphost;
                    IPAddress[] ip;
                    iphost = Dns.GetHostEntry(lhostname);
                    ip = iphost.AddressList;
                    if (Convert.ToString(ip[0]).Trim().Contains("192"))
                    {
                        localIP = ip[0].ToString();
                    }
                    else
                    {
                        localIP = ip[1].ToString();
                    }


                    HttpRequest httpReq = HttpContext.Current.Request;

                    //test for non-standard proxy server designations of client's IP
                    if (httpReq.ServerVariables["HTTP_CLIENT_IP"] != null)
                    {
                        strIP = httpReq.ServerVariables["HTTP_CLIENT_IP"].ToString();
                    }
                    else if (httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        strIP = httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    //test for host address reported by the server
                    else if
                    (
                        //if exists
                        (httpReq.UserHostAddress.Length != 0)
                        &&
                        //and if not localhost IPV6 or localhost name
                        ((httpReq.UserHostAddress != "::1") || (httpReq.UserHostAddress != "localhost"))
                    )
                    {
                        strIP = httpReq.UserHostAddress;
                    }
                    //finally, if all else fails, get the IP from a web scrape of another server
                    else
                    {
                        WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                        using (WebResponse response = request.GetResponse())
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            strIP = sr.ReadToEnd();
                        }
                        //scrape ip from the html
                        int i1 = strIP.IndexOf("Address: ") + 9;
                        int i2 = strIP.LastIndexOf("</body>");
                        strIP = strIP.Substring(i1, i2 - i1);
                    }

                    return localIP + "||" + strIP;
                }
                catch
                {
                    return localIP + "||" + strIP; ;
                }
            }
            catch
            {
                return localIP + "||" + strIP; ;
            }
        }

        public string GetLocalIPaddress1()
        {
            localIP = "";
            try
            {
                string lhostname = string.Empty;
                try
                {
                    lhostname = Dns.GetHostName();
                    IPHostEntry iphost;
                    IPAddress[] ip;
                    iphost = Dns.GetHostEntry(lhostname);
                    ip = iphost.AddressList;
                    if (Convert.ToString(ip[0]).Trim().Contains("192"))
                    {
                        localIP = ip[0].ToString();
                    }
                    else
                    {
                        localIP = ip[1].ToString();
                    }
                    return localIP;
                }
                catch
                {
                    return localIP;
                }
            }
            catch
            {
                return localIP;
            }
        }
    }
}
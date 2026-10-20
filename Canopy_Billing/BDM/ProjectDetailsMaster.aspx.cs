using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.BDM
{
    public partial class ProjectDetailsMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }

        [WebMethod(EnableSession = true)]
        public static CostingSecurityService.AccessState GetCostingAccessState()
        {
            return CostingSecurityService.GetState();
        }

        [WebMethod(EnableSession = true)]
        public static CostingSecurityService.AccessState SendCostingEmailOtp()
        {
            return CostingSecurityService.SendOtp();
        }

        [WebMethod(EnableSession = true)]
        public static CostingSecurityService.AccessState VerifyCostingOtp(string code)
        {
            return CostingSecurityService.Verify(code);
        }

        [WebMethod(EnableSession = true)]
        public static string GteAllConfiguredProjects()
        {
            if (!CostingSecurityService.HasActiveAccess()) throw new HttpException(403, "Costing OTP verification is required.");
            DataTable dt1 = new bllTracking().ViewAllProjectApp();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            if (dt1 != null)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt1.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            return ser.Serialize(rows);
        }
    }
}

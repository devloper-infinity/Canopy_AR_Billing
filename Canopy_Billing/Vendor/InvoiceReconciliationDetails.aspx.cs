using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.Vendor
{
    public partial class InvoiceReconciliationDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetStewartDataForReconcile(string Month, string Year)
        {
            DataTable dt1 = new bllInvoice().GetStewartDataForReconcile(Month, Year);
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

        [WebMethod]
        public static int VerifyLoans(int BillingID, string DisputeValue, string UserRemark)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam.Add("BillingId", BillingID);
            htParam.Add("DisputeValue", DisputeValue);
            htParam.Add("UserRemark", UserRemark);
            htParam.Add("AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            returnvalue = new bllInvoice().VerifyLoanNo(htParam);
            return returnvalue;
        }
    }
}
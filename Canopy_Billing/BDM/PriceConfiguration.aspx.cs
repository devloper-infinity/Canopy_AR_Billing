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

namespace Vendor_Portal.BDM
{
    public partial class PriceConfiguration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string getAllBillingParameters(int ProjectID, string ClientProcess)
        {
            Hashtable htParam = new Hashtable();
            htParam.Add("DomainId", 9);
            htParam.Add("ProjectID", ProjectID);
            htParam.Add("ClientProcess", ClientProcess);
            DataTable dt1 = new bllTracking().getAllBillingParameters(htParam);
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
        public static string GetProjectDetailsbyprocessID(int ProcessID)
        {
            DataTable dt1 = new bllTracking().getProjectDetailsByprocessID(ProcessID);
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
        public static int InsertCosting(int ProjectID, string ProcessName, string BasePrice, string AllParams)
        {
            int returnvalue = 0;
            string[] parameters = AllParams.Split('|');
            if(parameters.Length>0)
            {
                Hashtable Htparam = new Hashtable();
                Htparam.Add("IBV_ParameterId", Convert.ToString(1));
                Htparam.Add("IBV_Comment", Convert.ToString("Yes"));
                Htparam.Add("IBV_Additional", Convert.ToString("Bundled"));
                Htparam.Add("IBV_UWVerification", Convert.ToString("No"));
                Htparam.Add("IBV_Remark", Convert.ToString(BasePrice));

                Htparam.Add("IBV_ChargeType", Convert.ToString("Fix Amount"));
                Htparam.Add("IBV_isBundle", Convert.ToString("Included in Bundle"));

                Htparam.Add("IBV_CommentFromBDM", Convert.ToString(""));
                Htparam.Add("AddedBy", Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                Htparam.Add("ProjectId", ProjectID);
                Htparam.Add("ClientProcess", ProcessName);
                Htparam.Add("CostingColumn", Convert.ToString("Loan#"));
                int result = new bllTracking().InsertBillParameterUW(Htparam);
            }
            foreach (string paramlist in parameters)
            {
                if (paramlist != "")
                {
                    string[] inputs = paramlist.Split('~');
                    string ParameterID = inputs[0];
                    string BillingType = inputs[1];
                    string Price = inputs[2];
                    string ChargeType = inputs[3];
                    string BillingHeader = inputs[4];
                    //string ProjectID = Convert.ToString(HttpContext.Current.Request.Form["price_project_id"]);
                    //string ProcessName = Convert.ToString(HttpContext.Current.Request.Form["price_process_name"]);
                    Hashtable htParam = new Hashtable();
                    htParam.Add("IBV_ParameterId", Convert.ToString(ParameterID));
                    htParam.Add("IBV_Comment", Convert.ToString("Yes"));
                    htParam.Add("IBV_Additional", Convert.ToString(BillingType));
                    htParam.Add("IBV_UWVerification", Convert.ToString("No"));
                    htParam.Add("IBV_Remark", Convert.ToString(Price));

                    htParam.Add("IBV_ChargeType", Convert.ToString(ChargeType));
                    htParam.Add("IBV_isBundle", Convert.ToString("Select"));

                    htParam.Add("IBV_CommentFromBDM", Convert.ToString(""));
                    htParam.Add("AddedBy", Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                    htParam.Add("ProjectId", ProjectID);
                    htParam.Add("ClientProcess", Convert.ToString(ProcessName));
                    htParam.Add("CostingColumn", Convert.ToString(BillingHeader));
                    returnvalue = new bllTracking().InsertBillParameterUW(htParam);
                }
            }
            return returnvalue;
        }
    }
}
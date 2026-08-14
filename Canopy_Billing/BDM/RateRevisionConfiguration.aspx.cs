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

namespace Canopy_Billing.BDM
{
    public partial class RateRevisionConfiguration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string getAllBillingParameters_Rate(int ProjectID, string ClientProcess)
        {
            Hashtable htParam = new Hashtable();
            htParam.Add("DomainId", 9);
            htParam.Add("ProjectID", ProjectID);
            htParam.Add("ClientProcess", ClientProcess);
            DataTable dt1 = new bllTracking().getAllBillingParameters_RateRevision(htParam);
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
        public static int UpdateEmailList(int ProjectID, string EmailList)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam.Add("ProjectID", ProjectID);
            htParam.Add("EmailList", EmailList);
            returnvalue = new bllTracking().UpdateReminerEmailList(ProjectID, EmailList);
            return returnvalue;
        }

        [WebMethod]
        public static int InsertRateRevisionConfiguration(int ProjectID, int ProcessID, string AllParams)
        {
            int returnvalue = 0;
            string[] parameters = AllParams.Split('|');
            if (parameters.Length > 0)
            {
                foreach (string paramlist in parameters)
                {
                    if (paramlist != "")
                    {
                        string[] inputs = paramlist.Split('~');
                        string ParameterID = inputs[0];
                        string CurrentRate = inputs[1];
                        string EffectiveDate = inputs[2];
                        string RevisionType = inputs[3];
                        string RevisionDate = inputs[4];
                        string ReminderDays = inputs[5];
                        Hashtable htParam = new Hashtable();
                        htParam.Add("ProjectID", ProjectID);
                        htParam.Add("ProcessID", ProcessID);
                        htParam.Add("ParameterID", ParameterID);
                        htParam.Add("CurrentRate", CurrentRate);
                        htParam.Add("EffectiveDate", EffectiveDate);
                        htParam.Add("RevisionType", RevisionType);
                        htParam.Add("ReminderDate", RevisionDate);
                        htParam.Add("ReminderDays", ReminderDays);
                        //htParam.Add("EmailList", EmailList);
                        htParam.Add("AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
                        returnvalue = new bllTracking().InsertRateRevisionConfiguration(htParam);
                    }
                }
            }
            return returnvalue;
        }
    }
}
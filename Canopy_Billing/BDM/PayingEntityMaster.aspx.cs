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
    public partial class PayingEntityMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetPendingDealsForPayingEntity(string Month, string Year)
        {
            DataTable dt1 = new bllTracking().getPendingBillingForpayingEntity(Month, Year);
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
        public static int UpdatePayingEntity(string Parameters)
        {
            int returnvalue = 0;
            string DealNo = "";
            string Client = "";
            string PurchaseEntity = "";
            string PayingEntity = "";
            string[] allparams = Parameters.Split(':');
            foreach(string paramlist in allparams)
            {

                string[] params1 = paramlist.Split('~');
                DealNo = params1[0];
                Client = params1[1];
                PurchaseEntity = params1[2];
                PayingEntity = params1[3];
                Hashtable htParam = new Hashtable();
                htParam.Add("DealNo", DealNo);
                htParam.Add("Client", Client);
                htParam.Add("PurchaseEntity", PurchaseEntity);
                htParam.Add("PayingEntity", PayingEntity);
                returnvalue = new bllTracking().UpdatePayingEntity(htParam);
            }
            
            return returnvalue;
        }
    }
}
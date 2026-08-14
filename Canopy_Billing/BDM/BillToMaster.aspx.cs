using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;
using Vendor_Portal.App_Code.DAL;

namespace Canopy_Billing.BDM
{
    public partial class BillToMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        [WebMethod]
        public static void SaveBillToMaster(string transactionIdentifier, string buyerName, string sellerName, string BillTo)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO BillToMaster(transactionIdentifier, buyerName,sellerName,BillTo) OUTPUT INSERTED.MasterId VALUES(@transactionIdentifier, @buyerName,@sellerName, @BillTo)", con);
                cmd.Parameters.AddWithValue("@transactionIdentifier", transactionIdentifier);
                cmd.Parameters.AddWithValue("@buyerName", buyerName);
                cmd.Parameters.AddWithValue("@sellerName", sellerName);
                cmd.Parameters.AddWithValue("@BillTo", BillTo);
                int masterId = (int)cmd.ExecuteScalar();

            
            }
        }


        [WebMethod]
        public static string GetBillToMaster()
        {
            DataTable dt1 = new bllTracking().GetBillToMaster();
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
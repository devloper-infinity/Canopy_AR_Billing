using System;
using System.Collections;
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

namespace Vendor_Portal.BDM
{
    public partial class OtherCosting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetAllOtherCosting()
        {
            DataTable dt1 = new bllTracking().GetAllOtherCosting();
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
        public static int InsertOtherRates(int ProjectId, string Rate, string Type)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam.Add("ProjectId", ProjectId);
            htParam.Add("Rate", Rate);
            htParam.Add("Type", Type);
            htParam.Add("AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            returnvalue = new bllTracking().InsertRates(htParam);
            return returnvalue;
        }

        [WebMethod]
        public static int UpdateOtherRates(int ProjectId, string Rate, string Type)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam.Add("ProjectId", ProjectId);
            htParam.Add("Rate", Rate);
            htParam.Add("Type", Type);
            htParam.Add("AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            returnvalue = new bllTracking().InsertRates(htParam);
            return returnvalue;
        }

        [WebMethod]
        public static string GetOtherCostingHistory(int id)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT 
                L.*,
                P.ClientName, E.FirstName+' '+E.lastName as ChangedByName
            FROM OtherCosting_Log L
            LEFT JOIN Clients P ON P.ClientID = L.ProjectID
            left join InfinityERP.dbo.EmployeeInfo E on E.EmployeeID = L.ChangedBy
            WHERE RefID = @ID
            ORDER BY ChangedDate DESC
        ", con);

                da.SelectCommand.Parameters.AddWithValue("@ID", id);

                DataTable dt1 = new DataTable();
                da.Fill(dt1);
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

}
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
        public static object GetCostingConfiguration(int rateId)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT RateID, ProjectId, Rate, Type, BillingMethod, MinimumAmount, MaximumCap,
                       EffectiveFrom
                FROM SecuritizationRelianceLetterCosting WHERE RateID=@RateID;
                SELECT CostingDetailID, ProductType, Rate
                FROM SecuritizationRelianceLetterCostingDetail
                WHERE RateID=@RateID AND IsActive=1 ORDER BY ProductType;", con))
            {
                cmd.Parameters.Add("@RateID", SqlDbType.Int).Value = rateId;
                DataSet ds = new DataSet();
                new SqlDataAdapter(cmd).Fill(ds);
                if (ds.Tables[0].Rows.Count == 0) return new { Status = false, Message = "Configuration was not found." };
                return new
                {
                    Status = true,
                    Header = ToDictionary(ds.Tables[0].Rows[0]),
                    Details = ds.Tables.Count > 1 ? ToDictionaries(ds.Tables[1]) : new List<Dictionary<string, object>>()
                };
            }
        }

        [WebMethod]
        public static object SaveCostingConfiguration(CostingConfigurationModel model)
        {
            if (model == null || model.ProjectID <= 0 || string.IsNullOrWhiteSpace(model.DocumentType))
                return new { Status = false, Message = "Client and Document Type are required." };

            bool isSecuritization = model.DocumentType.Trim().Equals("Securitization", StringComparison.OrdinalIgnoreCase);
            bool isRelianceLetter = model.DocumentType.Trim().Equals("Reliance Letter", StringComparison.OrdinalIgnoreCase);
            if (isSecuritization && model.BillingMethod != "PerFile" && model.BillingMethod != "Hourly")
                return new { Status = false, Message = "Select Per File or Hourly billing for Securitization." };
            if (isSecuritization && model.Rate <= 0)
                return new { Status = false, Message = "Enter a rate greater than zero." };
            if (model.Rate < 0 || model.MinimumAmount < 0 || model.MaximumCap < 0)
                return new { Status = false, Message = "Rates, minimums, and caps cannot be negative." };
            if (isSecuritization && model.MinimumAmount > 0 && model.MaximumCap > 0 && model.MaximumCap < model.MinimumAmount)
                return new { Status = false, Message = "Maximum Cap cannot be less than Minimum Amount." };
            if (isRelianceLetter && (model.Details == null || model.Details.Count == 0) && model.Rate <= 0)
                return new { Status = false, Message = "Enter a legacy rate or at least one product rate." };

            int userId = int.Parse(HttpContext.Current.User.Identity.Name);
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        int rateId;
                        using (SqlCommand cmd = new SqlCommand("usp_RLSecCosting_SaveHeader", con, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@RateID", SqlDbType.Int).Value = model.RateID;
                            cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = model.ProjectID;
                            cmd.Parameters.Add("@DocumentType", SqlDbType.NVarChar, 200).Value = model.DocumentType.Trim();
                            cmd.Parameters.Add("@BillingMethod", SqlDbType.NVarChar, 20).Value = isSecuritization ? (object)model.BillingMethod : DBNull.Value;
                            cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = model.Rate;
                            cmd.Parameters["@Rate"].Precision = 18; cmd.Parameters["@Rate"].Scale = 2;
                            AddMoney(cmd, "@MinimumAmount", isSecuritization ? (decimal?)model.MinimumAmount : null);
                            AddMoney(cmd, "@MaximumCap", isSecuritization ? (decimal?)model.MaximumCap : null);
                            cmd.Parameters.Add("@EffectiveFrom", SqlDbType.Date).Value = (object)model.EffectiveFrom ?? DBNull.Value;
                            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                            rateId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        using (SqlCommand cmd = new SqlCommand("usp_RLSecCosting_DeleteDetails", con, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@RateID", SqlDbType.Int).Value = rateId;
                            cmd.ExecuteNonQuery();
                        }

                        if (isRelianceLetter && model.Details != null)
                        {
                            foreach (CostingDetailModel detail in model.Details)
                            {
                                if (string.IsNullOrWhiteSpace(detail.ProductType) || detail.Rate <= 0)
                                    throw new InvalidOperationException("Each product requires a description and a rate greater than zero.");
                                using (SqlCommand cmd = new SqlCommand("usp_RLSecCosting_SaveDetail", con, transaction))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@RateID", SqlDbType.Int).Value = rateId;
                                    cmd.Parameters.Add("@ProductType", SqlDbType.NVarChar, 250).Value = detail.ProductType.Trim();
                                    AddMoney(cmd, "@Rate", detail.Rate);
                                    cmd.Parameters.Add("@EffectiveFrom", SqlDbType.Date).Value = (object)model.EffectiveFrom ?? DBNull.Value;
                                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        transaction.Commit();
                        return new { Status = true, Message = "Costing configuration saved successfully." };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new { Status = false, Message = ex.Message };
                    }
                }
            }
        }

        private static void AddMoney(SqlCommand cmd, string name, decimal? value)
        {
            SqlParameter parameter = cmd.Parameters.Add(name, SqlDbType.Decimal);
            parameter.Precision = 18; parameter.Scale = 2;
            parameter.Value = value.HasValue ? (object)value.Value : DBNull.Value;
        }

        private static Dictionary<string, object> ToDictionary(DataRow row)
        {
            return row.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => row[c] == DBNull.Value ? null : row[c]);
        }

        private static List<Dictionary<string, object>> ToDictionaries(DataTable table)
        {
            return table.AsEnumerable().Select(ToDictionary).ToList();
        }

        public class CostingConfigurationModel
        {
            public int RateID { get; set; }
            public int ProjectID { get; set; }
            public string DocumentType { get; set; }
            public string BillingMethod { get; set; }
            public decimal Rate { get; set; }
            public decimal MinimumAmount { get; set; }
            public decimal MaximumCap { get; set; }
            public DateTime? EffectiveFrom { get; set; }
            public List<CostingDetailModel> Details { get; set; }
        }

        public class CostingDetailModel
        {
            public string ProductType { get; set; }
            public decimal Rate { get; set; }
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

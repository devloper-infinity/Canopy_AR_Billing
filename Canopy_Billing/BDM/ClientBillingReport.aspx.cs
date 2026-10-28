using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using MySqlConnector;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;

namespace Canopy_Billing.BDM
{
    public partial class ClientBillingReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod]
        public static string GetData(string FromDate, string ToDate)
        {
            System.Data.DataTable dt1 = new bllTracking().getBillingReport(FromDate, ToDate);
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
        public static string GetLauraMacSyncStatus()
        {
            using (SqlConnection cn = OpenSyncConnection())
            using (SqlCommand cmd = new SqlCommand(@"SELECT SYSDATETIME() AS ServerNow,
    (SELECT MAX(SyncEndTime) FROM dbo.DataSyncLog WHERE Status='Completed') AS LastSuccessfulSync", cn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                DateTime serverNow = reader.GetDateTime(0);
                DateTime? lastSync = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1);
                return Serialize(new
                {
                    IsFresh = lastSync.HasValue && serverNow.Subtract(lastSync.Value).TotalMinutes <= 40,
                    LastSuccessfulSyncDisplay = lastSync.HasValue ? lastSync.Value.ToString("MM/dd/yyyy hh:mm:ss tt") : "never"
                });
            }
        }

        [WebMethod]
        public static string SyncLauraMac()
        {
            //string allowedMachine = ConfigurationManager.AppSettings["LauraMacSyncAllowedMachineName"];
            //if (String.IsNullOrWhiteSpace(allowedMachine) ||
            //    !String.Equals(Environment.MachineName, allowedMachine, StringComparison.OrdinalIgnoreCase))
            //    return Serialize(new { Success = false, Message = "Synchronization is permitted only on the configured US portal server." });

            using (SqlConnection lockConnection = OpenSyncConnection())
            {
                if (!AcquireSyncLock(lockConnection))
                    return Serialize(new { Success = false, Message = "A LauraMac ODS synchronization request is already in progress." });

                long runningId = GetRunningExecutionId(lockConnection);
                if (runningId > 0)
                    return Serialize(new { Success = false, Message = "A LauraMac ODS synchronization is already running." });

                try
                {
                    RunIncrementalSync(lockConnection);
                    return Serialize(new { Success = true, Message = "" });
                }
                catch (Exception ex)
                {
                    Exception root = ex.GetBaseException();
                    return Serialize(new { Success = false, Message = "LauraMac ODS synchronization failed. The report was not generated. " + root.Message });
                }
            }
        }

        private static SqlConnection OpenSyncConnection()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainCon"].ConnectionString);
            cn.Open();
            return cn;
        }

        private static bool AcquireSyncLock(SqlConnection cn)
        {
            using (SqlCommand cmd = new SqlCommand("DECLARE @r int; EXEC @r=sys.sp_getapplock @Resource='LauraMacODSReportSync',@LockMode='Exclusive',@LockOwner='Session',@LockTimeout=0; SELECT @r;", cn))
                return Convert.ToInt32(cmd.ExecuteScalar()) >= 0;
        }

        private static long GetRunningExecutionId(SqlConnection cn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(SyncExecutionID),0) FROM dbo.DataSyncLog WHERE Status='Running' AND SyncStartTime>=DATEADD(HOUR,-2,SYSDATETIME());", cn))
                return Convert.ToInt64(cmd.ExecuteScalar());
        }

        private static void RunIncrementalSync(SqlConnection local)
        {
            DateTime fromDate;
            DateTime toDate;
            using (SqlCommand cmd = new SqlCommand(@"SELECT
ISNULL((SELECT MAX(SyncEndTime) FROM dbo.DataSyncLog WHERE Status='Completed'),'19000101'), SYSDATETIME();", local))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                fromDate = reader.GetDateTime(0);
                toDate = reader.GetDateTime(1);
            }

            long executionId;
            using (SqlCommand cmd = new SqlCommand("INSERT dbo.DataSyncLog(SyncStartTime,Status) OUTPUT INSERTED.SyncExecutionID VALUES(SYSDATETIME(),'Running');", local))
                executionId = Convert.ToInt64(cmd.ExecuteScalar());

            int processed = 0, inserted = 0, updated = 0, loanChanges = 0;
            try
            {
                foreach (SyncDefinition definition in SyncDefinitions)
                    SyncTable(local, definition, executionId, fromDate, toDate, ref processed, ref inserted, ref updated, ref loanChanges);

                using (SqlCommand cmd = new SqlCommand(@"UPDATE dbo.DataSyncLog SET SyncEndTime=SYSDATETIME(),Status='Completed',
TotalRecordsProcessed=@p,TotalRecordsInserted=@i,TotalRecordsUpdated=@u,TotalLoanIDChanges=@l,TotalErrors=0
WHERE SyncExecutionID=@id;", local))
                {
                    cmd.Parameters.AddWithValue("@p", processed);
                    cmd.Parameters.AddWithValue("@i", inserted);
                    cmd.Parameters.AddWithValue("@u", updated);
                    cmd.Parameters.AddWithValue("@l", loanChanges);
                    cmd.Parameters.AddWithValue("@id", executionId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE dbo.DataSyncLog SET SyncEndTime=SYSDATETIME(),Status='Failed',TotalErrors=1,ErrorMessage=@e WHERE SyncExecutionID=@id;", local))
                {
                    cmd.Parameters.AddWithValue("@e", ex.ToString());
                    cmd.Parameters.AddWithValue("@id", executionId);
                    cmd.ExecuteNonQuery();
                }
                throw;
            }
        }

        private static void SyncTable(SqlConnection local, SyncDefinition definition, long executionId,
            DateTime fromDate, DateTime toDate, ref int processed, ref int inserted, ref int updated, ref int loanChanges)
        {
            const int batchSize = 5000;
            int offset = 0;
            while (true)
            {
                DataTable data = new DataTable();
                using (MySqlConnection cloud = new MySqlConnection(GetCloudConnectionString()))
                using (MySqlCommand cmd = cloud.CreateCommand())
                {
                    cloud.Open();
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "SELECT * FROM `" + definition.CloudTable + "` WHERE " + definition.TimestampExpression +
                        " > @fromDate AND " + definition.TimestampExpression + " <= @toDate ORDER BY " + definition.TimestampExpression +
                        " LIMIT @batchSize OFFSET @offset;";
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@batchSize", batchSize);
                    cmd.Parameters.AddWithValue("@offset", offset);
                    using (MySqlDataReader reader = cmd.ExecuteReader()) data.Load(reader);
                }

                if (data.Rows.Count == 0) break;
                using (SqlTransaction tx = local.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand truncate = new SqlCommand("TRUNCATE TABLE dbo.[" + definition.StageTable + "];", local, tx))
                            truncate.ExecuteNonQuery();
                        using (SqlBulkCopy bulk = new SqlBulkCopy(local, SqlBulkCopyOptions.TableLock, tx))
                        {
                            bulk.DestinationTableName = "dbo.[" + definition.StageTable + "]";
                            bulk.BatchSize = batchSize;
                            bulk.BulkCopyTimeout = 300;
                            foreach (DataColumn column in data.Columns) bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            bulk.WriteToServer(data);
                        }
                        using (SqlCommand merge = new SqlCommand("dbo.[" + definition.Procedure + "]", local, tx))
                        {
                            merge.CommandType = CommandType.StoredProcedure;
                            merge.CommandTimeout = 300;
                            merge.Parameters.AddWithValue("@SyncExecutionID", executionId);
                            using (SqlDataReader reader = merge.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    inserted += Convert.ToInt32(reader["Inserted"]);
                                    updated += Convert.ToInt32(reader["Updated"]);
                                    loanChanges += Convert.ToInt32(reader["LoanIDChanges"]);
                                }
                            }
                        }
                        tx.Commit();
                    }
                    catch { tx.Rollback(); throw; }
                }
                processed += data.Rows.Count;
                if (data.Rows.Count < batchSize) break;
                offset += data.Rows.Count;
            }

            using (SqlCommand cmd = new SqlCommand(@"UPDATE dbo.DataSyncWatermark SET LastSuccessfulSyncTime=@time,
LastSyncExecutionID=@id,UpdatedOn=SYSDATETIME() WHERE CloudTableName=@cloud AND LocalTableName=@local;", local))
            {
                cmd.Parameters.AddWithValue("@time", toDate);
                cmd.Parameters.AddWithValue("@id", executionId);
                cmd.Parameters.AddWithValue("@cloud", definition.CloudTable);
                cmd.Parameters.AddWithValue("@local", definition.LocalTable);
                cmd.ExecuteNonQuery();
            }
        }

        private static string GetCloudConnectionString()
        {
            string value = ConfigurationManager.ConnectionStrings["LauraMacCloudDatabase"] == null
                ? null : ConfigurationManager.ConnectionStrings["LauraMacCloudDatabase"].ConnectionString;
            if (String.IsNullOrWhiteSpace(value)) throw new InvalidOperationException("LauraMac cloud database is not configured.");
            return value;
        }

        private static string Serialize(object value)
        {
            return new JavaScriptSerializer().Serialize(value);
        }

        private static readonly SyncDefinition[] SyncDefinitions =
        {
            new SyncDefinition("LoanRecord", "LoanRecord", "SyncStage_LoanRecord", "usp_Sync_LoanRecord", "COALESCE(modifiedDate,createdDate)"),
            new SyncDefinition("LoanTask", "LoanTask", "SyncStage_LoanTask", "usp_Sync_LoanTask", "GREATEST(COALESCE(loanTaskModifiedDate,'1000-01-01'),COALESCE(loanProcessFlowModifiedDate,'1000-01-01'),COALESCE(loanTaskCreatedDate,'1000-01-01'),COALESCE(loanProcessFlowCreatedDate,'1000-01-01'))"),
            new SyncDefinition("Snapshot", "Snapshot", "SyncStage_Snapshot", "usp_Sync_Snapshot", "COALESCE(modifiedDate,createdDate)"),
            new SyncDefinition("Transaction", "LoanTransactions", "SyncStage_LoanTransactions", "usp_Sync_LoanTransactions", "COALESCE(modifiedDate,createdDate)")
        };

        private sealed class SyncDefinition
        {
            public SyncDefinition(string cloudTable, string localTable, string stageTable, string procedure, string timestampExpression)
            { CloudTable = cloudTable; LocalTable = localTable; StageTable = stageTable; Procedure = procedure; TimestampExpression = timestampExpression; }
            public string CloudTable { get; private set; }
            public string LocalTable { get; private set; }
            public string StageTable { get; private set; }
            public string Procedure { get; private set; }
            public string TimestampExpression { get; private set; }
        }

    }
}

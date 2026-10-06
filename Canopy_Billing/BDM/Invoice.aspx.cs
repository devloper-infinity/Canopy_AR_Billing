using ClosedXML.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;
using Vendor_Portal.App_Code.DAL;

namespace Vendor_Portal.BDM
{
    public partial class Invoice : System.Web.UI.Page
    {
        static string NewFileName = "";
        static string GUIDFile = "";
        static string FolderPath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FolderPath = Server.MapPath(@"~\ReportDocument");
            try
            {
                HttpContext postedContext = HttpContext.Current;
                HttpPostedFile file = postedContext.Request.Files[0];

                string name = file.FileName;
                byte[] binaryWriteArray = new byte[file.InputStream.Length];
                file.InputStream.Read(binaryWriteArray, 0,
                (int)file.InputStream.Length);

                FileInfo file_Info = new FileInfo(file.FileName);
                string ext = file_Info.Extension;

                string file_Name = Guid.NewGuid().ToString() + "_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ext;
                NewFileName = Server.MapPath("..//TempFiles//" + file_Name);
                FileStream objfilestream = new FileStream(NewFileName, FileMode.Create, FileAccess.ReadWrite);
                objfilestream.Write(binaryWriteArray, 0,
                binaryWriteArray.Length);
                objfilestream.Close();
            }
            catch { }
        }

        #region For Excel upload

        [WebMethod]
        public static object GetBillingData()
        {
            List<object> list = new List<object>();

            string conStr = SQLHelper.ConnectionString2;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"SELECT 
                       InvoiceID, OurClient, Recipient, TradeName, InvoiceDate, Document, TM, DocuSign,
                        DocumentDate, ExecutedDate, LoanCount,
                        CASE
                            WHEN ISNULL(Cost, 0) = 0
                                 AND TRY_CONVERT(decimal(18,4), LoanCount) <> 0
                            THEN Floor(ExpectedBilling / TRY_CONVERT(decimal(18,4), LoanCount))
                            ELSE Cost
                        END AS Cost,
                        ExpectedBilling,
                        BillingEntity, ContactPerson, SubmittedForInvoice, InvoiceIssued, Notes,FilePath, isVerify, VerifyRemark, VerifiedOn,
                        CostingRateID, BillingMethod, HoursWorked, MinimumAmount, MaximumCap, BaseAmount,
                        MinimumApplied, CapApplied
                        FROM RLinvoice";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new
                        {
                            InvoiceID = dr["InvoiceID"].ToString(),
                            OurClient = dr["OurClient"].ToString(),
                            Recipient = dr["Recipient"].ToString(),
                            TradeName = dr["TradeName"].ToString(),
                            InvoiceDate = FormatInvoiceDate(dr["InvoiceDate"]),
                            Document = dr["Document"].ToString(),
                            TM = dr["TM"].ToString(),
                            DocSign = dr["DocuSign"].ToString(),
                            DocumentDate = dr["DocumentDate"].ToString(),
                            ExecutedDate = dr["ExecutedDate"].ToString(),
                            LoanCount = dr["LoanCount"].ToString(),
                            RLCost = dr["Cost"].ToString(),
                            ExpectedBilling = dr["ExpectedBilling"].ToString(),
                            BillingEntity = dr["BillingEntity"].ToString(),
                            EmailConfiguration = DeserializeEmailConfiguration(dr["ContactPerson"].ToString()),
                            SubmittedForInvoice = dr["SubmittedForInvoice"].ToString(),
                            InvoiceIssued = dr["InvoiceIssued"].ToString(),
                            Notes = dr["Notes"].ToString(),
                            FilePath = dr["FilePath"].ToString(),
                            IsVerify = Convert.ToString(dr["isVerify"]) == "" ? false : Convert.ToBoolean(dr["isVerify"]),
                            VerifyRemark = dr["VerifyRemark"].ToString(),
                            VerifiedOn = dr["VerifiedOn"].ToString()
                            ,CostingRateID = dr["CostingRateID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CostingRateID"])
                            ,BillingMethod = dr["BillingMethod"].ToString()
                            ,HoursWorked = dr["HoursWorked"].ToString()
                            ,MinimumAmount = dr["MinimumAmount"].ToString()
                            ,MaximumCap = dr["MaximumCap"].ToString()
                            ,BaseAmount = dr["BaseAmount"].ToString()
                            ,MinimumApplied = dr["MinimumApplied"] != DBNull.Value && Convert.ToBoolean(dr["MinimumApplied"])
                            ,CapApplied = dr["CapApplied"] != DBNull.Value && Convert.ToBoolean(dr["CapApplied"])
                            ,ProductDetails = GetInvoiceCostingDetails(Convert.ToInt32(dr["InvoiceID"]))
                        }); ;
                    }
                }
            }

            return list;
        }
        public static DataTable ReadExcelFile(string path)
        {
            DataTable dt = new DataTable();

            using (var workbook = new XLWorkbook(path))
            {
                var ws = workbook.Worksheet(1);
                var range = ws.RangeUsed();
                bool firstRow = true;

                foreach (var row in range.Rows())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                            dt.Columns.Add(cell.Value.ToString());
                        firstRow = false;
                    }
                    else
                    {
                        //dt.Rows.Add(row.Cells().Select(c => c.Value).ToArray());
                        dt.Rows.Add(row.Cells().Select(c => c.GetValue<string>() ?? "").ToArray());
                    }
                }
            }

            return dt;
        }

        [WebMethod]
        public static DataTable ValidateInvoiceExel()
        {
            DataTable dt = null;
            string conStr = ConfigurationManager.ConnectionStrings[SQLHelper.ConnectionString].ConnectionString;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                {
                    bulkCopy.DestinationTableName = "RLinvoice_Temp";
                    con.Open();
                    bulkCopy.WriteToServer(dt);
                }
            }
            return dt;
        }

        public static List<string> GetTableColumns(string tableName)
        {
            List<string> cols = new List<string>();

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT COLUMN_NAME 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = @Table", con);

                cmd.Parameters.AddWithValue("@Table", tableName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cols.Add(dr["COLUMN_NAME"].ToString());
                }
            }

            return cols;
        }

        public static void AddMissingColumns(DataTable dt, string tableName)
        {
            List<string> existingCols = GetTableColumns(tableName);

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                foreach (DataColumn col in dt.Columns)
                {
                    if (!existingCols.Contains(col.ColumnName))
                    {
                        string sql = $@"
                    ALTER TABLE {tableName} 
                    ADD [{col.ColumnName}] NVARCHAR(MAX) NULL";

                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        [WebMethod]
        public static object BulkImport()
        {
            DataTable dt = ReadExcelFile(NewFileName);

            // Clean data (important)
            //dt.Columns["Our Client"].ColumnName = "OurClient";
            //dt.Columns["Trade Name"].ColumnName = "TradeName";
            //dt.Columns["Invoice Date"].ColumnName = "InvoiceDate";
            //dt.Columns["Billing Entity"].ColumnName = "BillingEntity";
            foreach (DataColumn col in dt.Columns)
            {
                col.ColumnName = col.ColumnName.Replace(" ", "").Trim();
            }
            if (!dt.Columns.Contains("AddedBy"))
            {
                dt.Columns.Add("AddedBy", typeof(int));
                dt.Columns["AddedBy"].DefaultValue = int.Parse(HttpContext.Current.User.Identity.Name.ToString());
            }

            BulkInsertToTemp(dt);

            return ExecuteBulkProcedure();
        }
        public static void BulkInsertToTemp(DataTable dt)
        {
            string tableName = "RLinvoice_Temp";
            AddMissingColumns(dt, tableName);
            string conStr = SQLHelper.ConnectionString2;

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                {
                    bulkCopy.DestinationTableName = tableName;

                    // ✅ Dynamic mapping
                    foreach (DataColumn col in dt.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName.Trim(), col.ColumnName.Trim());
                    }

                    con.Open();
                    bulkCopy.WriteToServer(dt);
                }
            }
        }

        public static object ExecuteBulkProcedure()
        {
            ImportResult result = new ImportResult
            {
                Summary = new ImportSummary(),
                Duplicates = new List<object>(),
                Errors = new List<object>(),
                Inserted = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                using (SqlCommand cmd = new SqlCommand("BulkImportBilling", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    // ✅ 1. Summary
                    if (dr.Read())
                    {
                        result.Summary = new ImportSummary
                        {
                            Total = Convert.ToInt32(dr["TotalRecords"]),
                            Duplicate = Convert.ToInt32(dr["DuplicateRecords"]),
                            Inserted = Convert.ToInt32(dr["InsertedRecords"]),
                            Error = Convert.ToInt32(dr["ErrorRecords"])
                        };
                    }

                    // ✅ 2. Duplicates
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            result.Duplicates.Add(new
                            {
                                OurClient = dr["OurClient"].ToString(),
                                TradeName = dr["TradeName"].ToString(),
                                InvoiceDate = FormatInvoiceDate(dr["InvoiceDate"]),
                                BillingEntity = dr["BillingEntity"].ToString(),
                            });
                        }
                    }
                    // 3️⃣ Inserted
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            result.Inserted.Add(new
                            {
                                OurClient = dr["OurClient"].ToString(),
                                TradeName = dr["TradeName"].ToString(),
                                InvoiceDate = FormatInvoiceDate(dr["InvoiceDate"]),
                                BillingEntity = dr["BillingEntity"].ToString(),
                            });
                        }
                    }
                    // 4️⃣ Errors
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            result.Errors.Add(new
                            {
                                OurClient = dr["OurClient"].ToString(),
                                TradeName = dr["TradeName"].ToString(),
                                InvoiceDate = FormatInvoiceDate(dr["InvoiceDate"]),
                                BillingEntity = dr["BillingEntity"].ToString(),
                                TM = dr["TM"].ToString(),
                                ErrorMessage = dr["ErrorMessage"].ToString()
                            });
                        }
                    }
                }
            }

            // ✅ RETURN CLEAN STRUCTURE
            return result;
        }

        [WebMethod]
        public static string VerifyInvoices(List<int> ids, string remark)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                foreach (int id in ids)
                {
                    SqlCommand cmd = new SqlCommand(@"
                UPDATE RLinvoice
                SET IsVerify = 1,
                    VerifyRemark = @Remark,
                    VerifiedOn = GETDATE()
                WHERE InvoiceID = @Id", con);

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Remark", remark);

                    cmd.ExecuteNonQuery();
                }
            }

            return "Success";
        }

        [WebMethod]
        public static object DeleteInvoice(int invoiceId)
        {
            if (invoiceId <= 0)
                return new { Success = false, Message = "Invalid invoice." };

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        bool? isVerified = null;

                        using (SqlCommand statusCommand = new SqlCommand(@"
                            SELECT ISNULL(IsVerify, 0)
                            FROM RLinvoice WITH (UPDLOCK, HOLDLOCK)
                            WHERE InvoiceID = @InvoiceID", con, transaction))
                        {
                            statusCommand.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;
                            object status = statusCommand.ExecuteScalar();

                            if (status != null && status != DBNull.Value)
                                isVerified = Convert.ToBoolean(status);
                        }

                        if (!isVerified.HasValue)
                        {
                            transaction.Rollback();
                            return new { Success = false, Message = "Invoice was not found." };
                        }

                        if (isVerified.Value)
                        {
                            transaction.Rollback();
                            return new { Success = false, Message = "Verified invoices cannot be deleted." };
                        }

                        using (SqlCommand auditCommand = new SqlCommand(
                            "DELETE FROM RLInvoice_Audit WHERE InvoiceID = @InvoiceID", con, transaction))
                        {
                            auditCommand.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;
                            auditCommand.ExecuteNonQuery();
                        }

                        using (SqlCommand detailCommand = new SqlCommand(
                            "DELETE FROM RLInvoiceCostingDetail WHERE InvoiceID = @InvoiceID", con, transaction))
                        {
                            detailCommand.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;
                            detailCommand.ExecuteNonQuery();
                        }

                        int affectedRows;
                        using (SqlCommand deleteCommand = new SqlCommand(@"
                            DELETE FROM RLinvoice
                            WHERE InvoiceID = @InvoiceID
                              AND ISNULL(IsVerify, 0) = 0", con, transaction))
                        {
                            deleteCommand.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;
                            affectedRows = deleteCommand.ExecuteNonQuery();
                        }

                        if (affectedRows != 1)
                        {
                            transaction.Rollback();
                            return new { Success = false, Message = "Invoice could not be deleted." };
                        }

                        transaction.Commit();
                        return new { Success = true, Message = "Invoice deleted successfully." };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static object InsertBilling(BillingModel obj)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertRLInvoice", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@OurClient", obj.OurClient);
                        cmd.Parameters.AddWithValue("@Recipient", obj.Recipient);
                        cmd.Parameters.AddWithValue("@TradeName", obj.TradeName);
                        cmd.Parameters.AddWithValue("@InvoiceDate", ParseOptionalInvoiceDate(obj.InvoiceDate));
                        cmd.Parameters.AddWithValue("@Document", obj.Document);
                        cmd.Parameters.AddWithValue("@TM", obj.TM);
                        cmd.Parameters.AddWithValue("@DocSign", obj.DocSign);
                        cmd.Parameters.AddWithValue("@DocumentDate", ParseOptionalInvoiceDate(obj.DocumentDate));
                        cmd.Parameters.AddWithValue("@ExecutedDate", ParseOptionalInvoiceDate(obj.ExecutedDate));
                        cmd.Parameters.AddWithValue("@LoanCount", obj.LoanCount);
                        cmd.Parameters.AddWithValue("@Cost", obj.Cost);
                        cmd.Parameters.AddWithValue("@ExpectedBilling", obj.ExpectedBilling);
                        cmd.Parameters.AddWithValue("@BillingEntity", obj.BillingEntity);
                        cmd.Parameters.AddWithValue("@Notes", obj.Notes);
                        cmd.Parameters.AddWithValue("@AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));

                        con.Open();

                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            return new
                            {
                                Status = Convert.ToInt32(dr["Status"]) == 1,
                                Message = dr["Message"].ToString()
                            };
                        }
                    }
                }

                return new { Status = false, Message = "Unknown error" };
            }
            catch (Exception ex)
            {
                return new { Status = false, Message = ex.Message };
            }
        }

        [WebMethod]
        public static string GetAllClientList()
        {
            DataTable dt1 = new bllTracking().GetAllClientForAddInvoice_Onshore();
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
        public static string GetRLCostingByProject(int ProjectID)
        {
            DataTable dt1 = new bllTracking().GetRLSecCostByProject(ProjectID);
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
        public static List<object> GetContactPersons(int clientId)
        {
            List<object> list = new List<object>();

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT ContactID, ContactPerson, Email, ContactNo 
            FROM ClientContacts 
            WHERE ClientID = @ClientID", con);

                cmd.Parameters.AddWithValue("@ClientID", clientId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new
                    {
                        ContactID = Convert.ToInt32(dr["ContactID"]),
                        ContactName = dr["ContactPerson"].ToString(),
                        Email = dr["Email"] == DBNull.Value ? "" : dr["Email"].ToString(),
                        Phone = dr["ContactNo"] == DBNull.Value ? "" : dr["ContactNo"].ToString()
                    });
                }
            }

            return list;
        }

        [WebMethod]
        public static List<object> GetContactPersonsByName(string clientName)
        {
            List<object> list = new List<object>();

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT cc.ContactID, cc.ContactPerson, cc.Email, cc.ContactNo
            FROM ClientContacts cc
            INNER JOIN Clients c ON cc.ClientID = c.ClientID
            WHERE c.ClientName = @ClientName", con);

                cmd.Parameters.AddWithValue("@ClientName", clientName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new
                    {
                        ContactID = Convert.ToInt32(dr["ContactID"]),
                        ContactName = dr["ContactPerson"].ToString(),
                        Email = dr["Email"].ToString(),
                        Phone = dr["ContactNo"].ToString()
                    });
                }
            }

            return list;
        }

        [WebMethod]
        public static object GetFlexibleCosting(int projectId, string documentType, string invoiceDate)
        {
            if (projectId <= 0 || string.IsNullOrWhiteSpace(documentType))
                return new { Header = (object)null, Details = new List<object>() };

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            using (SqlCommand cmd = new SqlCommand("usp_RLSecCosting_GetForInvoice", con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = projectId;
                cmd.Parameters.Add("@DocumentType", SqlDbType.NVarChar, 200).Value = documentType.Trim();
                cmd.Parameters.Add("@InvoiceDate", SqlDbType.Date).Value = ParseNullableDate(invoiceDate);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return new { Header = (object)null, Details = new List<Dictionary<string, object>>() };
                return new
                {
                    Header = RowToDictionary(ds.Tables[0].Rows[0]),
                    Details = ds.Tables.Count > 1 ? TableToDictionaries(ds.Tables[1]) : new List<Dictionary<string, object>>()
                };
            }
        }

        [WebMethod]
        public static object GetBillingEntityEmails(string clientName)
        {
            List<string> emails = new List<string>();
            List<string> defaultEmails = null;

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT cc.Email
                    FROM ClientContacts cc
                    INNER JOIN Clients c ON cc.ClientID = c.ClientID
                    WHERE c.ClientName = @ClientName
                      AND NULLIF(LTRIM(RTRIM(cc.Email)), '') IS NOT NULL", con))
                {
                    cmd.Parameters.Add("@ClientName", SqlDbType.NVarChar, 250).Value = clientName;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            AddValidEmails(emails, Convert.ToString(dr["Email"]));
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 1 ContactPerson
                    FROM RLinvoice
                    WHERE BillingEntity = @ClientName
                      AND ContactPerson LIKE 'EMAILS:%'
                    ORDER BY InvoiceID DESC", con))
                {
                    cmd.Parameters.Add("@ClientName", SqlDbType.NVarChar, 250).Value = clientName;
                    object savedConfiguration = cmd.ExecuteScalar();
                    if (savedConfiguration != null && savedConfiguration != DBNull.Value)
                    {
                        defaultEmails = new List<string>();
                        AddValidEmails(defaultEmails, DeserializeEmailConfiguration(Convert.ToString(savedConfiguration)));
                    }
                }
            }

            return new
            {
                Emails = emails,
                DefaultEmails = defaultEmails ?? new List<string>(emails)
            };
        }

        [WebMethod]
        public static object AddBillingEntityEmail(string clientName, string email)
        {
            email = (email ?? string.Empty).Trim();
            if (!IsValidEmail(email))
            {
                return new { Status = false, Message = "Enter a valid email address." };
            }

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();
                int clientId;
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT TOP 1 ClientID FROM Clients WHERE ClientName = @ClientName", con))
                {
                    cmd.Parameters.Add("@ClientName", SqlDbType.NVarChar, 250).Value = clientName;
                    object value = cmd.ExecuteScalar();
                    if (value == null || value == DBNull.Value)
                    {
                        return new { Status = false, Message = "Select a valid Billing Entity." };
                    }

                    clientId = Convert.ToInt32(value);
                }

                using (SqlCommand cmd = new SqlCommand(@"
                    IF NOT EXISTS
                    (
                        SELECT 1 FROM ClientContacts
                        WHERE ClientID = @ClientID
                          AND LOWER(LTRIM(RTRIM(Email))) = LOWER(@Email)
                    )
                    BEGIN
                        INSERT INTO ClientContacts
                            (ClientID, ContactPerson, ContactNo, Email, IsPrimary)
                        VALUES
                            (@ClientID, '', '', @Email, 0)
                    END", con))
                {
                    cmd.Parameters.Add("@ClientID", SqlDbType.Int).Value = clientId;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 320).Value = email;
                    cmd.ExecuteNonQuery();
                }
            }

            return new { Status = true, Email = email, Message = "Email added." };
        }

        private static void AddValidEmails(ICollection<string> target, string value)
        {
            foreach (string email in (value ?? string.Empty).Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmedEmail = email.Trim();
                if (IsValidEmail(trimmedEmail) && !target.Any(item => string.Equals(item, trimmedEmail, StringComparison.OrdinalIgnoreCase)))
                {
                    target.Add(trimmedEmail);
                }
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress parsed = new MailAddress(email);
                return string.Equals(parsed.Address, email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static string SerializeEmailConfiguration(string emails)
        {
            List<string> validEmails = new List<string>();
            AddValidEmails(validEmails, emails);
            return "EMAILS:" + string.Join(",", validEmails);
        }

        private static string DeserializeEmailConfiguration(string value)
        {
            const string prefix = "EMAILS:";
            return value != null && value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                ? value.Substring(prefix.Length)
                : string.Empty;
        }

        [WebMethod]
        public static object SaveInvoice(InvoiceModel model)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        ApplyCostingRules(con, transaction, model);
                        SqlCommand cmd = new SqlCommand(@"
            INSERT INTO RLinvoice
            (
                OurClient, Recipient, TradeName, InvoiceDate,
                Document, TM, DocuSign, DocumentDate, ExecutedDate,
                BillingEntity, ContactPerson,
                LoanCount, Cost, ExpectedBilling, Notes, CostingRateID, BillingMethod,
                HoursWorked, MinimumAmount, MaximumCap, BaseAmount, MinimumApplied, CapApplied, AddedBy
            )
            OUTPUT INSERTED.InvoiceID
            VALUES
            (
                @OurClient, @Recipient, @TradeName, @InvoiceDate,
                @Document, @TM, @DocuSign, @DocumentDate, @ExecutedDate,
                @BillingEntity, @ContactPerson,
                @LoanCount, @Cost, @ExpectedBilling, @Notes, @CostingRateID, @BillingMethod,
                @HoursWorked, @MinimumAmount, @MaximumCap, @BaseAmount, @MinimumApplied, @CapApplied, @AddedBy
            )", con, transaction);

                        AddInvoiceParameters(cmd, model);
                        int invoiceId = Convert.ToInt32(cmd.ExecuteScalar());
                        SaveInvoiceDetails(con, transaction, invoiceId, model.ProductDetails);
                        transaction.Commit();
                        return new { Status = true, Message = "Invoice saved successfully." };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new { Status = false, Message = ex.Message };
                    }
                }
            }
        }
        public static void LogIfChanged(string field, object oldVal, object newVal, int invoiceId)
        {
            string oldValue = oldVal == DBNull.Value ? "" : oldVal.ToString();
            string newValue = newVal == null ? "" : newVal.ToString();

            if (oldValue != newValue)
            {
                using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
                {
                    SqlCommand cmd = new SqlCommand(@"
                INSERT INTO RLInvoice_Audit
                (InvoiceID, FieldName, OldValue, NewValue, UpdatedBy)
                VALUES (@InvoiceID, @FieldName, @OldValue, @NewValue, @UpdatedBy)", con);

                    cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);
                    cmd.Parameters.AddWithValue("@FieldName", field);
                    cmd.Parameters.AddWithValue("@OldValue", oldValue);
                    cmd.Parameters.AddWithValue("@NewValue", newValue);
                    cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.User.Identity.Name);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [WebMethod]
        public static object UpdateInvoice(InvoiceModel model)
        {
            DataTable dtOld = new DataTable();

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM RLinvoice WHERE InvoiceID=@Id", con);
                cmd.Parameters.AddWithValue("@Id", model.InvoiceID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtOld);
            }

            var oldRow = dtOld.Rows[0];

            LogIfChanged("OurClient", oldRow["OurClient"], model.OurClient, model.InvoiceID);
            LogIfChanged("Recipient", oldRow["Recipient"], model.Recipient, model.InvoiceID);
            LogIfChanged("TradeName", oldRow["TradeName"], model.TradeName, model.InvoiceID);
            LogIfChanged("DocuSign", oldRow["DocuSign"], model.DocSign, model.InvoiceID);
            LogIfChanged("TM", oldRow["TM"], model.TM, model.InvoiceID);
            LogIfChanged("DocumentDate", oldRow["DocumentDate"], model.DocumentDate, model.InvoiceID);
            LogIfChanged("ExecutedDate", oldRow["ExecutedDate"], model.ExecutedDate, model.InvoiceID);
            LogIfChanged("BillingEntity", oldRow["BillingEntity"], model.BillingEntity, model.InvoiceID);
            LogIfChanged("EmailConfiguration", DeserializeEmailConfiguration(Convert.ToString(oldRow["ContactPerson"])), model.EmailConfiguration, model.InvoiceID);
            LogIfChanged("LoanCount", oldRow["LoanCount"], model.LoanCount, model.InvoiceID);
            LogIfChanged("Cost", oldRow["Cost"], model.RLCost, model.InvoiceID);
            LogIfChanged("ExpectedBilling", oldRow["ExpectedBilling"], model.ExpectedBilling, model.InvoiceID);
            LogIfChanged("Notes", oldRow["Notes"], model.Notes, model.InvoiceID);
            LogIfChanged("BillingMethod", oldRow["BillingMethod"], model.BillingMethod, model.InvoiceID);
            LogIfChanged("HoursWorked", oldRow["HoursWorked"], model.HoursWorked, model.InvoiceID);
            LogIfChanged("MinimumAmount", oldRow["MinimumAmount"], model.MinimumAmount, model.InvoiceID);
            LogIfChanged("MaximumCap", oldRow["MaximumCap"], model.MaximumCap, model.InvoiceID);

            if (dtOld.Rows.Count == 0) return new { Status = false, Message = "Invoice was not found." };
            if (dtOld.Rows[0]["isVerify"] != DBNull.Value && Convert.ToBoolean(dtOld.Rows[0]["isVerify"]))
                return new { Status = false, Message = "Verified invoices cannot be edited." };

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        ApplyCostingRules(con, transaction, model);
                        SqlCommand cmd = new SqlCommand(@"
            UPDATE RLinvoice SET
                OurClient = @OurClient,
                Recipient = @Recipient,
                TradeName = @TradeName,
                InvoiceDate = @InvoiceDate,
                Document = @Document,
                TM = @TM,
                DocuSign = @DocuSign,
                DocumentDate = @DocumentDate,
                ExecutedDate = @ExecutedDate,
                BillingEntity = @BillingEntity,
                ContactPerson = @ContactPerson,
                LoanCount = @LoanCount,
                Cost = @Cost,
                ExpectedBilling = @ExpectedBilling,
                Notes = @Notes, CostingRateID=@CostingRateID, BillingMethod=@BillingMethod,
                HoursWorked=@HoursWorked, MinimumAmount=@MinimumAmount, MaximumCap=@MaximumCap,
                BaseAmount=@BaseAmount, MinimumApplied=@MinimumApplied, CapApplied=@CapApplied
            WHERE InvoiceID = @InvoiceID AND ISNULL(isVerify,0)=0", con, transaction);

                        cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = model.InvoiceID;
                        AddInvoiceParameters(cmd, model);
                        if (cmd.ExecuteNonQuery() != 1) throw new InvalidOperationException("Invoice could not be updated.");
                        using (SqlCommand deleteDetails = new SqlCommand("DELETE FROM RLInvoiceCostingDetail WHERE InvoiceID=@InvoiceID", con, transaction))
                        {
                            deleteDetails.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = model.InvoiceID;
                            deleteDetails.ExecuteNonQuery();
                        }
                        SaveInvoiceDetails(con, transaction, model.InvoiceID, model.ProductDetails);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new { Status = false, Message = ex.Message };
                    }
                }
            }

            return new { Status = true, Message = "Invoice updated successfully." };
        }

        [WebMethod]
        public static List<object> GetInvoiceHistory(int invoiceId)
        {
            List<object> list = new List<object>();

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT 
                FieldName,
                OldValue,
                NewValue,
                FirstName+' '+lastName as UpdatedBy,
                FORMAT(UpdatedOn, 'dd-MMM-yyyy HH:mm') AS UpdatedOn
            FROM RLInvoice_Audit RL inner join InfinityERp.dbo.EmployeeInfo E on E.EmployeeID=RL.UpdatedBy
            WHERE InvoiceID = @InvoiceID
            ORDER BY UpdatedOn DESC", con);

                cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new
                    {
                        FieldName = dr["FieldName"].ToString(),
                        OldValue = dr["OldValue"].ToString(),
                        NewValue = dr["NewValue"].ToString(),
                        UpdatedBy = dr["UpdatedBy"].ToString(),
                        UpdatedOn = dr["UpdatedOn"].ToString()
                    });
                }
            }

            return list;
        }

        #endregion
        private static string FormatInvoiceDate(object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                return string.Empty;
            }

            DateTime date;
            if (value is DateTime)
            {
                date = (DateTime)value;
            }
            else if (!DateTime.TryParseExact(
                Convert.ToString(value).Trim(),
                new[] { "MM.dd.yyyy", "M.d.yyyy", "yyyy-MM-dd" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
            {
                return string.Empty;
            }

            return date.ToString("MM.dd.yyyy", CultureInfo.InvariantCulture);
        }

        private static object ParseOptionalInvoiceDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DBNull.Value;
            }

            DateTime date;
            if (!DateTime.TryParseExact(
                value.Trim(),
                new[] { "MM.dd.yyyy", "M.d.yyyy", "yyyy-MM-dd" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
            {
                throw new FormatException("Date must be in MM.dd.yyyy format.");
            }

            return date;
        }

        private static object FormatOptionalInvoiceDate(string value)
        {
            object parsedDate = ParseOptionalInvoiceDate(value);
            return parsedDate == DBNull.Value
                ? parsedDate
                : ((DateTime)parsedDate).ToString("MM.dd.yyyy", CultureInfo.InvariantCulture);
        }

        private static object ParseNullableDate(string value)
        {
            object parsed = ParseOptionalInvoiceDate(value);
            return parsed == DBNull.Value ? DBNull.Value : parsed;
        }

        private static Dictionary<string, object> RowToDictionary(DataRow row)
        {
            return row.Table.Columns.Cast<DataColumn>().ToDictionary(
                column => column.ColumnName,
                column => row[column] == DBNull.Value ? null : row[column]);
        }

        private static List<Dictionary<string, object>> TableToDictionaries(DataTable table)
        {
            return table.AsEnumerable().Select(RowToDictionary).ToList();
        }

        private static List<object> GetInvoiceCostingDetails(int invoiceId)
        {
            List<object> details = new List<object>();
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT InvoiceDetailID, CostingDetailID, BillingDescription, Quantity, Rate, Amount
                FROM RLInvoiceCostingDetail WHERE InvoiceID=@InvoiceID ORDER BY InvoiceDetailID", con))
            {
                cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        details.Add(new
                        {
                            InvoiceDetailID = Convert.ToInt32(reader["InvoiceDetailID"]),
                            CostingDetailID = reader["CostingDetailID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CostingDetailID"]),
                            ProductType = Convert.ToString(reader["BillingDescription"]),
                            Quantity = Convert.ToDecimal(reader["Quantity"]),
                            Rate = Convert.ToDecimal(reader["Rate"]),
                            Amount = Convert.ToDecimal(reader["Amount"])
                        });
                    }
                }
            }
            return details;
        }

        private static void ApplyCostingRules(SqlConnection con, SqlTransaction transaction, InvoiceModel model)
        {
            if (model == null) throw new InvalidOperationException("Invoice details are required.");
            if (model.ProjectID <= 0)
            {
                using (SqlCommand clientCommand = new SqlCommand("SELECT TOP 1 ClientID FROM Clients WHERE LTRIM(RTRIM(ClientName))=LTRIM(RTRIM(@ClientName))", con, transaction))
                {
                    clientCommand.Parameters.Add("@ClientName", SqlDbType.NVarChar, 200).Value = model.BillingEntity ?? string.Empty;
                    object clientId = clientCommand.ExecuteScalar();
                    if (clientId != null && clientId != DBNull.Value) model.ProjectID = Convert.ToInt32(clientId);
                }
            }
            if (model.ProjectID <= 0) return; // unchanged legacy behavior for unmapped historical clients
            if (model.CostingRateID <= 0)
            {
                using (SqlCommand configCommand = new SqlCommand(@"
                    SELECT TOP 1 RateID FROM SecuritizationRelianceLetterCosting
                    WHERE ProjectId=@ProjectID AND LTRIM(RTRIM(Type))=LTRIM(RTRIM(@Document)) AND ISNULL(IsActive,1)=1
                      AND (EffectiveFrom IS NULL OR EffectiveFrom<=@InvoiceDate)
                    ORDER BY ISNULL(EffectiveFrom,CONVERT(date,'19000101')) DESC, COALESCE(UpdatedDate,AddedDate) DESC, RateID DESC", con, transaction))
                {
                    configCommand.Parameters.Add("@ProjectID", SqlDbType.Int).Value = model.ProjectID;
                    configCommand.Parameters.Add("@Document", SqlDbType.NVarChar, 200).Value = model.Document ?? string.Empty;
                    configCommand.Parameters.Add("@InvoiceDate", SqlDbType.Date).Value = ParseOptionalInvoiceDate(model.InvoiceDate);
                    object rateId = configCommand.ExecuteScalar();
                    if (rateId != null && rateId != DBNull.Value) model.CostingRateID = Convert.ToInt32(rateId);
                }
            }
            if (model.CostingRateID <= 0)
                model.CostingRateID = CreateCostingFromInvoice(con, transaction, model);
            if (model.CostingRateID <= 0) return;

            DataTable header = new DataTable();
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT RateID, ProjectId, Rate, Type, BillingMethod, MinimumAmount, MaximumCap
                FROM SecuritizationRelianceLetterCosting
                WHERE RateID=@RateID AND ProjectId=@ProjectID AND LTRIM(RTRIM(Type))=LTRIM(RTRIM(@Document))
                  AND ISNULL(IsActive,1)=1
                  AND (EffectiveFrom IS NULL OR EffectiveFrom <= @InvoiceDate)", con, transaction))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@RateID", SqlDbType.Int).Value = model.CostingRateID;
                cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = model.ProjectID;
                cmd.Parameters.Add("@Document", SqlDbType.NVarChar, 200).Value = model.Document ?? string.Empty;
                cmd.Parameters.Add("@InvoiceDate", SqlDbType.Date).Value = ParseOptionalInvoiceDate(model.InvoiceDate);
                da.Fill(header);
            }
            if (header.Rows.Count == 0) throw new InvalidOperationException("The selected costing configuration is no longer available.");

            DataRow config = header.Rows[0];
            string method = Convert.ToString(config["BillingMethod"]);
            decimal configuredRate = config["Rate"] == DBNull.Value ? 0 : Convert.ToDecimal(config["Rate"]);
            decimal minimum = config["MinimumAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(config["MinimumAmount"]);
            decimal cap = config["MaximumCap"] == DBNull.Value ? 0 : Convert.ToDecimal(config["MaximumCap"]);

            if (string.Equals(model.Document, "Securitization", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(method))
            {
                decimal masterRate = configuredRate, masterMinimum = minimum, masterCap = cap;
                if (model.RLCost > 0) configuredRate = model.RLCost;
                minimum = model.MinimumAmount;
                cap = model.MaximumCap;
                if (minimum > 0 && cap > 0 && cap < minimum)
                    throw new InvalidOperationException("Maximum Cap cannot be less than Minimum Billing.");
                if (model.UpdateMasterRates && (configuredRate != masterRate || minimum != masterMinimum || cap != masterCap))
                    UpdateHeaderRate(con, transaction, model.CostingRateID, configuredRate, method, minimum, cap);
                decimal quantity;
                if (method == "Hourly")
                {
                    if (model.HoursWorked <= 0) throw new InvalidOperationException("Hours Worked must be greater than zero.");
                    quantity = model.HoursWorked;
                    model.LoanCount = 0;
                }
                else if (method == "PerFile")
                {
                    if (model.LoanCount <= 0) throw new InvalidOperationException("File Count must be greater than zero.");
                    quantity = model.LoanCount;
                    model.HoursWorked = 0;
                }
                else throw new InvalidOperationException("Unsupported Securitization billing method.");

                model.BillingMethod = method;
                model.RLCost = configuredRate;
                model.MinimumAmount = minimum;
                model.MaximumCap = cap;
                model.BaseAmount = decimal.Round(quantity * configuredRate, 2);
                decimal amount = model.BaseAmount;
                model.MinimumApplied = minimum > 0 && amount < minimum;
                if (model.MinimumApplied) amount = minimum;
                model.CapApplied = cap > 0 && amount > cap;
                if (model.CapApplied) amount = cap;
                model.ExpectedBilling = amount;
                model.ProductDetails = new List<InvoiceCostingDetailModel>();
                return;
            }

            if (string.Equals(model.Document, "Securitization", StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(method))
            {
                method = model.BillingMethod;
                configuredRate = model.RLCost;
                minimum = model.MinimumAmount;
                cap = model.MaximumCap;
                if ((method != "PerFile" && method != "Hourly") || configuredRate <= 0)
                    throw new InvalidOperationException("Select a billing method and enter a rate greater than zero.");
                UpdateHeaderRate(con, transaction, model.CostingRateID, configuredRate, method, minimum, cap);
                ApplyCostingRules(con, transaction, model);
                return;
            }

            if (string.Equals(model.Document, "Reliance Letter", StringComparison.OrdinalIgnoreCase))
            {
                Dictionary<int, DataRow> configuredDetails = new Dictionary<int, DataRow>();
                DataTable detailTable = new DataTable();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT CostingDetailID, ProductType, Rate
                    FROM SecuritizationRelianceLetterCostingDetail
                    WHERE RateID=@RateID AND IsActive=1", con, transaction))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@RateID", SqlDbType.Int).Value = model.CostingRateID;
                    da.Fill(detailTable);
                }
                foreach (DataRow row in detailTable.Rows) configuredDetails[Convert.ToInt32(row["CostingDetailID"])] = row;

                if (configuredDetails.Count > 0 && (model.ProductDetails == null || model.ProductDetails.Count == 0))
                    throw new InvalidOperationException("Enter a quantity for at least one configured Reliance Letter product.");
                if (model.ProductDetails == null || model.ProductDetails.Count == 0) return;
                if (model.ProductDetails.Count > 1)
                    throw new InvalidOperationException("Select only one Reliance Letter scope per invoice.");

                decimal total = 0;
                foreach (InvoiceCostingDetailModel detail in model.ProductDetails)
                {
                    if (detail.CostingDetailID <= 0)
                    {
                        if (string.IsNullOrWhiteSpace(detail.ProductType) || detail.Rate <= 0)
                            throw new InvalidOperationException("Each new Reliance Letter scope requires a description and rate greater than zero.");
                        detail.CostingDetailID = InsertCostingDetail(con, transaction, model.CostingRateID, detail.ProductType, detail.Rate);
                        configuredDetails[detail.CostingDetailID] = null;
                    }
                    if (!configuredDetails.ContainsKey(detail.CostingDetailID)) throw new InvalidOperationException("A selected product rate is no longer available.");
                    if (detail.Quantity <= 0) throw new InvalidOperationException("Product quantity must be greater than zero.");
                    DataRow configured = configuredDetails[detail.CostingDetailID];
                    if (configured != null)
                    {
                        detail.ProductType = Convert.ToString(configured["ProductType"]);
                        decimal masterRate = Convert.ToDecimal(configured["Rate"]);
                        if (detail.Rate <= 0) detail.Rate = masterRate;
                        if (detail.Rate != masterRate && model.UpdateMasterRates)
                        {
                            using (SqlCommand updateRate = new SqlCommand("UPDATE SecuritizationRelianceLetterCostingDetail SET Rate=@Rate,UpdatedBy=@UserID,UpdatedDate=GETDATE() WHERE CostingDetailID=@ID", con, transaction))
                            {
                                AddDecimal(updateRate, "@Rate", detail.Rate);
                                updateRate.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(HttpContext.Current.User.Identity.Name);
                                updateRate.Parameters.Add("@ID", SqlDbType.Int).Value = detail.CostingDetailID;
                                updateRate.ExecuteNonQuery();
                            }
                        }
                    }
                    detail.Amount = decimal.Round(detail.Quantity * detail.Rate, 2);
                    total += detail.Amount;
                }
                model.BillingMethod = "ProductWise";
                model.LoanCount = Convert.ToInt32(model.ProductDetails.Sum(item => item.Quantity));
                model.RLCost = 0;
                model.BaseAmount = total;
                model.ExpectedBilling = total;
                model.MinimumAmount = 0; model.MaximumCap = 0; model.MinimumApplied = false; model.CapApplied = false;
            }
        }

        private static void UpdateHeaderRate(SqlConnection con, SqlTransaction transaction, int rateId, decimal rate, string method, decimal minimum, decimal cap)
        {
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE SecuritizationRelianceLetterCosting
                SET Rate=@Rate,BillingMethod=@Method,MinimumAmount=@Minimum,MaximumCap=@Cap,
                    UpdatedBy=@UserID,UpdatedDate=GETDATE() WHERE RateID=@RateID", con, transaction))
            {
                AddDecimal(cmd, "@Rate", rate); AddDecimal(cmd, "@Minimum", minimum); AddDecimal(cmd, "@Cap", cap);
                cmd.Parameters.Add("@Method", SqlDbType.NVarChar, 20).Value = method;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(HttpContext.Current.User.Identity.Name);
                cmd.Parameters.Add("@RateID", SqlDbType.Int).Value = rateId;
                cmd.ExecuteNonQuery();
            }
        }

        private static int CreateCostingFromInvoice(SqlConnection con, SqlTransaction transaction, InvoiceModel model)
        {
            bool securitization = string.Equals(model.Document, "Securitization", StringComparison.OrdinalIgnoreCase);
            bool relianceLetter = string.Equals(model.Document, "Reliance Letter", StringComparison.OrdinalIgnoreCase);
            if (!securitization && !relianceLetter) return 0;
            if (securitization && (model.BillingMethod != "PerFile" && model.BillingMethod != "Hourly" || model.RLCost <= 0))
                throw new InvalidOperationException("Select a billing method and enter a rate greater than zero.");
            if (relianceLetter && (model.ProductDetails == null || model.ProductDetails.Count == 0))
                throw new InvalidOperationException("Add at least one Reliance Letter scope and rate.");

            using (SqlCommand cmd = new SqlCommand(@"
                INSERT SecuritizationRelianceLetterCosting
                    (ProjectId,Rate,Type,BillingMethod,MinimumAmount,MaximumCap,EffectiveFrom,IsActive,AddedBy,AddedDate)
                OUTPUT INSERTED.RateID
                VALUES (@ProjectID,@Rate,@Type,@Method,@Minimum,@Cap,@EffectiveFrom,1,@UserID,GETDATE())", con, transaction))
            {
                cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = model.ProjectID;
                AddDecimal(cmd, "@Rate", securitization ? model.RLCost : 0);
                cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 200).Value = model.Document;
                cmd.Parameters.Add("@Method", SqlDbType.NVarChar, 20).Value = securitization ? (object)model.BillingMethod : DBNull.Value;
                AddDecimal(cmd, "@Minimum", securitization ? model.MinimumAmount : 0);
                AddDecimal(cmd, "@Cap", securitization ? model.MaximumCap : 0);
                cmd.Parameters.Add("@EffectiveFrom", SqlDbType.Date).Value = ParseOptionalInvoiceDate(model.InvoiceDate);
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(HttpContext.Current.User.Identity.Name);
                int rateId = Convert.ToInt32(cmd.ExecuteScalar());
                if (relianceLetter)
                    foreach (InvoiceCostingDetailModel detail in model.ProductDetails)
                        detail.CostingDetailID = InsertCostingDetail(con, transaction, rateId, detail.ProductType, detail.Rate);
                return rateId;
            }
        }

        private static int InsertCostingDetail(SqlConnection con, SqlTransaction transaction, int rateId, string productType, decimal rate)
        {
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT SecuritizationRelianceLetterCostingDetail
                    (RateID,ProductType,Rate,EffectiveFrom,IsActive,AddedBy)
                OUTPUT INSERTED.CostingDetailID
                VALUES (@RateID,@ProductType,@Rate,CAST(GETDATE() AS date),1,@UserID)", con, transaction))
            {
                cmd.Parameters.Add("@RateID", SqlDbType.Int).Value = rateId;
                cmd.Parameters.Add("@ProductType", SqlDbType.NVarChar, 250).Value = productType.Trim();
                AddDecimal(cmd, "@Rate", rate);
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(HttpContext.Current.User.Identity.Name);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private static void AddInvoiceParameters(SqlCommand cmd, InvoiceModel model)
        {
            cmd.Parameters.AddWithValue("@OurClient", model.OurClient ?? "");
            cmd.Parameters.AddWithValue("@Recipient", model.Recipient ?? "");
            cmd.Parameters.AddWithValue("@TradeName", model.TradeName ?? "");
            cmd.Parameters.AddWithValue("@InvoiceDate", FormatOptionalInvoiceDate(model.InvoiceDate));
            cmd.Parameters.AddWithValue("@Document", model.Document ?? "");
            cmd.Parameters.AddWithValue("@TM", model.TM ?? "");
            cmd.Parameters.AddWithValue("@DocuSign", model.DocSign ?? "");
            cmd.Parameters.AddWithValue("@DocumentDate", FormatOptionalInvoiceDate(model.DocumentDate));
            cmd.Parameters.AddWithValue("@ExecutedDate", FormatOptionalInvoiceDate(model.ExecutedDate));
            cmd.Parameters.AddWithValue("@BillingEntity", model.BillingEntity ?? "");
            cmd.Parameters.AddWithValue("@ContactPerson", SerializeEmailConfiguration(model.EmailConfiguration));
            cmd.Parameters.AddWithValue("@LoanCount", model.LoanCount);
            AddDecimal(cmd, "@Cost", model.RLCost);
            AddDecimal(cmd, "@ExpectedBilling", model.ExpectedBilling);
            cmd.Parameters.AddWithValue("@Notes", model.Notes ?? "");
            cmd.Parameters.Add("@CostingRateID", SqlDbType.Int).Value = model.CostingRateID > 0 ? (object)model.CostingRateID : DBNull.Value;
            cmd.Parameters.Add("@BillingMethod", SqlDbType.NVarChar, 20).Value = string.IsNullOrWhiteSpace(model.BillingMethod) ? (object)DBNull.Value : model.BillingMethod;
            AddDecimal(cmd, "@HoursWorked", model.HoursWorked);
            AddDecimal(cmd, "@MinimumAmount", model.MinimumAmount);
            AddDecimal(cmd, "@MaximumCap", model.MaximumCap);
            AddDecimal(cmd, "@BaseAmount", model.BaseAmount);
            cmd.Parameters.Add("@MinimumApplied", SqlDbType.Bit).Value = model.MinimumApplied;
            cmd.Parameters.Add("@CapApplied", SqlDbType.Bit).Value = model.CapApplied;
            cmd.Parameters.Add("@AddedBy", SqlDbType.Int).Value = int.Parse(HttpContext.Current.User.Identity.Name);
        }

        private static void AddDecimal(SqlCommand cmd, string name, decimal value)
        {
            SqlParameter parameter = cmd.Parameters.Add(name, SqlDbType.Decimal);
            parameter.Precision = 18; parameter.Scale = 2; parameter.Value = value;
        }

        private static void SaveInvoiceDetails(SqlConnection con, SqlTransaction transaction, int invoiceId, IEnumerable<InvoiceCostingDetailModel> details)
        {
            if (details == null) return;
            foreach (InvoiceCostingDetailModel detail in details)
            {
                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT RLInvoiceCostingDetail
                        (InvoiceID, CostingDetailID, BillingDescription, Quantity, Rate, Amount)
                    VALUES (@InvoiceID,@CostingDetailID,@Description,@Quantity,@Rate,@Amount)", con, transaction))
                {
                    cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;
                    cmd.Parameters.Add("@CostingDetailID", SqlDbType.Int).Value = detail.CostingDetailID > 0 ? (object)detail.CostingDetailID : DBNull.Value;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value = detail.ProductType ?? string.Empty;
                    AddDecimal(cmd, "@Quantity", detail.Quantity); AddDecimal(cmd, "@Rate", detail.Rate); AddDecimal(cmd, "@Amount", detail.Amount);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public class BillingModel
        {
            public string OurClient { get; set; }
            public string Recipient { get; set; }
            public string TradeName { get; set; }
            public string InvoiceDate { get; set; }
            public string Document { get; set; }
            public string TM { get; set; }
            public string DocSign { get; set; }
            public string DocumentDate { get; set; }
            public string ExecutedDate { get; set; }
            public string BillingEntity { get; set; }
            public string LoanCount { get; set; }
            public string Cost { get; set; }
            public string ExpectedBilling { get; set; }
            public string Notes { get; set; }
        }
        public class ImportSummary
        {
            public int Total { get; set; }
            public int Duplicate { get; set; }
            public int Inserted { get; set; }
            public int Error { get; set; }
        }

        public class ImportResult
        {
            public ImportSummary Summary { get; set; }
            public List<object> Duplicates { get; set; }
            public List<object> Errors { get; set; }

            public List<object> Inserted { get; set; }
        }
        public class InvoiceModel
        {
            public int InvoiceID { get; set; }
            public string OurClient { get; set; }
            public string Recipient { get; set; }
            public string TradeName { get; set; }
            public string InvoiceDate { get; set; }
            public string Document { get; set; }
            public string TM { get; set; }
            public string DocSign { get; set; }
            public string DocumentDate { get; set; }
            public string ExecutedDate { get; set; }
            public string BillingEntity { get; set; }
            public string EmailConfiguration { get; set; }
            public int LoanCount { get; set; }
            public decimal RLCost { get; set; }
            public decimal ExpectedBilling { get; set; }
            public string Notes { get; set; }
            public int ProjectID { get; set; }
            public int CostingRateID { get; set; }
            public string BillingMethod { get; set; }
            public decimal HoursWorked { get; set; }
            public decimal MinimumAmount { get; set; }
            public decimal MaximumCap { get; set; }
            public decimal BaseAmount { get; set; }
            public bool MinimumApplied { get; set; }
            public bool CapApplied { get; set; }
            public List<InvoiceCostingDetailModel> ProductDetails { get; set; }
            public bool UpdateMasterRates { get; set; }
        }

        public class InvoiceCostingDetailModel
        {
            public int CostingDetailID { get; set; }
            public string ProductType { get; set; }
            public decimal Quantity { get; set; }
            public decimal Rate { get; set; }
            public decimal Amount { get; set; }
        }
    }
}

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
                        DocumentDate, ExecutedDate, LoanCount, Cost, ExpectedBilling,
                        BillingEntity, ContactPerson, SubmittedForInvoice, InvoiceIssued, Notes,FilePath, isVerify, VerifyRemark, VerifiedOn
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
                            InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]).ToString("MM.dd.yyyy"),
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
                                InvoiceDate = dr["InvoiceDate"] == DBNull.Value
                                    ? ""
                                    : Convert.ToDateTime(dr["InvoiceDate"]).ToString("MM.dd.yyyy")
                                    ,
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
                                InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]).ToString("MM.dd.yyyy"),
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
                                InvoiceDate = dr["InvoiceDate"] == DBNull.Value
                                    ? ""
                                    : Convert.ToDateTime(dr["InvoiceDate"]).ToString("MM.dd.yyyy"),
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
                        cmd.Parameters.AddWithValue("@InvoiceDate", obj.InvoiceDate);
                        cmd.Parameters.AddWithValue("@Document", obj.Document);
                        cmd.Parameters.AddWithValue("@TM", obj.TM);
                        cmd.Parameters.AddWithValue("@DocSign", obj.DocSign);
                        cmd.Parameters.AddWithValue("@DocumentDate", string.IsNullOrEmpty(obj.DocumentDate) ? (object)DBNull.Value : Convert.ToDateTime(obj.DocumentDate));
                        cmd.Parameters.AddWithValue("@ExecutedDate", string.IsNullOrEmpty(obj.ExecutedDate) ? (object)DBNull.Value : Convert.ToDateTime(obj.ExecutedDate));
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
        public static string SaveInvoice(InvoiceModel model)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO RLinvoice
            (
                OurClient, Recipient, TradeName, InvoiceDate,
                Document, TM, DocuSign, DocumentDate, ExecutedDate,
                BillingEntity, ContactPerson,
                LoanCount, Cost, ExpectedBilling, Notes
            )
            VALUES
            (
                @OurClient, @Recipient, @TradeName, @InvoiceDate,
                @Document, @TM, @DocuSign, @DocumentDate, @ExecutedDate,
                @BillingEntity, @ContactPerson,
                @LoanCount, @Cost, @ExpectedBilling, @Notes
            )", con);

                cmd.Parameters.AddWithValue("@OurClient", model.OurClient ?? "");
                cmd.Parameters.AddWithValue("@Recipient", model.Recipient ?? "");
                cmd.Parameters.AddWithValue("@TradeName", model.TradeName ?? "");
                cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(model.InvoiceDate).ToString("MM.dd.yyyy"));

                cmd.Parameters.AddWithValue("@Document", model.Document ?? "");
                cmd.Parameters.AddWithValue("@TM", model.TM ?? "");
                cmd.Parameters.AddWithValue("@DocuSign", model.DocSign ?? "");

                cmd.Parameters.AddWithValue("@DocumentDate",
                    string.IsNullOrEmpty(model.DocumentDate) ? (object)DBNull.Value : Convert.ToDateTime(model.DocumentDate).ToString("MM.dd.yyyy"));

                cmd.Parameters.AddWithValue("@ExecutedDate",
                    string.IsNullOrEmpty(model.ExecutedDate) ? (object)DBNull.Value : Convert.ToDateTime(model.ExecutedDate).ToString("MM.dd.yyyy"));

                cmd.Parameters.AddWithValue("@BillingEntity", model.BillingEntity);
                cmd.Parameters.AddWithValue("@ContactPerson", SerializeEmailConfiguration(model.EmailConfiguration));

                cmd.Parameters.AddWithValue("@LoanCount", model.LoanCount);
                cmd.Parameters.AddWithValue("@Cost", model.RLCost);
                cmd.Parameters.AddWithValue("@ExpectedBilling", model.ExpectedBilling);
                cmd.Parameters.AddWithValue("@Notes", model.Notes ?? "");

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return "Saved";
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
        public static string UpdateInvoice(InvoiceModel model)
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

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
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
                Notes = @Notes
            WHERE InvoiceID = @InvoiceID", con);

                cmd.Parameters.AddWithValue("@InvoiceID", model.InvoiceID);

                cmd.Parameters.AddWithValue("@OurClient", model.OurClient ?? "");
                cmd.Parameters.AddWithValue("@Recipient", model.Recipient ?? "");
                cmd.Parameters.AddWithValue("@TradeName", model.TradeName ?? "");
                cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(model.InvoiceDate).ToString("MM.dd.yyyy"));

                cmd.Parameters.AddWithValue("@Document", model.Document ?? "");
                cmd.Parameters.AddWithValue("@TM", model.TM ?? "");
                cmd.Parameters.AddWithValue("@DocuSign", model.DocSign ?? "");

                cmd.Parameters.AddWithValue("@DocumentDate",
                    string.IsNullOrEmpty(model.DocumentDate) ? (object)DBNull.Value : Convert.ToDateTime(model.DocumentDate).ToString("MM.dd.yyyy"));

                cmd.Parameters.AddWithValue("@ExecutedDate",
                    string.IsNullOrEmpty(model.ExecutedDate) ? (object)DBNull.Value : Convert.ToDateTime(model.ExecutedDate).ToString("MM.dd.yyyy"));

                cmd.Parameters.AddWithValue("@BillingEntity", model.BillingEntity);
                cmd.Parameters.AddWithValue("@ContactPerson", SerializeEmailConfiguration(model.EmailConfiguration));

                cmd.Parameters.AddWithValue("@LoanCount", model.LoanCount);
                cmd.Parameters.AddWithValue("@Cost", model.RLCost);
                cmd.Parameters.AddWithValue("@ExpectedBilling", model.ExpectedBilling);
                cmd.Parameters.AddWithValue("@Notes", model.Notes ?? "");

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return "Updated";
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
        }
    }
}

using ClosedXML.Excel;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;
using Vendor_Portal.App_Code.DAL;


namespace Canopy_Billing.BDM
{
    public partial class ImportBilling : System.Web.UI.Page
    {
        static DataTable dtImport = new DataTable();

        static string NewFileName = "";
        static string FileName = "";
        static string GUIDFile = "";
        static string FolderPath = "";
        static Workbook book = new Workbook();
        static Worksheet wksheet = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            FolderPath = Server.MapPath(@"~\UploadBilling\");

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

                FileName = file.FileName;

                //NewFileName = Server.MapPath("..//TempFiles//" + file.FileName);
                NewFileName = Server.MapPath("..//UploadBilling//" + file.FileName);

                FileStream objfilestream = new FileStream(NewFileName, FileMode.Create, FileAccess.ReadWrite);
                objfilestream.Write(binaryWriteArray, 0,
                binaryWriteArray.Length);
                objfilestream.Close();
            }
            catch { }
        }


        [WebMethod]
        public static int VerifyAndSubmitData(string BillingType, string BillingPeriod)
        {
            int ReturnValue = 0;
            string TableName = "";

            if (dtImport.Rows.Count == 0)
                return ReturnValue = 0;

            try
            {
                DataTable Dt = dtImport;

                if (BillingType == "Griffin")
                    TableName = "dbo.GriffinPreClose";

                if (BillingType == "StewartAVM")
                    TableName = "dbo.StewartAVM";

                if (BillingType == "StewartCDA")
                    TableName = "dbo.StewartCDA_BPO";

                if (BillingType == "Billing")
                    TableName = "dbo.TempCanopyBilling";
                

                #region Griffin

                if (Dt != null)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.ConnectionString2))
                    {
                        sqlConnection.Open();

                        SqlBulkCopy objbulk = new SqlBulkCopy(sqlConnection);

                        objbulk.DestinationTableName = TableName;

                        string destTableQuery = "SELECT TOP 1 * FROM " + TableName;

                        SqlCommand cmd = new SqlCommand(destTableQuery, sqlConnection);

                        DataTable dtDest = SQLHelper.ExecuteDataSetCmd_Billing(cmd).Tables[0];

                        Dt.Columns.Add("AddedBy", typeof(int));
                        Dt.Columns.Add("AddedDate", typeof(DateTime));
                        Dt.Columns.Add("BillingPeriod", typeof(string));

                        Dt.AsEnumerable().ToList().ForEach(row => row["BillingPeriod"] = BillingPeriod);
                        Dt.AsEnumerable().ToList().ForEach(row => row["AddedDate"] = DateTime.Now);
                        Dt.AsEnumerable().ToList().ForEach(row => row["AddedBy"] = int.Parse(HttpContext.Current.User.Identity.Name));

                        for (int i = 0; i < dtDest.Columns.Count; i++)
                        {
                            string destinationColumnName = dtDest.Columns[i].ColumnName;

                            if (Dt.Columns.Contains(destinationColumnName))
                            {
                                objbulk.ColumnMappings.Add(destinationColumnName, destinationColumnName);
                            }
                        }

                        objbulk.WriteToServer(Dt);

                        ReturnValue = dtImport.Rows.Count;
                        dtImport = null;
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                ReturnValue = 0;
            }

            return ReturnValue;
        }


        [WebMethod]
        public static string GetExcelDataToBindGrid(string BillingType)
        {
            List<string> expectedColumns = new List<string>();
            DataTable dt1 = ReadExcelFile(NewFileName);

            dtImport = dt1;

            if (BillingType == "Griffin")
            {
                expectedColumns = new List<string> { "Loan ID", "Name", "Submission Date", "User", "Complete Date", "Loan Program (Client Lock)", "Griffin Status", "Base Price", "Bank Statements", "Tax Returns", "Multiple Property Calculation", "Esclator Adjustment", "Total" };
            }

            if (BillingType == "StewartAVM")
            {
                expectedColumns = new List<string> { "RequestDate", "CAID", "LoanNo", "Address", "City", "State", "ZipCode" };
            }

            if (BillingType == "StewartCDA")
            {
                expectedColumns = new List<string> { "InvoiceDate", "OrderDate", "CompleteDate", "BusinessDays", "InvoiceNumber", "Fee", "OrderID", "LoanNumber", "CaseNumber", "Borrower", "Address1", "Address2", "City", "State", "Zip" };
            }

            if (BillingType == "Billing")
            {
                expectedColumns = new List<string>  {"LoanId",
                 "createdDate",
                "submittedDate",
                "snapshotTakenDate",
                "buyerName",
                "sellerName",
                "scriptName",
                "TransactionIdentifier"
                     };
            }

            // Get Excel Columns
            List<string> excelColumns = dt1.Columns.Cast<DataColumn>().Select(c => c.ColumnName.Trim()).ToList();

            // Check mismatch
            bool isMismatch = expectedColumns.Count != excelColumns.Count || expectedColumns.Except(excelColumns).Any() || excelColumns.Except(expectedColumns).Any();

            // Return mismatch message
            if (isMismatch)
            {
                return "mismatched";
            }

            // Convert DataTable to JSON
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt1.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();

                foreach (DataColumn col in dt1.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }

                rows.Add(row);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;

            return ser.Serialize(rows);
        }


        [WebMethod]
        public static int ClearData()
        {
            dtImport = null;
            NewFileName = null;
            return 1;
        }


        public static DataTable ReadExcelFile(string path)
        {
            DataTable dt = new DataTable();
            if (path != null)
            {
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
                            dt.Rows.Add(row.Cells().Select(c => c.GetValue<string>() ?? "").ToArray());
                        }
                    }
                }
            }
            return dt;
        }

    }
}
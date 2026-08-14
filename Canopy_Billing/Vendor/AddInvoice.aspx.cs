using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.Vendor
{
    public partial class AddInvoice : System.Web.UI.Page
    {
        static string NewFileName = "";
        static string NewFileName_SciennaDoc = "";
        static string NewFileName_Excel = "";
        static string NewFileName_LaborCharges = "";
        static string NewFileName_LoanDetails = "";
        static string NewFileName_CompInvoice = "";
        static string NewFileName_CompExcel = "";
        static string NewFileName_RemoteUW = "";
        static string NewFileName_StewartInvoice = "";
        static string NewFileName_StewartExcel = "";
        static string GUIDFile = "";
        static string MainPath = "";
        public static string Con_Canopy = "Data Source=192.168.11.11,8989;Initial Catalog=Canopy-UWVendorBilling;User ID=sa;Password=idt15central";
        public static string Con = "Data Source=192.168.11.11,8989;Initial Catalog=Infinity-UWVendorBilling;User ID=sa;Password=idt15central";
        protected void Page_Load(object sender, EventArgs e)
        {
            MainPath = Server.MapPath(@"~\Invoice");
            //Regular Attachment
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
                GUIDFile = file_Name;
                if (file.FileName.Contains("SciennaDoc_"))
                {
                    NewFileName_SciennaDoc = Server.MapPath("..//TempFiles//" + "SciennaDoc_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_SciennaDoc, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                else if (file.FileName.Contains("SciennaExcel_"))
                {
                    NewFileName_Excel = Server.MapPath("..//TempFiles//" + "SciennaExcel_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_Excel, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                else if (file.FileName.Contains("LaborCharges_"))
                {
                    NewFileName_LaborCharges = Server.MapPath("..//TempFiles//" + "LaborCharges_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_LaborCharges, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                else if (file.FileName.Contains("LoanDetails_"))
                {
                    NewFileName_LoanDetails = Server.MapPath("..//TempFiles//" + "LoanDetails_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_LoanDetails, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                else if (file.FileName.Contains("ComplianceInvoice_"))
                {
                    NewFileName_CompInvoice = Server.MapPath("..//TempFiles//" + "ComplianceInvoice_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_CompInvoice, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                else if (file.FileName.Contains("ComplianceExcel_"))
                {
                    NewFileName_CompExcel = Server.MapPath("..//TempFiles//" + "ComplianceExcel_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_CompExcel, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                else if (file.FileName.Contains("RemoteUW_"))
                {
                    NewFileName_RemoteUW = Server.MapPath("..//TempFiles//" + "RemoteUW_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_RemoteUW, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                else if (file.FileName.Contains("StewartInvoice_"))
                {
                    NewFileName_StewartInvoice = Server.MapPath("..//TempFiles//" + "StewartInvoice_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_StewartInvoice, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                else if (file.FileName.Contains("StewartExcel_"))
                {
                    NewFileName_StewartExcel = Server.MapPath("..//TempFiles//" + "StewartExcel_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_StewartExcel, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                else
                {
                    NewFileName = Server.MapPath("..//TempFiles//" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }

            }
            catch { }

        }

        #region Infinity

        [WebMethod]
        public static string GetAllInvoices()
        {
            DataTable dt1 = new bllInvoice().GetInfinityInvoiceDetails(int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
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
        public static string GetSciennaDetails_AfterImport(int InvoiceID, string Month, string Year)
        {
            DataTable dt1 = new bllInvoice().GetSciennaDetailsAfterImport(InvoiceID, Month, Year);
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
        public static string GetLaborCharges_AfterImport(int InvoiceID, string Month, string Year)
        {
            DataTable dt1 = new bllInvoice().GetSciennaLaborDetailsAfterImport(InvoiceID, Month, Year);
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
        public static string GetLoanDetails_AfterImport()
        {
            DataTable dt1 = new bllInvoice().GetSciennaLoanDetailsAfterImport();
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
        public static int InsertInfinityInvoice(string Month, string Year, string InvoiceDate, string DueDate, string Domain, string Currency, string LoanCount, string InvoiceNumber, string InvoiceAmount, string BillTo, string InvoiceType, string OtherInvoice, string DelayRemark, string VerificationRemark)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam.Add("Month", Month);
            htParam.Add("Year", Year);
            htParam.Add("StatementDate", InvoiceDate);
            htParam.Add("DueDate", DueDate);
            htParam.Add("Domain", Domain);
            htParam.Add("Currency", Currency);
            htParam.Add("NoOfLoans", LoanCount);
            htParam.Add("VendorInvoiceNumber", InvoiceNumber);
            htParam.Add("TotalDue", InvoiceAmount);
            htParam.Add("VendorName", BillTo);
            if (InvoiceType == "Other" || InvoiceType == "Abstractor")
                htParam.Add("InvoiceType", InvoiceType + " : " + OtherInvoice);
            else
                htParam.Add("InvoiceType", InvoiceType);

            if (NewFileName != "")
            {
                string CodeDate = DateTime.Now.ToString("dd-MMM-yyyy-HHMMss");
                if (!Directory.Exists(MainPath))
                {
                    Directory.CreateDirectory(MainPath);
                }
                string SubPath = MainPath + "\\" + Convert.ToString(InvoiceType);
                if (!Directory.Exists(SubPath))
                {
                    Directory.CreateDirectory(SubPath);
                }
                string UniquePath = MainPath + "\\" + Convert.ToString(InvoiceType) + "\\" + Convert.ToString(CodeDate);
                if (!Directory.Exists(UniquePath))
                {
                    Directory.CreateDirectory(UniquePath);
                }
                File.Copy(NewFileName, UniquePath + "\\" + GUIDFile);
                File.Delete(NewFileName);
                htParam.Add("FilePath", UniquePath + "\\" + GUIDFile);
            }
            else
            {
                htParam.Add("FilePath", "");
            }

            htParam.Add("AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            htParam.Add("ProjectId", "0");
            htParam.Add("Delay", DelayRemark);
            htParam.Add("Remark", VerificationRemark);
            returnvalue = new bllInvoice().Insert_Infinity_InvoiceDetails(htParam);
            return returnvalue;
        }

        [WebMethod]
        public static int InsertInfinityInvoice_Scienna(string Month, string Year, string InvoiceDate, string DueDate, string Domain, string Currency, string LoanCount, string InvoiceNumber, string InvoiceAmount, string BillTo, string InvoiceType, string OtherInvoice, string DelayRemark, string VerificationRemark)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam.Add("Month", Month);
            htParam.Add("Year", Year);
            htParam.Add("StatementDate", InvoiceDate);
            htParam.Add("DueDate", DueDate);
            htParam.Add("Domain", Domain);
            htParam.Add("Currency", Currency);
            htParam.Add("NoOfLoans", LoanCount);
            htParam.Add("VendorInvoiceNumber", InvoiceNumber);
            htParam.Add("TotalDue", InvoiceAmount);
            htParam.Add("VendorName", BillTo);
            if (InvoiceType == "Other" || InvoiceType == "Abstractor")
                htParam.Add("InvoiceType", InvoiceType + " : " + OtherInvoice);
            else
                htParam.Add("InvoiceType", InvoiceType);

            if (NewFileName_SciennaDoc != "")
            {
                string CodeDate = DateTime.Now.ToString("dd-MMM-yyyy-HHMMss");
                if (!Directory.Exists(MainPath))
                {
                    Directory.CreateDirectory(MainPath);
                }
                string SubPath = MainPath + "\\" + Convert.ToString(InvoiceType);
                if (!Directory.Exists(SubPath))
                {
                    Directory.CreateDirectory(SubPath);
                }
                string UniquePath = MainPath + "\\" + Convert.ToString(InvoiceType) + "\\" + Convert.ToString(CodeDate);
                if (!Directory.Exists(UniquePath))
                {
                    Directory.CreateDirectory(UniquePath);
                }
                File.Copy(NewFileName_SciennaDoc, UniquePath + "\\" + GUIDFile);
                File.Delete(NewFileName_SciennaDoc);
                htParam.Add("FilePath", UniquePath + "\\" + GUIDFile);
            }
            else
            {
                htParam.Add("FilePath", "");
            }
            if (NewFileName_LoanDetails != "")
            {
                string CodeDate = DateTime.Now.ToString("dd-MMM-yyyy-HHMMss");
                if (!Directory.Exists(MainPath))
                {
                    Directory.CreateDirectory(MainPath);
                }
                string SubPath = MainPath + "\\" + Convert.ToString("SciennaLoan");
                if (!Directory.Exists(SubPath))
                {
                    Directory.CreateDirectory(SubPath);
                }
                string UniquePath = MainPath + "\\" + Convert.ToString("SciennaLoan") + "\\" + Convert.ToString(CodeDate);
                if (!Directory.Exists(UniquePath))
                {
                    Directory.CreateDirectory(UniquePath);
                }
                File.Copy(NewFileName_LoanDetails, UniquePath + "\\" + GUIDFile);
                File.Delete(NewFileName_LoanDetails);
                htParam.Add("LoanFilePath", UniquePath + "\\" + GUIDFile);
            }
            else
            {
                htParam.Add("LoanFilePath", "");
            }

            htParam.Add("AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            htParam.Add("ProjectId", "0");
            htParam.Add("Delay", DelayRemark);
            htParam.Add("Remark", VerificationRemark);
            // For Attachment Import Function
            htParam.Add("Type", "Image");
            htParam.Add("BillingType", "");
            htParam.Add("Frequency", "");
            htParam.Add("VendorAccountNumber", "");
            htParam.Add("VendorAddress", "");
            returnvalue = new bllInvoice().Insert_Infinity_InvoiceDetails(htParam);
            return returnvalue;
        }

        [WebMethod]
        public static int InsertSciennaInvoice(int InvoiceID, string Month, string Year)
        {
            int returnvalue = 0;
            int isdeleted = new bllInvoice().DeleteScienna(Month, Year);
            if (isdeleted > 0)
            {
                if (NewFileName_Excel != "")
                {
                    string fileName = NewFileName_Excel.Substring(NewFileName_Excel.LastIndexOf("\\") + 1);
                    string Extn = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    if (Extn == "xls" | Extn == "xlsx")
                    {

                        string ConExcel;
                        if (Extn.Contains("xlsx"))
                        {
                            ConExcel = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + NewFileName_Excel + "; Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
                        }
                        else
                        {
                            ConExcel = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + NewFileName_Excel + "; Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
                        }

                        DataSet dsExcel = new DataSet();
                        DataTable Dt = new DataTable("[Sheet1$]");
                        using (OleDbConnection myExcelConnection = new OleDbConnection(ConExcel))
                        {
                            string sqlExcel = "Select * from [Sheet1$]";
                            OleDbDataAdapter daExcel = new OleDbDataAdapter(sqlExcel, myExcelConnection);
                            daExcel.Fill(dsExcel);
                            daExcel.Dispose();
                            Dt = dsExcel.Tables[0];
                        }
                        try
                        {
                            DataTable dt1 = new DataTable();
                            dt1 = Dt;

                            DataTable dtProductSold = Dt;
                            Dt.Columns.Add("Month", typeof(String));
                            Dt.Columns.Add("Year", typeof(String));
                            Dt.Columns.Add("AddedBy", typeof(int));
                            Dt.Columns.Add("InvoiceID", typeof(int));
                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {
                                Dt.Rows[i]["Month"] = Month;
                                Dt.Rows[i]["Year"] = Year;
                                Dt.Rows[i]["AddedBy"] = int.Parse(HttpContext.Current.User.Identity.Name.ToString());
                                Dt.Rows[i]["InvoiceID"] = InvoiceID;
                            }
                            dt1 = Dt;

                            DataRow row = Dt.Rows[0];
                            Dt.Rows.Remove(row);

                            SqlConnection con = new SqlConnection(Con);
                            SqlBulkCopy objbulk = new SqlBulkCopy(Con);

                            //assigning Destination table name  
                            objbulk.DestinationTableName = "Scienna";

                            //Mapping Table column  
                            objbulk.ColumnMappings.Add("F1", "Client");
                            objbulk.ColumnMappings.Add("F2", "Project");
                            objbulk.ColumnMappings.Add("F3", "PeriodEnding");
                            objbulk.ColumnMappings.Add("F4", "LoansReviewed");
                            objbulk.ColumnMappings.Add("F5", "PerLoanUsageFees");
                            objbulk.ColumnMappings.Add("F6", "UsageFees");
                            objbulk.ColumnMappings.Add("F7", "Labour");
                            objbulk.ColumnMappings.Add("F8", "Total");
                            objbulk.ColumnMappings.Add("Month", "Month");
                            objbulk.ColumnMappings.Add("Year", "Year");
                            objbulk.ColumnMappings.Add("AddedBy", "AddedBy");
                            objbulk.ColumnMappings.Add("InvoiceID", "InvoiceID");

                            //inserting bulk Records into DataBase   
                            objbulk.WriteToServer(Dt);
                            returnvalue = 1;
                        }
                        catch (Exception ex)
                        {
                            returnvalue = 0;
                        }
                    }
                    else
                    {
                        returnvalue = -4;
                    }

                }
            }
            else if (isdeleted == 0)
            {
                returnvalue = -1;
            }
            else
            {
                returnvalue = -2;
            }

            return returnvalue;
        }

        [WebMethod]
        public static int InsertLaborCharges(int InvoiceID, string Month, string Year)
        {
            int returnvalue = 0;
            int isdeleted = new bllInvoice().DeleteSciennaLabour(Month, Year);
            if (isdeleted > 0)
            {
                if (NewFileName_LaborCharges != "")
                {
                    string fileName = NewFileName_LaborCharges.Substring(NewFileName_LaborCharges.LastIndexOf("\\") + 1);
                    string Extn = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    if (Extn == "xls" | Extn == "xlsx")
                    {

                        string ConExcel;
                        if (Extn.Contains("xlsx"))
                        {
                            ConExcel = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + NewFileName_LaborCharges + "; Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
                        }
                        else
                        {
                            ConExcel = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + NewFileName_LaborCharges + "; Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
                        }

                        DataSet dsExcel = new DataSet();
                        DataTable Dt = new DataTable("[Sheet1$]");
                        using (OleDbConnection myExcelConnection = new OleDbConnection(ConExcel))
                        {
                            string sqlExcel = "Select * from [Sheet1$]";
                            OleDbDataAdapter daExcel = new OleDbDataAdapter(sqlExcel, myExcelConnection);
                            daExcel.Fill(dsExcel);
                            daExcel.Dispose();
                            Dt = dsExcel.Tables[0];
                        }
                        try
                        {
                            DataTable dt1 = new DataTable();
                            dt1 = Dt;

                            DataTable dtProductSold = Dt;
                            Dt.Columns.Add("Month", typeof(String));
                            Dt.Columns.Add("Year", typeof(String));
                            Dt.Columns.Add("AddedBy", typeof(int));
                            Dt.Columns.Add("InvoiceID", typeof(int));
                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {
                                Dt.Rows[i]["Month"] = Month;
                                Dt.Rows[i]["Year"] = Year;
                                Dt.Rows[i]["AddedBy"] = int.Parse(HttpContext.Current.User.Identity.Name.ToString());
                                Dt.Rows[i]["InvoiceID"] = InvoiceID;
                            }
                            dt1 = Dt;

                            DataRow row = Dt.Rows[0];
                            Dt.Rows.Remove(row);

                            SqlConnection con = new SqlConnection(Con);
                            SqlBulkCopy objbulk = new SqlBulkCopy(Con);
                            //assigning Destination table name  
                            objbulk.DestinationTableName = "SciennaLabour";
                            //Mapping Table column  
                            objbulk.ColumnMappings.Add("F1", "Personnel");
                            objbulk.ColumnMappings.Add("F2", "Client");
                            objbulk.ColumnMappings.Add("F3", "Project");
                            objbulk.ColumnMappings.Add("F4", "Date");
                            objbulk.ColumnMappings.Add("F5", "BeginTime");
                            objbulk.ColumnMappings.Add("F6", "EndTime");
                            objbulk.ColumnMappings.Add("F7", "Hours");
                            objbulk.ColumnMappings.Add("F8", "Rate");
                            objbulk.ColumnMappings.Add("F9", "PreliminaryFee");
                            objbulk.ColumnMappings.Add("F10", "ChargedAt");
                            objbulk.ColumnMappings.Add("F11", "Reason");
                            objbulk.ColumnMappings.Add("F12", "Fee");
                            objbulk.ColumnMappings.Add("F13", "Activity");
                            objbulk.ColumnMappings.Add("F14", "DescriptionOfSessionActivities");
                            objbulk.ColumnMappings.Add("Month", "Month");
                            objbulk.ColumnMappings.Add("Year", "Year");
                            objbulk.ColumnMappings.Add("InvoiceID", "InvoiceID");
                            objbulk.ColumnMappings.Add("AddedBy", "AddedBy");

                            //inserting bulk Records into DataBase   
                            objbulk.WriteToServer(Dt);
                            returnvalue = 1;
                        }
                        catch (Exception ex)
                        {
                            returnvalue = 0;
                        }
                    }
                    else
                    {
                        returnvalue = -4;
                    }

                }
            }
            else if (isdeleted == 0)
            {
                returnvalue = -1;
            }
            else
            {
                returnvalue = -2;
            }
            return returnvalue;
        }

        [WebMethod]
        public static int InsertSciennaLoans()
        {
            int returnvalue = 0;
            if (NewFileName_LoanDetails != "")
            {
                string fileName = NewFileName_LoanDetails.Substring(NewFileName_LoanDetails.LastIndexOf("\\") + 1);
                string Extn = fileName.Substring(fileName.LastIndexOf(".") + 1);
                if (Extn == "xls" | Extn == "xlsx")
                {

                    string ConExcel;
                    if (Extn.Contains("xlsx"))
                    {
                        ConExcel = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + NewFileName_LoanDetails + "; Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
                    }
                    else
                    {
                        ConExcel = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + NewFileName_LoanDetails + "; Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
                    }

                    DataSet dsExcel = new DataSet();
                    DataTable Dt = new DataTable("[Sheet1$]");
                    using (OleDbConnection myExcelConnection = new OleDbConnection(ConExcel))
                    {
                        string sqlExcel = "Select * from [Sheet1$]";
                        OleDbDataAdapter daExcel = new OleDbDataAdapter(sqlExcel, myExcelConnection);
                        daExcel.Fill(dsExcel);
                        daExcel.Dispose();
                        Dt = dsExcel.Tables[0];
                    }
                    try
                    {
                        SqlConnection con = new SqlConnection(Con);
                        SqlBulkCopy objbulk = new SqlBulkCopy(Con);

                        //assigning Destination table name  
                        objbulk.DestinationTableName = "SciennaInvoice3";

                        //Mapping Table column  
                        objbulk.ColumnMappings.Add("F1", "ClientName");
                        objbulk.ColumnMappings.Add("F2", "ProjectName");
                        objbulk.ColumnMappings.Add("F3", "Loan#1");
                        objbulk.ColumnMappings.Add("F4", "SciennaId");
                        objbulk.ColumnMappings.Add("F5", "StartDate");
                        objbulk.ColumnMappings.Add("F6", "SignOffDate");
                        objbulk.ColumnMappings.Add("F7", "RawBillable");

                        //inserting bulk Records into DataBase   
                        objbulk.WriteToServer(Dt);
                        returnvalue = 1;
                    }
                    catch (Exception ex)
                    {
                        returnvalue = 0;
                    }
                }
                else
                {
                    returnvalue = -4;
                }

            }


            return returnvalue;
        }

        #endregion Infinity

        #region Canopy
        [WebMethod]
        public static int InsertStewartInvoice(string Month, string Year, string InvoiceDate, string InvoiceNumber, string InvoiceAmount, string BillTo)
        {
            int returnvalue = 0;
            int isdeleted = new bllInvoice().DeleteStewartLoan(Month, Year);
            if (isdeleted > 0)
            {
                Hashtable htParam = new Hashtable();
                if (NewFileName_StewartInvoice != "")
                {
                    string CodeDate = DateTime.Now.ToString("dd-MMM-yyyy-HHMMss");
                    if (!Directory.Exists(MainPath))
                    {
                        Directory.CreateDirectory(MainPath);
                    }
                    string SubPath = MainPath + "\\" + Convert.ToString("Stewart_IA");
                    if (!Directory.Exists(SubPath))
                    {
                        Directory.CreateDirectory(SubPath);
                    }
                    string UniquePath = MainPath + "\\" + Convert.ToString("Stewart_IA") + "\\" + Convert.ToString(CodeDate);
                    if (!Directory.Exists(UniquePath))
                    {
                        Directory.CreateDirectory(UniquePath);
                    }
                    File.Copy(NewFileName_StewartInvoice, UniquePath + "\\" + GUIDFile);
                    File.Delete(NewFileName_StewartInvoice);
                    htParam.Add("FilePath", UniquePath + "\\" + GUIDFile);
                }
                htParam.Add("InvoiceType", "Stewart_IA");
                htParam.Add("Type", "Image");
                htParam.Add("BillingType", "");
                htParam.Add("Frequency", "");
                htParam.Add("StatementDate", Convert.ToDateTime(InvoiceDate).ToString("dd-MMM-yyyy"));
                htParam.Add("VendorAccountNumber", "");
                htParam.Add("VendorInvoiceNumber", InvoiceNumber);
                htParam.Add("TotalDue", InvoiceAmount);
                htParam.Add("VendorName", BillTo);
                htParam.Add("VendorAddress", "");
                htParam.Add("AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
                returnvalue = new bllInvoice().InsertInvoiceDetails_Canopy(htParam);


            }
            else if (isdeleted == 0)
            {
                returnvalue = -1;
            }
            else
            {
                returnvalue = -2;
            }

            return returnvalue;
        }

        [WebMethod]
        public static int InsertStewartExcel(int InvoiceID, string Month, string Year)
        {
            int returnvalue = 0;
            if (NewFileName_StewartExcel != "")
            {
                string fileName = NewFileName_StewartExcel.Substring(NewFileName_StewartExcel.LastIndexOf("\\") + 1);
                string Extn = fileName.Substring(fileName.LastIndexOf(".") + 1);
                if (Extn == "xls" | Extn == "xlsx")
                {

                    string ConExcel;
                    if (Extn.Contains("xlsx"))
                    {
                        ConExcel = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + NewFileName_StewartExcel + "; Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
                    }
                    else
                    {
                        ConExcel = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + NewFileName_StewartExcel + "; Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
                    }

                    DataSet dsExcel = new DataSet();
                    DataTable Dt = new DataTable("[Sheet1$]");
                    using (OleDbConnection myExcelConnection = new OleDbConnection(ConExcel))
                    {
                        string sqlExcel = "Select * from [Sheet1$]";
                        OleDbDataAdapter daExcel = new OleDbDataAdapter(sqlExcel, myExcelConnection);
                        daExcel.Fill(dsExcel);
                        daExcel.Dispose();
                        Dt = dsExcel.Tables[0];
                    }
                    try
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = Dt;

                        DataTable dtProductSold = Dt;
                        Dt.Columns.Add("Month", typeof(String));
                        Dt.Columns.Add("Year", typeof(String));
                        Dt.Columns.Add("AddedBy", typeof(int));
                        Dt.Columns.Add("InvoiceID", typeof(int));
                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {
                            Dt.Rows[i]["Month"] = Month;
                            Dt.Rows[i]["Year"] = Year;
                            Dt.Rows[i]["AddedBy"] = int.Parse(HttpContext.Current.User.Identity.Name.ToString());
                            Dt.Rows[i]["InvoiceID"] = InvoiceID;
                        }

                        DataRow row = Dt.Rows[0];
                        Dt.Rows.Remove(row);

                        SqlConnection con = new SqlConnection(Con_Canopy);
                        SqlBulkCopy objbulk = new SqlBulkCopy(Con_Canopy);
                        //assigning Destination table name  
                        objbulk.DestinationTableName = "StewartIA";
                        //Mapping Table column  

                        objbulk.ColumnMappings.Add("F1", "InvoiceDate");
                        objbulk.ColumnMappings.Add("F2", "OrderDate");
                        objbulk.ColumnMappings.Add("F3", "CompleteDate");
                        objbulk.ColumnMappings.Add("F4", "BusinessDays");
                        objbulk.ColumnMappings.Add("F5", "InvoiceNumber");
                        objbulk.ColumnMappings.Add("F6", "Fee");
                        objbulk.ColumnMappings.Add("F7", "OrderID");
                        objbulk.ColumnMappings.Add("F8", "LoanNumber");
                        objbulk.ColumnMappings.Add("F9", "CaseNumber");
                        objbulk.ColumnMappings.Add("F10", "Borrower");
                        objbulk.ColumnMappings.Add("F11", "Address1");
                        objbulk.ColumnMappings.Add("F12", "Address2");
                        objbulk.ColumnMappings.Add("F13", "City");
                        objbulk.ColumnMappings.Add("F14", "State");
                        objbulk.ColumnMappings.Add("F15", "Zip");
                        objbulk.ColumnMappings.Add("Month", "Month");
                        objbulk.ColumnMappings.Add("Year", "Year");
                        objbulk.ColumnMappings.Add("AddedBy", "AddedBy");
                        objbulk.ColumnMappings.Add("InvoiceID", "InvoiceID");

                        //inserting bulk Records into DataBase   
                        objbulk.WriteToServer(Dt);
                        returnvalue = 1;
                    }
                    catch (Exception ex)
                    {
                        returnvalue = 0;
                    }
                }
                else
                {
                    returnvalue = -4;
                }

            }

            return returnvalue;
        }

        [WebMethod]
        public static string VerifyStewart(int InvoiceID, string Month, string Year)
        {
            DataTable dt1 = new bllInvoice().GetStewartDetailsForVerify(InvoiceID, Month, Year);
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

        #endregion Canopy
    }
}
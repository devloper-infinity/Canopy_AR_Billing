using ClosedXML.Excel;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code;
using Vendor_Portal.App_Code.BLL;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;

namespace Vendor_Portal.BDM
{
    public partial class SentToClient : System.Web.UI.Page
    {
        protected bool CanViewRlInvoices { get; private set; }
        static string InvoiceNumber;
        static string GroupName;
        static string DomainId;
        static string ProcessName;
        static string filename;
        static string FileName;
        static Workbook book = new Workbook();
        static Worksheet sheet;
        public static string ReportFileName = "";
        public static string ReportFilePath = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            int employeeId;
            CanViewRlInvoices = int.TryParse(User.Identity.Name, out employeeId) && RlInvoiceService.HasAccess(employeeId);
        }

        [WebMethod]
        public static string GetSentRlInvoices()
        {
            int employeeId;
            if (!int.TryParse(HttpContext.Current.User.Identity.Name, out employeeId) || !RlInvoiceService.HasAccess(employeeId))
                throw new HttpException(403, "You do not have access to RL/Sec invoices.");

            DataTable table = RlInvoiceService.GetInvoices(true);
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            foreach (DataRow dr in table.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    object value = dr[column] == DBNull.Value ? null : dr[column];
                    if (column.ColumnName == "SentDateTime" && value is DateTime)
                        value = ((DateTime)value).ToString("MM.dd.yyyy hh:mm tt");
                    row[column.ColumnName] = value;
                }
                rows.Add(row);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            return serializer.Serialize(rows);
        }

        [WebMethod]
        public static string GetAllSentToClientList()
        {
            DataTable dt1 = new bllTracking().GetSentToClientList();
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
        public static int GenerateInvoice(int ProjectID, string BillingPeriod, int InvoiceId, string Name1)
        {
            int returnvalue = 0;
            DataTable dt = new bllTracking().GetProjectClientConfiguration(ProjectID, InvoiceId);
            if (dt != null)
            {
                string InvoiceConfig = dt.Rows[0]["InvoiceConfiguration"].ToString();
                InvoiceNumber = dt.Rows[0]["InvoiceNumber"].ToString();
                GroupName = Convert.ToString(dt.Rows[0]["ProjectName"]);
                DomainId = dt.Rows[0]["DomainId"].ToString();
                ProcessName = dt.Rows[0]["ClientProcess"].ToString();
                if (ProcessName == "Other")
                {
                    ProcessName = ProcessName + "Sec~" + Name1;
                }
                //if (TrID != "")
                //{
                //    if (Convert.ToInt32(TrID) > 10)
                //    {
                //        Process = Process + " - Other" + " - " + TrID;
                //    }
                //}
                ReportDocument rpt = new ReportDocument();
                if (BillingPeriod != ProcessName)
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/PCQCSummary.rpt"));
                else if (Convert.ToString(dt.Rows[0]["Securitization"]) == "Yes")
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/Securitization.rpt"));
                else if (Convert.ToString(dt.Rows[0]["Securitization"]) == "Rebuttal")
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/Rebuttal.rpt"));
                else if (Convert.ToString(dt.Rows[0]["Securitization"]) == "Research")
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/Research.rpt"));
                else if (Convert.ToString(dt.Rows[0]["Securitization"]) == "Inventory")
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/Inventory.rpt"));
                else if (Convert.ToString(dt.Rows[0]["Securitization"]) == "642")
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/642.rpt"));
                else if (Convert.ToString(dt.Rows[0]["Securitization"]) == "670")
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/670.rpt"));
                else if (ProjectID == 70)
                    //rpt.Load("../Reports/NewSummaryReport1561.rpt");
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/NewSummaryReport1561.rpt"));
                else if (ProjectID == 512)
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/2091.rpt"));
                else
                    rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/NewSummaryReport1.rpt"));

                try
                {
                    CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
                    CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
                    CrystalDecisions.Shared.ParameterValues pval3 = new ParameterValues();
                    CrystalDecisions.Shared.ParameterValues pval4 = new ParameterValues();
                    CrystalDecisions.Shared.ParameterValues pval5 = new ParameterValues();
                    CrystalDecisions.Shared.ParameterValues pval6 = new ParameterValues();
                    CrystalDecisions.Shared.ParameterValues pval7 = new ParameterValues();

                    ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
                    pdisval2.Value = GroupName;
                    pval2.Add(pdisval2);

                    ParameterDiscreteValue pdisval3 = new ParameterDiscreteValue();
                    pdisval3.Value = BillingPeriod;
                    pval3.Add(pdisval3);

                    ParameterDiscreteValue pdisval4 = new ParameterDiscreteValue();
                    if (BillingPeriod != ProcessName)
                        pdisval4.Value = ProcessName;
                    else
                        pdisval4.Value = "";
                    pval4.Add(pdisval4);

                    ParameterDiscreteValue pdisval5 = new ParameterDiscreteValue();
                    pdisval5.Value = ProjectID;
                    pval5.Add(pdisval5);

                    ParameterDiscreteValue pdisval6 = new ParameterDiscreteValue();
                    pdisval6.Value = InvoiceId;
                    pval6.Add(pdisval6);

                    ParameterDiscreteValue pdisval7 = new ParameterDiscreteValue();
                    //if (Slot == "") Slot = "0";
                    pdisval7.Value = Convert.ToString(Convert.ToString("0"));
                    pval7.Add(pdisval7);


                    rpt.DataDefinition.ParameterFields["@BillingPeriod"].ApplyCurrentValues(pval3);
                    rpt.DataDefinition.ParameterFields["@ProcessName"].ApplyCurrentValues(pval4);
                    rpt.DataDefinition.ParameterFields["@ProjectId"].ApplyCurrentValues(pval5);
                    rpt.DataDefinition.ParameterFields["@InvoiceId"].ApplyCurrentValues(pval6);

                    int chkSlot = new bllTracking().CheckSlotPassing(ProjectID, BillingPeriod, ProcessName);
                    if (chkSlot == 1)
                        rpt.DataDefinition.ParameterFields["@Slot"].ApplyCurrentValues(pval7);

                    CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    CrystalDecisions.Shared.ConnectionInfo crConnectionInfo;
                    CrystalDecisions.Shared.TableLogOnInfos crtableLogoninfos;
                    CrystalDecisions.Shared.TableLogOnInfo crtableLogoninfo;
                    CrystalDecisions.CrystalReports.Engine.Tables CrTables;
                    crConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();
                    crtableLogoninfos = new CrystalDecisions.Shared.TableLogOnInfos();
                    crtableLogoninfo = new CrystalDecisions.Shared.TableLogOnInfo();

                    crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ServerName"];
                    crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
                    crConnectionInfo.UserID = ConfigurationManager.AppSettings["UserID"];
                    crConnectionInfo.Password = ConfigurationManager.AppSettings["Password"];

                    CrTables = rpt.Database.Tables;

                    foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                    {
                        crtableLogoninfo = CrTable.LogOnInfo;
                        crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                        CrTable.ApplyLogOnInfo(crtableLogoninfo);
                    }

                    CrystalReportViewer rptviewer1 = new CrystalReportViewer();
                    rptviewer1.AutoDataBind = true;
                    rptviewer1.SeparatePages = false;
                    rptviewer1.ToolPanelView = ToolPanelViewType.None;
                    rptviewer1.RefreshReport();
                    rptviewer1.Visible = true;
                    rptviewer1.HasExportButton = false;
                    rptviewer1.HasPrintButton = false;
                    rptviewer1.HasPageNavigationButtons = true;
                    rptviewer1.HasCrystalLogo = false;
                    rptviewer1.HasDrillUpButton = false;
                    rptviewer1.HasSearchButton = false;

                    rptviewer1.HasToggleGroupTreeButton = false;
                    rptviewer1.HasZoomFactorList = false;
                    rptviewer1.ToolbarStyle.Width = new Unit("750px");
                    rptviewer1.ReportSource = rpt;
                    string strDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                    string strTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    filename = ProjectID + "_" + BillingPeriod + "_" + strDate + strTime;
                    //rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, filename);
                    if (InvoiceNumber == "")
                    {
                        InvoiceNumber = filename;
                    }
                    else
                    {
                        filename = InvoiceNumber;
                    }
                    filename = filename.Replace(",", "_");
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(@"~/BillingDocuments/")))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~/BillingDocuments/"));
                    }
                    string filePath =
                        HttpContext.Current.Server.MapPath("~/BillingDocuments/") + filename + ".pdf";
                    int result = 0;
                    if (ProcessName == BillingPeriod)
                        result = new bllTracking().InsertGroupAttachmentPath_PDf(ProcessName, BillingPeriod, Convert.ToString(@"~/BillingDocuments/" + filename + ".pdf"));
                    else
                        result = new bllTracking().InsertGroupAttachmentPath_PDf(GroupName + "-" + ProcessName, BillingPeriod, Convert.ToString(@"~/BillingDocuments/" + filename + ".pdf"));
                    rpt.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);
                    ReportFilePath = filePath;
                    ReportFileName = filename + ".pdf";

                    rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, filename);
                }
                catch (Exception ex) { throw ex; }
            }
            return returnvalue;
        }
        public static string SanitizeFileName(string fileName)
        {
            // Replace any character NOT letter, digit, _, - or .
            return Regex.Replace(fileName, @"[^A-Za-z0-9_\.-]", "_");
        }
        protected void btn1_Click(object sender, EventArgs e)
        {
            //FileName = Server.MapPath(@"~\ReportDocument\Credit_Consolidated_Report_" + Convert.ToString(Month) + "-" + Convert.ToString(Year) + DateTime.Now.ToString("hhmmss") + ".xlsx");
            // FormatExcel(FileName);

            string filePath = FileName;
            string outputPath = FileName;

            // Zero-based index: e.g., index 0 = first sheet
            int sheetIndexToDelete = 1;

            using (var workbook = new XLWorkbook(filePath))
            {
                // Check if index is within bounds
                if (sheetIndexToDelete >= 0 && sheetIndexToDelete < workbook.Worksheets.Count)
                {
                    var worksheet = workbook.Worksheet(1);
                    workbook.Worksheets.Delete(worksheet.Name);
                    worksheet = workbook.Worksheet(1);
                    workbook.Worksheets.Delete(worksheet.Name);
                    worksheet = workbook.Worksheet(1);
                    workbook.Worksheets.Delete(worksheet.Name);
                    worksheet = workbook.Worksheet(3);
                    workbook.Worksheets.Delete(worksheet.Name);
                    //worksheet = workbook.Worksheet(sheetIndexToDelete + 1);
                    //workbook.Worksheets.Delete(worksheet.Name);
                    //worksheet = workbook.Worksheet(sheetIndexToDelete + 2);
                    //workbook.Worksheets.Delete(worksheet.Name);
                    //worksheet = workbook.Worksheet(sheetIndexToDelete + 1);
                    //workbook.Worksheets.Delete(worksheet.Name);
                }
                else
                {

                }

                // Save the updated workbook
                workbook.SaveAs(outputPath);

            }


            //Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            //if (xlApp == null)
            //{
            //    return;
            //}
            //xlApp.DisplayAlerts = false;
            //Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(FileName);
            //System.Threading.Thread.Sleep(1000);
            //Excel.Sheets worksheets = xlWorkBook.Worksheets;
            //worksheets[1].Delete();
            //worksheets[1].Delete();
            //worksheets[1].Delete();
            //worksheets[2].Delete();
            //worksheets[1].Select();
            //xlWorkBook.Save();
            //xlWorkBook.Close();
            //xlApp.Quit();

            //releaseObject(worksheets);
            //releaseObject(xlWorkBook);
            //releaseObject(xlApp);

            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", "application/xlsx");
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FileName));
            Response.TransmitFile(FileName);
            Response.End();
        }

        [WebMethod]
        public static int GenerateExcel1(int ProjectID, string BillingPeriod, int InvoiceId, string Process)
        {
            int returnvalue = 0;
            FileName = HttpContext.Current.Server.MapPath(@"~\ReportDocument\Order_Excel_" + DateTime.Now.ToString("hhmmss") + ".xlsx");

            book.DefaultFontSize = 10;
            book.DefaultFontName = "Aptos Narrow";

            int rowcount = 0;
            int colcount = 0;

            #region Project Inflow
            sheet = book.Worksheets.Add("Order Details");
            //DataTable dt = new bllTracking().GetDataForInvoiceExcel(ProjectID, BillingPeriod, ProcessName, Slot);
            DataSet ds = new bllTracking().GetDataForInvoiceExcel_DS(ProjectID, BillingPeriod, Process, "0");
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null)
                {
                    if (dt.Columns.Contains("ProjectID1"))
                        dt.Columns.Remove("ProjectID1");
                    if (dt.Columns.Contains("SummaryReport"))
                        dt.Columns.Remove("SummaryReport");
                    if (dt.Columns.Contains("TredingReport"))
                        dt.Columns.Remove("TredingReport");
                    if (dt.Columns.Contains("TrackingSheetID"))
                        dt.Columns.Remove("TrackingSheetID");
                    if (dt.Columns.Contains("Criteria"))
                        dt.Columns.Remove("Criteria");
                    int ColCount = dt.Columns.Count;
                    if (dt.Columns.Contains("TotalCharges"))
                        dt.Columns["TotalCharges"].SetOrdinal(ColCount - 1);
                    if (dt.Columns.Contains("Price"))
                        dt.Columns["Price"].Caption = "Rate in USD";
                    if (dt.Columns.Contains("TotalCharges"))
                        dt.Columns["TotalCharges"].Caption = "Total Charges in US $";

                    sheet.InsertDataTable(dt, true, 1, 1);
                    string Col = GetColumnName_Static(dt.Columns.Count - 1);
                    CellRange range = sheet.Range["A1:" + Col + "1"];
                    HeaderFormat_Static(range);
                    range = sheet.Range["A1:" + Col + (dt.Rows.Count + 1)];
                    AllBorder_Static(range);
                    ContentCenter_Static(range);
                    rowcount = sheet.LastRow;
                    colcount = sheet.LastColumn;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        string ColNameMerge = "";
                        decimal TotalCost = 0;
                        TotalCost = Convert.ToDecimal(ds.Tables[1].Rows[0][0]);
                        if (ds.Tables[0].Columns.Contains("SummaryReport"))
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[0]["SummaryReport"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["SummaryReport"]) != "0" && Convert.ToString(ds.Tables[0].Rows[0]["SummaryReport"]) != "0.00")
                            {
                                ColNameMerge = GetColumnName_Static(ColCount - 2);
                                sheet.Range["A" + (rowcount + 1) + ":" + ColNameMerge + (rowcount + 1)].Merge();
                                sheet.Range["A" + (rowcount + 1) + ":" + ColNameMerge + (rowcount + 1)].Value = "Summary Report Charges";
                                ColNameMerge = GetColumnName_Static(ColCount - 1);
                                sheet.Range[ColNameMerge + (rowcount + 1)].Value = "" + Convert.ToString(ds.Tables[0].Rows[0]["SummaryReport"]);
                            }
                        }
                        if (ds.Tables[0].Columns.Contains("TredingReport"))
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[0]["TredingReport"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["TredingReport"]) != "0" && Convert.ToString(ds.Tables[0].Rows[0]["TredingReport"]) != "0.00")
                            {
                                ColNameMerge = GetColumnName_Static(ColCount - 2);
                                sheet.Range["A" + (rowcount + 1) + ":" + ColNameMerge + (rowcount + 1)].Merge();
                                sheet.Range["A" + (rowcount + 1) + ":" + ColNameMerge + (rowcount + 1)].Value = "Treding Report Charges";
                                ColNameMerge = GetColumnName_Static(ColCount - 1);
                                sheet.Range[ColNameMerge + (rowcount + 1)].Value = "" + Convert.ToString(ds.Tables[0].Rows[0]["TredingReport"]);
                            }
                        }
                        rowcount = sheet.LastRow;
                        colcount = sheet.LastColumn;
                        ColNameMerge = GetColumnName_Static(ColCount - 2);
                        sheet.Range["A" + (rowcount + 1) + ":" + ColNameMerge + (rowcount + 1)].Merge();
                        sheet.Range["A" + (rowcount + 1) + ":" + ColNameMerge + (rowcount + 1)].Value = "NET USD";
                        ColNameMerge = GetColumnName_Static(ColCount - 1);
                        sheet.Range[ColNameMerge + (rowcount + 1)].Value = "" + Convert.ToString(TotalCost);

                        rowcount = sheet.LastRow;
                        colcount = sheet.LastColumn;
                        ColNameMerge = GetColumnName_Static(ColCount - 2);
                        sheet.Range["A" + (rowcount + 1) + ":" + ColNameMerge + (rowcount + 1)].Merge();
                        sheet.Range["A" + (rowcount + 1) + ":" + ColNameMerge + (rowcount + 1)].Value = "USD " + Number_ToText.Convert(TotalCost).ToString();
                        ColNameMerge = GetColumnName_Static(ColCount - 1);
                        AllBorder_Static(sheet.Range["A1:" + ColNameMerge + (rowcount + 1)]);
                    }

                    sheet.AllocatedRange.Style.Font.FontName = "Aptos Narrow";
                    sheet.AllocatedRange.Style.Font.Size = 10;

                    sheet.AllocatedRange.AutoFitColumns();
                    sheet.AllocatedRange.AutoFitRows();
                }
            }

            if (File.Exists(FileName))
            {
                try
                {
                    File.Delete(FileName);
                }
                catch { }
            }

            book.SaveToFile(FileName, ExcelVersion.Version2010);
            #endregion
            return returnvalue;
        }

        [WebMethod]
        public static int GenerateExcel(int ProjectID, string BillingPeriod, int InvoiceId, string Process)
        {
            int returnvalue = 0;
            DataTable dtInvoice = new bllTracking().GetProjectClientConfiguration(ProjectID, InvoiceId);
            if (dtInvoice != null)
            {
                if (dtInvoice.Rows.Count > 0)
                {
                    string InvoiceConfig = dtInvoice.Rows[0]["InvoiceConfiguration"].ToString();
                    string newFile = dtInvoice.Rows[0]["InvoiceNumber"].ToString();
                    newFile = SanitizeFileName(newFile);
                    FileName = HttpContext.Current.Server.MapPath(@"~\ReportDocument\" + newFile + ".xlsx");
                }
            }
            if (FileName == "")
            {
                FileName = HttpContext.Current.Server.MapPath(@"~\ReportDocument\Order_Excel_" + DateTime.Now.ToString("hhmmss") + ".xlsx");
            }
            book = new Workbook();
            book.DefaultFontSize = 9;
            book.DefaultFontName = "biome";

            int rowcount = 0;
            int colcount = 0;

            #region Invoice
            sheet = book.Worksheets.Add("Invoice");
            //DataTable dt = new bllTracking().GetDataForInvoiceExcel(ProjectID, BillingPeriod, ProcessName, Slot);
            //DataSet ds = new bllTracking().GetDataForInvoiceExcel_DS(ProjectID, BillingPeriod, Process, "0");
            DataSet ds = new bllTracking().GetDataForNewExcelFormat(ProjectID, BillingPeriod, Process);
            if (ds != null)
            {
                DataTable dt = ds.Tables[2];
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        sheet.SetColumnWidth(1, 52);
                        sheet.SetColumnWidth(2, 60);

                        //Header with Client Details
                        sheet.Range["A1:B1"].Merge();
                        sheet.Range["A1:B1"].RowHeight = 90;
                        try
                        {
                            ExcelPicture picture = sheet.Pictures.Add(1, 1, HttpContext.Current.Server.MapPath(@"../images/logo-canopy.png"));
                            picture.Height = 120;
                        }
                        catch (Exception ex)
                        {
                        }

                        sheet.Range["A2"].Value = "Client Name";
                        sheet.Range["A3"].Value = "Client Address";
                        sheet.Range["A4"].Value = "Contact Person";
                        sheet.Range["A5"].Value = "Client Contact 1";
                        sheet.Range["A6"].Value = "Client Contact 2";
                        sheet.Range["A7"].Value = "Client Contact 3";
                        sheet.Range["A8"].Value = "Deal Name";

                        sheet.Range["B2"].Value = Convert.ToString(dt.Rows[0]["PAI_Company_Name"]);
                        sheet.Range["B3"].Value = Convert.ToString(dt.Rows[0]["PAI_Address"]);
                        sheet.Range["B4"].Value = Convert.ToString(dt.Rows[0]["PAI_Contact_Person"]);
                        sheet.Range["B5"].Value = Convert.ToString(dt.Rows[0]["PAI_Email_Id"]);
                        sheet.Range["B6"].Value = "";
                        sheet.Range["B7"].Value = "";
                        sheet.Range["B8"].Value = Convert.ToString(dt.Rows[0]["DealName"]);

                        // Parameter wise Costing
                        sheet.Range["A10:B10"].Style.Color = Color.FromArgb(189, 215, 238);
                        sheet.Range["A10:B10"].Style.Font.IsBold = true;
                        //sheet.Range["A9:B9"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        sheet.Range["A10"].Value = "Total Loan Count";
                        try
                        {
                            sheet.Range["B10"].Value = Convert.ToString(ds.Tables[1].Rows[0]["RowCounts"]);
                        }
                        catch
                        {
                            sheet.Range["B10"].Value = Convert.ToString("#N/A");
                        }
                        //sheet.Range["A9:B9"].Merge();

                        sheet.Range["A11:B11"].Value = "Loan File Review";
                        sheet.Range["A11:B11"].Style.Color = Color.FromArgb(237, 125, 49);
                        sheet.Range["A11:B11"].Style.Font.Color = Color.White;
                        sheet.Range["A11:B11"].Style.Font.IsBold = true;
                        sheet.Range["A11:B11"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        sheet.Range["A11:B11"].Merge();

                        DataTable dtCostDetails = ds.Tables[0];
                        if (dtCostDetails != null)
                        {
                            if (dtCostDetails.Rows.Count > 0)
                            {
                                rowcount = sheet.LastRow;
                                rowcount++;
                                int row = rowcount;
                                for (int i = 0; i < dtCostDetails.Rows.Count; i++)
                                {
                                    sheet.Range["A" + (i + row)].Value = "Loan Count (" + Convert.ToString(dtCostDetails.Rows[i]["Description"]) + ")";
                                    sheet.Range["B" + (i + row)].Value = Convert.ToString(dtCostDetails.Rows[i]["LoanCount"]);
                                    row++;
                                    sheet.Range["A" + (i + row)].Value = "Price";
                                    sheet.Range["B" + (i + row)].Value = "$" + Convert.ToString(dtCostDetails.Rows[i]["Rate"]);
                                    row++;
                                    sheet.Range["A" + (i + row)].Value = "Total";
                                    sheet.Range["A" + (i + row)].Style.Color = Color.FromArgb(189, 215, 238);
                                    sheet.Range["B" + (i + row)].Value = Convert.ToString(dtCostDetails.Rows[i]["TotalCharges"]);
                                    sheet.Range["B" + (i + row)].NumberFormat = "$#,##0";
                                    sheet.Range["B" + (i + row)].Style.Color = Color.FromArgb(189, 215, 238);
                                    row++;
                                }
                            }
                        }
                        DataTable dtTotal = ds.Tables[1];
                        if (dtTotal != null)
                        {
                            if (dtTotal.Rows.Count > 0)
                            {
                                rowcount = sheet.LastRow;
                                rowcount++;
                                sheet.Range["A" + (rowcount + 1)].Value = "Total Invoice Amount";
                                sheet.Range["A" + (rowcount + 1)].Style.Color = Color.FromArgb(198, 89, 17);
                                sheet.Range["A" + (rowcount + 1)].Style.Font.Color = Color.White;
                                sheet.Range["A" + (rowcount + 1)].Style.Font.IsBold = true;
                                sheet.Range["B" + (rowcount + 1)].Value = "$" + Convert.ToString(dtTotal.Rows[0]["TotalCost"]);
                                sheet.Range["B" + (rowcount + 1)].NumberFormat = "$#,##0";
                                sheet.Range["B" + (rowcount + 1)].Style.Color = Color.FromArgb(198, 89, 17);
                                sheet.Range["B" + (rowcount + 1)].Style.Font.Color = Color.White;
                                sheet.Range["B" + (rowcount + 1)].Style.Font.IsBold = true;
                            }
                        }


                    }

                    sheet.AllocatedRange.Style.Font.FontName = "biome";
                    sheet.AllocatedRange.Style.Font.Size = 9;

                    //set the border of Range["C4"]
                    sheet.AllocatedRange.Borders.LineStyle = LineStyleType.Thin;
                    sheet.AllocatedRange.Borders[BordersLineType.DiagonalUp].LineStyle = LineStyleType.None;
                    sheet.AllocatedRange.Borders[BordersLineType.DiagonalDown].LineStyle = LineStyleType.None;
                    sheet.AllocatedRange.Borders.Color = Color.Black;

                    //sheet.AllocatedRange.AutoFitColumns();
                    //sheet.AllocatedRange.AutoFitRows();
                }
            }

            #endregion
            #region Pricing Details
            sheet = book.Worksheets.Add("Pricing Details");


            DataSet dsDetails = null;
            DataTable dtDetails = new bllTracking().GetExcelDataForExportSecondSheet(ProjectID, BillingPeriod, Process);
            if (dtDetails != null)
            {


                sheet.InsertDataTable(dtDetails, true, 1, 1);
                string Col = GetColumnName_Static(dtDetails.Columns.Count - 1);
                CellRange range = sheet.Range["A1:" + Col + "1"];
                HeaderFormat_Static(range);
                range = sheet.Range["A1:" + Col + (dtDetails.Rows.Count + 1)];
                AllBorder_Static(range);
                ContentCenter_Static(range);
                rowcount = sheet.LastRow;
                colcount = sheet.LastColumn;
                string ColName = GetColumnName_Static(colcount - 1);
                sheet.Range["D1:" + ColName + "" + rowcount].IgnoreErrorOptions = IgnoreErrorType.NumberAsText;
                CellRange range1 = sheet.Range["D1:" + ColName + rowcount];
                foreach (CellRange cell in range1)
                {
                    double number;
                    if (double.TryParse(cell.Value, out number))
                    {
                        cell.NumberValue = number * 1;
                    }
                }
                range1.Style.NumberFormat = "$#,##0.00";
                //sheet.Range["D1:" + ColName + "" + rowcount].Style.NumberFormat = "[$$-409]#,##0.00";
                sheet.Range["A" + (rowcount + 1)].Value = "Total";
                for (int i = 4; i <= dtDetails.Columns.Count; i++)
                {
                    int columnIndex = i; // Column A is index 1, B is 2, and so on.

                    //double columnSum = 0;

                    //// Iterate through the rows of the specified column
                    //// Assuming data starts from the second row (index 2) to skip headers if any
                    //for (int row = 2; row <= sheet.LastRow; row++)
                    //{
                    //    // Get the cell in the current row and specified column
                    //    CellRange cell = sheet.Range[row, columnIndex];

                    //    // Try to parse the cell value to a double and add to the sum
                    //    if (double.TryParse(cell.Text, out double cellValue))
                    //    {
                    //        columnSum += cellValue;
                    //    }
                    //}
                    ColName = GetColumnName_Static(i - 1);
                    sheet.Range[rowcount + 1, columnIndex].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range[rowcount + 1, columnIndex].Formula = "=SUM(" + ColName + "2:" + ColName + "" + (rowcount) + ")";
                    sheet.Range[rowcount + 1, columnIndex].Style.NumberFormat = "$#,##0.00";
                }
                ColName = GetColumnName_Static(sheet.LastColumn - 1);
                sheet.Range["A" + (rowcount + 1) + ":" + ColName + "" + (rowcount + 1)].Style.Font.IsBold = true;
                sheet.Range["A" + (rowcount + 1) + ":" + ColName + "" + (rowcount + 1)].Style.Color = Color.FromArgb(31, 73, 125);
                sheet.Range["A" + (rowcount + 1) + ":" + ColName + "" + (rowcount + 1)].Style.Font.Color = Color.White;

                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();
            }
            #endregion


            if (File.Exists(FileName))
            {
                try
                {
                    File.Delete(FileName);
                }
                catch { }
            }

            book.SaveToFile(FileName, ExcelVersion.Version2010);
            return returnvalue;
        }

        static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
            }
        }

        static string GetColumnName_Static(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }

        public static void HeaderFormat_Static(CellRange range)
        {
            range.Style.Borders.LineStyle = LineStyleType.Thin;
            range.Style.Borders[BordersLineType.DiagonalUp].LineStyle = LineStyleType.None;
            range.Style.Borders[BordersLineType.DiagonalDown].LineStyle = LineStyleType.None;
            range.Style.Color = Color.FromArgb(113, 147, 209);
            range.Style.Font.Color = Color.White;
            range.Style.HorizontalAlignment = HorizontalAlignType.Center;
            range.Style.Font.IsBold = true;
        }

        public static void AllBorder_Static(CellRange range)
        {
            range.Style.Borders.LineStyle = LineStyleType.Thin;
            range.Style.Borders[BordersLineType.DiagonalUp].LineStyle = LineStyleType.None;
            range.Style.Borders[BordersLineType.DiagonalDown].LineStyle = LineStyleType.None;
        }
        public static void ContentCenter_Static(CellRange range)
        {
            range.Style.HorizontalAlignment = HorizontalAlignType.Center;
        }

        public static void DashboardHeader_Static(CellRange range)
        {
            range.Style.HorizontalAlignment = HorizontalAlignType.Center;
            range.Style.Font.Size = 12;
            range.Style.Font.IsBold = true;
            range.Style.Borders.LineStyle = LineStyleType.Thin;
            range.Style.Borders[BordersLineType.DiagonalUp].LineStyle = LineStyleType.None;
            range.Style.Borders[BordersLineType.DiagonalDown].LineStyle = LineStyleType.None;
        }

        public static void DashboardContent_Static(CellRange range)
        {
            range.Style.HorizontalAlignment = HorizontalAlignType.Center;
            range.Style.Font.Size = 10;
            range.Style.Borders.LineStyle = LineStyleType.Thin;
            range.Style.Borders[BordersLineType.DiagonalUp].LineStyle = LineStyleType.None;
            range.Style.Borders[BordersLineType.DiagonalDown].LineStyle = LineStyleType.None;
        }

        protected void btndownloadsenttoclient_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", "application/xlsx");
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(ReportFilePath));
            Response.TransmitFile(ReportFilePath);
            Response.End();
        }

        [WebMethod]
        public static int SendInvoiceEmail(int ProjectID, string BillingPeriod, int InvoiceID, string Process)
        {
            int returnvalue = 0;
            int filegenerated = GenerateInvoice(ProjectID, BillingPeriod, InvoiceID, Process);
            if (ReportFilePath != "")
            {
                string Header = "<html><head><meta content='text/html; charset=utf-8' http-equiv='Content-Type'><title></title><style type='text/css'>a:hover { text-decoration: none !important; }.header h1 {color: #fff !important; font: normal 33px Georgia, serif; margin: 0; padding: 0; line-height: 33px;}.header p {color: #dfa575; font: normal 11px Georgia, serif; margin: 0; padding: 0; line-height: 11px; letter-spacing: 2px}.content h2 {color:#8598a3 !important; font-weight: normal; margin: 0; padding: 0; font-style: italic; line-height: 30px; font-size: 30px;font-family: Georgia, serif; }.content p {color:#767676; font-weight: normal; margin: 0; padding: 0; line-height: 20px; font-size: 12px;font-family: Georgia, serif;}.content a {color: #d18648; text-decoration: none;}.footer p {padding: 0; font-size: 11px; color:#fff; margin: 0; font-family: Georgia, serif;}.footer a {color: #f7a766; text-decoration: none;}</style></head><body><table cellpadding='0' cellspacing='0' border='1'><tr><td ><table cellpadding='0' cellspacing='0' border='0' align='center' width='100%' style='font-family: Georgia, serif;' class='header'><tr><td bgcolor='#c65204' height='70' align='center'><h1 style='color: #fff; font: normal 25px Verdana; margin: 0; padding: 0; line-height: 33px;'>Canopy Invoice</h1></td></tr><tr><td style='font-size: 1px; height: 5px; line-height: 1px;' height='5'>&nbsp;</td></tr></table>";

                string Footer = "<table cellpadding='0' cellspacing='0' border='0' align='center' width='100%' style='font-family: Georgia, serif; line-height: 10px; margin-top:30px;' bgcolor='#c65204' class='footer'><tr><td bgcolor='#c65204'  align='center' style='padding: 15px 0 10px; font-size: 11px; color:#fff; margin: 0; line-height: 1.2;font-family: Verdana;' valign='top'><p style='padding: 0; font-size: 11px; color:#fff; margin: 0; font-family: Georgia, serif;'>!!! This is software generated e-mail...Please do not reply. !!</p></td></tr> </table></td></tr></table></body></html>";
                StringBuilder htmlBody = new StringBuilder();
                StringBuilder htmlBody_Service = new StringBuilder();
                string ToAddress = "";
                string ToCC = "";
                string sub = "";
                string ToBCC = "";
                string PName = new bllTracking().GetProjectNamebyID(ProjectID);
                DataTable DtInvoice = new bllTracking().GetSummaryReportAttachments(ProjectID, BillingPeriod, Process);
                DataTable DTClientDetails = new bllTracking().GetSummaryReportInvoice(PName, BillingPeriod);
                DataTable dtClientInfo = new bllTracking().GetClientDetails(ProjectID, BillingPeriod);
                string ClientEmail = Convert.ToString(dtClientInfo.Rows[0]["PAI_Email_Id"]);
                string ToCCs = Convert.ToString(dtClientInfo.Rows[0]["CEC_CC"]);

                string clientName = "";
                if (dtClientInfo != null)
                {
                    if (dtClientInfo.Rows.Count > 0)
                    {
                        clientName = Convert.ToString(dtClientInfo.Rows[0]["PAI_Contact_Person"]);
                    }
                }
                try
                {
                    clientName = clientName.Substring(0, clientName.IndexOf(" "));
                }
                catch { }

                htmlBody_Service.Append("<table width=\"650px\" style='margin-left:10px;'><tr><td align=\"left\"><b>Hello " + clientName + ",</b></td></tr><tr>");
                htmlBody_Service.Append("<td align=\"left\">Good Morning!! <br /><br />Please find attached invoice# " + Convert.ToString(DTClientDetails.Rows[0]["InvNo"]) + " for your review.");
                htmlBody_Service.Append("<br /><br />Kindly, For all invoices related queries direct email to following.");
                htmlBody_Service.Append("<br /><br /><a href='mailTo:anita@Infinity-data.com'>anita@Infinity-data.com</a>");
                htmlBody_Service.Append("<br /><a href='mailTo:jim@Infinity-data.com'>jim@Infinity-data.com</a>");
                htmlBody_Service.Append("</td></tr></table><br /><br /><br /><br /><table width=\"650px\" style='margin-left:10px;'><tr><td align=\"left\">Thanks,<br />Anita Londhe<br />VP Controller<br />Infinity IPS<br /><a href='mailTo:anita@Infinity-data.com'>anita@Infinity-data.com</a><br /><a href='www.infinity-data.com'>www.infinity-data.com</a></td></tr> </table>");

                htmlBody_Service.Append("<table width=\"650px\"><tr><td align=\"left\"></td></tr></table>");
                htmlBody_Service.Append("<br /><span style='color:red; font-size:14px;'>**WE DO NOT ACCEPT OR REQUEST CHANGES TO WIRING INSTRUCTION VIA EMAIL - Always call to verify**</span>");
                htmlBody_Service.Append("<br /><span>**********************************************************************************************************</span>");
                htmlBody_Service.Append("<br /><br /><span style='font-size:11px;'>Disclaimer: The information contained in this e-mail and any attachments may be confidential or privileged under applicable law, or otherwise may be protected from disclosure to anyone other than the intended recipient(s). Any use, distribution, or copying of this e-mail, including any of its contents or attachments by any person other than the intended recipient, or for any purpose other than its intended use, is strictly prohibited. If you believe you have received this e-mail in error, please notify us by e-mail and permanently delete the e-mail and any attachments, and do not save, copy, disclose, or reply on any part of the information contained in this e-mail or its attachments</span>");

                string Subject = "Canopy invoice " + Convert.ToString(DtInvoice.Rows[0]["InvoiceNumber"]);
                //ToAddress = ClientEmail + ".pointofmail.com";

                if (ToAddress == "")
                {
                    ToAddress = "k.adam@infinity-data.com";
                }
                
                //ToAddress = "n.nilkanth@infinityinternationals.us";
                ToBCC = "n.nilkanth@infinityinternationals.us";

                string password = new bllTracking().GetPassword("ackdata");
                StringBuilder body = new StringBuilder();
                body.Append(Header);
                body.Append(htmlBody_Service);
                body.Append(Footer);

                string Attachment = ReportFilePath;// System.Web.HttpContext.Current.Server.MapPath(DtInvoice.Rows[0]["PGA_AttachmentsPDF"].ToString());
                MailMessage mail = new MailMessage();

                mail.To.Add(ToAddress);
                mail.CC.Add("jim@infinity-data.com");
                mail.CC.Add("anita@infinity-data.com");
                if (ToBCC != "")
                    mail.Bcc.Add(ToBCC);

                mail.From = new MailAddress("ack@infinity-data.com", "Canopy Billing", System.Text.Encoding.UTF8);
                mail.Subject = Subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = body.ToString();
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = System.Net.Mail.MailPriority.High;
                if (Attachment != "")
                {
                    mail.Attachments.Add(new Attachment(Attachment));
                }

                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("ack@infinity-data.com", password);
                client.Port = 587;
                client.Host = "smtp.office365.com";
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                try
                {
                    client.Send(mail);
                    returnvalue = 1;
                }
                catch
                {
                    returnvalue = 0;
                    // return false;
                }
            }

            return returnvalue;
        }
    }
}

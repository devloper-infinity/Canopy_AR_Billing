using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;
using Excel = Microsoft.Office.Interop.Excel;

namespace Vendor_Portal.BDM
{
    public partial class ClientBillingDeviationReport : System.Web.UI.Page
    {
        static string FileName = "";
        static Workbook book = new Workbook();
        static Worksheet sheet;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string GetDDSummary(string FromDate, string ToDate)
        {
            System.Data.DataTable dt1 = new bllTracking().getDeviationReport(FromDate, ToDate);
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
        public static string GetDDDetails(string FromDate, string ToDate)
        {
            System.Data.DataTable dt1 = new bllTracking().getDeviationReport_Detailed(FromDate, ToDate);
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
        public static string GetNonDDSummary(string FromDate, string ToDate)
        {
            System.Data.DataTable dt1 = new bllTracking().getDeviationReportNonDD(FromDate, ToDate);
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
        public static string GetNonDDDetails(string FromDate, string ToDate)
        {
            System.Data.DataTable dt1 = new bllTracking().getDeviationReport_DetailedNonDD(FromDate, ToDate);
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
        public static string GetCanopySummary(string FromDate, string ToDate)
        {
            System.Data.DataTable dt1 = new bllTracking().getDeviationReport_Canopy(FromDate, ToDate);
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
        public static string GetCanopyDetails(string FromDate, string ToDate)
        {
            System.Data.DataTable dt1 = new bllTracking().getDeviationReport_DetailedCanopy(FromDate, ToDate);
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

        static string GetColumnName(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }

        [WebMethod]
        public static int GenerateDDSummary(string FromDate, string ToDate)
        {
            int returnvalue = 1;
            FileName = HttpContext.Current.Server.MapPath(@"~\ReportDocument\Client Billing Deviation Report_" + DateTime.Now.ToString("hhmmss") + ".xlsx");
            book = new Workbook();
            book.DefaultFontSize = 10;
            book.DefaultFontName = "Aptos Narrow";

            int rowcount = 0;
            int colcount = 0;
            sheet = null;
            sheet = book.Worksheets.Add("IPS - DD - Summary");
            DataTable dt = new bllTracking().getDeviationReport(FromDate, ToDate);
            if (dt != null)
            {

                sheet.InsertDataTable(dt, true, 1, 1);
                string Col = GetColumnName(dt.Columns.Count - 1);
                CellRange range = sheet.Range["A1:" + Col + "1"];
                HeaderFormat_Static(range);
                range = sheet.Range["A1:" + Col + (dt.Rows.Count + 1)];
                AllBorder_Static(range);
                ContentCenter_Static(range);
                rowcount = sheet.LastRow;
                colcount = sheet.LastColumn;

                sheet.AllocatedRange.Style.Font.FontName = "Aptos Narrow";
                sheet.AllocatedRange.Style.Font.Size = 10;

                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();
            }
            return returnvalue;
        }

        [WebMethod]
        public static int GenerateDDDetails(string FromDate, string ToDate)
        {
            int returnvalue = 1;
            
            int rowcount = 0;
            int colcount = 0;
            sheet = null;
            sheet = book.Worksheets.Add("IPS - DD - Parameter wise");
            DataTable dt = new bllTracking().getDeviationReport_Detailed(FromDate, ToDate);
            if (dt != null)
            {

                sheet.InsertDataTable(dt, true, 1, 1);
                string Col = GetColumnName(dt.Columns.Count - 1);
                CellRange range = sheet.Range["A1:" + Col + "1"];
                HeaderFormat_Static(range);
                range = sheet.Range["A1:" + Col + (dt.Rows.Count + 1)];
                AllBorder_Static(range);
                ContentCenter_Static(range);
                rowcount = sheet.LastRow;
                colcount = sheet.LastColumn;

                sheet.AllocatedRange.Style.Font.FontName = "Aptos Narrow";
                sheet.AllocatedRange.Style.Font.Size = 10;

                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();
            }
            return returnvalue;
        }

        [WebMethod]
        public static int GenerateNonDDSummary(string FromDate, string ToDate)
        {
            int returnvalue = 1;

            int rowcount = 0;
            int colcount = 0;
            sheet = null;
            sheet = book.Worksheets.Add("IPS - Non DD - Summary");
            DataTable dt = new bllTracking().getDeviationReportNonDD(FromDate, ToDate);
            if (dt != null)
            {

                sheet.InsertDataTable(dt, true, 1, 1);
                string Col = GetColumnName(dt.Columns.Count - 1);
                CellRange range = sheet.Range["A1:" + Col + "1"];
                HeaderFormat_Static(range);
                range = sheet.Range["A1:" + Col + (dt.Rows.Count + 1)];
                AllBorder_Static(range);
                ContentCenter_Static(range);
                rowcount = sheet.LastRow;
                colcount = sheet.LastColumn;

                sheet.AllocatedRange.Style.Font.FontName = "Aptos Narrow";
                sheet.AllocatedRange.Style.Font.Size = 10;

                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();
            }
            return returnvalue;
        }

        [WebMethod]
        public static int GenerateNonDDDetails(string FromDate, string ToDate)
        {
            int returnvalue = 1;

            int rowcount = 0;
            int colcount = 0;
            sheet = null;
            sheet = book.Worksheets.Add("IPS - Non DD - Parameter wise");
            DataTable dt = new bllTracking().getDeviationReport_DetailedNonDD(FromDate, ToDate);
            if (dt != null)
            {

                sheet.InsertDataTable(dt, true, 1, 1);
                string Col = GetColumnName(dt.Columns.Count - 1);
                CellRange range = sheet.Range["A1:" + Col + "1"];
                HeaderFormat_Static(range);
                range = sheet.Range["A1:" + Col + (dt.Rows.Count + 1)];
                AllBorder_Static(range);
                ContentCenter_Static(range);
                rowcount = sheet.LastRow;
                colcount = sheet.LastColumn;

                sheet.AllocatedRange.Style.Font.FontName = "Aptos Narrow";
                sheet.AllocatedRange.Style.Font.Size = 10;

                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();
            }
            return returnvalue;
        }

        [WebMethod]
        public static int GenerateCanopySummary(string FromDate, string ToDate)
        {
            int returnvalue = 1;

            int rowcount = 0;
            int colcount = 0;
            sheet = null;
            sheet = book.Worksheets.Add("Canopy - Summary");
            DataTable dt = new bllTracking().getDeviationReport_Canopy(FromDate, ToDate);
            if (dt != null)
            {

                sheet.InsertDataTable(dt, true, 1, 1);
                string Col = GetColumnName(dt.Columns.Count - 1);
                CellRange range = sheet.Range["A1:" + Col + "1"];
                HeaderFormat_Static(range);
                range = sheet.Range["A1:" + Col + (dt.Rows.Count + 1)];
                AllBorder_Static(range);
                ContentCenter_Static(range);
                rowcount = sheet.LastRow;
                colcount = sheet.LastColumn;

                sheet.AllocatedRange.Style.Font.FontName = "Aptos Narrow";
                sheet.AllocatedRange.Style.Font.Size = 10;

                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();
            }
            return returnvalue;
        }

        [WebMethod]
        public static int GenerateCanopyDetails(string FromDate, string ToDate)
        {
            int returnvalue = 1;

            int rowcount = 0;
            int colcount = 0;
            sheet = null;
            sheet = book.Worksheets.Add("Canopy - Parameter wise");
            DataTable dt = new bllTracking().getDeviationReport_DetailedCanopy(FromDate, ToDate);
            if (dt != null)
            {

                sheet.InsertDataTable(dt, true, 1, 1);
                string Col = GetColumnName(dt.Columns.Count - 1);
                CellRange range = sheet.Range["A1:" + Col + "1"];
                HeaderFormat_Static(range);
                range = sheet.Range["A1:" + Col + (dt.Rows.Count + 1)];
                AllBorder_Static(range);
                ContentCenter_Static(range);
                rowcount = sheet.LastRow;
                colcount = sheet.LastColumn;

                sheet.AllocatedRange.Style.Font.FontName = "Aptos Narrow";
                sheet.AllocatedRange.Style.Font.Size = 10;

                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();
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
            return returnvalue;
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                return;
            }
            xlApp.DisplayAlerts = false;
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(FileName);
            System.Threading.Thread.Sleep(1000);
            Excel.Sheets worksheets = xlWorkBook.Worksheets;
            worksheets[1].Delete();
            worksheets[1].Delete();
            worksheets[1].Delete();
            worksheets[7].Delete();
            worksheets[1].Select();
            xlWorkBook.Save();
            xlWorkBook.Close();
            xlApp.Quit();

            releaseObject(worksheets);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);


            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", "application/xlsx");
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FileName));
            Response.TransmitFile(FileName);
            Response.End();
        }
    }
}
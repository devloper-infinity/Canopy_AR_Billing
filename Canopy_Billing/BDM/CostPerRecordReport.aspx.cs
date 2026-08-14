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
    public partial class CostPerRecordReport : System.Web.UI.Page
    {
        static DataTable dtUser = new DataTable();
        static DataTable dtDomain = new DataTable();
        static DataTable dtProject = new DataTable();
        static string FileName = "";
        static Workbook book = new Workbook();
        static Worksheet sheet;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetCostPerRecord_Userwise(string Month, string Year)
        {
            DataSet ds = new bllTracking().GetCostPerRecordReport(Month, Year);
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            if (ds != null)
            {
                dtUser = ds.Tables[0];
                dtDomain = ds.Tables[1];
                dtProject = ds.Tables[2];

                if (dtUser != null)
                {
                    foreach (DataRow dr in dtUser.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dtUser.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                }
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            return ser.Serialize(rows);
        }

        [WebMethod]
        public static string GetCostPerRecord_Domain(string Month, string Year)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            if (dtDomain != null)
            {
                foreach (DataRow dr in dtDomain.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dtDomain.Columns)
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
        public static string GetCostPerRecord_Project(string Month, string Year)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            if (dtProject != null)
            {
                try
                {
                    dtProject.Columns.Remove("ProjectID");
                }
                catch { }
                foreach (DataRow dr in dtProject.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dtProject.Columns)
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
        public static int UserwiseReport(string Month, string Year)
        {
            int returnvalue = 1;
            FileName = HttpContext.Current.Server.MapPath(@"~\ReportDocument\Cost Per Record - " + Month + "_" + Year + ".xlsx");
            book = new Workbook();
            book.DefaultFontSize = 10;
            book.DefaultFontName = "Aptos Narrow";

            int rowcount = 0;
            int colcount = 0;
            sheet = null;
            sheet = book.Worksheets.Add("Userwise");
            DataSet ds = new bllTracking().GetCostPerRecordReport(Month, Year);
            if (ds != null)
            {
                dtUser = ds.Tables[0];
                dtDomain = ds.Tables[1];
                dtProject = ds.Tables[2];
                if (dtUser != null)
                {
                    sheet.InsertDataTable(dtUser, true, 1, 1);
                    string Col = GetColumnName(dtUser.Columns.Count - 1);
                    CellRange range = sheet.Range["A1:" + Col + "1"];
                    HeaderFormat_Static(range);
                    range = sheet.Range["A1:" + Col + (dtUser.Rows.Count + 1)];
                    AllBorder_Static(range);
                    ContentCenter_Static(range);
                    rowcount = sheet.LastRow;
                    colcount = sheet.LastColumn;

                    sheet.AllocatedRange.Style.Font.FontName = "Aptos Narrow";
                    sheet.AllocatedRange.Style.Font.Size = 10;

                    sheet.AllocatedRange.AutoFitColumns();
                    sheet.AllocatedRange.AutoFitRows();
                }
            }
            return returnvalue;
        }

        [WebMethod]
        public static int DomainwiseReport(string Month, string Year)
        {
            int returnvalue = 1;

            int rowcount = 0;
            int colcount = 0;
            sheet = null;
            sheet = book.Worksheets.Add("Domainwise");
            if (dtDomain != null)
            {

                sheet.InsertDataTable(dtDomain, true, 1, 1);
                string Col = GetColumnName(dtDomain.Columns.Count - 1);
                CellRange range = sheet.Range["A1:" + Col + "1"];
                HeaderFormat_Static(range);
                range = sheet.Range["A1:" + Col + (dtDomain.Rows.Count + 1)];
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
        public static int ProjectwiseReport(string Month, string Year)
        {
            int returnvalue = 1;

            int rowcount = 0;
            int colcount = 0;
            sheet = null;
            sheet = book.Worksheets.Add("Projectwise");
            if (dtProject != null)
            {
                try
                {
                    dtProject.Columns.Remove("ProjectID");
                }
                catch { }
                sheet.InsertDataTable(dtProject, true, 1, 1);
                string Col = GetColumnName(dtProject.Columns.Count - 1);
                CellRange range = sheet.Range["A1:" + Col + "1"];
                HeaderFormat_Static(range);
                range = sheet.Range["A1:" + Col + (dtProject.Rows.Count + 1)];
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
            worksheets[4].Delete();
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
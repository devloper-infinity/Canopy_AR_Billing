using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;
using ClosedXML.Excel;

namespace Canopy_Billing.BDM
{
    public partial class DashboardNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetClientwiseDashboardNew()
        {
            DataTable dt1 = new bllTracking().GetClientwiseDashboard_New();
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
        public static string GetClientwiseDashboardNew_Client_RL()
        {
            DataTable dt1 = new bllTracking().GetClientwiseDashboard_New_Client_RL();
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
        public static string GetClientwiseDashboardNew_Client_SS()
        {
            DataTable dt1 = new bllTracking().GetClientwiseDashboard_New_Client_SS();
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
        public static string GetClientwiseDashboardNew_Client()
        {
            DataTable dt1 = new bllTracking().GetClientwiseDashboard_New_Client();
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


        //[WebMethod]
        //public static void ExportDashboardExcel()
        protected void exp_allgrid_Click(object sender, EventArgs e)
        {
            XLWorkbook wb = new XLWorkbook();

            CreateSheet(wb, "Summary",
                new bllTracking().GetClientwiseDashboard_New());

            CreateSheet(wb, "Review",
                new bllTracking().GetClientwiseDashboard_New_Client());

            CreateSheet(wb, "RelianceLetter",
                new bllTracking().GetClientwiseDashboard_New_Client_RL());

            CreateSheet(wb, "Securitization",
                new bllTracking().GetClientwiseDashboard_New_Client_SS());

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType =
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            HttpContext.Current.Response.AddHeader(
                "content-disposition",
                "attachment;filename=Dashboard_All_Tabs.xlsx");

            using (MemoryStream ms = new MemoryStream())
            {
                wb.SaveAs(ms);
                ms.WriteTo(HttpContext.Current.Response.OutputStream);
            }

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        private static void CreateSheet(XLWorkbook wb, string sheetName, DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                wb.Worksheets.Add(sheetName);
                return;
            }

            var ws = wb.Worksheets.Add(sheetName);

            int row1 = 1;
            int row2 = 2;
            int col = 1;

            bool hasClient = dt.Columns.Contains("Client");
            bool hasProcess = dt.Columns.Contains("Process");

            // =========================
            // Fixed Columns
            // =========================
            if (hasClient)
            {
                ws.Cell(row1, col).Value = "Client";
                ws.Range(row1, col, row2, col).Merge();
                col++;
            }

            if (hasProcess)
            {
                ws.Cell(row1, col).Value = "Process";
                ws.Range(row1, col, row2, col).Merge();
                col++;
            }

            int ytdStartCol = col;

            // =========================
            // Group Month Columns
            // =========================
            var months = new Dictionary<string, Dictionary<string, string>>();

            foreach (DataColumn dc in dt.Columns)
            {
                string columnName = dc.ColumnName;

                if (columnName == "ResultID" ||
                    columnName == "Client" ||
                    columnName == "Process")
                    continue;

                string[] parts = columnName.Split(
                    new string[] { "-ERP-" },
                    StringSplitOptions.None);

                if (parts.Length < 2)
                    continue;

                string month = parts[0];
                string type = parts[1];

                if (!months.ContainsKey(month))
                    months[month] = new Dictionary<string, string>();

                months[month][type] = columnName;
            }

            var monthKeys = months.Keys.ToList();

            // =========================
            // YTD Band Header
            // =========================
            ws.Cell(row1, col).Value = "YTD";
            ws.Range(row1, col, row1, col + 2).Merge();

            ws.Cell(row2, col).Value = "Count";
            ws.Cell(row2, col + 1).Value = "US $";
            ws.Cell(row2, col + 2).Value = "Rate";

            ws.Range(row1, col, row2, col + 2)
                .Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

            col += 3;

            // =========================
            // Month Band Headers
            // =========================
            foreach (string month in monthKeys)
            {
                int bandStart = col;

                ws.Cell(row1, col).Value = month;
                ws.Range(row1, col, row1, col + 2).Merge();

                ws.Cell(row2, col).Value = "Count";
                ws.Cell(row2, col + 1).Value = "US $";
                ws.Cell(row2, col + 2).Value = "Rate";

                ws.Range(row1, bandStart, row2, bandStart + 2)
                    .Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                col += 3;
            }

            int lastCol = col - 1;

            // =========================
            // Header Style
            // =========================
            var headerRange = ws.Range(row1, 1, row2, lastCol);

            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;

            headerRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            // =========================
            // Data Rows
            // =========================
            int dataRow = 3;

            foreach (DataRow dr in dt.Rows)
            {
                col = 1;

                if (hasClient)
                {
                    ws.Cell(dataRow, col).Value = dr["Client"].ToString();
                    col++;
                }

                if (hasProcess)
                {
                    ws.Cell(dataRow, col).Value = dr["Process"].ToString();
                    col++;
                }

                decimal ytdCount = 0;
                decimal ytdAmount = 0;

                foreach (string month in monthKeys)
                {
                    decimal cnt = 0;
                    decimal amt = 0;

                    if (months[month].ContainsKey("LoanCount"))
                        cnt = SafeDecimal(dr[months[month]["LoanCount"]]);

                    if (months[month].ContainsKey("Amount"))
                        amt = SafeDecimal(dr[months[month]["Amount"]]);

                    ytdCount += cnt;
                    ytdAmount += amt;
                }

                ws.Cell(dataRow, col).Value = ytdCount;
                ws.Cell(dataRow, col + 1).Value = ytdAmount;
                ws.Cell(dataRow, col + 2).Value =
                    ytdCount == 0 ? 0 : ytdAmount / ytdCount;

                col += 3;

                foreach (string month in monthKeys)
                {
                    decimal cnt = 0;
                    decimal amt = 0;

                    if (months[month].ContainsKey("LoanCount"))
                        cnt = SafeDecimal(dr[months[month]["LoanCount"]]);

                    if (months[month].ContainsKey("Amount"))
                        amt = SafeDecimal(dr[months[month]["Amount"]]);

                    ws.Cell(dataRow, col).Value = cnt;
                    ws.Cell(dataRow, col + 1).Value = amt;
                    ws.Cell(dataRow, col + 2).Value =
                        cnt == 0 ? 0 : amt / cnt;

                    col += 3;
                }

                dataRow++;
            }

            int lastRow = dataRow - 1;

            // =========================
            // Number Formatting
            // =========================
            for (int c = 1; c <= lastCol; c++)
            {
                string header = ws.Cell(row2, c).GetString();

                if (header == "Count")
                    ws.Column(c).Style.NumberFormat.Format = "#,##0";

                if (header == "US $")
                    ws.Column(c).Style.NumberFormat.Format = "#,##0.00";

                if (header == "Rate")
                    ws.Column(c).Style.NumberFormat.Format = "#,##0.00";
            }

            // =========================
            // Border for Full Used Range
            // =========================
            var fullRange = ws.Range(1, 1, lastRow, lastCol);

            fullRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            fullRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            fullRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            fullRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            fullRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            fullRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            // =========================
            // Medium Border Around Bands
            // =========================
            int bandCol = ytdStartCol;

            ws.Range(1, bandCol, lastRow, bandCol + 2)
                .Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

            bandCol += 3;

            foreach (string month in monthKeys)
            {
                ws.Range(1, bandCol, lastRow, bandCol + 2)
                    .Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                bandCol += 3;
            }

            // =========================
            // Alignment
            // =========================
            ws.Range(1, 1, lastRow, lastCol)
                .Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Range(3, 1, lastRow, lastCol)
                .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            if (hasClient)
                ws.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            if (hasProcess)
                ws.Column(hasClient ? 2 : 1).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Left;

            // =========================
            // Freeze Header and First Columns
            // =========================
            ws.SheetView.FreezeRows(2);

            if (hasClient && hasProcess)
                ws.SheetView.FreezeColumns(2);
            else
                ws.SheetView.FreezeColumns(1);

            ws.Columns().AdjustToContents();
        }

        private static decimal SafeDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;

            decimal result;
            return decimal.TryParse(value.ToString(), out result) ? result : 0;
        }



    }
}
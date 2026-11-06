using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;

namespace Canopy_Billing.BDM
{
    public partial class GenerateBilling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string GetBillingDecriptionSummary()
        {
            DataTable dt1 = new bllTracking().GetBillingDecriptionSummary();
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
        public static string GetBillingDecriptionDetails()
        {
            DataTable dt1 = new bllTracking().GetBillingDecriptionDetails();
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
        public static string GetBillingProjectWise(int ProjectID)
        {
            DataTable dt1 = new bllTracking().GetBillingProjectWise(ProjectID);
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
        public static int UpdateBillingStatus(int loanId)
        {
            int returnvalue = 0;
            try
            {
                Hashtable htParam = new Hashtable();
                htParam.Add("LoanId", loanId);
                returnvalue = new bllTracking().UpdateBillingStatus(htParam);
            }
            catch (Exception ex)
            {
                returnvalue = 0;
            }
            return returnvalue;
        }

        public static string GetBillingProjectIsNull()
        {
            DataTable dt1 = new bllTracking().GetBillingProjectIsNull();
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
        public static void SendBillingEmailWithExcel()
        {
            try
            {
                DataTable dtSummary = new bllTracking().GetBillingDecriptionSummary();
                DataTable dtDetails = new bllTracking().GetBillingDecriptionDetails();
                DataTable dtNullProject = new bllTracking().GetBillingProjectIsNull();
                int nullProjectCount = 0;
                if (dtNullProject != null)
                {
                    nullProjectCount = dtNullProject.Rows.Count;
                }
               
                if (dtSummary != null && dtSummary.Rows.Count > 0)
                {
                    DataView dvSummary = dtSummary.DefaultView;
                    if (dtSummary.Columns.Contains("Client") && dtSummary.Columns.Contains("ProjectName"))
                    {
                        dvSummary.Sort = "Client ASC, ProjectName ASC";
                    }
                    dtSummary = dvSummary.ToTable();
                }

                decimal totalLoanCount = 0;

                if (dtSummary != null && dtSummary.Columns.Contains("LoanCount"))
                {

                    foreach (DataRow row in dtSummary.Rows)
                    {
                        if (row["LoanCount"] != DBNull.Value)
                            totalLoanCount += Convert.ToDecimal(row["LoanCount"]);
                    }
                    DataRow totalRow = dtSummary.NewRow();
                    if (dtSummary.Columns.Contains("Client"))
                        totalRow["Client"] = "Total";
                    totalRow["LoanCount"] = totalLoanCount;
                  // dtSummary.Rows.Add(totalRow);

                }

                var projectList = new List<Tuple<string, string>>();
                if (dtSummary != null)
                {
                    foreach (DataRow row in dtSummary.Rows)
                    {
                        string clientVal = row["Client"] != DBNull.Value ? row["Client"].ToString() : "";
                        if (clientVal != "Total" && dtSummary.Columns.Contains("ProjectId") && dtSummary.Columns.Contains("ProjectName"))
                        {
                            if (row["ProjectId"] != DBNull.Value && row["ProjectName"] != DBNull.Value)
                            {
                                string pId = row["ProjectId"].ToString();
                                string pName = row["Client"].ToString();
                                if (!projectList.Any(p => p.Item1 == pId))
                                {
                                    projectList.Add(Tuple.Create(pId, pName));
                                }
                            }
                        }
                    }
                }

                OfficeOpenXml.ExcelPackage.License.SetNonCommercialPersonal("Billing Team");

                using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage())
                {
                    // ---------------------------------------------
                    // 1. SUMMARY SHEET (Blue Theme)
                    // ---------------------------------------------
                    OfficeOpenXml.ExcelWorksheet wsSummary = package.Workbook.Worksheets.Add("Summary");
                    int row = 1;
                    int col = 1;

                    DataTable dtSummaryForExcel = dtSummary.Copy();
                    if (dtSummaryForExcel != null && dtSummaryForExcel.Columns.Contains("LoanCount"))
                    {
                        DataRow totalRow = dtSummaryForExcel.NewRow();
                        if (dtSummaryForExcel.Columns.Contains("Client"))
                        {
                            totalRow["Client"] = "Total";
                        }
                        totalRow["LoanCount"] = totalLoanCount;
                        dtSummaryForExcel.Rows.Add(totalRow);
                    }
                    if (dtSummaryForExcel.Columns.Contains("ProjectId"))
                    {
                        dtSummaryForExcel.Columns.Remove("ProjectId");
                        dtSummaryForExcel.Columns.Remove("Slot");
                        dtSummaryForExcel.Columns.Remove("Securitization");
                        dtSummaryForExcel.Columns.Remove("RelianceLetter");
                        dtSummaryForExcel.Columns.Remove("Status1");
                        dtSummaryForExcel.Columns.Remove("Attachment");
                        dtSummaryForExcel.Columns.Remove("TrackingSheetID");

                    }

                    wsSummary.Cells[row, col].Value = "Sr.No";
                    for (int i = 0; i < dtSummaryForExcel.Columns.Count; i++)
                    {
                        wsSummary.Cells[row, i + 2].Value = dtSummaryForExcel.Columns[i].ColumnName;
                    }

                    using (var rng = wsSummary.Cells[1, 1, 1, dtSummaryForExcel.Columns.Count + 1])
                    {
                        rng.Style.Font.Name = "Calibri";
                        rng.Style.Font.Bold = true;
                        rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(48, 107, 166)); // #244b73
                        rng.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        rng.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    }
                    wsSummary.Cells[1, 1, 1, dtSummaryForExcel.Columns.Count + 1].AutoFilter = true;
                    row = 2;
                    foreach (DataRow dr in dtSummaryForExcel.Rows)
                    {
                        wsSummary.Cells[row, 1].Value = row - 1;
                        for (int c = 0; c < dtSummaryForExcel.Columns.Count; c++)
                        {
                            wsSummary.Cells[row, c + 2].Value = dr[c];
                        }

                        using (var rng = wsSummary.Cells[row, 1, row, dtSummaryForExcel.Columns.Count + 1])
                        {
                            rng.Style.Font.Name = "Calibri";
                            rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            if (row % 2 == 0)
                            {
                                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            }
                            else
                            {
                                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(228, 237, 247)); // #e4edf7
                            }
                            rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }
                        row++;
                    }

                    for (int c = 2; c <= dtSummaryForExcel.Columns.Count + 1; c++)
                    {
                        string colName = wsSummary.Cells[1, c].Text.ToUpper();
                        if (colName.Contains("AMOUNT") || colName.Contains("RATE") || colName.Contains("TOTAL") || colName.Contains("COUNT"))
                        {
                            wsSummary.Column(c).Style.Numberformat.Format = "#,##0.00";
                        }
                        if (colName.Contains("DATE"))
                        {
                            wsSummary.Column(c).Style.Numberformat.Format = "MM/dd/yyyy";
                        }
                    }

                    wsSummary.Cells[wsSummary.Dimension.Address].AutoFitColumns();

                    // ---------------------------------------------
                    // 2. DETAILS SHEETS
                    // ---------------------------------------------
                    if (dtDetails != null && dtDetails.Rows.Count > 0 && dtDetails.Columns.Contains("ProjectId"))
                    {
                        foreach (var proj in projectList)
                        {
                            string pIdStr = proj.Item1;
                            string rawSheetName = proj.Item2;

                            string sheetName = string.IsNullOrEmpty(rawSheetName) ? ("Project_" + pIdStr) : rawSheetName;
                            if (sheetName.Length > 31) sheetName = sheetName.Substring(0, 31);

                            string baseSheetName = sheetName;
                            int counter = 1;
                            while (package.Workbook.Worksheets.Any(w => w.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase)))
                            {
                                string suffix = "_" + counter;
                                int maxBaseLen = 31 - suffix.Length;
                                sheetName = (baseSheetName.Length > maxBaseLen ? baseSheetName.Substring(0, maxBaseLen) : baseSheetName) + suffix;
                                counter++;
                            }

                            DataTable dtFiltered = dtDetails.Clone();
                            var rows = dtDetails.AsEnumerable()
                                                .Where(r => r.Field<object>("ProjectId") != null && r.Field<object>("ProjectId").ToString() == pIdStr);

                            foreach (var r in rows)
                            {
                                dtFiltered.ImportRow(r);
                            }
                            if (dtFiltered.Columns.Contains("ProjectId"))
                            {
                                dtFiltered.Columns.Remove("ProjectId");
                            }

                            OfficeOpenXml.ExcelWorksheet wsDetails = package.Workbook.Worksheets.Add(sheetName);

                            int detRow = 1;
                            wsDetails.Cells[detRow, 1].Value = "Sr.No";
                            for (int i = 0; i < dtFiltered.Columns.Count; i++)
                            {
                                wsDetails.Cells[detRow, i + 2].Value = dtFiltered.Columns[i].ColumnName;
                            }

                            using (var rng = wsDetails.Cells[1, 1, 1, dtFiltered.Columns.Count + 1])
                            {
                                rng.Style.Font.Name = "Calibri";
                                rng.Style.Font.Bold = true;
                                rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                                rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(48, 107, 166)); // #244b73
                                rng.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                rng.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            }
                            wsDetails.Cells[1, 1, 1, dtFiltered.Columns.Count + 1].AutoFilter = true;
                            detRow = 2;
                            int srNo = 1;
                            foreach (DataRow dr in dtFiltered.Rows)
                            {
                                wsDetails.Cells[detRow, 1].Value = srNo++;
                                for (int c = 0; c < dtFiltered.Columns.Count; c++)
                                {
                                    wsDetails.Cells[detRow, c + 2].Value = dr[c];
                                }

                                using (var rng = wsDetails.Cells[detRow, 1, detRow, dtFiltered.Columns.Count + 1])
                                {
                                    rng.Style.Font.Name = "Calibri";
                                    rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    if (detRow % 2 == 1)
                                    {
                                        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(228, 237, 247)); // #e4edf7
                                    }
                                    else
                                    {
                                        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                    }
                                    rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                }
                                detRow++;
                            }

                            for (int c = 2; c <= dtFiltered.Columns.Count + 1; c++)
                            {
                                string colName = wsDetails.Cells[1, c].Text.ToUpper();
                                if (colName.Contains("AMOUNT") || colName.Contains("RATE") || colName.Contains("TOTAL"))
                                {
                                    wsDetails.Column(c).Style.Numberformat.Format = "#,##0.00";
                                }
                                if (colName.Contains("DATE"))
                                {
                                    wsDetails.Column(c).Style.Numberformat.Format = "MM/dd/yyyy";
                                }
                            }

                            wsDetails.Cells[wsDetails.Dimension.Address].AutoFitColumns();
                        }
                    }


                    package.Encryption.Password = "@ar2026";
                    package.Encryption.IsEncrypted = true;

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        package.SaveAs(memoryStream);
                        memoryStream.Position = 0;

                       string ToAddress = "p.kedar@infinity-data.com";
                        //string ToAddress = "k.aarti@infinity-data.com";
                       

                     //   string CC = "jim @infinity-data.com, s.chandrakant @infinity-data.com,p.kedar@infinity-data.com";
                        string Subject = "Canopy Client Billing";

                        StringBuilder htmlBody = new StringBuilder();

                        htmlBody.Append("<html><head></head><body>");
                        htmlBody.Append("<br />Hello,<br />");
                        htmlBody.Append("<br /><font color=brown face=Verdana size=2><b>Please find the project-wise client billing summary below:</b></font><br /><br />");

                        if (dtSummary != null && dtSummary.Rows.Count > 0)
                        {
                            Dictionary<string, string> selectedColumns = new Dictionary<string, string>()
                    {
                        { "Client", "Client" },
                        { "ProjectName", "Project Name" },
                        { "BillingPeriod", "Billing Period" },
                        { "BillingSentDate", "Billing Sent Date" },
                        { "LoanCount", "Loan Count" }
                    };
                            htmlBody.Append("<style>");
                            htmlBody.Append("tr.data-row:hover { background-color: #cbdbee !important; }");
                            htmlBody.Append("tr.data-row td { padding: 8px 10px; }");
                            htmlBody.Append("</style>");
                            htmlBody.Append("<table border=\"1\" bordercolor='Black' style='border:solid 1px black;border-collapse:collapse;padding:5px; font-size:12px; font-family:Verdana;'>");
                            htmlBody.Append("<tr><td colspan='" + (selectedColumns.Count + 1) + "' style='background-color:#244b73; color:White; text-align:center; padding:8px; font-size:14px;'><b>Client Billing Summary</b></td></tr>");
                            htmlBody.Append("<tr><td colspan='" + (selectedColumns.Count + 1) + "' style='border:none; height:2px; background-color:White;'></td></tr>");

                            htmlBody.Append("<tr style='background-color:#244b73; color:White;'>");
                            htmlBody.Append("<td><center><b>Sr. #</b></center></td>");

                            foreach (var cols in selectedColumns)
                            {
                                if (dtSummary.Columns.Contains(cols.Key))
                                {
                                    htmlBody.Append("<td><center><b>").Append(cols.Value).Append("</b></center></td>");
                                }
                            }
                            htmlBody.Append("</tr>");

                            for (int i = 0; i < dtSummary.Rows.Count; i++)
                            {
                                string rowBgColor = (i % 2 == 0) ? "White" : "#e4edf7";

                                htmlBody.Append("<tr class='data-row' style='background-color:").Append(rowBgColor).Append("; color:black;'>");
                                htmlBody.Append("<td><center>").Append(i + 1).Append("</center></td>");

                                foreach (var cols in selectedColumns)
                                {
                                    if (dtSummary.Columns.Contains(cols.Key))
                                    {
                                        htmlBody.Append("<td><center>").Append(Convert.ToString(dtSummary.Rows[i][cols.Key])).Append("</center></td>");
                                    }
                                }
                                htmlBody.Append("</tr>");
                            }
                            htmlBody.Append("<tr style='background-color:#e4edf7; font-weight:bold; color:black;'>");
                            htmlBody.Append("<td colspan='").Append(selectedColumns.Count).Append("' style='text-align:right; padding:8px;'>Total:</td>");
                            htmlBody.Append("<td><center>").Append(totalLoanCount).Append("</center></td>");
                            htmlBody.Append("</tr>");

                            htmlBody.Append("<tr style='background-color:#e4edf7; font-weight:bold; color:black;'>");
                            htmlBody.Append("<td colspan='").Append(selectedColumns.Count).Append("' style='text-align:right; padding:8px;'>Bill To not found:</td>");
                            htmlBody.Append("<td><center>").Append(nullProjectCount).Append("</center></td>");
                            htmlBody.Append("</tr>");

                            htmlBody.Append("<tr style='background-color:#e4edf7; font-weight:bold; color:black;'>");
                            htmlBody.Append("<td colspan='").Append(selectedColumns.Count).Append("' style='text-align:right; padding:8px;'>Grand Total:</td>");
                            htmlBody.Append("<td><center>").Append(totalLoanCount + nullProjectCount).Append("</center></td>");
                            htmlBody.Append("</tr>");
                            htmlBody.Append("</table>");
                        }
                        else
                        {
                            htmlBody.Append("<p>No summary records found.</p>");
                        }

                        htmlBody.Append("<br /><br /><font color=brown face=Verdana size=2><b>Detailed, Project-Wise Loans and Null Project-Wise Loans are attached in the Excel sheet for your ready reference.</b></font><br /><br />");
                        htmlBody.Append("<table width=\"650px\" style='font-size:13px;'><tr><td align=\"left\">Thanks,<br />Billing Team </td></tr> <tr> <br /><td align=\"center\"><b>!!! This is a software-generated e-mail... Please do not reply. !!!</b></td></tr></table>");
                        htmlBody.Append("</body></html>");

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.To.Add(ToAddress);

                            mail.From = new MailAddress("ack@infinity-data.com", "Canopy Client Billing", System.Text.Encoding.UTF8);
                            mail.Subject = Subject;
                            mail.SubjectEncoding = System.Text.Encoding.UTF8;
                            mail.Body = htmlBody.ToString();
                            mail.BodyEncoding = System.Text.Encoding.UTF8;
                            mail.IsBodyHtml = true;
                            mail.Priority = System.Net.Mail.MailPriority.High;

                            Attachment attachment = new Attachment(memoryStream, "ClientBilling_Report.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                            mail.Attachments.Add(attachment);
                            if (dtNullProject != null && dtNullProject.Rows.Count > 0)
                            {
                                using (OfficeOpenXml.ExcelPackage nullPackage = new OfficeOpenXml.ExcelPackage())
                                {
                                    OfficeOpenXml.ExcelWorksheet wsNull = nullPackage.Workbook.Worksheets.Add("Null_Projects");
                                    if (dtNullProject.Columns.Contains("ProjectId"))
                                    {
                                        dtNullProject.Columns.Remove("ProjectId");
                                    }
                                    int nRow = 1;
                                    wsNull.Cells[nRow, 1].Value = "Sr.No";
                                    for (int i = 0; i < dtNullProject.Columns.Count; i++)
                                    {
                                        wsNull.Cells[nRow, i + 2].Value = dtNullProject.Columns[i].ColumnName;
                                    }

                                    using (var rng = wsNull.Cells[1, 1, 1, dtNullProject.Columns.Count + 1])
                                    {
                                        rng.Style.Font.Name = "Calibri";
                                        rng.Style.Font.Bold = true;
                                        rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(48, 107, 166)); // #306ba6
                                        rng.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        rng.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    }

                                    wsNull.Cells[1, 1, 1, dtNullProject.Columns.Count + 1].AutoFilter = true;

                                    nRow = 2;
                                    int sr = 1;
                                    foreach (DataRow dr in dtNullProject.Rows)
                                    {
                                        wsNull.Cells[nRow, 1].Value = sr++;
                                        for (int c = 0; c < dtNullProject.Columns.Count; c++)
                                        {
                                            wsNull.Cells[nRow, c + 2].Value = dr[c];
                                        }

                                        using (var rng = wsNull.Cells[nRow, 1, nRow, dtNullProject.Columns.Count + 1])
                                        {
                                            rng.Style.Font.Name = "Calibri";
                                            rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                                            if (nRow % 2 == 0)
                                            {
                                                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                            }
                                            else
                                            {
                                                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(228, 237, 247)); // #e4edf7
                                            }

                                            rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                            rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                            rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                            rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        }

                                        nRow++;
                                    }

                                    wsNull.Cells[wsNull.Dimension.Address].AutoFitColumns();

                                    nullPackage.Encryption.Password = "@ap2024";
                                    nullPackage.Encryption.IsEncrypted = true;

                                    MemoryStream nullMemoryStream = new MemoryStream();
                                    nullPackage.SaveAs(nullMemoryStream);
                                    nullMemoryStream.Position = 0;

                                    Attachment nullAttachment = new Attachment(nullMemoryStream, "ClientBilling_NullProjects_Report.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                                    mail.Attachments.Add(nullAttachment);
                                }
                            }
                            string pass = new bllTracking().GetPassword("ackdata");

                            SmtpClient client = new SmtpClient();
                            client.UseDefaultCredentials = false;
                            client.Credentials = new System.Net.NetworkCredential("ack@infinity-data.com", pass);
                            client.Host = "smtp.office365.com";
                            client.Port = 587;
                            client.EnableSsl = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            try
                            {
                                client.Send(mail);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in sending billing email: " + ex.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.BDM
{
    public class RLInvoiceExcel : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            int employeeId, invoiceId;
            if (!int.TryParse(context.User.Identity.Name, out employeeId) || !int.TryParse(context.Request.QueryString["id"], out invoiceId) || !RlInvoiceService.HasAccess(employeeId))
            { context.Response.StatusCode = 403; return; }

            DataRow invoice = RlInvoiceService.GetInvoice(invoiceId);
            string number = "RL-" + invoiceId.ToString("D6", CultureInfo.InvariantCulture);
            byte[] file = BuildExcel(invoice);
            context.Response.Clear();
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + number + ".xlsx");
            context.Response.OutputStream.Write(file, 0, file.Length);
            context.Response.Flush();
            context.ApplicationInstance.CompleteRequest();
        }

        private static byte[] BuildExcel(DataRow invoice)
        {
            using (XLWorkbook book = new XLWorkbook())
            {
                IXLWorksheet sheet = book.Worksheets.Add("Invoice");
                sheet.Style.Font.FontName = "Biome";
                sheet.Style.Font.FontSize = 9;
                sheet.Column(1).Width = 52;
                sheet.Column(2).Width = 60;
                sheet.Range("A1:B1").Merge();
                sheet.Row(1).Height = 90;
                string logoPath = HttpContext.Current.Server.MapPath("~/images/logo-canopy.png");
                if (File.Exists(logoPath)) { IXLPicture logo = sheet.AddPicture(logoPath).MoveTo(sheet.Cell("A1")); logo.Height = 85; }

                string address = string.IsNullOrWhiteSpace(Convert.ToString(invoice["Address"])) ? Convert.ToString(invoice["ClientAddress"]) : Convert.ToString(invoice["Address"]);
                string contacts = Convert.ToString(invoice["ContactPerson"]);
                if (contacts.StartsWith("EMAILS:", StringComparison.OrdinalIgnoreCase)) contacts = contacts.Substring(7);
                string[] emails = contacts.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                Pair(sheet, 2, "Client Name", Convert.ToString(invoice["BillingEntity"]));
                Pair(sheet, 3, "Client Address", address);
                Pair(sheet, 4, "Contact Person", Convert.ToString(invoice["Recipient"]));
                Pair(sheet, 5, "Client Contact 1", emails.Length > 0 ? emails[0].Trim() : "");
                Pair(sheet, 6, "Client Contact 2", emails.Length > 1 ? emails[1].Trim() : "");
                Pair(sheet, 7, "Client Contact 3", emails.Length > 2 ? emails[2].Trim() : "");
                Pair(sheet, 8, "Deal Name", Convert.ToString(invoice["TradeName"]));

                bool hourly = string.Equals(Convert.ToString(invoice["BillingMethod"]), "Hourly", StringComparison.OrdinalIgnoreCase);
                Pair(sheet, 10, hourly ? "Total Hours Worked" : "Total Loan Count", ToDecimal(invoice, hourly ? "HoursWorked" : "LoanCount"));
                sheet.Range("A10:B10").Style.Fill.SetBackgroundColor(XLColor.FromArgb(189, 215, 238));
                sheet.Range("A10:B10").Style.Font.SetBold();
                sheet.Range("A11:B11").Merge();
                sheet.Cell("A11").Value = Convert.ToString(invoice["Document"]);
                sheet.Range("A11:B11").Style.Fill.SetBackgroundColor(XLColor.FromArgb(237, 125, 49));
                sheet.Range("A11:B11").Style.Font.SetBold().Font.SetFontColor(XLColor.White);
                sheet.Range("A11:B11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                int row = 12;
                DataTable details = RlInvoiceService.GetInvoiceDetails(Convert.ToInt32(invoice["InvoiceID"]));
                if (details.Rows.Count > 0)
                {
                    if (string.Equals(Convert.ToString(invoice["Document"]), "Both", StringComparison.OrdinalIgnoreCase))
                    {
                        decimal rlTotal = details.AsEnumerable().Sum(item => Convert.ToDecimal(item["Amount"]));
                        Pair(sheet, row++, hourly ? "Hours Worked (Securitization Services - Hourly)" : "Loan Count (Securitization Services - Per File)", ToDecimal(invoice, hourly ? "HoursWorked" : "LoanCount"));
                        Money(sheet, row++, "Price", ToDecimal(invoice, "Cost"));
                        Money(sheet, row, "Total", ToDecimal(invoice, "ExpectedBilling") - rlTotal);
                        sheet.Range(row, 1, row, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(189, 215, 238));
                        row++;
                    }
                    foreach (DataRow detail in details.Rows)
                    {
                        Pair(sheet, row++, "Loan Count (" + Convert.ToString(detail["BillingDescription"]) + ")", Convert.ToDecimal(detail["Quantity"]));
                        Money(sheet, row++, "Price", Convert.ToDecimal(detail["Rate"]));
                        Money(sheet, row, "Total", Convert.ToDecimal(detail["Amount"]));
                        sheet.Range(row, 1, row, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(189, 215, 238));
                        row++;
                    }
                }
                else
                {
                    bool securitization = string.Equals(Convert.ToString(invoice["Document"]), "Securitization", StringComparison.OrdinalIgnoreCase);
                    string description = securitization
                        ? (hourly ? "Hours Worked (Securitization Services - Hourly)" : "Loan Count (Securitization Services - Per File)")
                        : "Loan Count (Reliance Letter Services)";
                    Pair(sheet, row++, description, ToDecimal(invoice, hourly ? "HoursWorked" : "LoanCount"));
                    Money(sheet, row++, "Price", ToDecimal(invoice, "Cost"));
                    decimal baseAmount = ToDecimal(invoice, "BaseAmount");
                    Money(sheet, row, "Total", baseAmount == 0 ? ToDecimal(invoice, "ExpectedBilling") : baseAmount);
                    sheet.Range(row, 1, row, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(189, 215, 238));
                    row++;
                }

                if (ToDecimal(invoice, "MinimumAmount") > 0) Money(sheet, row++, "Minimum Billing", ToDecimal(invoice, "MinimumAmount"));
                if (ToDecimal(invoice, "MaximumCap") > 0) Money(sheet, row++, "Maximum Cap", ToDecimal(invoice, "MaximumCap"));
                Money(sheet, row, "Total Invoice Amount", ToDecimal(invoice, "ExpectedBilling"));
                sheet.Range(row, 1, row, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(198, 89, 17));
                sheet.Range(row, 1, row, 2).Style.Font.SetBold().Font.SetFontColor(XLColor.White);
                Borders(sheet.Range(1, 1, row, 2));
                sheet.Range("B3:B8").Style.Alignment.WrapText = true;

                AddLoanDetails(book, invoice);
                using (MemoryStream stream = new MemoryStream()) { book.SaveAs(stream); return stream.ToArray(); }
            }
        }

        private static void AddLoanDetails(XLWorkbook book, DataRow invoice)
        {
            IXLWorksheet target = book.Worksheets.Add("Loan Details");
            target.Style.Font.FontName = "Biome"; target.Style.Font.FontSize = 9;
            string filePath = Convert.ToString(invoice["FilePath"]);
            if (string.IsNullOrWhiteSpace(filePath)) { target.Cell("A1").Value = "No loan list was uploaded for this invoice."; return; }
            string root = HttpContext.Current.Server.MapPath("~/Uploads/");
            string path = Path.GetFullPath(Path.Combine(root, Path.GetFileName(filePath)));
            if (!path.StartsWith(Path.GetFullPath(root), StringComparison.OrdinalIgnoreCase) || !File.Exists(path)) { target.Cell("A1").Value = "The uploaded loan list is not available."; return; }
            string extension = Path.GetExtension(path);
            if (!extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) && !extension.Equals(".xlsm", StringComparison.OrdinalIgnoreCase)) { target.Cell("A1").Value = "Unsupported uploaded loan-list format: " + Path.GetFileName(filePath); return; }
            using (XLWorkbook source = new XLWorkbook(path))
            {
                IXLRange used = source.Worksheet(1).RangeUsed();
                if (used == null) return;
                used.CopyTo(target.Cell(1, 1));
                int loanColumn = 0;
                for (int column = 1; column <= used.ColumnCount(); column++)
                {
                    string header = new string(target.Cell(1, column).GetString().Where(char.IsLetterOrDigit).ToArray());
                    if (header.Equals("LoanNo", StringComparison.OrdinalIgnoreCase) || header.Equals("LoanId", StringComparison.OrdinalIgnoreCase)) { loanColumn = column; break; }
                }
                int lastColumn = used.ColumnCount();
                if (loanColumn > 0)
                {
                    List<string> loanIds = new List<string>();
                    for (int row = 2; row <= used.RowCount(); row++) loanIds.Add(target.Cell(row, loanColumn).GetString().Trim());
                    target.Column(loanColumn).Delete();
                    lastColumn--;
                    DataTable metadata = RlInvoiceService.GetLoanMetadata(loanIds);
                    Dictionary<string, DataRow> lookup = metadata.Columns.Contains("loanid")
                        ? metadata.AsEnumerable().GroupBy(row => Convert.ToString(row["loanid"]), StringComparer.OrdinalIgnoreCase).ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase)
                        : new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);
                    string[] headers = { "transactionIdentifier", "loanid", "createdDate", "snapShotDate", "RL Rate", "RL Amount" };
                    for (int index = 0; index < headers.Length; index++) target.Cell(1, lastColumn + index + 1).Value = headers[index];
                    bool hasRl = Convert.ToString(invoice["Document"]) == "Reliance Letter" || Convert.ToString(invoice["Document"]) == "Both";
                    DataTable billing = RlInvoiceService.GetInvoiceDetails(Convert.ToInt32(invoice["InvoiceID"]));
                    decimal rlRate = billing.Rows.Count > 0 ? Convert.ToDecimal(billing.Rows[0]["Rate"]) : 0m;
                    for (int row = 2; row <= used.RowCount(); row++)
                    {
                        string loanId = loanIds[row - 2]; DataRow meta;
                        if (lookup.TryGetValue(loanId, out meta))
                        {
                            target.Cell(row, lastColumn + 1).Value = Convert.ToString(meta["transactionIdentifier"]);
                            target.Cell(row, lastColumn + 2).Value = Convert.ToString(meta["loanid"]);
                            target.Cell(row, lastColumn + 3).Value = Convert.ToString(meta["createdDate"]);
                            target.Cell(row, lastColumn + 4).Value = Convert.ToString(meta["snapShotTakenDate"]);
                        }
                        if (hasRl) { target.Cell(row, lastColumn + 5).Value = rlRate; target.Cell(row, lastColumn + 6).Value = rlRate; }
                    }
                    target.Columns(lastColumn + 5, lastColumn + 6).Style.NumberFormat.Format = "$#,##0.00";
                    int summaryRow = used.RowCount() + 2;
                    decimal rlTotal = billing.AsEnumerable().Sum(item => Convert.ToDecimal(item["Amount"]));
                    if (hasRl) { target.Cell(summaryRow, lastColumn + 5).Value = "RL Total"; target.Cell(summaryRow++, lastColumn + 6).Value = rlTotal; }
                    bool hasSec = Convert.ToString(invoice["Document"]) == "Securitization" || Convert.ToString(invoice["Document"]) == "Both";
                    if (hasSec)
                    {
                        bool hourly = Convert.ToString(invoice["BillingMethod"]) == "Hourly";
                        target.Cell(summaryRow, 1).Value = hourly ? "Securitization Services - Hourly" : "Securitization Services - Per File";
                        target.Cell(summaryRow, 2).Value = ToDecimal(invoice, hourly ? "HoursWorked" : "LoanCount");
                        target.Cell(summaryRow, 3).Value = ToDecimal(invoice, "Cost");
                        target.Cell(summaryRow, 4).Value = ToDecimal(invoice, "ExpectedBilling") - rlTotal;
                        target.Range(summaryRow, 3, summaryRow, 4).Style.NumberFormat.Format = "$#,##0.00";
                        target.Range(summaryRow, 1, summaryRow, 4).Style.Font.SetBold();
                    }
                    lastColumn += headers.Length;
                }
                target.Range(1, 1, 1, lastColumn).Style.Fill.SetBackgroundColor(XLColor.FromArgb(113, 147, 209));
                target.Range(1, 1, 1, lastColumn).Style.Font.SetBold().Font.SetFontColor(XLColor.White);
                target.Range(1, 1, 1, lastColumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                Borders(target.RangeUsed());
                target.ColumnsUsed().AdjustToContents(8, 45);
                target.SheetView.FreezeRows(1);
            }
        }

        private static void Pair(IXLWorksheet sheet, int row, string label, object value)
        { sheet.Cell(row, 1).Value = label; if (value is decimal) sheet.Cell(row, 2).Value = (decimal)value; else sheet.Cell(row, 2).Value = Convert.ToString(value); }

        private static void Money(IXLWorksheet sheet, int row, string label, decimal value)
        { Pair(sheet, row, label, value); sheet.Cell(row, 2).Style.NumberFormat.Format = "$#,##0.00"; }

        private static void Borders(IXLRange range)
        { if (range == null) return; range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin; range.Style.Border.InsideBorder = XLBorderStyleValues.Thin; }

        private static decimal ToDecimal(DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return 0m;
            decimal value;
            return decimal.TryParse(Convert.ToString(row[column]), NumberStyles.Any, CultureInfo.InvariantCulture, out value) ? value : 0m;
        }

        public bool IsReusable { get { return false; } }
    }
}

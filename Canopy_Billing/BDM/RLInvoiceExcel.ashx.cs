using System;
using System.Data;
using System.Globalization;
using System.IO;
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
            int employeeId;
            int invoiceId;
            if (!int.TryParse(context.User.Identity.Name, out employeeId) ||
                !int.TryParse(context.Request.QueryString["id"], out invoiceId) ||
                !RlInvoiceService.HasAccess(employeeId))
            {
                context.Response.StatusCode = 403;
                return;
            }

            DataRow invoice = RlInvoiceService.GetInvoice(invoiceId);
            string invoiceNumber = "RL-" + invoiceId.ToString("D6", CultureInfo.InvariantCulture);
            byte[] excel = BuildExcel(invoice, invoiceNumber);

            context.Response.Clear();
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + invoiceNumber + ".xlsx");
            context.Response.OutputStream.Write(excel, 0, excel.Length);
            context.Response.Flush();
            context.ApplicationInstance.CompleteRequest();
        }

        private static byte[] BuildExcel(DataRow invoice, string invoiceNumber)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet summary = workbook.Worksheets.Add("Invoice");
                summary.Style.Font.FontName = "Biome";
                summary.Style.Font.FontSize = 9;
                summary.Column(1).Width = 52;
                summary.Column(2).Width = 60;
                summary.Range("A1:B1").Merge();
                summary.Row(1).Height = 90;
                string logoPath = HttpContext.Current.Server.MapPath("~/images/logo-canopy.png");
                if (File.Exists(logoPath))
                {
                    IXLPicture logo = summary.AddPicture(logoPath).MoveTo(summary.Cell("A1"));
                    logo.Height = 85;
                }

                string address = string.IsNullOrWhiteSpace(Convert.ToString(invoice["Address"]))
                    ? Convert.ToString(invoice["ClientAddress"]) : Convert.ToString(invoice["Address"]);
                string contacts = Convert.ToString(invoice["ContactPerson"]);
                if (contacts.StartsWith("EMAILS:", StringComparison.OrdinalIgnoreCase)) contacts = contacts.Substring(7);
                string[] emails = contacts.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                summary.Cell("A2").Value = "Client Name"; summary.Cell("B2").Value = Convert.ToString(invoice["BillingEntity"]);
                summary.Cell("A3").Value = "Client Address"; summary.Cell("B3").Value = address;
                summary.Cell("A4").Value = "Contact Person"; summary.Cell("B4").Value = Convert.ToString(invoice["Recipient"]);
                summary.Cell("A5").Value = "Client Contact 1"; summary.Cell("B5").Value = emails.Length > 0 ? emails[0].Trim() : string.Empty;
                summary.Cell("A6").Value = "Client Contact 2"; summary.Cell("B6").Value = emails.Length > 1 ? emails[1].Trim() : string.Empty;
                summary.Cell("A7").Value = "Client Contact 3"; summary.Cell("B7").Value = emails.Length > 2 ? emails[2].Trim() : string.Empty;
                summary.Cell("A8").Value = "Deal Name"; summary.Cell("B8").Value = Convert.ToString(invoice["TradeName"]);

                bool hourly = string.Equals(Convert.ToString(invoice["BillingMethod"]), "Hourly", StringComparison.OrdinalIgnoreCase);
                summary.Cell("A10").Value = hourly ? "Total Hours Worked" : "Total Loan Count";
                summary.Cell("B10").Value = ToDecimal(invoice, hourly ? "HoursWorked" : "LoanCount");
                summary.Range("A10:B10").Style.Fill.SetBackgroundColor(XLColor.FromArgb(189, 215, 238));
                summary.Range("A10:B10").Style.Font.SetBold();

                summary.Range("A11:B11").Merge();
                summary.Cell("A11").Value = Convert.ToString(invoice["Document"]);
                summary.Range("A11:B11").Style.Fill.SetBackgroundColor(XLColor.FromArgb(237, 125, 49));
                summary.Range("A11:B11").Style.Font.SetBold().Font.SetFontColor(XLColor.White);
                summary.Range("A11:B11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                int row = 12;

                DataTable details = RlInvoiceService.GetInvoiceDetails(Convert.ToInt32(invoice["InvoiceID"]));
                if (details.Rows.Count > 0)
                {
                    foreach (DataRow detail in details.Rows)
                    {
                        summary.Cell(row, 1).Value = "Loan Count (" + Convert.ToString(detail["BillingDescription"]) + ")";
                        summary.Cell(row++, 2).Value = Convert.ToDecimal(detail["Quantity"]);
                        summary.Cell(row, 1).Value = "Price";
                        summary.Cell(row, 2).Value = Convert.ToDecimal(detail["Rate"]);
                        summary.Cell(row++, 2).Style.NumberFormat.Format = "$#,##0.00";
                        summary.Cell(row, 1).Value = "Total";
                        summary.Cell(row, 2).Value = Convert.ToDecimal(detail["Amount"]);
                        summary.Cell(row, 2).Style.NumberFormat.Format = "$#,##0.00";
                        summary.Range(row, 1, row, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(189, 215, 238));
                        row++;
                    }
                }
                else
                {
                    summary.Cell(row, 1).Value = hourly ? "Hours Worked (Securitization Services)" : "Loan Count (Securitization Services)";
                    summary.Cell(row++, 2).Value = ToDecimal(invoice, hourly ? "HoursWorked" : "LoanCount");
                    summary.Cell(row, 1).Value = "Price";
                    summary.Cell(row, 2).Value = ToDecimal(invoice, "Cost");
                    summary.Cell(row++, 2).Style.NumberFormat.Format = "$#,##0.00";
                    decimal baseAmount = ToDecimal(invoice, "BaseAmount");
                    summary.Cell(row, 1).Value = "Total";
                    summary.Cell(row, 2).Value = baseAmount == 0 ? ToDecimal(invoice, "ExpectedBilling") : baseAmount;
                    summary.Cell(row, 2).Style.NumberFormat.Format = "$#,##0.00";
                    summary.Range(row, 1, row, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(189, 215, 238));
                    row++;
                }

                if (ToDecimal(invoice, "MinimumAmount") > 0) { summary.Cell(row, 1).Value = "Minimum Billing"; summary.Cell(row, 2).Value = ToDecimal(invoice, "MinimumAmount"); summary.Cell(row++, 2).Style.NumberFormat.Format = "$#,##0.00"; }
                if (ToDecimal(invoice, "MaximumCap") > 0) { summary.Cell(row, 1).Value = "Maximum Cap"; summary.Cell(row, 2).Value = ToDecimal(invoice, "MaximumCap"); summary.Cell(row++, 2).Style.NumberFormat.Format = "$#,##0.00"; }

                summary.Cell(row, 1).Value = "Total Invoice Amount";
                summary.Cell(row, 2).Value = ToDecimal(invoice, "ExpectedBilling");
                summary.Cell(row, 2).Style.NumberFormat.Format = "$#,##0.00";
                summary.Range(row, 1, row, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(198, 89, 17));
                summary.Range(row, 1, row, 2).Style.Font.SetBold().Font.SetFontColor(XLColor.White);
                summary.Range(1, 1, row, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                summary.Range(1, 1, row, 2).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                summary.Range(1, 1, row, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                summary.Range("B3:B8").Style.Alignment.WrapText = true;

                AddLoanDetailsSheet(workbook, Convert.ToString(invoice["FilePath"]));
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private static decimal ToDecimal(DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return 0m;
            decimal value;
            return decimal.TryParse(Convert.ToString(row[column]), NumberStyles.Any, CultureInfo.InvariantCulture, out value) ? value : 0m;
        }

        private static void AddLoanDetailsSheet(XLWorkbook workbook, string filePath)
        {
            IXLWorksheet target = workbook.Worksheets.Add("Loan Details");
            if (string.IsNullOrWhiteSpace(filePath)) { target.Cell("A1").Value = "No loan list was uploaded for this invoice."; return; }

            string uploadRoot = HttpContext.Current.Server.MapPath("~/Uploads/");
            string fullPath = Path.GetFullPath(Path.Combine(uploadRoot, Path.GetFileName(filePath)));
            if (!fullPath.StartsWith(Path.GetFullPath(uploadRoot), StringComparison.OrdinalIgnoreCase) || !File.Exists(fullPath))
            { target.Cell("A1").Value = "The uploaded loan list is not available."; return; }

            string extension = Path.GetExtension(fullPath);
            if (string.Equals(extension, ".xls", StringComparison.OrdinalIgnoreCase))
            {
                Spire.Xls.Workbook source = new Spire.Xls.Workbook();
                source.LoadFromFile(fullPath);
                Spire.Xls.CellRange used = source.Worksheets[0].AllocatedRange;
                for (int r = 1; r <= used.RowCount; r++)
                    for (int c = 1; c <= used.ColumnCount; c++) target.Cell(r, c).Value = source.Worksheets[0].Range[r, c].Value;
                FormatLoanSheet(target, used.ColumnCount);
                return;
            }

            if (!string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase) && !string.Equals(extension, ".xlsm", StringComparison.OrdinalIgnoreCase))
            { target.Cell("A1").Value = "Unsupported uploaded loan-list format: " + Path.GetFileName(filePath); return; }

            using (XLWorkbook source = new XLWorkbook(fullPath))
            {
                IXLRange used = source.Worksheet(1).RangeUsed();
                if (used == null) return;
                used.CopyTo(target.Cell(1, 1));
                FormatLoanSheet(target, used.ColumnCount());
            }
        }

        private static void FormatLoanSheet(IXLWorksheet sheet, int columnCount)
        {
            sheet.Style.Font.FontName = "Biome";
            sheet.Style.Font.FontSize = 9;
            if (columnCount > 0)
                sheet.Range(1, 1, 1, columnCount).Style.Font.SetBold().Font.SetFontColor(XLColor.White)
                    .Fill.SetBackgroundColor(XLColor.FromArgb(113, 147, 209))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            IXLRange used = sheet.RangeUsed();
            if (used != null)
            {
                used.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                used.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }
            sheet.ColumnsUsed().AdjustToContents(8, 45);
            sheet.SheetView.FreezeRows(1);
        }

        public bool IsReusable { get { return false; } }
    }
}

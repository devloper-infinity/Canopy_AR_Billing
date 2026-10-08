using System;
using System.IO;
using System.Web;
using ClosedXML.Excel;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.BDM
{
    public class RLInvoiceImportTemplate : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            int employeeId;
            if (!int.TryParse(context.User.Identity.Name, out employeeId) || !RlInvoiceService.HasAccess(employeeId)) { context.Response.StatusCode = 403; return; }
            using (XLWorkbook book = new XLWorkbook())
            using (MemoryStream stream = new MemoryStream())
            {
                IXLWorksheet sheet = book.Worksheets.Add("Invoice Import");
                string[] headers = { "Our Client", "Recipient", "Trade Name", "Invoice Date", "Document Type", "TM", "Document Signed", "Document Date", "Executed Date", "Billing Entity", "Email Configuration", "Billing Method", "Loan Count", "Hours Worked", "Rate", "Minimum Billing", "Maximum Cap", "RL Scope Product Type", "RL Quantity", "RL Rate", "Notes" };
                for (int i = 0; i < headers.Length; i++) sheet.Cell(1, i + 1).Value = headers[i];
                sheet.Range(1, 1, 1, headers.Length).Style.Font.SetBold().Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.FromHtml("#0F766E"));
                sheet.SheetView.FreezeRows(1); sheet.Range("D2:D500").Style.DateFormat.Format = "mm/dd/yyyy"; sheet.Range("H2:I500").Style.DateFormat.Format = "mm/dd/yyyy";
                sheet.Columns().AdjustToContents(8, 35);

                IXLWorksheet help = book.Worksheets.Add("Instructions");
                help.Cell("A1").Value = "RL / Securitization Invoice Import"; help.Cell("A1").Style.Font.SetBold().Font.SetFontSize(14);
                help.Cell("A3").Value = "One row creates one invoice. Do not rename or remove columns.";
                help.Cell("A5").Value = "Reliance Letter"; help.Cell("B5").Value = "Enter Loan Count and one RL Scope Product Type. Rate may be blank when that scope is configured.";
                help.Cell("A6").Value = "Securitization - Per File"; help.Cell("B6").Value = "Billing Method = Per File; enter Loan Count. Rate/minimum/cap may use configured values.";
                help.Cell("A7").Value = "Securitization - Hourly"; help.Cell("B7").Value = "Billing Method = Hourly; enter Hours Worked. Rate/cap may use configured values.";
                help.Cell("A8").Value = "Missing configuration"; help.Cell("B8").Value = "Enter Rate and required costing fields; the new rate is saved to costing master.";
                help.Cell("A9").Value = "Existing rate override"; help.Cell("B9").Value = "The imported rate applies only to the invoice and does not overwrite an existing master rate.";
                help.Cell("A10").Value = "Both"; help.Cell("B10").Value = "Enter Securitization method/count or hours/rate plus one Reliance Letter scope and RL Quantity.";
                help.Columns().AdjustToContents(12, 90); help.Column(2).Style.Alignment.WrapText = true;
                book.SaveAs(stream);
                byte[] bytes = stream.ToArray();
                context.Response.Clear(); context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=RL_Securitization_Invoice_Import_Template.xlsx");
                context.Response.OutputStream.Write(bytes, 0, bytes.Length); context.Response.Flush(); context.ApplicationInstance.CompleteRequest();
            }
        }
        public bool IsReusable { get { return false; } }
    }
}

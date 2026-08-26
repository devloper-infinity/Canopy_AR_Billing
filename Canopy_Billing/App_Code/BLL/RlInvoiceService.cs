using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using Vendor_Portal.App_Code.DAL;

namespace Vendor_Portal.App_Code.BLL
{
    public static class RlInvoiceService
    {
        public static bool HasAccess(int employeeId)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            using (SqlCommand cmd = new SqlCommand("usp_RLInvoice_HasAccess", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeId;
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        public static DataTable GetInvoices(bool sent)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            using (SqlCommand cmd = new SqlCommand("usp_RLInvoice_GetBySentStatus", con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IsSent", SqlDbType.Bit).Value = sent;
                DataTable table = new DataTable();
                da.Fill(table);
                return table;
            }
        }

        public static bool MarkSent(int invoiceId, int employeeId)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            using (SqlCommand cmd = new SqlCommand("usp_RLInvoice_MarkSent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RLInvoiceID", SqlDbType.Int).Value = invoiceId;
                cmd.Parameters.Add("@SentBy", SqlDbType.Int).Value = employeeId;
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        public static void WriteInvoicePdf(int invoiceId, HttpResponse response)
        {
            DataRow invoice = GetInvoice(invoiceId);
            string invoiceNumber = "RL-" + invoiceId.ToString("D6", CultureInfo.InvariantCulture);
            byte[] pdf = BuildPdf(invoice, invoiceNumber);

            response.Clear();
            response.ContentType = "application/pdf";
            response.AddHeader("Content-Disposition", "inline; filename=" + invoiceNumber + ".pdf");
            response.OutputStream.Write(pdf, 0, pdf.Length);
            response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private static DataRow GetInvoice(int invoiceId)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT r.*, c.Address AS ClientAddress, costing.Rate AS ConfiguredRate
                FROM dbo.RLinvoice r
                LEFT JOIN dbo.Clients c
                    ON LTRIM(RTRIM(c.ClientName)) = LTRIM(RTRIM(r.BillingEntity))
                OUTER APPLY
                (
                    SELECT TOP 1 src.Rate
                    FROM dbo.SecuritizationRelianceLetterCosting src
                    WHERE src.ProjectId = c.ClientID
                      AND LTRIM(RTRIM(src.Type)) = LTRIM(RTRIM(r.Document))
                    ORDER BY COALESCE(src.UpdatedDate, src.AddedDate) DESC, src.RateID DESC
                ) costing
                WHERE r.InvoiceID = @InvoiceID", con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;
                DataTable table = new DataTable();
                da.Fill(table);
                if (table.Rows.Count == 0)
                {
                    throw new InvalidOperationException("Invoice was not found.");
                }
                return table.Rows[0];
            }
        }

        private static byte[] BuildPdf(DataRow row, string invoiceNumber)
        {
            DateTime invoiceDate;
            DateTime.TryParseExact(Convert.ToString(row["InvoiceDate"]), "M.d.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate);
            string date = invoiceDate == DateTime.MinValue ? Convert.ToString(row["InvoiceDate"]) : invoiceDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            string dueDate = invoiceDate == DateTime.MinValue ? string.Empty : invoiceDate.AddDays(30).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (row["ConfiguredRate"] == DBNull.Value)
            {
                throw new InvalidOperationException("Rate is not configured for this Billing Entity and Document Type.");
            }
            decimal rate = Convert.ToDecimal(row["ConfiguredRate"]);
            decimal quantity;
            if (!decimal.TryParse(Convert.ToString(row["LoanCount"]), NumberStyles.Number, CultureInfo.InvariantCulture, out quantity)) quantity = 0;
            decimal amount = row["ExpectedBilling"] == DBNull.Value ? quantity * rate : Convert.ToDecimal(row["ExpectedBilling"]);
            string activity = Convert.ToString(row["Document"]);
            bool isSecuritization = string.Equals(activity.Trim(), "Securitization", StringComparison.OrdinalIgnoreCase);
            string description = isSecuritization ? "Loan Count (Securitization Services)" : "Income from RL";

            StringBuilder content = new StringBuilder();
            Text(content, 36, 748, 12, "Canopy Financial Technology Partners, LLC", true);
            Text(content, 36, 732, 8, "1 Research Ct");
            Text(content, 36, 719, 8, "Rockville, MD 208503221 USA");
            Text(content, 36, 706, 8, "AR@canopytpr.com");
            ColorText(content, 504, 728, 22, "Canopy");
            ColorText(content, 36, 660, 15, "INVOICE");

            GrayText(content, 36, 635, 9, "BILL TO");
            Text(content, 36, 620, 9, Convert.ToString(row["BillingEntity"]), true);
            int addressY = 606;
            string address = string.IsNullOrWhiteSpace(Convert.ToString(row["Address"])) ? Convert.ToString(row["ClientAddress"]) : Convert.ToString(row["Address"]);
            foreach (string line in Wrap(address, 48))
            {
                Text(content, 36, addressY, 9, line);
                addressY -= 13;
            }
            Text(content, 405, 635, 9, "INVOICE"); Text(content, 495, 635, 9, invoiceNumber);
            Text(content, 405, 620, 9, "DATE"); Text(content, 495, 620, 9, date);
            Text(content, 405, 605, 9, "TERMS"); Text(content, 495, 605, 9, "Net 30");
            Text(content, 405, 590, 9, "DUE DATE"); Text(content, 495, 590, 9, dueDate);

            GrayText(content, 36, 555, 9, "DEAL NAME");
            Text(content, 36, 540, 9, Convert.ToString(row["TradeName"]), true);
            content.Append("0.98 0.91 0.88 rg 36 497 540 22 re f\n");
            ColorText(content, 40, 504, 8, "DATE"); ColorText(content, 125, 504, 8, "ACTIVITY");
            ColorText(content, 240, 504, 8, "DESCRIPTION"); ColorText(content, 425, 504, 8, "QTY");
            ColorText(content, 485, 504, 8, "RATE"); ColorText(content, 540, 504, 8, "AMOUNT");
            Text(content, 125, 474, 9, activity); Text(content, 240, 474, 9, description);
            Text(content, 438, 474, 9, Convert.ToString(row["LoanCount"]));
            Text(content, 482, 474, 9, rate.ToString("N2", CultureInfo.InvariantCulture));
            Text(content, 535, 474, 9, amount.ToString("N2", CultureInfo.InvariantCulture));
            content.Append("0.75 G [2 2] 0 d 36 457 m 576 457 l S [] 0 d\n");
            GrayText(content, 305, 432, 10, "BALANCE DUE");
            Text(content, 505, 432, 13, "$" + amount.ToString("N2", CultureInfo.InvariantCulture), true);

            GrayText(content, 260, 82, 8, "Wiring Instructions:");
            GrayText(content, 258, 69, 8, "JP Morgan Chase Bank");
            GrayText(content, 270, 56, 8, "Acct # 660378701");
            GrayText(content, 250, 43, 8, "Routing #021000021 (Wire)");
            GrayText(content, 249, 30, 8, "Routing #074000010 (ACH)");

            return CreatePdf(content.ToString());
        }

        public static string SendInvoiceEmail(int invoiceId)
        {
            DataRow invoice = GetInvoice(invoiceId);
            if (invoice["isVerify"] == DBNull.Value || !Convert.ToBoolean(invoice["isVerify"]))
            {
                throw new InvalidOperationException("Only verified invoices can be sent to the client.");
            }
            string invoiceNumber = "RL-" + invoiceId.ToString("D6", CultureInfo.InvariantCulture);
            byte[] pdf = BuildPdf(invoice, invoiceNumber);
            List<string> recipients = GetRecipients(invoice);
            if (recipients.Count == 0)
            {
                throw new InvalidOperationException("No valid invoice recipients are configured.");
            }

            using (MailMessage mail = new MailMessage())
            using (MemoryStream attachmentStream = new MemoryStream(pdf))
            using (Attachment attachment = new Attachment(attachmentStream, invoiceNumber + ".pdf", "application/pdf"))
            {
                foreach (string recipient in recipients) mail.To.Add(recipient);
                mail.From = new MailAddress("ack@infinity-data.com", "Canopy Billing", Encoding.UTF8);
                mail.Subject = "Canopy invoice " + invoiceNumber + " Test Email";
                mail.Body = "<p>Hello,</p><p>Please find attached invoice " + invoiceNumber + " for your review.</p><p>Thanks,<br/>Canopy Billing</p>";
                mail.IsBodyHtml = true;
                mail.Attachments.Add(attachment);

                using (SmtpClient client = new SmtpClient("smtp.office365.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("ack@infinity-data.com", new bllTracking().GetPassword("ackdata"));
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = true;
                    client.Send(mail);
                }
            }
            return string.Join(", ", recipients);
        }

        private static List<string> GetRecipients(DataRow invoice)
        {
            bool testMode = !string.Equals(ConfigurationManager.AppSettings["RLInvoiceEmailMode"], "Live", StringComparison.OrdinalIgnoreCase);
            string configured = testMode
                ? ConfigurationManager.AppSettings["RLInvoiceTestRecipients"]
                : DeserializeRecipients(Convert.ToString(invoice["ContactPerson"]));

            List<string> recipients = new List<string>();
            foreach (string value in (configured ?? string.Empty).Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string email = value.Trim();
                try
                {
                    MailAddress parsed = new MailAddress(email);
                    if (string.Equals(parsed.Address, email, StringComparison.OrdinalIgnoreCase) && !recipients.Contains(email, StringComparer.OrdinalIgnoreCase)) recipients.Add(email);
                }
                catch { }
            }
            return recipients;
        }

        private static string DeserializeRecipients(string value)
        {
            const string prefix = "EMAILS:";
            return value != null && value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ? value.Substring(prefix.Length) : string.Empty;
        }

        private static IEnumerable<string> Wrap(string value, int width)
        {
            if (string.IsNullOrWhiteSpace(value)) yield break;
            for (int index = 0; index < value.Length; index += width)
                yield return value.Substring(index, Math.Min(width, value.Length - index));
        }

        private static void Text(StringBuilder b, int x, int y, int size, string value, bool bold = false)
        {
            b.AppendFormat(CultureInfo.InvariantCulture, "BT /{0} {1} Tf {2} {3} Td ({4}) Tj ET\n", bold ? "F2" : "F1", size, x, y, Escape(value));
        }

        private static void GrayText(StringBuilder b, int x, int y, int size, string value)
        {
            b.Append("0.52 g "); Text(b, x, y, size, value); b.Append("0 g\n");
        }

        private static void ColorText(StringBuilder b, int x, int y, int size, string value)
        {
            b.Append("0.91 0.57 0.45 rg "); Text(b, x, y, size, value); b.Append("0 g\n");
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)").Replace("\r", " ").Replace("\n", " ");
        }

        private static byte[] CreatePdf(string pageContent)
        {
            List<byte[]> objects = new List<byte[]>();
            objects.Add(Ascii("<< /Type /Catalog /Pages 2 0 R >>"));
            objects.Add(Ascii("<< /Type /Pages /Kids [3 0 R] /Count 1 >>"));
            objects.Add(Ascii("<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 5 0 R /F2 6 0 R >> >> /Contents 4 0 R >>"));
            byte[] stream = Ascii(pageContent);
            objects.Add(Ascii("<< /Length " + stream.Length + " >>\nstream\n" + pageContent + "endstream"));
            objects.Add(Ascii("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>"));
            objects.Add(Ascii("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold >>"));

            using (MemoryStream output = new MemoryStream())
            {
                Write(output, "%PDF-1.4\n");
                List<long> offsets = new List<long> { 0 };
                for (int i = 0; i < objects.Count; i++)
                {
                    offsets.Add(output.Position);
                    Write(output, (i + 1) + " 0 obj\n"); output.Write(objects[i], 0, objects[i].Length); Write(output, "\nendobj\n");
                }
                long xref = output.Position;
                Write(output, "xref\n0 " + (objects.Count + 1) + "\n0000000000 65535 f \n");
                for (int i = 1; i < offsets.Count; i++) Write(output, offsets[i].ToString("D10", CultureInfo.InvariantCulture) + " 00000 n \n");
                Write(output, "trailer << /Size " + (objects.Count + 1) + " /Root 1 0 R >>\nstartxref\n" + xref + "\n%%EOF");
                return output.ToArray();
            }
        }

        private static byte[] Ascii(string value) { return Encoding.ASCII.GetBytes(value); }
        private static void Write(Stream stream, string value) { byte[] bytes = Ascii(value); stream.Write(bytes, 0, bytes.Length); }
    }
}

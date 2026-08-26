using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.BDM
{
    public partial class Yettobebilled : System.Web.UI.Page
    {
        protected bool CanViewRlInvoices { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            int employeeId;
            CanViewRlInvoices = int.TryParse(User.Identity.Name, out employeeId) && RlInvoiceService.HasAccess(employeeId);
        }

        [WebMethod]
        public static string GetRlInvoices()
        {
            EnsureRlInvoiceAccess();
            return Serialize(RlInvoiceService.GetInvoices(false));
        }

        [WebMethod]
        public static object SendRlInvoiceToClient(int invoiceId)
        {
            try
            {
                int employeeId = EnsureRlInvoiceAccess();
                string recipients = RlInvoiceService.SendInvoiceEmail(invoiceId);
                bool sent = RlInvoiceService.MarkSent(invoiceId, employeeId);
                return new { Success = sent, Message = sent ? "Invoice sent successfully to " + recipients + "." : "Email was sent, but the sent status could not be recorded." };
            }
            catch (Exception ex)
            {
                return new { Success = false, Message = "Invoice was not sent. " + ex.Message };
            }
        }

        private static int EnsureRlInvoiceAccess()
        {
            int employeeId;
            if (!int.TryParse(HttpContext.Current.User.Identity.Name, out employeeId) || !RlInvoiceService.HasAccess(employeeId))
                throw new HttpException(403, "You do not have access to RL/Sec invoices.");
            return employeeId;
        }

        private static string Serialize(DataTable table)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            foreach (DataRow dr in table.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns) row[column.ColumnName] = dr[column] == DBNull.Value ? null : dr[column];
                rows.Add(row);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            return serializer.Serialize(rows);
        }

        [WebMethod]
        public static string GetAllBilledProjects()
        {
            DataTable dt1 = new bllTracking().GetAllProjectSendToAccountsDetailsBasedonDomain(0, null, 9, true);
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
        public static string GetYetobeBilled()
        {
            DataTable dt1 = new bllTracking().GetYettobeBilled_Revised();
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
    }

}

using System;
using System.Web;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.BDM
{
    public class RLInvoicePdf : IHttpHandler
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

            RlInvoiceService.WriteInvoicePdf(invoiceId, context.Response);
        }

        public bool IsReusable { get { return false; } }
    }
}

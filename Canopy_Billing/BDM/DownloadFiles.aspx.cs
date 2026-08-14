using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.BDM
{
    public partial class DownloadFiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["TrackingSheetID"] != null)
            {
                try
                {
                    DataTable dt = new bllTracking().GetDirectBillingAttachment(Convert.ToInt32(Request.QueryString["TrackingSheetID"]));
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            string Attachment = Convert.ToString(dt.Rows[0]["Attachment"]);
                            if (Attachment != "")
                            {
                                //Attachment = Server.MapPath(Attachment);
                                Response.ContentType = "application/octet-stream";
                                Response.AppendHeader("Content-Disposition", "attachment;filename=" + Attachment.Substring(Attachment.LastIndexOf("\\") + 1));
                                Response.TransmitFile(Attachment);
                                Response.End();
                            }
                        }
                    }
                }
                catch { }
            }
            if (Request.QueryString["ProjectID"] != null && Request.QueryString["Process"] != null)
            {
                try
                {
                    string ProjectId = Convert.ToString(Request.QueryString["ProjectID"]);
                    string Month = Convert.ToString(Request.QueryString["Month"]);
                    string Year = Convert.ToString(Request.QueryString["Year"]);
                    string Process = Convert.ToString(Request.QueryString["Process"]);
                    DataTable dt = new bllTracking().GetClientProcesswiseUploadedDocument(Convert.ToInt32(ProjectId), Month, Year, Process);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            string Attachment = Convert.ToString(dt.Rows[0]["Attachment"]);
                            if (Attachment != "")
                            {
                                //Attachment = Server.MapPath(Attachment);
                                Response.ContentType = "application/octet-stream";
                                Response.AppendHeader("Content-Disposition", "attachment;filename=" + Attachment.Substring(Attachment.LastIndexOf("\\") + 1));
                                Response.TransmitFile(Attachment);
                                Response.End();
                            }
                        }
                    }
                }
                catch { }
            }
        }
    }
}
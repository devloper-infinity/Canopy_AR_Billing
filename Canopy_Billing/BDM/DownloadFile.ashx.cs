using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Canopy_Billing.BDM
{
    /// <summary>
    /// Summary description for DownloadFile
    /// </summary>
    public class DownloadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileName = context.Request.QueryString["file"];

            string fullPath = context.Server.MapPath("~/Uploads/" + fileName);

            if (File.Exists(fullPath))
            {
                context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                context.Response.AddHeader("Content-Disposition", "inline; filename=" + fileName);
                context.Response.WriteFile(fullPath);
            }
            else
            {
                context.Response.Write("File not found");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
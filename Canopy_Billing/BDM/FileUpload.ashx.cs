using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using Vendor_Portal.App_Code.DAL;

namespace Canopy_Billing.BDM
{
    /// <summary>
    /// Summary description for FileUpload
    /// </summary>
    public class FileUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var file = context.Request.Files[0];
            string id = context.Request["id"];

            string fileName = Path.GetFileName(file.FileName);
            string path = context.Server.MapPath("~/Uploads/" + fileName);

            file.SaveAs(path);

            // Save in DB
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                string query = "UPDATE RLInvoice SET FilePath=@FilePath WHERE InvoiceID=@Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FilePath", fileName);
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            context.Response.ContentType = "application/json";
            context.Response.Write("{\"status\":true}");
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
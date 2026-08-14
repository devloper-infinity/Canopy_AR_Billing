using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.BDM
{
    public partial class AddInvoice : System.Web.UI.Page
    {
        static string NewFileName = "";
        static string GUIDFile = "";
        static string FolderPath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FolderPath = Server.MapPath(@"~\ReportDocument");
            try
            {
                HttpContext postedContext = HttpContext.Current;
                HttpPostedFile file = postedContext.Request.Files[0];

                string name = file.FileName;
                byte[] binaryWriteArray = new byte[file.InputStream.Length];
                file.InputStream.Read(binaryWriteArray, 0,
                (int)file.InputStream.Length);

                FileInfo file_Info = new FileInfo(file.FileName);
                string ext = file_Info.Extension;

                string file_Name = Guid.NewGuid().ToString() + "_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ext;
                NewFileName = Server.MapPath("..//TempFiles//" + file_Name);
                FileStream objfilestream = new FileStream(NewFileName, FileMode.Create, FileAccess.ReadWrite);
                objfilestream.Write(binaryWriteArray, 0,
                binaryWriteArray.Length);
                objfilestream.Close();
            }
            catch { }
        }

        [WebMethod]
        public static string GetAllDirectBilledInvoices()
        {
            DataTable dt1 = new bllTracking().GetAllDirectBilledInvoices();
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
        public static string GetAllClientList()
        {
            DataTable dt1 = new bllTracking().GetAllClientForAddInvoice();
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
        public static int InsertDirectBilling(string Name, string InvoiceDate, string ClientName, string TotalFiles, string TotalAmount, string SecRelLetter, string Thirdpartyrevenue, string SecLoanCount, string SecAmount, string RelLoanCount, string RelAmount, int ProjectID)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            string BillingPeriod = Name + "_" + ClientName + "_" + InvoiceDate;
            htParam.Add("BillingPeriod", BillingPeriod);
            htParam.Add("BillingAddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            htParam.Add("Name", Name);
            htParam.Add("InvoiceDate", InvoiceDate);
            if (NewFileName != "")
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                string SubPath = FolderPath + "\\" + Convert.ToString(DateTime.Now.ToString("dd-MMM-yyyy"));
                if (!Directory.Exists(SubPath))
                {
                    Directory.CreateDirectory(SubPath);
                }
                string UniquePath = SubPath + "\\" + DateTime.Now.ToString("Invoices");
                if (!Directory.Exists(UniquePath))
                {
                    Directory.CreateDirectory(UniquePath);
                }
                string FileName = UniquePath + "\\" + NewFileName.Substring(NewFileName.LastIndexOf("\\") + 1);
                File.Copy(NewFileName, FileName);

                htParam.Add("Attachment", FileName);
            }
            else
                htParam.Add("Attachment", "");

            htParam.Add("ClientName", ClientName);
            htParam.Add("TotalFiles", TotalFiles);
            htParam.Add("TotalAmount", TotalAmount);
            htParam.Add("SecRel", SecRelLetter);
            htParam.Add("ThirdPartyRevenue", Thirdpartyrevenue);
            htParam.Add("SecLoanCount", SecLoanCount);
            htParam.Add("SecAmount", SecAmount);
            htParam.Add("RelLoanCount", RelLoanCount);
            htParam.Add("RelAmount", RelAmount);
            htParam.Add("ProjectID", ProjectID);
            returnvalue = new bllTracking().InsertDirectBilling(htParam);
            return returnvalue;
        }
    }
}
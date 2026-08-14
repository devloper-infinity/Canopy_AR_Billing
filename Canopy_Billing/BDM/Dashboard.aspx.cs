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
    public partial class Dashboard : System.Web.UI.Page
    {
        static string NewFileName = "";
        static string FileName = "";
        static string GUIDFile = "";
        static string FolderPath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 9978 || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 9977 ||
               int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 9976 || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 8967
                || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 10332
                 || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 10333
                  || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 10334)
            {
                dvmain.Style.Add("display", "none");
            }
            FolderPath = Server.MapPath(@"~\ProjectDocuments");
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
                GUIDFile = file_Name;
                NewFileName = Server.MapPath("..//TempFiles//" + file_Name);
                FileStream objfilestream = new FileStream(NewFileName, FileMode.Create, FileAccess.ReadWrite);
                objfilestream.Write(binaryWriteArray, 0,
                binaryWriteArray.Length);
                objfilestream.Close();
            }
            catch { }
        }

        [WebMethod]
        public static string GetUserAccesInfo()
        {
            DataTable dt1 = new bllLogin().GetUserInformation(int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
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
        public static string GetClientwiseDashboard()
        {
            DataTable dt1 = new bllTracking().GetClientwiseDashboard();
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
        public static string GetClientwiseDashboard_ERpData()
        {
            DataTable dt1 = new bllTracking().GetClientwiseDashboard_ERPData();
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
        public static string GetClientwiseDashboard_DifferencewithQuickbook()
        {
            DataTable dt1 = new bllTracking().GetClientwiseDashboard_DifferencewithQuickbook();
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
        public static string GetClientwiseDashboard_DifferencewithQuickbookRL()
        {
            DataTable dt1 = new bllTracking().GetClientwiseDashboard_DifferencewithQuickbookRL();
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
        public static int UpdateQuickbookData(int ProjectID, string Process, string txtID, string txtValue)
        {
            int returnvalue = 0;
            string[] outer = txtID.Split('_');
            string[] inner = outer[1].Split('-');
            Hashtable htParam = new Hashtable();
            htParam.Add("ProjectId", ProjectID);
            htParam.Add("Process", Process);
            htParam.Add("Month", Convert.ToString(inner[0]));
            htParam.Add("Year", Convert.ToString(inner[1]));
            if (Convert.ToString(inner[2]) == "LoanCount")
            {
                htParam.Add("Type", "LoanCount");
                htParam.Add("LoanCount", Convert.ToString(txtValue));
            }
            else
            {
                htParam.Add("Type", "Amount");
                htParam.Add("Amount", Convert.ToString(txtValue));
            }
            returnvalue = new bllTracking().UpdateQuickbookData(htParam);


            return returnvalue;
        }

        [WebMethod]
        public static int InsertReconciliationRemark(int ProjectId, string Month, string Year, string Process, string Remark)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam.Add("ProjectID", ProjectId);
            htParam.Add("Month", Month);
            htParam.Add("Year", Year);
            htParam.Add("Process", Process);
            htParam.Add("Remark", Remark);
            returnvalue = new bllTracking().InsertReconciliationRemark(htParam);
            return returnvalue;
        }

        [WebMethod]
        public static int UploadAttachment(int ProjectId, string Month, string Year, string Process)
        {
            int returnvalue = 0;
            if (NewFileName != "")
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                string SubPath = FolderPath + "\\" + Convert.ToString(Year);
                if (!Directory.Exists(SubPath))
                {
                    Directory.CreateDirectory(SubPath);
                }
                string UniquePath = SubPath + "\\" + Convert.ToString(Month);
                if (!Directory.Exists(UniquePath))
                {
                    Directory.CreateDirectory(UniquePath);
                }
                string FinalPath = UniquePath + "\\" + Convert.ToString(ProjectId) + "_" + Convert.ToString(Process);
                if (!Directory.Exists(FinalPath))
                {
                    Directory.CreateDirectory(FinalPath);
                }
                File.Copy(NewFileName, FinalPath + "\\" + GUIDFile);
                Hashtable htParam = new Hashtable();
                htParam.Add("ProjectID", ProjectId);
                htParam.Add("Month", Month);
                htParam.Add("Year", Year);
                htParam.Add("Process", Process);
                htParam.Add("Attachment", FinalPath + "\\" + GUIDFile);
                returnvalue = new bllTracking().InsertReconciliationAttachment(htParam);
            }
            else
                returnvalue = 0;
            return returnvalue;
        }

    }
}
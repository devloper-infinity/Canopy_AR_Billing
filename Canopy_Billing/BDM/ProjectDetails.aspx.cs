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
    public partial class ProjectDetails : System.Web.UI.Page
    {
        static string NewFileName_scope = "";
        static string NewFileName_agreement = "";
        static string NewFileName_sla = "";
        static string GUIDFile = "";
        static string FolderPath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
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
                if (file.FileName.Contains("ScopeDoc_"))
                {
                    NewFileName_scope = Server.MapPath("..//TempFiles//" + "ScopeDoc_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_scope, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                if (file.FileName.Contains("AgreementDoc_"))
                {
                    NewFileName_agreement = Server.MapPath("..//TempFiles//" + "AgreementDoc_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_agreement, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }
                if (file.FileName.Contains("SLADoc_"))
                {
                    NewFileName_sla = Server.MapPath("..//TempFiles//" + "SLADoc_" + file_Name);
                    FileStream objfilestream = new FileStream(NewFileName_sla, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(binaryWriteArray, 0,
                    binaryWriteArray.Length);
                    objfilestream.Close();
                }

            }
            catch { }
        }
        [WebMethod]
        public static string GetProjectInformation(int ProjectID)
        {
            DataTable dt1 = new bllTracking().GetbyProjectApproval_IdReq(ProjectID);
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
        public static string GetAllSalesBDM()
        {
            DataTable dt1 = new bllTracking().getallMarketingEmployee();
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
        public static string GetCOnfiguredProcessDeals(int ProjectID)
        {
            DataTable dt1 = new bllTracking().GetUWProcess(ProjectID);
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
        public static int InsertProject(string Project, string Company, string ContactPerson, string Phone, string Email, string WebURL, string Address, string Remark)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam["Company_Id"] = "0";
            htParam["ProjectName"] = Project;
            htParam["Company_Name"] = Company;
            htParam["Contact_Person"] = ContactPerson;
            htParam["Phonenumber"] = Phone;
            htParam["Email-Id"] = Email;
            htParam["Url"] = WebURL;
            htParam["Address"] = Address;
            htParam["Result"] = Remark;
            htParam["ProcessName"] = "";
            htParam["Type"] = true;
            htParam["Dealwise"] = false;
            htParam["AddedBy"] = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            returnvalue = new bllTracking().InsertProjectApprovalRequest(htParam);
            return returnvalue;

        }
        [WebMethod]
        public static int UpdateProject(int ProjectApprovalID, string Project, string Company, string ContactPerson, string Phone, string Email, string WebURL, string Address, string Remark, int ProjectID)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam["ProjectApproval_Id"] = ProjectApprovalID;
            htParam["Company_Id"] = "0";
            htParam["ProjectName"] = Project;
            htParam["Company_Name"] = Company;
            htParam["Contact_Person"] = ContactPerson;
            htParam["Phonenumber"] = Phone;
            htParam["Email-Id"] = Email;
            htParam["Url"] = WebURL;
            htParam["Address"] = Address;
            htParam["Result"] = Remark;
            htParam["UpdatedBY"] = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            htParam["ERPProjectId"] = ProjectID;
            returnvalue = new bllTracking().UpdateProjectApprovalRequest(htParam);
            return returnvalue;
        }

        [WebMethod]
        public static int AddNewProcessDeal(string ProcessName, int ProjectID, bool CopyDeal, string FromDealNo)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam.Add("ProcessName", ProcessName);
            htParam.Add("ProjectId", ProjectID);
            htParam.Add("AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            if (CopyDeal == true)
            {
                htParam.Add("FromDealNo", FromDealNo);
                htParam.Add("ToDealNo", ProcessName);
            }
            returnvalue = new bllTracking().InsertUWProcess(htParam);
            return returnvalue;
        }

        [WebMethod]
        public static string GetSalesInformation(int ProjectID)
        {
            DataTable dt1 = new bllTracking().GetProjectApprovalInformation(ProjectID);
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
        public static int AddSalesInformation(int projectApprovalID, string ProjectName, int BDM, string Scope, string ExpectedVolume, string ExpectedStartDate, string isNDA, string NDAStartDate, string NDAENdDate, string NDAClient, string NDAInfinity, string isSLA, string SLAStartDate, string SLAEndDate, string SLAClient, string SLAInfinity, string ProjectStatus, string Duration, string Remark, string RateRevisionDate)
        {
            int returnvalue = 0;
            Hashtable htParam = new Hashtable();
            htParam.Add("ProjectApproval_Id", projectApprovalID);
            htParam.Add("ProcessName", "");
            htParam.Add("ProjectName", ProjectName);
            htParam.Add("BDM", BDM);
            htParam.Add("RequestedDate", DateTime.Now.ToString("dd-MMM-yyyy"));
            htParam.Add("ScopeOfProject", Scope);
            bool nda = isNDA == "1" ? true : false;
            if (isNDA == "0")
                htParam.Add("NDASigned", null);
            else if (isNDA == "1")
            {
                htParam.Add("NDASigned", isNDA);
                htParam.Add("DateOfNDAAgreement", NDAStartDate);
                htParam.Add("ExpirationDateofNDAAgreement", NDAENdDate);
                htParam.Add("NDASignedByClient", NDAClient == "1" ? true : false);
                htParam.Add("NDASignedBYInfinity", NDAInfinity == "1" ? true : false);
            }
            else
            {
                htParam.Add("NDASigned", false);
                htParam.Add("NDASignedByClient", null);
                htParam.Add("NDASignedBYInfinity", null);
            }
            if (isSLA == "0")
                htParam.Add("SLASigned", null);
            else if (isSLA == "1")
            {
                htParam.Add("SLASigned", isSLA);
                htParam.Add("DateOfSLAAgreement", SLAStartDate);
                htParam.Add("ExpirationDateofSLAAgreement", SLAEndDate);
                htParam.Add("SLASignedByClient", SLAClient == "1" ? true : false);
                htParam.Add("SLASignedByInfinity", SLAInfinity == "1" ? true : false);
            }
            else
            {
                htParam.Add("SLASigned", false);
                htParam.Add("SLASignedByClient", null);
                htParam.Add("SLASignedByInfinity", null);
            }
            htParam.Add("ProjectStatus", ProjectStatus);
            htParam.Add("ExpectedVolume", ExpectedVolume);
            htParam.Add("ExpectedStartDate", ExpectedStartDate);
            htParam.Add("ProjectDuration", Duration);
            htParam.Add("Remark", Remark);
            htParam.Add("AddedBy", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            htParam.Add("ERPProjectId", projectApprovalID);
            htParam.Add("RateRevisionDate", RateRevisionDate);
            string projName = ProjectName;

            if (NewFileName_scope != "")
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                string SubPath = FolderPath + "\\" + projName;
                if (!Directory.Exists(SubPath))
                {
                    Directory.CreateDirectory(SubPath);
                }
                string UniquePath = SubPath + "\\Scope";
                if (!Directory.Exists(UniquePath))
                {
                    Directory.CreateDirectory(UniquePath);
                }
                string FileName = UniquePath + "\\" + NewFileName_scope.Substring(NewFileName_scope.LastIndexOf("\\") + 1);
                File.Copy(NewFileName_scope, FileName);

                htParam.Add("ScopeDocument", FileName);
            }
            else
                htParam.Add("ScopeDocument", "");


            if (NewFileName_agreement != "")
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                string SubPath = FolderPath + "\\" + projName;
                if (!Directory.Exists(SubPath))
                {
                    Directory.CreateDirectory(SubPath);
                }
                string UniquePath = SubPath + "\\NDA";
                if (!Directory.Exists(UniquePath))
                {
                    Directory.CreateDirectory(UniquePath);
                }
                string FileName = UniquePath + "\\" + NewFileName_agreement.Substring(NewFileName_agreement.LastIndexOf("\\") + 1);
                File.Copy(NewFileName_agreement, FileName);

                htParam.Add("LogoPathNDA", FileName);
            }
            else
                htParam.Add("LogoPathNDA", "");

            if (NewFileName_sla != "")
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                string SubPath = FolderPath + "\\" + projName;
                if (!Directory.Exists(SubPath))
                {
                    Directory.CreateDirectory(SubPath);
                }
                string UniquePath = SubPath + "\\MSA";
                if (!Directory.Exists(UniquePath))
                {
                    Directory.CreateDirectory(UniquePath);
                }
                string FileName = UniquePath + "\\" + NewFileName_sla.Substring(NewFileName_sla.LastIndexOf("\\") + 1);
                File.Copy(NewFileName_sla, FileName);

                htParam.Add("LogoPathMSA", FileName);
            }
            else
                htParam.Add("LogoPathMSA", "");


            returnvalue = new bllTracking().InsertSalesInformation(htParam);
            return returnvalue;
        }
    }
}
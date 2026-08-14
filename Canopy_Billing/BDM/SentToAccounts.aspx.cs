using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class SentToAccounts : System.Web.UI.Page
    {
        public static string ProjectID;
        public static string BillingPeriod;
        public static string ProjectName;
        public static string TrID;
        public static int DomainId;
        public static string InvoiceNumber = "";
        public static string ReportFileName = "";
        public static string ReportFilePath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectID = Convert.ToString(Request.QueryString["ProjectID"]);
            BillingPeriod = Convert.ToString(Request.QueryString["BillingPeriod"]);
            ProjectName = Convert.ToString(Request.QueryString["ProjectName"]);
            TrID = Convert.ToString(Request.QueryString["T"]);
            DomainId = Convert.ToInt32(Request.QueryString["DomainId"]);
        }
        [WebMethod]
        public static string GetTotalProjectAmount(int ProjectID, string ProjectName, string BillingPeriod, string Slot)
        {
            string ProcessName = "";
            if (ProjectName.Contains("-"))
            {
                ProcessName = ProjectName.Substring(ProjectName.IndexOf("-") + 1);
            }
            if (TrID != "")
            {
                if (Convert.ToInt32(TrID) > 10)
                {
                    Slot = TrID;
                    ProcessName = "Other";
                }
            }
            DataSet ds = new bllTracking().GetTotalProjectAmount_UW(ProjectID, BillingPeriod, ProcessName, Slot);
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            if (ds != null)
            {
                DataTable dt1 = ds.Tables[0];
                if (dt1 != null)
                {
                    try
                    {
                        dt1.Columns.Remove("TrackingSheetID");
                        if (dt1.Columns.Contains("Loan #2"))
                            dt1.Columns.Remove("Loan #2");
                        if (dt1.Columns.Contains("Review"))
                            dt1.Columns.Remove("Review");
                        if (dt1.Columns.Contains("Review Status"))
                            dt1.Columns.Remove("Review Status");
                        if (dt1.Columns.Contains("QC"))
                            dt1.Columns.Remove("QC");
                        if (dt1.Columns.Contains("Loan #3"))
                            dt1.Columns.Remove("Loan #3");
                        if (dt1.Columns.Contains("Loan #4"))
                            dt1.Columns.Remove("Loan #4");
                        if (dt1.Columns.Contains("Borrower Name"))
                            dt1.Columns.Remove("Borrower Name");
                        if (dt1.Columns.Contains("State"))
                            dt1.Columns.Remove("State");
                        if (dt1.Columns.Contains("State"))
                            dt1.Columns.Remove("State");
                        dt1.AcceptChanges();
                        dt1.Columns["TotalCharges"].SetOrdinal(dt1.Columns.Count - 1);
                        dt1.AcceptChanges();
                    }
                    catch { }
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
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;
            return ser.Serialize(rows);
        }

        [WebMethod]
        public static int VerifyOrders(int ProjectID, string ProjectName, string BillingPeriod, string Slot)
        {
            int returnvalue = 0;
            string ProcessName = "";
            if (ProjectName.Contains("-"))
            {
                ProcessName = ProjectName.Substring(ProjectName.IndexOf("-") + 1);
            }
            returnvalue = new bllTracking().VerifyBDMOrders(ProjectID, BillingPeriod, ProcessName, Slot);
            return returnvalue;
        }

        [WebMethod]
        public static int GetBillingPeriodVerifiedStatus(int ProjectID, string ProjectName, string BillingPeriod)
        {
            int returnvalue = 0;
            string ProcessName = "";
            if (ProjectName.Contains("-"))
            {
                ProcessName = ProjectName.Substring(ProjectName.IndexOf("-") + 1);
            }
            returnvalue = new bllTracking().GetBillingPeriodVerifiedStatus(ProjectID, BillingPeriod, ProcessName);
            return returnvalue;
        }

        [WebMethod]
        public static string GetInvoiceNumber(int ProjectID, string ProjectName, string BillingPeriod)
        {
            string ProcessName = "";
            if (ProjectName.Contains("-"))
            {
                ProcessName = ProjectName.Substring(ProjectName.IndexOf("-") + 1);
            }
            DataTable dt1 = new bllTracking().GetInvoicePreview(ProjectID, ProcessName, BillingPeriod, "");
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
        public static int GenerateInvoice(string Amount)
        {
            int returnvalue = 1;
            CrystalReportViewer crReport = new CrystalReportViewer();
            crReport.AutoDataBind = true;
            string Process = "";
            string Securitization = "";
            if (ProjectName.Contains("-"))
            {
                Process = Convert.ToString(ProjectName.Substring(ProjectName.IndexOf("-") + 1));
            }
            else
            {
                Process = BillingPeriod;
            }
            if (TrID != "")
            {
                if (Convert.ToInt32(TrID) > 10)
                {
                    Process = Process + " - Other" + " - " + TrID;
                }
            }

            DataTable dtPreview = new bllTracking().GetInvoicePreview(int.Parse(ProjectID), Process, BillingPeriod, Amount);
            if (dtPreview != null)
            {
                if (dtPreview.Rows.Count > 0)
                {
                    Securitization = Convert.ToString(dtPreview.Rows[0]["Securitization"]);
                    InvoiceNumber = Convert.ToString(dtPreview.Rows[0]["InvoiceNumber"]);
                }
            }

            DataSet dt = new DataSet();

            ReportDocument rpt = new ReportDocument();
            if (Process != BillingPeriod)
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/PCQCSummary_Preview.rpt"));
            else if (Securitization == "Yes")
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/SecuritizationPreview.rpt"));
            else if (Securitization == "Rebuttal")
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/RebuttalPreview.rpt"));
            else if (Securitization == "Research")
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/Research_Preview.rpt"));
            else if (Securitization == "Inventory")
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/Inventory_Preview.rpt"));
            else if (Securitization == "642")
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/642_Preview.rpt"));
            else if (Securitization == "670")
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/670_Preview.rpt"));
            else if (int.Parse(ProjectID) == 70)
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/NewSummaryReport1561_Preview.rpt"));
            else if (int.Parse(ProjectID) == 512)
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/2091_Preview.rpt"));
            else
                rpt.Load(HttpContext.Current.Server.MapPath("~/Reports/NewSummaryReport1_Preview.rpt"));


            CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval3 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval4 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval5 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval6 = new ParameterValues();
            CrystalDecisions.Shared.ParameterValues pval7 = new ParameterValues();

            ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
            pdisval2.Value = ProjectID;
            pval2.Add(pdisval2);

            ParameterDiscreteValue pdisval3 = new ParameterDiscreteValue();
            pdisval3.Value = BillingPeriod;
            pval3.Add(pdisval3);

            ParameterDiscreteValue pdisval4 = new ParameterDiscreteValue();
            if (BillingPeriod != Process)
                pdisval4.Value = Process;
            else
                pdisval4.Value = "";
            pval4.Add(pdisval4);

            ParameterDiscreteValue pdisval5 = new ParameterDiscreteValue();
            pdisval5.Value = Convert.ToString(HttpContext.Current.Request.Form["billdetails_header_totalamount"]);
            pval5.Add(pdisval5);

            ParameterDiscreteValue pdisval6 = new ParameterDiscreteValue();
            pdisval6.Value = Convert.ToString(Convert.ToString(HttpContext.Current.Request.QueryString["Slot"]));
            pval7.Add(pdisval6);

            rpt.DataDefinition.ParameterFields["@ProjectID"].ApplyCurrentValues(pval2);
            rpt.DataDefinition.ParameterFields["@ProcessName"].ApplyCurrentValues(pval4);
            rpt.DataDefinition.ParameterFields["@BillingPeriod"].ApplyCurrentValues(pval3);
            rpt.DataDefinition.ParameterFields["@TotalCost"].ApplyCurrentValues(pval5);
            int chkSlot = new bllTracking().CheckSlotPassing(int.Parse(ProjectID), BillingPeriod, Process);
            if (chkSlot == 1)
                rpt.DataDefinition.ParameterFields["@Slot"].ApplyCurrentValues(pval7);

            CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            CrystalDecisions.Shared.ConnectionInfo crConnectionInfo;
            CrystalDecisions.Shared.TableLogOnInfos crtableLogoninfos;
            CrystalDecisions.Shared.TableLogOnInfo crtableLogoninfo;
            CrystalDecisions.CrystalReports.Engine.Tables CrTables;
            crConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();
            crtableLogoninfos = new CrystalDecisions.Shared.TableLogOnInfos();
            crtableLogoninfo = new CrystalDecisions.Shared.TableLogOnInfo();

            crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ServerName"];
            crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
            crConnectionInfo.UserID = ConfigurationManager.AppSettings["UserID"];
            crConnectionInfo.Password = ConfigurationManager.AppSettings["Password"];

            CrTables = rpt.Database.Tables;

            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }


            crReport.RefreshReport();
            crReport.Visible = true;
            crReport.HasExportButton = false;
            crReport.HasPrintButton = false;
            crReport.HasPageNavigationButtons = true;
            crReport.HasCrystalLogo = false;
            crReport.HasDrillUpButton = false;
            crReport.HasSearchButton = false;

            crReport.HasToggleGroupTreeButton = false;
            crReport.HasZoomFactorList = false;
            crReport.ToolbarStyle.Width = new Unit("750px");
            crReport.ReportSource = rpt;
            string strDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            string strTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            string filename = ProjectID + "_" + BillingPeriod + "_" + strDate + strTime;
            try
            {
                InvoiceNumber = Convert.ToString(dtPreview.Rows[0]["InvoiceNumber"]);
            }
            catch { }
            if (InvoiceNumber == "")
            {
                InvoiceNumber = filename;
            }
            else
            {
                filename = InvoiceNumber;
            }
            filename = filename.Replace(",", "_");
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(@"~/BillingDocuments/")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~/BillingDocuments/"));
            }
            string filePath =
                HttpContext.Current.Server.MapPath("~/BillingDocuments/") + filename + ".pdf";

            int result = new bllTracking().InsertGroupAttachmentPath_PDf_Before(ProjectName + "-" + Process, BillingPeriod, Convert.ToString(@"~/BillingDocuments/" + filename + ".pdf"), InvoiceNumber);
            rpt.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);
            ReportFilePath = filePath;
            ReportFileName = filename + ".pdf";

            rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, filename);

            return returnvalue;

        }

        public void btndownload_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", "application/xlsx");
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(ReportFilePath));
            Response.TransmitFile(ReportFilePath);
            Response.End();
        }

        protected void newbtn_Click(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static int Sendtoclient(int ProjectID, string ProjectName, string BillingPeriod, string Amount, int OrderCount, bool IsManual, string InvoiceNumber)
        {
            int returnvalue = 0;
            string ProcessName = "";
            if (ProjectName.Contains("-"))
            {
                ProcessName = ProjectName.Substring(ProjectName.IndexOf("-") + 1);
            }
            Hashtable htParam = new Hashtable();
            htParam.Add("ProjectID", Convert.ToString(ProjectID));
            htParam.Add("BillingPeriod", Convert.ToString(BillingPeriod));
            htParam.Add("ProjectName", Convert.ToString(ProjectName));
            htParam.Add("Amount", Amount);
            htParam.Add("Added_By", int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            htParam.Add("ClientProcess", ProcessName);
            htParam.Add("OrderCount", OrderCount);
            htParam.Add("IsManual", IsManual);
            htParam.Add("InvoiceNoManual", InvoiceNumber);
            htParam.Add("Slot", "0");
            returnvalue = new bllTracking().UpdateSendToclient(htParam);
            return returnvalue;
        }


        //protected void btn12_Click(object sender, EventArgs e)
        //{
        //    string Process = "";
        //    string Securitization = "";
        //    if (Convert.ToString(Request.QueryString["ProjectName"]).Contains("-"))
        //    {
        //        Process = Convert.ToString(Request.QueryString["ProjectName"]).Substring(Convert.ToString(Request.QueryString["ProjectName"]).IndexOf("-") + 1);
        //    }
        //    else
        //    {
        //        Process = BillingPeriod;
        //    }
        //    DataTable dtPreview = new bllTracking().GetInvoicePreview(int.Parse(ProjectID), Process, BillingPeriod, Convert.ToString(Request.Form["billdetails_header_totalamount"]));
        //    if (dtPreview != null)
        //    {
        //        if (dtPreview.Rows.Count > 0)
        //        {
        //            Securitization = Convert.ToString(dtPreview.Rows[0]["Securitization"]);
        //            InvoiceNumber = Convert.ToString(dtPreview.Rows[0]["InvoiceNumber"]);
        //        }
        //    }

        //    DataSet dt = new DataSet();

        //    ReportDocument rpt = new ReportDocument();
        //    if (Process != BillingPeriod)
        //        rpt.Load(Server.MapPath("~/Reports/PCQCSummary_Preview.rpt"));
        //    else if (Securitization == "Yes")
        //        rpt.Load(Server.MapPath("~/Reports/SecuritizationPreview.rpt"));
        //    else if (Securitization == "Rebuttal")
        //        rpt.Load(Server.MapPath("~/Reports/RebuttalPreview.rpt"));
        //    else if (Securitization == "Research")
        //        rpt.Load(Server.MapPath("~/Reports/Research_Preview.rpt"));
        //    else if (Securitization == "Inventory")
        //        rpt.Load(Server.MapPath("~/Reports/Inventory_Preview.rpt"));
        //    else if (Securitization == "642")
        //        rpt.Load(Server.MapPath("~/Reports/642_Preview.rpt"));
        //    else if (Securitization == "670")
        //        rpt.Load(Server.MapPath("~/Reports/670_Preview.rpt"));
        //    else if (int.Parse(ProjectID) == 70)
        //        rpt.Load(Server.MapPath("~/Reports/NewSummaryReport1561_Preview.rpt"));
        //    else if (int.Parse(ProjectID) == 512)
        //        rpt.Load(Server.MapPath("~/Reports/2091_Preview.rpt"));
        //    else
        //        rpt.Load(Server.MapPath("~/Reports/NewSummaryReport1_Preview.rpt"));


        //    CrystalDecisions.Shared.ParameterValues pval1 = new ParameterValues();
        //    CrystalDecisions.Shared.ParameterValues pval2 = new ParameterValues();
        //    CrystalDecisions.Shared.ParameterValues pval3 = new ParameterValues();
        //    CrystalDecisions.Shared.ParameterValues pval4 = new ParameterValues();
        //    CrystalDecisions.Shared.ParameterValues pval5 = new ParameterValues();
        //    CrystalDecisions.Shared.ParameterValues pval6 = new ParameterValues();
        //    CrystalDecisions.Shared.ParameterValues pval7 = new ParameterValues();

        //    ParameterDiscreteValue pdisval2 = new ParameterDiscreteValue();
        //    pdisval2.Value = ProjectID;
        //    pval2.Add(pdisval2);

        //    ParameterDiscreteValue pdisval3 = new ParameterDiscreteValue();
        //    pdisval3.Value = BillingPeriod;
        //    pval3.Add(pdisval3);

        //    ParameterDiscreteValue pdisval4 = new ParameterDiscreteValue();
        //    if (BillingPeriod != Process)
        //        pdisval4.Value = Process;
        //    else
        //        pdisval4.Value = "";
        //    pval4.Add(pdisval4);

        //    ParameterDiscreteValue pdisval5 = new ParameterDiscreteValue();
        //    pdisval5.Value = Convert.ToString(Request.Form["billdetails_header_totalamount"]);
        //    pval5.Add(pdisval5);

        //    ParameterDiscreteValue pdisval6 = new ParameterDiscreteValue();
        //    pdisval6.Value = Convert.ToString(Convert.ToString(Request.QueryString["Slot"]));
        //    pval7.Add(pdisval6);

        //    rpt.DataDefinition.ParameterFields["@ProjectID"].ApplyCurrentValues(pval2);
        //    rpt.DataDefinition.ParameterFields["@ProcessName"].ApplyCurrentValues(pval4);
        //    rpt.DataDefinition.ParameterFields["@BillingPeriod"].ApplyCurrentValues(pval3);
        //    rpt.DataDefinition.ParameterFields["@TotalCost"].ApplyCurrentValues(pval5);
        //    int chkSlot = new bllTracking().CheckSlotPassing(int.Parse(ProjectID), BillingPeriod, Process);
        //    if (chkSlot == 1)
        //        rpt.DataDefinition.ParameterFields["@Slot"].ApplyCurrentValues(pval7);

        //    CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        //    CrystalDecisions.Shared.ConnectionInfo crConnectionInfo;
        //    CrystalDecisions.Shared.TableLogOnInfos crtableLogoninfos;
        //    CrystalDecisions.Shared.TableLogOnInfo crtableLogoninfo;
        //    CrystalDecisions.CrystalReports.Engine.Tables CrTables;
        //    crConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();
        //    crtableLogoninfos = new CrystalDecisions.Shared.TableLogOnInfos();
        //    crtableLogoninfo = new CrystalDecisions.Shared.TableLogOnInfo();

        //    crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ServerName"];
        //    crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
        //    crConnectionInfo.UserID = ConfigurationManager.AppSettings["UserID"];
        //    crConnectionInfo.Password = ConfigurationManager.AppSettings["Password"];

        //    CrTables = rpt.Database.Tables;

        //    foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
        //    {
        //        crtableLogoninfo = CrTable.LogOnInfo;
        //        crtableLogoninfo.ConnectionInfo = crConnectionInfo;
        //        CrTable.ApplyLogOnInfo(crtableLogoninfo);
        //    }


        //    crReport.RefreshReport();
        //    crReport.Visible = true;
        //    crReport.HasExportButton = false;
        //    crReport.HasPrintButton = false;
        //    crReport.HasPageNavigationButtons = true;
        //    crReport.HasCrystalLogo = false;
        //    crReport.HasDrillUpButton = false;
        //    crReport.HasSearchButton = false;

        //    crReport.HasToggleGroupTreeButton = false;
        //    crReport.HasZoomFactorList = false;
        //    crReport.ToolbarStyle.Width = new Unit("750px");
        //    crReport.ReportSource = rpt;
        //    string strDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
        //    string strTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        //    string filename = ProjectID + "_" + BillingPeriod + "_" + strDate + strTime;
        //    try
        //    {
        //        InvoiceNumber = Convert.ToString(dtPreview.Rows[0]["InvoiceNumber"]);
        //    }
        //    catch { }
        //    if (InvoiceNumber == "")
        //    {
        //        InvoiceNumber = filename;
        //    }
        //    else
        //    {
        //        filename = InvoiceNumber;
        //    }
        //    filename = filename.Replace(",", "_");
        //    if (!Directory.Exists(Server.MapPath(@"~/BillingDocuments/")))
        //    {
        //        Directory.CreateDirectory(Server.MapPath(@"~/BillingDocuments/"));
        //    }
        //    string filePath =
        //        Server.MapPath("~/BillingDocuments/") + filename + ".pdf";

        //    int result = new bllTracking().InsertGroupAttachmentPath_PDf_Before(ProjectName + "-" + Process, BillingPeriod, Convert.ToString(@"~/BillingDocuments/" + filename + ".pdf"), InvoiceNumber);
        //    rpt.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

        //    rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, filename);
        //}
    }
}
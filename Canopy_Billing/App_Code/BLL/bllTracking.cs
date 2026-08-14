using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Vendor_Portal.App_Code.DAL;

namespace Vendor_Portal.App_Code.BLL
{
    public class bllTracking
    {
        DALTracking dalTracking = new DALTracking();
        public bllTracking()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region Tracking Sheet


        public DataSet GetAllProjectSendToAccounts(int ProjectID, string BillingPeriod)
        {
            return dalTracking.GetAllProjectSendToAccounts(ProjectID, BillingPeriod);
        }
        public DataSet GetTotalProjectAmount(int ProjectID, string BillingPeriod)
        { return dalTracking.GetTotalProjectAmount(ProjectID, BillingPeriod); }

        public DataSet GetTotalProjectAmountForReport(int ProjectID, string BillingPeriod, string ProcessName)
        { return dalTracking.GetTotalProjectAmountForReport(ProjectID, BillingPeriod, ProcessName); }

        public DataTable GetAllProjectByUserRights()
        {
            return dalTracking.GetAllProjectByUserRights();
        }

        public DataTable GetAllProjectSendToAccountsDetails(int ProjectID, string BillingPeriod)
        {
            return dalTracking.GetAllProjectSendToAccountsDetails(ProjectID, BillingPeriod);
        }
        public DataTable GetAllProjectSendToAccountsDetailsBasedonDomain(int ProjectID, string BillingPeriod, int DomainId)
        {
            return dalTracking.GetAllProjectSendToAccountsDetailsBasedonDomain(ProjectID, BillingPeriod, DomainId);
        }
        public DataTable GetAllProjectBillingPeriod()
        {
            return dalTracking.GetAllProjectBillingPeriod();
        }
        public int InsertAllProjectSendToAccountsDetails(int ProjectID, string BillingPeriod, int AddedBy)
        {
            return dalTracking.InsertAllProjectSendToAccountsDetails(ProjectID, BillingPeriod, AddedBy);
        }

        //public int UpdateAllProjectSendToAccountsDetails(int ProjectID, string BillingPeriod)
        //{
        //    return dalTracking.InsertAllProjectSendToAccountsDetails(ProjectID, BillingPeriod);
        //}
        public string getActualColumnName(string HeaderName, int ProjectID)
        {
            return dalTracking.getActualColumnName(HeaderName, ProjectID);
        }
        public DataTable GetIsUniqueColumnForHeader(int ProjectID)
        {
            return dalTracking.GetIsUniqueColumnForHeader(ProjectID);
        }
        public int SetIsverifyTrueForBillingOrder(int ProjectID, string TempOrderNumberColumn, string OrderNumber, string OrderDate, string TempOrderDateColumn, string BillingPeriod, int AddedBy, string TempProcessColumn, string ActualProcess)
        {
            return dalTracking.SetIsverifyTrueForBillingOrder(ProjectID, TempOrderNumberColumn, OrderNumber, OrderDate, TempOrderDateColumn, BillingPeriod, AddedBy, TempProcessColumn, ActualProcess);
        }
        public DataTable GetAllProjectApprovedforClient(int ProjectID, string BillingPeriod)
        {
            return dalTracking.GetAllProjectApprovedforClient(ProjectID, BillingPeriod);
        }
        public int InsertAllProjectSendToAccountsDetailsBackToProduction(int ProjectID, string BillingPeriod, int AddedBy)
        {
            return dalTracking.InsertAllProjectSendToAccountsDetailsBackToProduction(ProjectID, BillingPeriod, AddedBy);
        }
        public int UpdateSendToclient(Hashtable ht)
        {
            return dalTracking.UpdateSendToclient(ht);
        }
        public DataTable GetAllProjectforClientApproved(int ProjectID, string BillingPeriod)
        {
            return dalTracking.GetAllProjectforClientApproved(ProjectID, BillingPeriod);
        }

        public DataTable GetInvoiceDetails(int InvoiceID)
        {
            return dalTracking.GetInvoiceDetails(InvoiceID);
        }

        public int UpdateInvoiceDetails(Hashtable ht)
        {
            return dalTracking.UpdateInvoiceDetails(ht);
        }
        public DataTable GetInvoiceRemarkDetails(int InvoiceID)
        {
            return dalTracking.GetInvoiceRemarkDetails(InvoiceID);
        }

        public int Insertbillingparam(Hashtable Htparam)
        {
            return dalTracking.Insertbillingparam(Htparam);
        }
        public DataTable bindGridBillingRates()
        {
            return dalTracking.bindGridBillingRates();
        }

        public int DeleteProjectcost(int Projectvalue)
        {
            return dalTracking.DeleteProjectcost(Projectvalue);
        }
        public DataTable GetbyProject_Cost_IdReq(int Project_Cost_Id)
        {
            return dalTracking.GetbyProject_Cost_IdReq(Project_Cost_Id);
        }

        public int UpdateProjectCost(Hashtable Htparam)
        {
            return dalTracking.UpdateProjectCost(Htparam);
        }
        public int Insertbillingcostdeatils(Hashtable Htparam)
        {
            return dalTracking.Insertbillingcostdeatils(Htparam);
        }
        public DataTable Viewprocostdetails(string ProjectCost_Id)
        {
            return dalTracking.Viewprocostdetails(ProjectCost_Id);
        }

        public DataTable GetProjectId_by_Cost_Id(int Project_Cost_Id)
        {
            return dalTracking.GetProjectId_by_Cost_Id(Project_Cost_Id);
        }
        public DataTable GetProjectName(int Project_Id)
        {
            return dalTracking.GetProjectName(Project_Id);
        }
        public DataTable GetColumndata(string ProjectId)
        {
            return dalTracking.GetColumndata(ProjectId);
        }
        public DataTable GetCostDetails_ById(int Cost_Id)
        {
            return dalTracking.GetCostDetails_ById(Cost_Id);
        }
        public DataTable GetProductTypeByProjectID(string ProjectId)
        {
            return dalTracking.GetProductTypeByProjectID(ProjectId);
        }
        public DataTable GetStatusByProjectID(string ProjectId)
        {
            return dalTracking.GetStatusByProjectID(ProjectId);
        }
        public int Updatebillingcostdetails(Hashtable Htparam)
        {
            return dalTracking.Updatebillingcostdetails(Htparam);
        }
        public int UpdateInvoiceRemark(Hashtable Htparam)
        {
            return dalTracking.UpdateInvoiceRemark(Htparam);
        }

        public DataTable GetClientDetails(int ClientId)
        {
            return dalTracking.GetClientDetails(ClientId);
        }


        public int UpdateClientDetails(Hashtable ht)
        {
            return dalTracking.UpdateClientDetails(ht);
        }

        public int InsertClientDetails(Hashtable ht)
        {
            return dalTracking.InsertClientDetails(ht);
        }
        public DataTable GetAllClientDetails()
        {
            return dalTracking.GetAllClientDetails();
        }
        public DataTable GetAllClientListProjectWise(int InvoiceId)
        {
            return dalTracking.GetAllClientListProjectWise(InvoiceId);
        }
        public int DeleteClientDetails(int ClientID, int userId)
        {
            return dalTracking.DeleteClientDetails(ClientID, userId);
        }
        public DataTable GetProjectClientConfiguration(int ProjectId, int InvoiceId)
        {
            return dalTracking.GetProjectClientConfiguration(ProjectId, InvoiceId);
        }
        public int UpdateInvoicePath(int InvId, string strAttachmentPath)
        {
            return dalTracking.UpdateInvoicePath(InvId, strAttachmentPath);
        }
        public int UpdateInvoiceClient(int InvId, int strClientId)
        {
            return dalTracking.UpdateInvoiceClient(InvId, strClientId);
        }

        public DataTable GetCrystalReport(int ProjectID, string BillingPeriod)
        {
            return dalTracking.GetCrystalReport(ProjectID, BillingPeriod);
        }
        public DataTable GetAllOfficialIdsUser()
        {
            return dalTracking.GetAllOfficialIdsUser();
        }
        #region Reviewers
        public DataTable GetAllReviewers()
        {
            return dalTracking.GetAllReviewers();
        }
        public DataTable GetReviewersByID(int ReviewerId)
        {
            return dalTracking.GetReviewersByID(ReviewerId);
        }
        public int InsertReviewerDetails(Hashtable ht)
        {
            return dalTracking.InsertReviewerDetails(ht);
        }

        public int UpdateReviewerDetails(Hashtable ht)
        {
            return dalTracking.UpdateReviewerDetails(ht);
        }
        #endregion

        #region Email COnfiguration
        public DataTable GetAllEmailConfiguration()
        {
            return dalTracking.GetAllEmailConfiguration();
        }
        public DataTable GetEmailConfigurationByID(int ClientId)
        {
            return dalTracking.GetEmailConfigurationByID(ClientId);
        }
        public int InsertEmailConfiguration(Hashtable ht)
        {
            return dalTracking.InsertEmailConfiguration(ht);
        }

        public int UpdateEmailConfiguration(Hashtable ht)
        {
            return dalTracking.UpdateEmailConfiguration(ht);
        }
        #endregion
        public DataTable GetAllClientDetailsProjectWise(int ProjectId)
        {
            return dalTracking.GetAllClientDetailsProjectWise(ProjectId);
        }

        #region Product
        public DataTable GetAllProductProjectWise()
        {
            return dalTracking.GetAllProductProjectWise();
        }
        public DataTable GetAllProductProjectWiseByID(int Product_Id)
        {
            return dalTracking.GetAllProductProjectWiseByID(Product_Id);
        }
        public int InsertProduct(Hashtable ht)
        {
            return dalTracking.InsertProduct(ht);
        }

        public int UpdateProduct(Hashtable ht)
        {
            return dalTracking.UpdateProduct(ht);
        }
        #endregion
        public DataTable EmailDetailsByInvoiceID(int InvoiceID)
        {
            return dalTracking.EmailDetailsByInvoiceID(InvoiceID);
        }
        public DataTable GetBillingDetailsforReport()
        {
            return dalTracking.GetBillingDetailsforReport();
        }
        public DataTable GetBillingDetailsforReportTest(int DomainId, string BillingPeriod)
        {
            return dalTracking.GetBillingDetailsforReportTest(DomainId, BillingPeriod);
        }
        public DataTable GetBillingDetailsforReportPeriodWise(string Month, int Year)
        {
            return dalTracking.GetBillingDetailsforReportPeriodWise(Month, Year);
        }
        #endregion

        public DataTable ViewprocostdetailsHistory(string Project_Id)
        {
            return dalTracking.ViewprocostdetailsHistory(Project_Id);
        }
        public DataTable BindProductTypeprojectWise(string Project_Id)
        {
            return dalTracking.BindProductTypeprojectWise(Project_Id);
        }

        #region FinalStatus
        public DataTable GetAllFinalStatusProjectWise()
        {
            return dalTracking.GetAllFinalStatusProjectWise();
        }
        public DataTable GetAllFinalStatusProjectWiseByID(int Product_Id)
        {
            return dalTracking.GetAllFinalStatusProjectWiseByID(Product_Id);
        }
        public int InsertFinalStatus(Hashtable ht)
        {
            return dalTracking.InsertFinalStatus(ht);
        }

        public int UpdateFinalStatus(Hashtable ht)
        {
            return dalTracking.UpdateFinalStatus(ht);
        }
        public DataTable BindFinalStatusprojectWise(string Project_Id)
        {
            return dalTracking.BindFinalStatusprojectWise(Project_Id);
        }
        public DataTable getalldomains(int EmployeeId)
        {
            return dalTracking.getalldomains(EmployeeId);
        }
        public DataTable GetAllProjectByDomainWise(int DomainId, int EmployeeId)
        {
            return dalTracking.GetAllProjectByDomainWise(DomainId, EmployeeId);
        }
        public DataTable BindProjectApprovalDetails(int Project_Id)
        {
            return dalTracking.BindProjectApprovalDetails(Project_Id);
        }
        public int InsertbillingcostApproval(Hashtable Htparam)
        {
            return dalTracking.InsertbillingcostApproval(Htparam);
        }
        public DataTable BindVerifyProject()
        {
            return dalTracking.BindVerifyProject();
        }
        public DataTable BindProjectForPriceDetails()
        {
            return dalTracking.BindProjectForPriceDetails();
        }
        public DataTable BindVerifyProjectById(int Project_Id)
        {
            return dalTracking.BindVerifyProjectById(Project_Id);
        }
        public DataTable BindVerifyProjectForModifiedRates()
        {
            return dalTracking.BindVerifyProjectForModifiedRates();
        }

        #endregion
        public DataTable getallMarketingEmployee()
        {
            return dalTracking.getallMarketingEmployee();
        }
        public int UpdateProjectApprovalRequest(Hashtable InsertprojectApproval)
        {
            return dalTracking.UpdateProjectApprovalRequest(InsertprojectApproval);
        }
        public int InsertProjectApprovalRequest(Hashtable InsertprojectApproval)
        {
            return dalTracking.InsertProjectApprovalRequest(InsertprojectApproval);
        }
        public DataTable ViewAllProjectApp()
        {
            return dalTracking.ViewAllProjectApp();
        }
        public DataTable GetProjectApprovalInformation(int PIA_Id)
        {
            return dalTracking.GetProjectApprovalInformation(PIA_Id);
        }
        public DataTable GetbyProjectApproval_IdReq(int ProjectApproval_Id)
        {
            return dalTracking.GetbyProjectApproval_IdReq(ProjectApproval_Id);
        }
        public int InsertSalesInformation(Hashtable InsertprojectApproval)
        {
            return dalTracking.InsertSalesInformation(InsertprojectApproval);
        }
        public int InsertBillParameter(Hashtable htParam)
        {
            return dalTracking.InsertBillParameter(htParam);
        }

        public int InsertBillParameterUW(Hashtable htParam)
        {
            return dalTracking.InsertBillParameterUW(htParam);
        }


        public int UpdateSalesInformation(Hashtable InsertprojectApproval)
        {
            return dalTracking.UpdateSalesInformation(InsertprojectApproval);
        }
        public DataTable GetBiilingValuesDomainwise(int ProjectApproval_Id)
        {
            return dalTracking.GetBiilingValuesDomainwise(ProjectApproval_Id);
        }
        public DataTable ViewAllProjectGroup()
        {
            return dalTracking.ViewAllProjectGroup();
        }
        public DataTable ViewAllGroupProjects(string Groupname)
        {
            return dalTracking.ViewAllGroupProjects(Groupname);
        }
        public DataTable GetBillingDetailsCount(string BillingPeriod)
        {
            return dalTracking.GetBillingDetailsCount(BillingPeriod);
        }
        public int ApproveBillParameter(Hashtable htParam)
        {
            return dalTracking.ApproveBillParameter(htParam);
        }
        public DataSet BindTestOrders(int DomainId, string BillingPeriod)
        {
            return dalTracking.BindTestOrders(DomainId, BillingPeriod);
        }
        public DataTable BindSummaryReport(string GroupName, string BillingPeriod)
        {
            return dalTracking.BindSummaryReport(GroupName, BillingPeriod);
        }

        public int InsertGroupAttachmentPath(string GroupName, string BillingPeriod, string strAttachmentPath)
        {
            return dalTracking.InsertGroupAttachmentPath(GroupName, BillingPeriod, strAttachmentPath);
        }
        public DataTable GetSummaryReportAttachments(string GroupName, string BillingPeriod)
        {
            return dalTracking.GetSummaryReportAttachments(GroupName, BillingPeriod);
        }
        public DataTable GetSummaryReportAttachments(int ProjectId, string BillingPeriod, string Process)
        {
            return dalTracking.GetSummaryReportAttachments(ProjectId, BillingPeriod, Process);
        }
        public DataTable GetSummaryReportInvoice(string GroupName, string BillingPeriod)
        {
            return dalTracking.GetSummaryReportInvoice(GroupName, BillingPeriod);
        }

        public DataTable GetClientDetails(string GroupName, string BillingPeriod)
        {
            return dalTracking.GetClientDetails(GroupName, BillingPeriod);
        }

        public DataTable GetClientDetails(int ProjectID, string BillingPeriod)
        {
            return dalTracking.GetClientDetails(ProjectID, BillingPeriod);
        }

        public DataTable GetUWProcess(int ProjectId)
        {
            return dalTracking.GetUWProcess(ProjectId);
        }
        public DataTable GetAllProjectSendToAccountsDetailsBasedonDomain(int ProjectID, string BillingPeriod, int DomainId, bool Refresh)
        {
            return dalTracking.GetAllProjectSendToAccountsDetailsBasedonDomain(ProjectID, BillingPeriod, DomainId, Refresh);
        }
        public DataSet GetTotalProjectAmount_UW(int ProjectID, string BillingPeriod, string ProcessName, string Slot)
        {
            return dalTracking.GetTotalProjectAmount_UW(ProjectID, BillingPeriod, ProcessName, Slot);
        }
        public int VerifyBDMOrders(int ProjectID, string BillingPeriod, string ProcessName, string Slot)
        {
            return dalTracking.VerifyBDMOrders(ProjectID, BillingPeriod, ProcessName, Slot);
        }
        public int GetBillingPeriodVerifiedStatus(int ProjectID, string BillingPeriod, string ProcessName)
        {
            return dalTracking.GetBillingPeriodVerifiedStatus(ProjectID, BillingPeriod, ProcessName);
        }
        public DataTable GetSentToClientList()
        {
            return dalTracking.GetSentToClientList();
        }
        public DataTable GetClientwiseDashboard()
        {
            return dalTracking.GetClientwiseDashboard();
        }

        public DataTable GetClientwiseDashboard_New()
        {
            return dalTracking.GetClientwiseDashboard_New();
        }

        public DataTable GetClientwiseDashboard_New_Client()
        {
            return dalTracking.GetClientwiseDashboard_New_Client();
        }

        public DataTable GetClientwiseDashboard_New_Client_RL()
        {
            return dalTracking.GetClientwiseDashboard_New_Client_RL();
        }

        public DataTable GetClientwiseDashboard_New_Client_SS()
        {
            return dalTracking.GetClientwiseDashboard_New_Client_SS();
        }
        public DataTable GetClientwiseDashboard_ERPData()
        {
            return dalTracking.GetClientwiseDashboard_ERPData();
        }
        public DataTable GetClientwiseDashboard_DifferencewithQuickbook()
        {
            return dalTracking.GetClientwiseDashboard_DifferencewithQuickbook();
        }

        public DataTable GetClientwiseDashboard_DifferencewithQuickbookRL()
        {
            return dalTracking.GetClientwiseDashboard_DifferencewithQuickbookRL();
        }
        public int CheckSlotPassing(int ProjectId, string BillingPeriod, string ProcessName)
        {
            return dalTracking.CheckSlotPassing(ProjectId, BillingPeriod, ProcessName);
        }
        public int InsertGroupAttachmentPath_PDf(string GroupName, string BillingPeriod, string strAttachmentPath)
        {
            return dalTracking.InsertGroupAttachmentPath_PDf(GroupName, BillingPeriod, strAttachmentPath);
        }
        public DataTable GetDataForInvoiceExcel(int ProjectID, string BillingPeriod, string ProcessName, string Slot)
        {
            return dalTracking.GetDataForInvoiceExcel(ProjectID, BillingPeriod, ProcessName, Slot);
        }
        public DataSet GetDataForInvoiceExcel_DS(int ProjectID, string BillingPeriod, string ProcessName, string Slot)
        {
            return dalTracking.GetDataForInvoiceExcel_DS(ProjectID, BillingPeriod, ProcessName, Slot);
        }
        public DataSet GetDataForNewExcelFormat(int ProjectID, string BillingPeriod, string ProcessName)
        {
            return dalTracking.GetDataForNewExcelFormat(ProjectID, BillingPeriod, ProcessName);
        }
        public DataTable getBillingReport(string FromDate, string ToDate)
        {
            return dalTracking.getBillingReport(FromDate, ToDate);
        }


        public DataTable GetExcelDataForExportSecondSheet(int ProjectID, string BillingPeriod, string ProcessName)
        {
            return dalTracking.GetExcelDataForExportSecondSheet(ProjectID, BillingPeriod, ProcessName);
        }
        public int InsertUWProcess(Hashtable htParam)
        {
            return dalTracking.InsertUWProcess(htParam);
        }
        public DataTable getAllBillingParameters(Hashtable htParam)
        {
            return dalTracking.getAllBillingParameters(htParam);
        }
        public DataTable getAllBillingParameters_RateRevision(Hashtable htParam)
        {
            return dalTracking.getAllBillingParameters_RateRevision(htParam);
        }
        public DataTable getProjectDetailsByprocessID(int ProcessID)
        {
            return dalTracking.getProjectDetailsByprocessID(ProcessID);
        }
        public DataTable GetAllDirectBilledInvoices()
        {
            return dalTracking.GetAllDirectBilledInvoices();
        }
        public int InsertDirectBilling(Hashtable htParam)
        {
            return dalTracking.InsertDirectBilling(htParam);
        }
        public DataTable GetAllProjectBilingReport(string FromDate, string ToDate)
        {
            return dalTracking.GetAllProjectBilingReport(FromDate, ToDate);
        }
        public DataTable getDeviationReport(string FromDate, string ToDate)
        {
            return dalTracking.getDeviationReport(FromDate, ToDate);
        }
        public DataTable getDeviationReport_Detailed(string FromDate, string ToDate)
        {
            return dalTracking.getDeviationReport_Detailed(FromDate, ToDate);
        }
        public DataTable getDeviationReportNonDD(string FromDate, string ToDate)
        {
            return dalTracking.getDeviationReportNonDD(FromDate, ToDate);
        }
        public DataTable getDeviationReport_DetailedNonDD(string FromDate, string ToDate)
        {
            return dalTracking.getDeviationReport_DetailedNonDD(FromDate, ToDate);
        }

        public DataTable getDeviationReport_Canopy(string FromDate, string ToDate)
        {
            return dalTracking.getDeviationReport_Canopy(FromDate, ToDate);
        }

        public DataTable getDeviationReport_DetailedCanopy(string FromDate, string ToDate)
        {
            return dalTracking.getDeviationReport_DetailedCanopy(FromDate, ToDate);
        }

        public DataTable GetBillingDifference(string Month, string Year)
        {
            return dalTracking.GetBillingDifference(Month, Year);
        }
        public DataSet GetCostPerRecordReport(string Month, string Year)
        {
            return dalTracking.GetCostPerRecordReport(Month, Year);
        }
        public DataTable GetMergedBillingFor561(string Month, string Year, int ProjectID)
        {
            return dalTracking.GetMergedBillingFor561(Month, Year, ProjectID);
        }
        public DataTable GetDirectBillingAttachment(int TrackingSheetID)
        {
            return dalTracking.GetDirectBillingAttachment(TrackingSheetID);
        }
        public DataTable GetYettobeBilled()
        {
            return dalTracking.GetYettobeBilled();
        }
        public DataTable GetYettobeBilled_Revised()
        {
            return dalTracking.GetYettobeBilled_Revised();
        }

        public DataTable GetInvoicePreview(int ProjectID, string ProcessName, string BillingPeriod, string TotalCost)
        {
            return dalTracking.GetInvoicePreview(ProjectID, ProcessName, BillingPeriod, TotalCost);
        }

        public int InsertGroupAttachmentPath_PDf_Before(string GroupName, string BillingPeriod, string strAttachmentPath, string InvoiceNumber)
        {
            return dalTracking.InsertGroupAttachmentPath_PDf_Before(GroupName, BillingPeriod, strAttachmentPath, InvoiceNumber);
        }

        public DataTable GetAllClientForAddInvoice()
        {
            return dalTracking.GetAllClientForAddInvoice();
        }
        public DataTable GetAllClientForAddInvoice_Onshore()
        {
            return dalTracking.GetAllClientForAddInvoice_Onshore();
        }

        public DataTable getPendingBillingForpayingEntity(string Month, string Year)
        {
            return dalTracking.getPendingBillingForpayingEntity(Month, Year);
        }
        public int UpdatePayingEntity(Hashtable htParam)
        {
            return dalTracking.UpdatePayingEntity(htParam);
        }
        public DataTable PullCanopyData(string Month, string Year)
        {
            return dalTracking.PullCanopyData(Month, Year);
        }

        public DataTable GetCanopyData(string FromDate, string ToDate)
        {
            return dalTracking.GetCanopyData(FromDate, ToDate);
        }

        public DataTable GetCanopyDataDetails(string FromDate, string ToDate)
        {
            return dalTracking.GetCanopyDataDetails(FromDate, ToDate);
        }

        public DataTable GetCanopyTask(int LoanID)
        {
            return dalTracking.GetCanopyTask(LoanID);
        }

        public DataTable GetBillingSummary(string Month, string Year)
        {
            return dalTracking.GetBillingSummary(Month, Year);
        }

        public DataTable GetBillToMaster()
        {
            return dalTracking.GetBillToMaster();
        }

        public string GetProjectNamebyID(int ProjectId)
        {
            return dalTracking.GetProjectNamebyID(ProjectId);
        }
        public int UpdateQuickbookData(Hashtable htParam)
        {
            return dalTracking.UpdateQuickbookData(htParam);
        }

        public int InsertReconciliationRemark(Hashtable htParam)
        {
            return dalTracking.InsertReconciliationRemark(htParam);
        }

        public int InsertReconciliationAttachment(Hashtable htParam)
        {
            return dalTracking.InsertReconciliationAttachment(htParam);
        }
        public DataTable GetClientProcesswiseUploadedDocument(int ProjectID, string Month, string Year, string Process)
        {
            return dalTracking.GetClientProcesswiseUploadedDocument(ProjectID, Month, Year, Process);
        }

        public int InsertBillingHeader(Hashtable htParam)
        {
            return dalTracking.InsertBillingHeader(htParam);
        }

        public DataTable GetBillingHeaderValues()
        {
            return dalTracking.GetBillingHeaderValues();
        }

        public int InsertRateRevisionConfiguration(Hashtable htParam)
        {
            return dalTracking.InsertRateRevisionConfiguration(htParam);
        }
        public int UpdateReminerEmailList(int ProjectID, string EmailList)
        {
            return dalTracking.UpdateReminerEmailList(ProjectID, EmailList);
        }

        public string GetPassword(string Username)
        {
            return dalTracking.GetPassword(Username);
        }
        public DataTable GetRLSecCostByProject(int ProjectID)
        {
            return dalTracking.GetRLSecCostByProject(ProjectID);
        }
        public DataTable GetAllOtherCosting()
        {
            return dalTracking.GetAllOtherCosting();
        }
        public int InsertRates(Hashtable htParam)
        {
            return dalTracking.InsertRates(htParam);
        }
    }
}
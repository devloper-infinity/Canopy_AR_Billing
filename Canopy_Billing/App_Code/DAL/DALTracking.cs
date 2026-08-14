using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;

namespace Vendor_Portal.App_Code.DAL
{
    public class DALTracking
    {
        #region Tracking Sheet


        public DataSet GetAllProjectSendToAccounts(int ProjectID, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "WBT_usp_GetTrackingsheetByProjectByBillingPeriodInfinity");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);


            DataSet dt = SQLHelper.ExecuteDataSetCmd(cmd);
            return dt;
        }

        public DataTable GetAllProjectByUserRights()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetAllProjectforbilling");
            // SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeID", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, EmployeeID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetAllProjectSendToAccountsDetails(int ProjectID, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "WBT_usp_GetTrackingsheetDetailsSendtoAccount");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);


            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable GetAllProjectSendToAccountsDetailsBasedonDomain(int ProjectID, string BillingPeriod, int DomainId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[WBT_usp_GetTrackingsheetDetailsSendtoAccountDomain_Local_UW_Revised]");
            SQLHelper.AddParamToSQLCmd(cmd, "@DomainID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, DomainId);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataSet GetTotalProjectAmount(int ProjectID, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetProjectCost3_HighestFirst]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);

            DataSet dt = SQLHelper.ExecuteDataSetCmd_Billing(cmd);
            return dt;
        }


        public DataSet GetTotalProjectAmountForReport(int ProjectID, string BillingPeriod, string ProcessName)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetProjectCost_ForReport_UW]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProcessName);


            DataSet dt = SQLHelper.ExecuteDataSetCmd_Billing(cmd);
            return dt;
        }
        public DataTable GetAllProjectBillingPeriod()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "WBT_usp_GetTrackingsheetBillingDetails");
            // SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeID", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, EmployeeID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public int InsertAllProjectSendToAccountsDetails(int ProjectID, string BillingPeriod, int AddedBy)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_InsertTrackingsheetDetailsSendtoAccount");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, AddedBy);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public int InsertAllProjectSendToAccountsDetailsBackToProduction(int ProjectID, string BillingPeriod, int AddedBy)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_InsertTrackingsheetDetailsBackToProd");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, AddedBy);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
        public string getActualColumnName(string HeaderName, int ProjectID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "WBT_usp_GetColumnName");
            SQLHelper.AddParamToSQLCmd(cmd, "@HeaderName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, HeaderName);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ProjectID);
            string Columnname = Convert.ToString(SQLHelper.ExecuteScalarCmd(cmd));
            return Columnname;
        }

        public DataTable GetIsUniqueColumnForHeader(int ProjectID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "WBT_usp_GetIsUniqueColumnForHeader"); //usp_getuniquecolumn
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ProjectID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public int SetIsverifyTrueForBillingOrder(int ProjectID, string TempOrderNumberColumn, string OrderNumber, string OrderDate, string TempOrderDateColumn, string BillingPeriod, int AddedBy, string TempProcessColumn, string ActualProcess)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_SetIsverifyTrueForBillingOrder");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@TempOrderNumberColumn", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, TempOrderNumberColumn);
            SQLHelper.AddParamToSQLCmd(cmd, "@OrderNumber", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, OrderNumber);
            SQLHelper.AddParamToSQLCmd(cmd, "@OrderDate", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, OrderDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@TempOrderDateColumn", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, TempOrderDateColumn);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@TempProcessColumn", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, TempProcessColumn);
            SQLHelper.AddParamToSQLCmd(cmd, "@ActualProcess", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, ActualProcess);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public DataTable GetAllProjectApprovedforClient(int ProjectID, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetProjectApprovedforClient");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);


            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public int UpdateSendToclient(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[IB_usp_UpdateProjectApprovedforClient_Sec_Rev_2]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["BillingPeriod"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["ProjectName"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@Amount", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["Amount"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@OrderNo", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["OrderCount"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ClientProcess", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["ClientProcess"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceNoManual", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["InvoiceNoManual"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IsManual", System.Data.SqlDbType.Bit, 1000, System.Data.ParameterDirection.Input, htParam["IsManual"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Slot", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Slot"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public DataTable GetAllProjectforClientApproved(int ProjectID, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[IB_usp_GetAllProjectForClientApproval_UW]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);


            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetInvoiceDetails(int InvoiceID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetInvoiceDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, InvoiceID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int UpdateInvoiceRemark(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_UpdateInvoiceDetailsRemark");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["InvoiceID"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", SqlDbType.BigInt, 0, ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Remark", SqlDbType.NVarChar, 5000, ParameterDirection.Input, htParam["InvoiceRemark"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);

            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue; //-1=Exist, 0=Fail, >0=Success
        }
        public int UpdateInvoiceDetails(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_UpdateInvoiceDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["InvoiceID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceRecievedByClientDate", System.Data.SqlDbType.VarChar, 5000, System.Data.ParameterDirection.Input, htParam["InvoiceRecievedByClientDate"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@CommunicationReciept", SqlDbType.NVarChar, 50, ParameterDirection.Input, htParam["CommunicationReciept"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Remark", SqlDbType.NVarChar, 5000, ParameterDirection.Input, htParam["Remark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@NoDisputeConfirmByClient", SqlDbType.NVarChar, 5000, ParameterDirection.Input, htParam["NoDisputeConfirmByClient"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvCompleteDate", SqlDbType.NVarChar, 5000, ParameterDirection.Input, htParam["InvCompleteDate"]);


            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);

            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue; //-1=Exist, 0=Fail, >0=Success
        }
        public DataTable GetbyProject_Cost_IdReq(int Project_Cost_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetProjectCost_ById ");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectApproval_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Project_Cost_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetInvoiceRemarkDetails(int InvoiceID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetInvoiceremarkDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, InvoiceID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetAllOfficialIdsUser()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetOfficialIdsUsers_New");
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public int Insertbillingparam(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertProjectCost");
            SQLHelper.AddParamToSQLCmd(cmd, "@Project_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Project_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Currency", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Currency"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Rate_Per_Invoice", System.Data.SqlDbType.Decimal, 10, System.Data.ParameterDirection.Input, Htparam["Rate_Per_Invoice"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Wire_Transfer_Charges", System.Data.SqlDbType.Decimal, 10, System.Data.ParameterDirection.Input, Htparam["Wire_trans_Charges"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Addedby", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedIP", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Added_IP"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Type", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Type"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@others", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Others"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;

        }


        public int UpdateProjectCost(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_UpdateProjectCost");
            SQLHelper.AddParamToSQLCmd(cmd, "@Project_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Project_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Currency", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Currency"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Rate_Per_Invoice", System.Data.SqlDbType.Decimal, 10, System.Data.ParameterDirection.Input, Htparam["Rate_Per_Invoice"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Wire_Transfer_Charges", System.Data.SqlDbType.Decimal, 10, System.Data.ParameterDirection.Input, Htparam["Wire_trans_Charges"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Addedby", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedIP", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Added_IP"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Type", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Type"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@others", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Others"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectCostId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["ProjectCostId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;

        }

        public int DeleteProjectcost(int Projectvalue)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_DeleteProjectCost");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectCostId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Projectvalue);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue; //-1=Exist, 0=Fail, >0=Success
        }

        public DataTable bindGridBillingRates()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetallProjectCost");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;

        }
        public int Insertbillingcostdeatils(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertBillingcostdetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectCostdetails_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["ProjectCostdetails_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Column_Name", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Column_Name"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Units", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Units"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Rate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Rate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Ismultiply", System.Data.SqlDbType.Bit, 10, System.Data.ParameterDirection.Input, Htparam["Ismultiply"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IsConditional", System.Data.SqlDbType.Bit, 10, System.Data.ParameterDirection.Input, Htparam["IsConditional"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Conditional_Column", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Conditional_Column"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Value", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Value"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IsBunchWise", System.Data.SqlDbType.Bit, 10, System.Data.ParameterDirection.Input, Htparam["IsBunchWise"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@For_First", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["For_First"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Rate_for_first", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Rate_for_first"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Rate_for_others", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Rate_for_others"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Addedby", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@OrderType", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["OrderType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Updatelevel", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Updatelevel"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@productType", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["productType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@FinalStatus", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, Htparam["FinalStatus"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@WeekdayWeekend", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["WeekdayWeekend"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AdditionalConditions", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["AdditionalConditions"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public DataTable Viewprocostdetails(string ProjectCost_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetNewprocostdetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectCost_Id", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ProjectCost_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetProjectId_by_Cost_Id(int Project_Cost_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "GetProjectNamebycostId");
            SQLHelper.AddParamToSQLCmd(cmd, "@Project_Cost_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Project_Cost_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetProjectName(int Project_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "GetProjectNamebyProjectId");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Project_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetColumndata(string ProjectId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "GetColumndatabyprojectId");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ProjectId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetProductTypeByProjectID(string ProjectId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "WBT_usp_GetProductTypeByProjectForBilling");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, ProjectId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable GetStatusByProjectID(string ProjectId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "WBT_usp_GetStatusByProjectForBilling");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, ProjectId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetCostDetails_ById(int Cost_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetCostDetails_ById");
            SQLHelper.AddParamToSQLCmd(cmd, "@Cost_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Cost_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int Updatebillingcostdetails(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_UpdateBillingcostdetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectCostdetails_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["ProjectCostdetails_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Column_Name", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Column_Name"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Units", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Units"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Rate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Rate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Ismultiply", System.Data.SqlDbType.Bit, 10, System.Data.ParameterDirection.Input, Htparam["Ismultiply"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IsConditional", System.Data.SqlDbType.Bit, 10, System.Data.ParameterDirection.Input, Htparam["IsConditional"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Conditional_Column", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Conditional_Column"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Value", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Value"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IsBunchWise", System.Data.SqlDbType.Bit, 10, System.Data.ParameterDirection.Input, Htparam["IsBunchWise"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@For_First", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["For_First"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Rate_for_first", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Rate_for_first"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Rate_for_others", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Rate_for_others"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@OrderType", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["OrderType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Updatelevel", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["Updatelevel"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@productType", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["productType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@FinalStatus", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, Htparam["FinalStatus"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@WeekdayWeekend", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["WeekdayWeekend"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AdditionalConditions", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["AdditionalConditions"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@Cost_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Cost_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Addedby", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public DataTable GetClientDetails(int ClientId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetClientDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ClientId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, ClientId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public int UpdateClientDetails(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_UpdateClientDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ClientName", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["ClientName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Address", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["Address"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@State", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["State"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@County", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["County"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Zip", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["Zip"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Country", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["Country"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceConfiguration", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["InvoiceConfiguration"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@EmailId", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["EmailId"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@ClientId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["ClientId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Addedby", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public int InsertClientDetails(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_InsertClientDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ClientName", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["ClientName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Address", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["Address"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@State", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["State"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@County", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["County"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Zip", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["Zip"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Country", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["Country"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceConfiguration", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["InvoiceConfiguration"]);
            //SQLHelper.AddParamToSQLCmd(cmd, "@ClientId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["ClientId"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@EmailId", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Htparam["EmailId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Addedby", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public DataTable GetAllClientDetails()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetAllClientDetails");
            //SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, ProjectId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetAllClientListProjectWise(int InvId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetAllClientDetailsProjectWise");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, InvId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetAllClientDetailsProjectWise(int ProjectId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetClientDetailsProjectWise");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, ProjectId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int DeleteClientDetails(int ClientId, int Userid)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_DeleteClientDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ClientId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ClientId);

            SQLHelper.AddParamToSQLCmd(cmd, "@Addedby", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Userid);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public DataTable GetProjectClientConfiguration(int ProjectID, int InvoiceId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetProjectWiseClientInvoiceConfiguration_UW_Sec");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, InvoiceId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int UpdateInvoicePath(int InvId, string strAttachmentPath)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_UpdateInvoicepath");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, InvId);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceAttachmentsPDF", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, strAttachmentPath);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public int UpdateInvoiceClient(int InvId, int strClientId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_UpdateInvoiceClientId");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, InvId);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceClientId", System.Data.SqlDbType.BigInt, 4, System.Data.ParameterDirection.Input, strClientId);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public DataTable GetCrystalReport(int ProjectID, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetCrystalReportQC");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        #region Reviewers
        public DataTable GetAllReviewers()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetAllReviewers");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetReviewersByID(int ReviewerId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetReviewerById");
            SQLHelper.AddParamToSQLCmd(cmd, "@ReviewerId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ReviewerId);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int InsertReviewerDetails(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Insert_ReviewerDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ReviewerName", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["ReviewerName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReviewerCodes", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, Htparam["ReviewerCodes"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public int UpdateReviewerDetails(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Update_ReviewerDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ReviewerName", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["ReviewerName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReviewerCodes", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, Htparam["ReviewerCodes"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReviewerID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ReviewerID"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        #endregion

        #region EmailConfiguration
        public DataTable GetAllEmailConfiguration()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Get_ClientEmailConfiguration");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetEmailConfigurationByID(int CEC_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Get_ClientEmailConfigurationByID");
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, CEC_Id);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int InsertEmailConfiguration(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Insert_ClientEmailConfiguration");
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_ClientId", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["ClientId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_To", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, Htparam["CEC_To"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_CC", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["CEC_CC"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_BCC", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, Htparam["CEC_BCC"]);
            //SQLHelper.AddParamToSQLCmd(cmd, "@CEC_Body", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["CEC_Body"]);


            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBY", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public int UpdateEmailConfiguration(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Update_ClientEmailConfiguration");
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_ClientId", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["ClientId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_To", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, Htparam["CEC_To"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_CC", System.Data.SqlDbType.NVarChar, 200, System.Data.ParameterDirection.Input, Htparam["CEC_CC"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_BCC", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, Htparam["CEC_BCC"]);
            //SQLHelper.AddParamToSQLCmd(cmd, "@CEC_Body", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["CEC_Body"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CEC_Id", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["CEC_Id"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        #endregion

        #region Product
        public DataTable GetAllProductProjectWise()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Get_ProductBy_Project");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetAllProductProjectWiseByID(int Product_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Get_ProductBy_ProjectID");
            SQLHelper.AddParamToSQLCmd(cmd, "@Product_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Product_Id);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int InsertProduct(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Insert_Product");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Productname", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, Htparam["Productname"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBY", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public int UpdateProduct(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Update_Product");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Productname", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, Htparam["Productname"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IPM_Id", System.Data.SqlDbType.BigInt, 2000, System.Data.ParameterDirection.Input, Htparam["IPM_Id"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBY", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        #endregion

        public DataTable EmailDetailsByInvoiceID(int Invoiceid)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Get_EmailDetailsByInvoiceID");
            SQLHelper.AddParamToSQLCmd(cmd, "@Invoice_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Invoiceid);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetBillingDetailsforReport()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetBillingDetailsforReport");
            //SQLHelper.AddParamToSQLCmd(cmd, "@Invoice_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Invoiceid);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetBillingDetailsforReportTest(int DomainId, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetBillingDetailsforReportNew");
            SQLHelper.AddParamToSQLCmd(cmd, "@DomainId", System.Data.SqlDbType.Int, 10, System.Data.ParameterDirection.Input, DomainId);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetBillingDetailsforReportPeriodWise(string Month, int Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetBillingDetailsforReportPeriodWise");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Year);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        #region FinalStatus
        public DataTable GetAllFinalStatusProjectWise()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Get_FinalStatusBy_Project");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetAllFinalStatusProjectWiseByID(int Product_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Get_FinalStatusBy_ProjectID");
            SQLHelper.AddParamToSQLCmd(cmd, "@FinalStatus_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Product_Id);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int InsertFinalStatus(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Insert_FinalStatus");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@StatusName", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, Htparam["StatusName"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBY", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public int UpdateFinalStatus(Hashtable Htparam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_update_FinalStatus");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, Htparam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@StatusName", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, Htparam["StatusName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectStatusMasterId", System.Data.SqlDbType.BigInt, 2000, System.Data.ParameterDirection.Input, Htparam["ProjectStatusMasterId"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBY", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Htparam["Added_By"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public DataTable BindFinalStatusprojectWise(string Project_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetFinalStatusProjectWise");
            SQLHelper.AddParamToSQLCmd(cmd, "@Project_Id", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Project_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        #endregion
        #endregion

        public DataTable ViewprocostdetailsHistory(string Project_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetNewprocostdetailsHistory");
            SQLHelper.AddParamToSQLCmd(cmd, "@Project_Id", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Project_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable BindProductTypeprojectWise(string Project_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetProductTypeProjectWise");
            SQLHelper.AddParamToSQLCmd(cmd, "@Project_Id", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Project_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable getalldomains(int EmployeeId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetAlldomainsForBilling");
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeID", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, EmployeeId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable GetAllProjectByDomainWise(int DomainId, int EmployeeId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "WBT_usp_GetAllProjectByDomainWiseForBilling");
            SQLHelper.AddParamToSQLCmd(cmd, "@DomainID", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, DomainId);
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, EmployeeId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable BindProjectApprovalDetails(int Project_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_BindProjectApprovalDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Project_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int InsertbillingcostApproval(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_ApproveCosting_MasterDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_DomainID", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_DomainID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_ProjectID", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_ForNewOrders", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_ForNewOrders"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_ForNewOrdersRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_ForNewOrdersRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_ExpectedOrder", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_ExpectedOrder"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_ExpectedOrderRemark", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_ExpectedOrderRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_AdditionalChargesCharge1025", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_AdditionalChargesCharge1025"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_AdditionalCharges1025Remark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_AdditionalCharges1025Remark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_AdditionalCharge1025Type", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_AdditionalCharge1025Type"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_FlowOfOrders", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_FlowOfOrders"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_FlowOfOrdersRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_FlowOfOrdersRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_DeliveryChecklist", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_DeliveryChecklist"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_DeliveryChecklistRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_DeliveryChecklistRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_AdditionalAddendum", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_AdditionalAddendum"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_AdditionalAddendumRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_AdditionalAddendumRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_RushOrders", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_RushOrders"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_RushOrdersType", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_RushOrdersType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_RushOrdersRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_RushOrdersRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_updateBillable", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_updateBillable"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_updateBillableType", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_updateBillableType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_updateBillableRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_updateBillableRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_WeekendBillable", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_WeekendBillable"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_WeekendBillableType", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_WeekendBillableType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_WeekendBillableRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_WeekendBillableRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_LenderwiseBilling", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_LenderwiseBilling"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_LenderwiseBillingRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_LenderwiseBillingRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_ExtendedHoursBillable", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_ExtendedHoursBillable"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_ExtendedHoursBillableRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_ExtendedHoursBillableRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_ClientFeedbaclBillable", SqlDbType.NVarChar, 100, ParameterDirection.Input, htParam["CM_ClientFeedbaclBillable"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_ClientFeedbaclBillableRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_ClientFeedbaclBillableRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CM_CostingRemark", SqlDbType.NVarChar, 500, ParameterDirection.Input, htParam["CM_CostingRemark"]);


            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);

            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
        public DataTable BindVerifyProject()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetProjectOverAllDetails");
            //SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Project_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable BindProjectForPriceDetails()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetProjectDetailsForPriceReport");
            //SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, Project_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable BindVerifyProjectById(int ProjectId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetProjectOverAllDetails_ProjectId");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable BindVerifyProjectForModifiedRates()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetProjectOverAllDetails_ModifiedValues");
            //SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable getallMarketingEmployee()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetAllMarketingEmployee");
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public int InsertProjectApprovalRequest(Hashtable InsertprojectApproval)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertProjectApprovalInfo");
            SQLHelper.AddParamToSQLCmd(cmd, "@Company_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, InsertprojectApproval["Company_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Domain_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, InsertprojectApproval["Domain_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["ProjectName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Company_Name", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["Company_Name"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Contact_Person", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["Contact_Person"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Phonenumber", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["Phonenumber"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@EmailId", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["Email-Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Url", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, InsertprojectApproval["Url"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Address", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, InsertprojectApproval["Address"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Result", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, InsertprojectApproval["Result"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy ", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, InsertprojectApproval["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["ProcessName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Type", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, InsertprojectApproval["Type"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Dealwise", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, InsertprojectApproval["Dealwise"]);
            //SQLHelper.AddParamToSQLCmd(cmd, "@PAI_ClientProcess", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, InsertprojectApproval["ClientProcess"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
        public int UpdateProjectApprovalRequest(Hashtable InsertprojectApproval)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_UpdateProject_ApprovalInfo");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectApproval_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, InsertprojectApproval["ProjectApproval_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Company_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, InsertprojectApproval["Company_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["ProjectName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Company_Name", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["Company_Name"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Contact_Person", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["Contact_Person"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Phonenumber", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["Phonenumber"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@EmailId", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, InsertprojectApproval["Email-Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Url", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, InsertprojectApproval["Url"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Address", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, InsertprojectApproval["Address"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Result", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, InsertprojectApproval["Result"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@UpdatedBY", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, InsertprojectApproval["UpdatedBY"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ERPProjectId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, InsertprojectApproval["ERPProjectId"]);
            //SQLHelper.AddParamToSQLCmd(cmd, "@PAI_ClientProcess", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, InsertprojectApproval["ClientProcess"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
        public DataTable ViewAllProjectApp()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetAllProjectDetails_UW]");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetProjectApprovalInformation(int ApprovalId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Billing_GetProjectApprovalInformation");
            SQLHelper.AddParamToSQLCmd(cmd, "@PAI_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ApprovalId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetbyProjectApproval_IdReq(int ProjectApproval_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Billing_GetProjectDetailsByID");
            SQLHelper.AddParamToSQLCmd(cmd, "@PAI_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectApproval_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public int InsertSalesInformation(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_BillingInsertSalesInformation");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectApproval_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["ProjectApproval_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["ProcessName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["ProjectName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@BDM", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["BDM"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@RequestedDate", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["RequestedDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ScopeOfProject", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["ScopeOfProject"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@NDASigned", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["NDASigned"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@DateOfNDAAgreement", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["DateOfNDAAgreement"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ExpirationDateofNDAAgreement", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["ExpirationDateofNDAAgreement"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@NDASignedByClient", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["NDASignedByClient"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@NDASignedBYInfinity", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["NDASignedBYInfinity"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@SLASigned", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["SLASigned"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@DateOfSLAAgreement", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["DateOfSLAAgreement"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ExpirationDateofSLAAgreement", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["ExpirationDateofSLAAgreement"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@SLASignedByClient", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["SLASignedByClient"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@SLASignedByInfinity", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["SLASignedByInfinity"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectStatus", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["ProjectStatus"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ExpectedVolume", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["ExpectedVolume"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ExpectedStartDate", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["ExpectedStartDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectDuration", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["ProjectDuration"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Remark", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["Remark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@LogoPathMSA", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["LogoPathMSA"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@LogoPathNDA", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["LogoPathNDA"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@StoppedDate", System.Data.SqlDbType.NVarChar, 12, System.Data.ParameterDirection.Input, htParam["StoppedDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@StoppedRemark", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["StoppedRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@RateRevisionDate", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["RateRevisionDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ERPProjectId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["ERPProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ScopeDocument", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["ScopeDocument"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public int InsertBillParameterUW(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_BillingInsertParametersUW_Revised]");
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_ParameterId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["IBV_ParameterId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_Comment", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_Comment"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_Additional", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_Additional"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_UWVerification", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_UWVerification"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_Remark", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_Remark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_ChargeType", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_ChargeType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_CommentFromBDM", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_CommentFromBDM"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["ProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_isBundle", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["IBV_isBundle"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ClientProcess", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["ClientProcess"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CostingColumn", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["CostingColumn"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }



        public int InsertBillParameter(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_BillingInsertParameters");
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_ParameterId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["IBV_ParameterId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_Comment", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_Comment"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_Additional", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_Additional"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_Remark", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_Remark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_ChargeType", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_ChargeType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_CommentFromBDM", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_CommentFromBDM"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["ProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
        public int UpdateSalesInformation(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_BillingInsertSalesInformation");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectApproval_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["ProjectApproval_Id"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["ProcessName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["ProjectName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@BDM", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["BDM"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@RequestedDate", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["RequestedDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ScopeOfProject", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["ScopeOfProject"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@NDASigned", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["NDASigned"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@DateOfNDAAgreement", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["DateOfNDAAgreement"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ExpirationDateofNDAAgreement", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["ExpirationDateofNDAAgreement"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@NDASignedByClient", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["NDASignedByClient"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@NDASignedBYInfinity", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["NDASignedBYInfinity"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@SLASigned", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["SLASigned"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@DateOfSLAAgreement", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["DateOfSLAAgreement"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ExpirationDateofSLAAgreement", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["ExpirationDateofSLAAgreement"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@SLASignedByClient", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["SLASignedByClient"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@SLASignedByInfinity", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, htParam["SLASignedByInfinity"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectStatus", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["ProjectStatus"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ExpectedVolume", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["ExpectedVolume"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ExpectedStartDate", System.Data.SqlDbType.Date, 12, System.Data.ParameterDirection.Input, htParam["ExpectedStartDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectDuration", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["ProjectDuration"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Remark", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["Remark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@LogoPathMSA", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["LogoPathMSA"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@LogoPathNDA", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["LogoPathNDA"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@StoppedDate", System.Data.SqlDbType.NVarChar, 12, System.Data.ParameterDirection.Input, htParam["StoppedDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@StoppedRemark", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["StoppedRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@RateRevisionDate", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["RateRevisionDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ERPProjectId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["ERPProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ScopeDocument", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["ScopeDocument"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
        public DataTable GetBiilingValuesDomainwise(int ProjectApproval_Id)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetBillingValuesForDomain");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectApproval_Id", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectApproval_Id);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        #region SummaryReport
        public DataTable ViewAllProjectGroup()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetProjectGroups");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable ViewAllGroupProjects(string Groupname)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetGroupsProjectname ");
            SQLHelper.AddParamToSQLCmd(cmd, "@GroupName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Groupname);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        #endregion
        public DataTable GetBillingDetailsCount(string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetBillingDetailsforReportCount");
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int ApproveBillParameter(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_ApproveBillingParameters");
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_ParameterId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["IBV_ParameterId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_Comment", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_Comment"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_Additional", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_Additional"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@IBV_Remark", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["IBV_Remark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["ProjectId"]);

            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
        public DataSet BindTestOrders(int DomainId, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetBillingDetailsforReportTest");
            SQLHelper.AddParamToSQLCmd(cmd, "@DomainId", System.Data.SqlDbType.Int, 10, System.Data.ParameterDirection.Input, DomainId);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);

            DataSet dt = SQLHelper.ExecuteDataSetCmd_Billing(cmd);
            return dt;
        }
        public DataTable BindSummaryReport(string GroupName, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "WBT_usp_GetAllGroupSummaryReportForBinding");
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@GroupName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, GroupName);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int InsertGroupAttachmentPath(string GroupName, string BillingPeriod, string strAttachmentPath)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_UpdateGroupWisepath_UW");
            SQLHelper.AddParamToSQLCmd(cmd, "@GroupName", System.Data.SqlDbType.NVarChar, 400, System.Data.ParameterDirection.Input, GroupName);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceAttachmentsPDF", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, strAttachmentPath);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 400, System.Data.ParameterDirection.Input, BillingPeriod);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public DataTable GetSummaryReportAttachments(string GroupName, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetGroupWisepath");
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@GroupName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, GroupName);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetSummaryReportAttachments(int ProjectID, string BillingPeriod, string Process)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_GetGroupWisepath_Processwise");
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@Process", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, Process);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }


        public DataTable GetSummaryReportInvoice(string GroupName, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[WBT_usp_GetAllGroupwiseInvoiceNumber_UW]");
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@GroupName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, GroupName);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetPriceDetailsByProjectId(int ProjectId, int DomainId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_getallbillingParametersForCostingForCM");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 10, System.Data.ParameterDirection.Input, ProjectId);
            SQLHelper.AddParamToSQLCmd(cmd, "@DomainId", System.Data.SqlDbType.Int, 10, System.Data.ParameterDirection.Input, DomainId);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;

        }

        public string GetProjectNamebyID(int ProjectId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GteProjectNamebyID");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 10, System.Data.ParameterDirection.Input, ProjectId);

            string dt = (string)SQLHelper.ExecuteScalarCmdBilling(cmd);
            return dt;

        }

        public DataTable GetProjectDetailsToSendToProductionTeam(int ProjectId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetProjectDetailsToSendToProductionTeam]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 10, System.Data.ParameterDirection.Input, ProjectId);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;

        }

        public DataTable GetClientDetails(string GroupName, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetClientInformation");
            SQLHelper.AddParamToSQLCmd(cmd, "@GroupName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, GroupName);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetClientDetails(int ProjectID, string BillingPeriod)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetClientInformation_byProjectId");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetUWProcess(int ProjectId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetUWProcess1");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ProjectId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetAllProjectSendToAccountsDetailsBasedonDomain(int ProjectID, string BillingPeriod, int DomainId, bool Refresh)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[WBT_usp_GetTrackingsheetDetailsSendtoAccountDomain_Local_UW_Revised_Slot_Canopy_Revised]");
            SQLHelper.AddParamToSQLCmd(cmd, "@DomainID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, DomainId);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@Refresh", System.Data.SqlDbType.Bit, 10, System.Data.ParameterDirection.Input, Refresh);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataSet GetTotalProjectAmount_UW(int ProjectID, string BillingPeriod, string ProcessName, string Slot)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetProjectCost3_HighestFirst_UW]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProcessName);
            SQLHelper.AddParamToSQLCmd(cmd, "@Slot", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Slot);

            DataSet dt = SQLHelper.ExecuteDataSetCmd_Billing(cmd);
            return dt;
        }
        public int VerifyBDMOrders(int ProjectID, string BillingPeriod, string ProcessName, string Slot)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_VerifyBDMOrders]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProcessName);
            SQLHelper.AddParamToSQLCmd(cmd, "@Slot", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Slot);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.Int, 100, System.Data.ParameterDirection.Input, int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public int GetBillingPeriodVerifiedStatus(int ProjectID, string BillingPeriod, string ProcessName)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getVerifiedStatus]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ProcessName);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public DataTable GetSentToClientList()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getAllSentToClientList_Revised_1]");//usp_getAllSentToClientList_Revised
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetClientwiseDashboard()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getClientwiseDashBoard_ForUpdate]");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetClientwiseDashboard_New()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getClientwiseDashBoard_ERPData_New_Summary]");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetClientwiseDashboard_New_Client()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getClientwiseDashBoard_ERPData_New]");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetClientwiseDashboard_New_Client_RL()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getClientwiseDashBoard_ERPData_New_RL]");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetClientwiseDashboard_New_Client_SS()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getClientwiseDashBoard_ERPData_New_Securitization]");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetClientwiseDashboard_ERPData()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getClientwiseDashBoard_ERPData]");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetClientwiseDashboard_DifferencewithQuickbook()
        {
            //SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getClientwiseDashBoard_DifferencewithQuickobook_Remark_BillNo_Revised_YTD]"); //usp_getClientwiseDashBoard_DifferencewithQuickobook_Remark_BillNo_Revised
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getClientwiseDashBoard_DifferencewithQuickobook_Remark_BillNo_Revised_YTD_Reivew]"); //usp_getClientwiseDashBoard_DifferencewithQuickobook_Remark_BillNo_Revised
            
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetClientwiseDashboard_DifferencewithQuickbookRL()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getClientwiseDashBoard_DifferencewithQuickobook_Remark_BillNo_Revised_YTD_RL_1]"); //usp_getClientwiseDashBoard_DifferencewithQuickobook_Remark_BillNo_Revised
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int CheckSlotPassing(int ProjectId, string BillingPeriod, string ProcessName)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetSlotPassing");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 50, System.Data.ParameterDirection.Input, ProjectId);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, ProcessName);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue; //-1=User Not Exist, 0=Invalid Password, >0=Success

        }
        public int InsertGroupAttachmentPath_PDf(string GroupName, string BillingPeriod, string strAttachmentPath)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_UpdateGroupWisepath_UW_PDF");
            SQLHelper.AddParamToSQLCmd(cmd, "@GroupName", System.Data.SqlDbType.NVarChar, 400, System.Data.ParameterDirection.Input, GroupName);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceAttachmentsPDF", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, strAttachmentPath);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 400, System.Data.ParameterDirection.Input, BillingPeriod);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public int InsertGroupAttachmentPath_PDf_Before(string GroupName, string BillingPeriod, string strAttachmentPath, string InvoiceNumber)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "IB_usp_UpdateGroupWisepath_UW_PDF_Before");
            SQLHelper.AddParamToSQLCmd(cmd, "@GroupName", System.Data.SqlDbType.NVarChar, 400, System.Data.ParameterDirection.Input, GroupName);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceAttachmentsPDF", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, strAttachmentPath);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 400, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceNumber", System.Data.SqlDbType.NVarChar, 400, System.Data.ParameterDirection.Input, InvoiceNumber);

            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public DataTable GetDataForInvoiceExcel(int ProjectID, string BillingPeriod, string ProcessName, string Slot)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetProjectCost3_HighestFirst_UW]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProcessName);
            SQLHelper.AddParamToSQLCmd(cmd, "@Slot", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Slot);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataSet GetDataForInvoiceExcel_DS(int ProjectID, string BillingPeriod, string ProcessName, string Slot)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetProjectCost3_HighestFirst_UW]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProcessName);
            SQLHelper.AddParamToSQLCmd(cmd, "@Slot", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Slot);

            DataSet dt = SQLHelper.ExecuteDataSetCmd_Billing(cmd);
            return dt;
        }

        public DataSet GetDataForNewExcelFormat(int ProjectID, string BillingPeriod, string ProcessName)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getPCQCSummary_RevisedForExcel]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProcessName);

            DataSet dt = SQLHelper.ExecuteDataSetCmd_Billing(cmd);
            return dt;
        }

        public DataTable getBillingReport(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_PullCanopyData]");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ToDate);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetExcelDataForExportSecondSheet(int ProjectID, string BillingPeriod, string ProcessName)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetLoanDetails_ForExcel]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProcessName);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }


        public int InsertUWProcess(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertUWProcess_Revised");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["ProcessName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, htParam["ProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDealNo", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["FromDealNo"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDealNo", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["ToDealNo"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public DataTable getAllBillingParameters(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetAllConfiguredParameters]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, htParam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ClientProcess", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["ClientProcess"]);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable getAllBillingParameters_RateRevision(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetAllConfiguredParameters_RateRevision]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, htParam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ClientProcess", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, htParam["ClientProcess"]);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable getProjectDetailsByprocessID(int ProcessID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetProjectInfofromProcessID]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessID", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, ProcessID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetAllDirectBilledInvoices()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getAlldirectbilling]");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public int InsertDirectBilling(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertDirectBilling_Revised");
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["BillingPeriod"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingAddedBy", System.Data.SqlDbType.BigInt, 100, System.Data.ParameterDirection.Input, htParam["BillingAddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Name", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["Name"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvocieDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["InvoiceDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Attachment", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["Attachment"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ClientName", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["ClientName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@TotalFiles", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["TotalFiles"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@TotalAmount", System.Data.SqlDbType.Decimal, 10, System.Data.ParameterDirection.Input, htParam["TotalAmount"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@SecRel", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["SecRel"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ThirdPartyRevenue", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["ThirdPartyRevenue"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@SecLoanCount", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["SecLoanCount"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@SecAmount", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["SecAmount"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@RelLoanCount", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["RelLoanCount"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@RelAmount", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["RelAmount"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 100, System.Data.ParameterDirection.Input, htParam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public DataTable GetAllProjectBilingReport(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getAllProjectBillingReport]");
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ToDate);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable getDeviationReport(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getMainDeviationReport]");
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ToDate);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable getDeviationReport_Detailed(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getMainDeviationReport_Detailed]");
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ToDate);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable getDeviationReportNonDD(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getMainDeviationReportNonDD]");
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ToDate);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable getDeviationReport_DetailedNonDD(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getMainDeviationReport_DetailedNonDD]");
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ToDate);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable getDeviationReport_Canopy(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getMainDeviationReport_Canopy]");
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ToDate);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable getDeviationReport_DetailedCanopy(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getMainDeviationReport_DetailedNonDD_Canopy]");
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, ToDate);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetBillingDifference(string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetAutovsManualReport_Revised]");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Year);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataSet GetCostPerRecordReport(string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[GetCostPerRecordReport_NewERP]");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Year);

            DataSet dt = SQLHelper.ExecuteDataSetCmd(cmd);
            return dt;
        }

        public DataTable GetMergedBillingFor561(string Month, string Year, int ProjectID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getMergedBillingFor561]");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Year);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 100, System.Data.ParameterDirection.Input, ProjectID);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetDirectBillingAttachment(int TrackingSheetID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getDirectBillingAttachment]");
            SQLHelper.AddParamToSQLCmd(cmd, "@TrackingSheetID", System.Data.SqlDbType.Int, 100, System.Data.ParameterDirection.Input, TrackingSheetID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetYettobeBilled()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetYetToBeBilled");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetYettobeBilled_Revised()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetYetToBeBilled_Revised");
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetInvoicePreview(int ProjectID, string ProcessName, string BillingPeriod, string TotalCost)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "GetInvoicePReviwDetails_Sec_Rev");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessName", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, ProcessName);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingPeriod", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, BillingPeriod);
            SQLHelper.AddParamToSQLCmd(cmd, "@TotalCost", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, TotalCost);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetAllClientForAddInvoice()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetAllClientsByRights]");
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            //SQLHelper.AddParamToSQLCmd(cmd, "@text", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, "");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetAllClientForAddInvoice_Onshore()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetAllClientsByRights_Onshore]");
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeID", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            //SQLHelper.AddParamToSQLCmd(cmd, "@text", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, "");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable getPendingBillingForpayingEntity(string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_getPendingDealsForpayingEntity]");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Year);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public int UpdatePayingEntity(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_UpdatePayingEntity");
            SQLHelper.AddParamToSQLCmd(cmd, "@DealNo", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["DealNo"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Client", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["Client"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@PurchaseEntity", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["PurchaseEntity"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@PayEntity", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["PayingEntity"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 100, System.Data.ParameterDirection.Input, int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public DataTable PullCanopyData(string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_PullCanopyData]");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Year);

            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetBillingSummary(string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetSummaryReportAll]");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Year);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }


        public DataTable GetBillToMaster()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetBillToMaster]");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public DataTable GetCanopyData(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetCanopyData");
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDate", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDate", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, ToDate);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetCanopyDataDetails(string FromDate, string ToDate)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetCanopyLoanandTaskkDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@FromDate", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, FromDate);
            SQLHelper.AddParamToSQLCmd(cmd, "@ToDate", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, ToDate);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetCanopyTask(int LoanID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetCanopyTaskDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@LoanId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, LoanID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public int UpdateQuickbookData(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_UpdateQuickbookData");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 100, System.Data.ParameterDirection.Input, htParam["ProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Process", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Process"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Month"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Year"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Type", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Type"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@LoanCount", System.Data.SqlDbType.Int, 100, System.Data.ParameterDirection.Input, htParam["LoanCount"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Amount", System.Data.SqlDbType.Decimal, 100, System.Data.ParameterDirection.Input, htParam["Amount"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 100, System.Data.ParameterDirection.Input, int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public int InsertReconciliationRemark(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertReconciliationRemark");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 100, System.Data.ParameterDirection.Input, htParam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Month"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Year"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Process", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Process"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Remark", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["Remark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 100, System.Data.ParameterDirection.Input, int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
        public int InsertReconciliationAttachment(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertReconciliationAttachment");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 100, System.Data.ParameterDirection.Input, htParam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Month"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Year"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Process", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Process"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Attachment", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["Attachment"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 100, System.Data.ParameterDirection.Input, int.Parse(HttpContext.Current.User.Identity.Name.ToString()));
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public DataTable GetClientProcesswiseUploadedDocument(int ProjectID, string Month, string Year, string Process)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_GetClientProcesswiseUploadedDocument]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 100, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Year);
            SQLHelper.AddParamToSQLCmd(cmd, "@Process", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Process);

            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int InsertBillingHeader(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "[usp_InsertParameterandBillingHeader]");
            SQLHelper.AddParamToSQLCmd(cmd, "@ParameterName", System.Data.SqlDbType.NVarChar, 4000, System.Data.ParameterDirection.Input, htParam["ParameterName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public DataTable GetBillingHeaderValues()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetAllBillingParametersForDisplay");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }

        public int InsertRateRevisionConfiguration(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertRateRevisionConfiguration");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, htParam["ProjectID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProcessID", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, htParam["ProcessID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ParameterID", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, htParam["ParameterID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@CurrentRate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["CurrentRate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@EffectiveDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["EffectiveDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@RevisionType", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["RevisionType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReminderDate", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["ReminderDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReminderDays", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["ReminderDays"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@EmailList", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["EmailList"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
        public int UpdateReminerEmailList(int ProjectID, string EmailList)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_UpdateEmailsList");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, ProjectID);
            SQLHelper.AddParamToSQLCmd(cmd, "@EmailList", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, EmailList);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }

        public string GetPassword(string Username)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetEmailPassword");
            SQLHelper.AddParamToSQLCmd(cmd, "@Username", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Username);
            string Password = (string)SQLHelper.ExecuteScalarCmd(cmd);
            return Password;
        }
        public DataTable GetRLSecCostByProject(int ProjectID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_getRLSecCostByProjectID");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectID", System.Data.SqlDbType.Int, 100, System.Data.ParameterDirection.Input, ProjectID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public DataTable GetAllOtherCosting()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetAllOtherCosting_Client");
            DataTable dt = SQLHelper.ExecuteDataTableCmd_billing(cmd);
            return dt;
        }
        public int InsertRates(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertOtherCosting_Log");
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, htParam["ProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Rate", System.Data.SqlDbType.Decimal, 10, System.Data.ParameterDirection.Input, htParam["Rate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Type ", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["Type"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy ", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmdBilling(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            return ReturnValue;
        }
    }
}
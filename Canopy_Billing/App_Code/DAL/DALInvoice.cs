using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Vendor_Portal.App_Code.DAL
{
    public class dalInvoice
    {

        #region Infinity
        public DataTable GetInfinityInvoiceDetails(int EmployeeId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetInfinity_InvoiceDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeId", System.Data.SqlDbType.BigInt, 10, System.Data.ParameterDirection.Input, EmployeeId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public int Insert_Infinity_InvoiceDetails(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_Insert_Infinity_InvoiceDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["Month"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["Year"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@StatementDate", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["StatementDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@DueDate", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["DueDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Domain", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["Domain"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Currency", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["Currency"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@NoOfLoans", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["NoOfLoans"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorInvoiceNumber", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["VendorInvoiceNumber"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@TotalDue", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, htParam["TotalDue"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["VendorName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceType", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, htParam["InvoiceType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@FilePath", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["FilePath"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ProjectId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, htParam["ProjectId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Delay", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["Delay"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Remark", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["Remark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmd(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public int InsertInvoiceDetails(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertInvoiceDetails");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, htParam["InvoiceID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingType", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, htParam["BillingType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Frequency", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, htParam["Frequency"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@StatementDate", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["StatementDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorAccountNumber", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["VendorAccountNumber"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorInvoiceNumber", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["VendorInvoiceNumber"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@TotalDue", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, htParam["TotalDue"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["VendorName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorAddress", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["VendorAddress"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Type", System.Data.SqlDbType.NVarChar, 10, System.Data.ParameterDirection.Input, htParam["Type"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceType", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, htParam["InvoiceType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@FilePath", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["FilePath"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@LoanFilePath", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["LoanFilePath"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmd(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public int DeleteScienna(string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_DeleteScienna");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, Year);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmd(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public int DeleteSciennaLabour(string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_DeleteSciennaLabour");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, Year);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmd(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public DataTable GetSciennaDetailsAfterImport(int InvoiceID, string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetSciennaDetailsAfterImport");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, InvoiceID);
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 30, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 30, System.Data.ParameterDirection.Input, Year);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetSciennaLaborDetailsAfterImport(int InvoiceID, string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetSciennaLaborDetailsAfterImport");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, InvoiceID);
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 30, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 30, System.Data.ParameterDirection.Input, Year);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable GetSciennaLoanDetailsAfterImport()
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetSciennaLoanDetailsAfterImport_1");
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        #endregion Infinity

        #region Canopy
        public int DeleteStewartLoan(string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_DeleteAVMLoan");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, Year);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmd(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }
        public int InsertInvoiceDetails_Canopy(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_InsertInvoiceDetails_Canopy");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, htParam["InvoiceID"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingType", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, htParam["BillingType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Frequency", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, htParam["Frequency"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@StatementDate", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["StatementDate"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorAccountNumber", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["VendorAccountNumber"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorInvoiceNumber", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["VendorInvoiceNumber"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@TotalDue", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, htParam["TotalDue"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, htParam["VendorName"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@VendorAddress", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, htParam["VendorAddress"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@Type", System.Data.SqlDbType.NVarChar, 10, System.Data.ParameterDirection.Input, htParam["Type"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceType", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, htParam["InvoiceType"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@FilePath", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["FilePath"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@LoanFilePath", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["LoanFilePath"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmd(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        public DataTable GetStewartDetailsForVerify(int InvoiceID, string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetStewartIADetails_ForVerify");
            SQLHelper.AddParamToSQLCmd(cmd, "@InvoiceID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, InvoiceID);
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 30, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 30, System.Data.ParameterDirection.Input, Year);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetInfinityInvoiceDetails_Canopy(int EmployeeId)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetInfinity_InvoiceDetails_Approval_canopy");
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, EmployeeId);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetStewartDataForReconcile(string Month, string Year)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetStewartIADetails_ForVerify_InvoiceApproval_1");
            SQLHelper.AddParamToSQLCmd(cmd, "@Month", System.Data.SqlDbType.NVarChar, 30, System.Data.ParameterDirection.Input, Month);
            SQLHelper.AddParamToSQLCmd(cmd, "@Year", System.Data.SqlDbType.NVarChar, 30, System.Data.ParameterDirection.Input, Year);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public int VerifyLoanNo(Hashtable htParam)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_VerifyLoanNo_Stewart_1");
            SQLHelper.AddParamToSQLCmd(cmd, "@BillingId", System.Data.SqlDbType.NVarChar, 400, System.Data.ParameterDirection.Input, htParam["BillingId"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@DisputeValue", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["DisputeValue"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@UserRemark", System.Data.SqlDbType.NVarChar, 5000, System.Data.ParameterDirection.Input, htParam["UserRemark"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@AddedBy", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, htParam["AddedBy"]);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmd(cmd);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue;
        }

        #endregion Canopy
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Vendor_Portal.App_Code.DAL;

namespace Vendor_Portal.App_Code.BLL
{
    public class bllInvoice
    {
        dalInvoice dalInvoice = new dalInvoice();

        #region Infinity
        public DataTable GetInfinityInvoiceDetails(int EmployeeId)
        {
            return dalInvoice.GetInfinityInvoiceDetails(EmployeeId);
        }
        public int Insert_Infinity_InvoiceDetails(Hashtable htParam)
        {
            return dalInvoice.Insert_Infinity_InvoiceDetails(htParam);
        }
        public int InsertInvoiceDetails(Hashtable htParam)
        {
            return dalInvoice.InsertInvoiceDetails(htParam);
        }
        public int DeleteScienna(string Month, string Year)
        {
            return dalInvoice.DeleteScienna(Month, Year);
        }
        public int DeleteSciennaLabour(string Month, string Year)
        {
            return dalInvoice.DeleteSciennaLabour(Month, Year);
        }
        public DataTable GetSciennaDetailsAfterImport(int InvoiceID, string Month, string Year)
        {
            return dalInvoice.GetSciennaDetailsAfterImport(InvoiceID, Month, Year);
        }
        public DataTable GetSciennaLaborDetailsAfterImport(int InvoiceID, string Month, string Year)
        {
            return dalInvoice.GetSciennaLaborDetailsAfterImport(InvoiceID, Month, Year);
        }
        public DataTable GetSciennaLoanDetailsAfterImport()
        {
            return dalInvoice.GetSciennaLoanDetailsAfterImport();
        }

        #endregion Infinity

        #region Canopy
        public int DeleteStewartLoan(string Month, string Year)
        {
            return dalInvoice.DeleteStewartLoan(Month, Year);
        }
        public int InsertInvoiceDetails_Canopy(Hashtable htParam)
        {
            return dalInvoice.InsertInvoiceDetails_Canopy(htParam);
        }
        public DataTable GetStewartDetailsForVerify(int InvoiceID, string Month, string Year)
        {
            return dalInvoice.GetStewartDetailsForVerify(InvoiceID, Month, Year);
        }
        public DataTable GetInfinityInvoiceDetails_Canopy(int EmployeeId)
        {
            return dalInvoice.GetInfinityInvoiceDetails_Canopy(EmployeeId);
        }
        public DataTable GetStewartDataForReconcile(string Month, string Year)
        {
            return dalInvoice.GetStewartDataForReconcile(Month, Year);
        }
        public int VerifyLoanNo(Hashtable htParam)
        {
            return dalInvoice.VerifyLoanNo(htParam);
        }


        #endregion Canopy
    }
}
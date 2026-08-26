IF OBJECT_ID('dbo.usp_RLInvoice_GetBySentStatus', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RLInvoice_GetBySentStatus;
GO
CREATE PROCEDURE dbo.usp_RLInvoice_GetBySentStatus @IsSent BIT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.InvoiceID, r.OurClient, r.Recipient, r.TradeName, r.InvoiceDate,
           r.Document, r.TM, r.Docusign, r.DocumentDate, r.ExecutedDate,
           r.LoanCount,
           CASE
               WHEN ISNULL(r.Cost, 0) = 0
                    AND TRY_CONVERT(decimal(18,4), r.LoanCount) <> 0
               THEN r.ExpectedBilling / TRY_CONVERT(decimal(18,4), r.LoanCount)
               ELSE r.Cost
           END AS Cost,
           r.ExpectedBilling, r.BillingEntity, r.SubmittedforInvoice,
           r.InvoiceIssued, r.Notes, r.FilePath, r.AddedBy, r.isVerify,
           r.VerifyRemark, r.VerifiedOn, r.ContactPerson, r.ContactEmail,
           r.ContactPhone, r.Address, s.SentBy, s.SentDateTime
    FROM dbo.RLinvoice r
    LEFT JOIN dbo.RLInvoiceSentToClient s ON s.RLInvoiceID=r.InvoiceID AND s.IsSent=1
    WHERE ((@IsSent=1 AND s.ID IS NOT NULL) OR (@IsSent=0 AND s.ID IS NULL))
      AND (@IsSent=1 OR ISNULL(r.isVerify,0)=1)
    ORDER BY r.InvoiceID DESC;
END;
GO

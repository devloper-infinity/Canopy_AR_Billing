/*
    Flexible Securitization / Reliance Letter costing
    SQL Server 2016 compatible and safe to run more than once.

    Existing rows remain valid: BillingMethod NULL keeps the existing Rate x LoanCount flow.
*/
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF COL_LENGTH('dbo.SecuritizationRelianceLetterCosting', 'BillingMethod') IS NULL
    ALTER TABLE dbo.SecuritizationRelianceLetterCosting ADD BillingMethod nvarchar(20) NULL;
IF COL_LENGTH('dbo.SecuritizationRelianceLetterCosting', 'MinimumAmount') IS NULL
    ALTER TABLE dbo.SecuritizationRelianceLetterCosting ADD MinimumAmount decimal(18,2) NULL;
IF COL_LENGTH('dbo.SecuritizationRelianceLetterCosting', 'MaximumCap') IS NULL
    ALTER TABLE dbo.SecuritizationRelianceLetterCosting ADD MaximumCap decimal(18,2) NULL;
IF COL_LENGTH('dbo.SecuritizationRelianceLetterCosting', 'EffectiveFrom') IS NULL
    ALTER TABLE dbo.SecuritizationRelianceLetterCosting ADD EffectiveFrom date NULL;
IF COL_LENGTH('dbo.SecuritizationRelianceLetterCosting', 'IsActive') IS NULL
    ALTER TABLE dbo.SecuritizationRelianceLetterCosting ADD IsActive bit NULL;

IF OBJECT_ID('dbo.SecuritizationRelianceLetterCostingDetail', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SecuritizationRelianceLetterCostingDetail
    (
        CostingDetailID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_SRL_CostingDetail PRIMARY KEY,
        RateID int NOT NULL,
        ProductType nvarchar(250) NOT NULL,
        Rate decimal(18,2) NOT NULL,
        EffectiveFrom date NULL,
        IsActive bit NOT NULL CONSTRAINT DF_SRL_CostingDetail_IsActive DEFAULT (1),
        AddedBy int NULL,
        AddedDate datetime NOT NULL CONSTRAINT DF_SRL_CostingDetail_AddedDate DEFAULT (GETDATE()),
        UpdatedBy int NULL,
        UpdatedDate datetime NULL,
        CONSTRAINT FK_SRL_CostingDetail_Header FOREIGN KEY (RateID)
            REFERENCES dbo.SecuritizationRelianceLetterCosting(RateID)
    );
    CREATE INDEX IX_SRL_CostingDetail_RateID ON dbo.SecuritizationRelianceLetterCostingDetail(RateID, IsActive);
END;

IF COL_LENGTH('dbo.RLinvoice', 'CostingRateID') IS NULL ALTER TABLE dbo.RLinvoice ADD CostingRateID int NULL;
IF COL_LENGTH('dbo.RLinvoice', 'BillingMethod') IS NULL ALTER TABLE dbo.RLinvoice ADD BillingMethod nvarchar(20) NULL;
IF COL_LENGTH('dbo.RLinvoice', 'HoursWorked') IS NULL ALTER TABLE dbo.RLinvoice ADD HoursWorked decimal(18,2) NULL;
IF COL_LENGTH('dbo.RLinvoice', 'MinimumAmount') IS NULL ALTER TABLE dbo.RLinvoice ADD MinimumAmount decimal(18,2) NULL;
IF COL_LENGTH('dbo.RLinvoice', 'MaximumCap') IS NULL ALTER TABLE dbo.RLinvoice ADD MaximumCap decimal(18,2) NULL;
IF COL_LENGTH('dbo.RLinvoice', 'BaseAmount') IS NULL ALTER TABLE dbo.RLinvoice ADD BaseAmount decimal(18,2) NULL;
IF COL_LENGTH('dbo.RLinvoice', 'MinimumApplied') IS NULL ALTER TABLE dbo.RLinvoice ADD MinimumApplied bit NULL;
IF COL_LENGTH('dbo.RLinvoice', 'CapApplied') IS NULL ALTER TABLE dbo.RLinvoice ADD CapApplied bit NULL;

IF OBJECT_ID('dbo.RLInvoiceCostingDetail', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.RLInvoiceCostingDetail
    (
        InvoiceDetailID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_RLInvoiceCostingDetail PRIMARY KEY,
        InvoiceID int NOT NULL,
        CostingDetailID int NULL,
        BillingDescription nvarchar(250) NOT NULL,
        Quantity decimal(18,2) NOT NULL,
        Rate decimal(18,2) NOT NULL,
        Amount decimal(18,2) NOT NULL,
        CONSTRAINT FK_RLInvoiceCostingDetail_Invoice FOREIGN KEY (InvoiceID) REFERENCES dbo.RLinvoice(InvoiceID),
        CONSTRAINT FK_RLInvoiceCostingDetail_Config FOREIGN KEY (CostingDetailID) REFERENCES dbo.SecuritizationRelianceLetterCostingDetail(CostingDetailID)
    );
    CREATE INDEX IX_RLInvoiceCostingDetail_InvoiceID ON dbo.RLInvoiceCostingDetail(InvoiceID);
END;

COMMIT TRANSACTION;
GO

/* Run column-dependent statements in a new batch so SQL Server binds the new column correctly. */
SET XACT_ABORT ON;
BEGIN TRANSACTION;

UPDATE dbo.SecuritizationRelianceLetterCosting
SET IsActive = 1
WHERE IsActive IS NULL;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.SecuritizationRelianceLetterCosting')
      AND c.name = 'IsActive'
)
    ALTER TABLE dbo.SecuritizationRelianceLetterCosting
        ADD CONSTRAINT DF_SRL_Costing_IsActive DEFAULT (1) FOR IsActive;

ALTER TABLE dbo.SecuritizationRelianceLetterCosting
    ALTER COLUMN IsActive bit NOT NULL;

COMMIT TRANSACTION;
GO

IF OBJECT_ID('dbo.usp_RLSecCosting_GetForInvoice', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RLSecCosting_GetForInvoice;
GO
CREATE PROCEDURE dbo.usp_RLSecCosting_GetForInvoice
    @ProjectID int,
    @DocumentType nvarchar(200),
    @InvoiceDate date = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @AsOf date = ISNULL(@InvoiceDate, CAST(GETDATE() AS date));
    DECLARE @RateID int;

    SELECT TOP (1) @RateID = RateID
    FROM dbo.SecuritizationRelianceLetterCosting
    WHERE ProjectId = @ProjectID
      AND LTRIM(RTRIM(Type)) = LTRIM(RTRIM(@DocumentType))
      AND ISNULL(IsActive, 1) = 1
      AND (EffectiveFrom IS NULL OR EffectiveFrom <= @AsOf)
    ORDER BY ISNULL(EffectiveFrom, CONVERT(date,'19000101')) DESC,
             COALESCE(UpdatedDate, AddedDate) DESC, RateID DESC;

    SELECT RateID, ProjectId, Rate, Type, BillingMethod, MinimumAmount, MaximumCap,
           EffectiveFrom, ISNULL(IsActive,1) AS IsActive
    FROM dbo.SecuritizationRelianceLetterCosting WHERE RateID = @RateID;

    SELECT CostingDetailID, RateID, ProductType, Rate, EffectiveFrom, IsActive
    FROM dbo.SecuritizationRelianceLetterCostingDetail
    WHERE RateID = @RateID AND IsActive = 1
      AND (EffectiveFrom IS NULL OR EffectiveFrom <= @AsOf)
    ORDER BY ProductType, CostingDetailID;
END;
GO

IF OBJECT_ID('dbo.usp_RLSecCosting_SaveHeader', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RLSecCosting_SaveHeader;
GO
CREATE PROCEDURE dbo.usp_RLSecCosting_SaveHeader
    @RateID int = 0, @ProjectID int, @DocumentType nvarchar(200),
    @BillingMethod nvarchar(20) = NULL, @Rate decimal(18,2),
    @MinimumAmount decimal(18,2) = NULL, @MaximumCap decimal(18,2) = NULL,
    @EffectiveFrom date = NULL, @UserID int
AS
BEGIN
    SET NOCOUNT ON;
    IF @RateID = 0
    BEGIN
        SELECT TOP (1) @RateID = RateID
        FROM dbo.SecuritizationRelianceLetterCosting
        WHERE ProjectId=@ProjectID AND LTRIM(RTRIM(Type))=LTRIM(RTRIM(@DocumentType))
        ORDER BY RateID DESC;
    END;

    IF ISNULL(@RateID,0) = 0
    BEGIN
        INSERT dbo.SecuritizationRelianceLetterCosting
            (ProjectId, Rate, Type, BillingMethod, MinimumAmount, MaximumCap,
             EffectiveFrom, IsActive, AddedBy, AddedDate)
        VALUES
            (@ProjectID, @Rate, @DocumentType, @BillingMethod, @MinimumAmount, @MaximumCap,
             @EffectiveFrom, 1, @UserID, GETDATE());
        SET @RateID = SCOPE_IDENTITY();
    END
    ELSE
        UPDATE dbo.SecuritizationRelianceLetterCosting
        SET ProjectId=@ProjectID, Type=@DocumentType, BillingMethod=@BillingMethod, Rate=@Rate,
            MinimumAmount=@MinimumAmount, MaximumCap=@MaximumCap, EffectiveFrom=@EffectiveFrom, IsActive=1,
            UpdatedBy=@UserID, UpdatedDate=GETDATE()
        WHERE RateID=@RateID;

    INSERT dbo.OtherCosting_Log(RefID, ProjectID, Rate, Type, ActionType, ChangedBy)
    VALUES(@RateID, @ProjectID, @Rate, @DocumentType, 'CONFIGURATION SAVE', @UserID);

    SELECT @RateID AS RateID;
END;
GO

IF OBJECT_ID('dbo.usp_RLSecCosting_DeleteDetails', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RLSecCosting_DeleteDetails;
GO
CREATE PROCEDURE dbo.usp_RLSecCosting_DeleteDetails @RateID int
AS UPDATE dbo.SecuritizationRelianceLetterCostingDetail SET IsActive=0, UpdatedDate=GETDATE() WHERE RateID=@RateID AND IsActive=1;
GO

IF OBJECT_ID('dbo.usp_RLSecCosting_SaveDetail', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RLSecCosting_SaveDetail;
GO
CREATE PROCEDURE dbo.usp_RLSecCosting_SaveDetail
    @RateID int, @ProductType nvarchar(250), @Rate decimal(18,2),
    @EffectiveFrom date=NULL, @UserID int
AS
BEGIN
    INSERT dbo.SecuritizationRelianceLetterCostingDetail
        (RateID, ProductType, Rate, EffectiveFrom, IsActive, AddedBy)
    VALUES (@RateID, @ProductType, @Rate, @EffectiveFrom, 1, @UserID);
END;
GO

ALTER PROCEDURE dbo.usp_getRLSecCostByProjectID @ProjectID int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.SecuritizationRelianceLetterCosting
    WHERE ProjectId=@ProjectID AND ISNULL(IsActive,1)=1
    ORDER BY COALESCE(UpdatedDate,AddedDate) DESC, RateID DESC;
END;
GO

ALTER PROCEDURE dbo.usp_RLInvoice_GetBySentStatus @IsSent bit
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.InvoiceID, r.OurClient, r.Recipient, r.TradeName, r.InvoiceDate,
           r.Document, r.TM, r.Docusign, r.DocumentDate, r.ExecutedDate,
           r.LoanCount,
           CASE WHEN ISNULL(r.Cost,0)=0 AND TRY_CONVERT(decimal(18,4),r.LoanCount)<>0
                THEN r.ExpectedBilling/TRY_CONVERT(decimal(18,4),r.LoanCount) ELSE r.Cost END AS Cost,
           r.ExpectedBilling, r.BillingEntity, r.SubmittedforInvoice, r.InvoiceIssued,
           r.Notes, r.FilePath, r.AddedBy, r.isVerify, r.VerifyRemark, r.VerifiedOn,
           r.ContactPerson, r.ContactEmail, r.ContactPhone, r.Address,
           r.CostingRateID, r.BillingMethod, r.HoursWorked, r.MinimumAmount,
           r.MaximumCap, r.BaseAmount, r.MinimumApplied, r.CapApplied,
           s.SentBy, s.SentDateTime
    FROM dbo.RLinvoice r
    LEFT JOIN dbo.RLInvoiceSentToClient s ON s.RLInvoiceID=r.InvoiceID AND s.IsSent=1
    WHERE ((@IsSent=1 AND s.ID IS NOT NULL) OR (@IsSent=0 AND s.ID IS NULL))
      AND (@IsSent=1 OR ISNULL(r.isVerify,0)=1)
    ORDER BY r.InvoiceID DESC;
END;
GO

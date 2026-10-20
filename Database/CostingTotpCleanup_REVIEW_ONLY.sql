/* REVIEW ONLY. Run separately after backup/retention approval and after the Email OTP release is stable. */
/* This removes only the obsolete Android/TOTP enrollment table. Permissions, sessions and audit history remain. */
SET XACT_ABORT ON; BEGIN TRANSACTION;
IF OBJECT_ID(N'dbo.CostingAuthenticatorEnrollment',N'U') IS NOT NULL
    DROP TABLE dbo.CostingAuthenticatorEnrollment;
/* Replace ROLLBACK with COMMIT only after DBA review. */
ROLLBACK;

# Costing Email OTP deployment

1. Review and run `CostingEmailOtpMigration.sql` against `InfinityBilling`. It preserves permissions, sessions, audits and any old TOTP enrollment data.
2. No additional environment key is required. OTP hashes are keyed from the existing protected `ackdata` mail credential with a purpose-specific SHA-256 derivation. Rotating that credential invalidates only currently pending five-minute OTPs.
3. The existing mail route is reused: `ack@infinity-data.com`, Office 365 SMTP/TLS 587, and `bllTracking.GetPassword("ackdata")`. Confirm this credential works before release.
4. Admins manage employee codes at `/BDM/CostingAccessMaster.aspx`. OTP mail uses `EmployeeInfo.OfficialEmailID` only.
5. Review `CostingTotpCleanup_REVIEW_ONLY.sql` later. It defaults to `ROLLBACK` and must not be run automatically.

Tests: assigned/unassigned and missing-email users; initial send and masked email; 30-second resend; sixth request inside 15 minutes; old-code invalidation; five wrong attempts and 15-minute lock; five-minute expiry; single use; fixed 30-minute unlock; direct GET/POST/WebMethod bypass; logout/removal/session invalidation; and regression checks for invoice, reports, imports and automated billing.

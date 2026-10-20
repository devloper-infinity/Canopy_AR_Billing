using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Vendor_Portal.App_Code.DAL;

namespace Vendor_Portal.App_Code.BLL
{
    public static class CostingSecurityService
    {
        public const int AccessMinutes = 30; const int OtpMinutes = 5, ResendSeconds = 30, MaxRequests = 5, RequestWindowMinutes = 15, MaxFailures = 5, LockoutMinutes = 15; const string Purpose = "COSTING_MASTER_ACCESS";
        public static string CurrentEmployeeCode() { HttpContext h = HttpContext.Current; if (h == null || h.User == null || !h.User.Identity.IsAuthenticated) return null; string identity = (h.User.Identity.Name ?? "").Trim(), candidate = identity.Contains("\\") ? identity.Substring(identity.LastIndexOf('\\') + 1) : identity; int id; bool numeric = int.TryParse(candidate, out id); using (SqlConnection c = new SqlConnection(SQLHelper.ConnectionString)) using (SqlCommand x = new SqlCommand(numeric ? "SELECT TOP 1 Code FROM dbo.EmployeeInfo WHERE EmployeeID=@Id" : "SELECT TOP 1 Code FROM dbo.EmployeeInfo WHERE Code=@Code", c)) { if (numeric) x.Parameters.AddWithValue("@Id", id); else x.Parameters.AddWithValue("@Code", candidate); c.Open(); object v = x.ExecuteScalar(); return v == null || v == DBNull.Value ? null : Convert.ToString(v).Trim(); } }
        public static bool HasActiveAccess() { string code = CurrentEmployeeCode(), sid = SessionId(); if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(sid)) return false; using (SqlConnection c = new SqlConnection(SQLHelper.ConnectionString2)) using (SqlCommand x = new SqlCommand(@"SELECT COUNT(1) FROM dbo.CostingAuthenticatorPermission p INNER JOIN dbo.CostingAuthorizationSession s ON s.EmployeeCode=p.EmployeeCode WHERE p.EmployeeCode=@Code AND p.IsAllowed=1 AND s.WebSessionId=@Sid AND s.RevokedOnUtc IS NULL AND s.ExpiresOnUtc>SYSUTCDATETIME()", c)) { x.Parameters.AddWithValue("@Code", code); x.Parameters.AddWithValue("@Sid", sid); c.Open(); return Convert.ToInt32(x.ExecuteScalar()) > 0; } }
        public static AccessState GetState() { string code = CurrentEmployeeCode(); if (string.IsNullOrEmpty(code)) return S("unauthenticated", "Windows sign-in is required."); if (HasActiveAccess()) return S("unlocked", "Access granted."); if (!HasPermission(code)) return S("denied", "Costing access has not been assigned."); string email = CompanyEmail(code); if (string.IsNullOrWhiteSpace(email)) return S("email_missing", "A registered company email was not found. Contact an administrator."); AccessState s = S("otp_required", "Request a verification code sent to " + Mask(email) + "."); s.MaskedEmail = Mask(email); return s; }
        public static AccessState SendOtp()
        {
            string code = CurrentEmployeeCode(), sid = SessionId(); if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(sid)) return S("unauthenticated", "Windows sign-in is required."); if (!HasPermission(code)) return S("denied", "Costing access has not been assigned."); string email = CompanyEmail(code); if (string.IsNullOrWhiteSpace(email)) return S("email_missing", "A registered company email was not found. Contact an administrator."); string otp = NewOtp(), salt = Convert.ToBase64String(RandomBytes(32)); byte[] hash = Hash(code, sid, otp, salt);
            using (SqlConnection c = new SqlConnection(SQLHelper.ConnectionString2))
            {
                c.Open(); using (SqlTransaction t = c.BeginTransaction(IsolationLevel.Serializable))
                {
                    DateTime? last = null, locked = null; int recent = 0; using (SqlCommand q = new SqlCommand(@"SELECT MAX(RequestedOnUtc),SUM(CASE WHEN RequestedOnUtc>DATEADD(MINUTE,-@Window,SYSUTCDATETIME()) THEN 1 ELSE 0 END),MAX(LockedUntilUtc) FROM dbo.CostingEmailOtpChallenge WITH(UPDLOCK,HOLDLOCK) WHERE EmployeeCode=@Code AND WebSessionId=@Sid AND Purpose=@Purpose", c, t)) { q.Parameters.AddWithValue("@Window", RequestWindowMinutes); q.Parameters.AddWithValue("@Code", code); q.Parameters.AddWithValue("@Sid", sid); q.Parameters.AddWithValue("@Purpose", Purpose); using (SqlDataReader r = q.ExecuteReader()) if (r.Read()) { if (r[0] != DBNull.Value) last = Convert.ToDateTime(r[0]); if (r[1] != DBNull.Value) recent = Convert.ToInt32(r[1]); if (r[2] != DBNull.Value) locked = Convert.ToDateTime(r[2]); } }
                    if (locked.HasValue && locked > DateTime.UtcNow) { t.Rollback(); return S("locked_out", "Too many invalid codes. Try again later."); }
                    if (last.HasValue && last.Value.AddSeconds(ResendSeconds) > DateTime.UtcNow) { t.Rollback(); AccessState w = S("rate_limited", "Please wait before requesting another code."); w.RetryAfterSeconds = Math.Max(1, (int)(last.Value.AddSeconds(ResendSeconds) - DateTime.UtcNow).TotalSeconds); return w; }
                    if (recent >= MaxRequests) { t.Rollback(); return S("rate_limited", "Too many code requests. Try again later."); }
                    using (SqlCommand q = new SqlCommand(@"UPDATE dbo.CostingEmailOtpChallenge SET InvalidatedOnUtc=SYSUTCDATETIME() WHERE EmployeeCode=@Code AND WebSessionId=@Sid AND Purpose=@Purpose AND UsedOnUtc IS NULL AND InvalidatedOnUtc IS NULL; INSERT dbo.CostingEmailOtpChallenge(EmployeeCode,WebSessionId,Purpose,OtpHash,OtpSalt,RequestedOnUtc,ExpiresOnUtc,AttemptCount)VALUES(@Code,@Sid,@Purpose,@Hash,@Salt,SYSUTCDATETIME(),DATEADD(MINUTE,@Minutes,SYSUTCDATETIME()),0); INSERT dbo.CostingSecurityAudit(EmployeeCode,ActionName,Succeeded,OccurredOnUtc,IpAddress,Detail)VALUES(@Code,'EMAIL_OTP_REQUEST',1,SYSUTCDATETIME(),@Ip,'Company email delivery queued')", c, t)) { q.Parameters.AddWithValue("@Code", code); q.Parameters.AddWithValue("@Sid", sid); q.Parameters.AddWithValue("@Purpose", Purpose); q.Parameters.Add("@Hash", SqlDbType.VarBinary, 32).Value = hash; q.Parameters.AddWithValue("@Salt", salt); q.Parameters.AddWithValue("@Minutes", OtpMinutes); q.Parameters.AddWithValue("@Ip", Ip()); q.ExecuteNonQuery(); }
                    t.Commit();
                }
            }
            QueueEmail(email, otp); AccessState sent = S("otp_sent", "A verification code was sent to " + Mask(email) + ". It expires in 5 minutes."); sent.MaskedEmail = Mask(email); sent.RetryAfterSeconds = ResendSeconds; return sent;
        }
        public static AccessState Verify(string otp) { string code = CurrentEmployeeCode(), sid = SessionId(); if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(sid)) return S("unauthenticated", "Windows sign-in is required."); if (!HasPermission(code)) return S("denied", "Costing access has not been assigned."); if (string.IsNullOrWhiteSpace(otp) || otp.Length != 6) return S("otp_required", "Enter the 6-digit code."); using (SqlConnection c = new SqlConnection(SQLHelper.ConnectionString2)) { c.Open(); using (SqlTransaction t = c.BeginTransaction(IsolationLevel.Serializable)) using (SqlCommand q = new SqlCommand(@"SELECT TOP 1 ChallengeId,OtpHash,OtpSalt,ExpiresOnUtc,AttemptCount,LockedUntilUtc FROM dbo.CostingEmailOtpChallenge WITH(UPDLOCK,HOLDLOCK) WHERE EmployeeCode=@Code AND WebSessionId=@Sid AND Purpose=@Purpose AND UsedOnUtc IS NULL AND InvalidatedOnUtc IS NULL ORDER BY ChallengeId DESC", c, t)) { q.Parameters.AddWithValue("@Code", code); q.Parameters.AddWithValue("@Sid", sid); q.Parameters.AddWithValue("@Purpose", Purpose); long id; byte[] saved; string salt; DateTime expires; int attempts; DateTime? locked = null; using (SqlDataReader r = q.ExecuteReader()) { if (!r.Read()) { t.Rollback(); return S("otp_required", "Request a new verification code."); } id = Convert.ToInt64(r[0]); saved = (byte[])r[1]; salt = Convert.ToString(r[2]); expires = Convert.ToDateTime(r[3]); attempts = Convert.ToInt32(r[4]); if (r[5] != DBNull.Value) locked = Convert.ToDateTime(r[5]); } if (locked.HasValue && locked > DateTime.UtcNow) { t.Rollback(); return S("locked_out", "Too many invalid codes. Try again later."); } if (expires <= DateTime.UtcNow) { Exec(c, t, "UPDATE dbo.CostingEmailOtpChallenge SET InvalidatedOnUtc=SYSUTCDATETIME() WHERE ChallengeId=@Id", id); t.Commit(); return S("otp_required", "The code expired. Request a new code."); } if (!Equal(saved, Hash(code, sid, otp, salt))) { attempts++; using (SqlCommand f = new SqlCommand(@"UPDATE dbo.CostingEmailOtpChallenge SET AttemptCount=@Attempts,LockedUntilUtc=CASE WHEN @Attempts>=@Max THEN DATEADD(MINUTE,@Lockout,SYSUTCDATETIME()) ELSE NULL END WHERE ChallengeId=@Id;INSERT dbo.CostingSecurityAudit(EmployeeCode,ActionName,Succeeded,OccurredOnUtc,IpAddress)VALUES(@Code,'EMAIL_OTP_VERIFY',0,SYSUTCDATETIME(),@Ip)", c, t)) { f.Parameters.AddWithValue("@Attempts", attempts); f.Parameters.AddWithValue("@Max", MaxFailures); f.Parameters.AddWithValue("@Lockout", LockoutMinutes); f.Parameters.AddWithValue("@Id", id); f.Parameters.AddWithValue("@Code", code); f.Parameters.AddWithValue("@Ip", Ip()); f.ExecuteNonQuery(); } t.Commit(); return S(attempts >= MaxFailures ? "locked_out" : "otp_required", attempts >= MaxFailures ? "Too many invalid codes. Try again in 15 minutes." : "Invalid verification code."); } using (SqlCommand ok = new SqlCommand(@"UPDATE dbo.CostingEmailOtpChallenge SET UsedOnUtc=SYSUTCDATETIME() WHERE ChallengeId=@Id;UPDATE dbo.CostingAuthorizationSession SET RevokedOnUtc=SYSUTCDATETIME() WHERE EmployeeCode=@Code AND WebSessionId=@Sid AND RevokedOnUtc IS NULL;INSERT dbo.CostingAuthorizationSession(EmployeeCode,WebSessionId,GrantedOnUtc,ExpiresOnUtc)VALUES(@Code,@Sid,SYSUTCDATETIME(),DATEADD(MINUTE,@Minutes,SYSUTCDATETIME()));INSERT dbo.CostingSecurityAudit(EmployeeCode,ActionName,Succeeded,OccurredOnUtc,IpAddress)VALUES(@Code,'EMAIL_OTP_VERIFY',1,SYSUTCDATETIME(),@Ip)", c, t)) { ok.Parameters.AddWithValue("@Id", id); ok.Parameters.AddWithValue("@Code", code); ok.Parameters.AddWithValue("@Sid", sid); ok.Parameters.AddWithValue("@Minutes", AccessMinutes); ok.Parameters.AddWithValue("@Ip", Ip()); ok.ExecuteNonQuery(); } t.Commit(); return S("unlocked", "Access granted for 30 minutes."); } } }
        public static void RevokeCurrentSession() { if (HttpContext.Current == null || HttpContext.Current.Session == null) return; using (SqlConnection c = new SqlConnection(SQLHelper.ConnectionString2)) using (SqlCommand q = new SqlCommand("UPDATE dbo.CostingAuthorizationSession SET RevokedOnUtc=SYSUTCDATETIME() WHERE WebSessionId=@Sid AND RevokedOnUtc IS NULL;IF OBJECT_ID(N'dbo.CostingEmailOtpChallenge',N'U') IS NOT NULL UPDATE dbo.CostingEmailOtpChallenge SET InvalidatedOnUtc=SYSUTCDATETIME() WHERE WebSessionId=@Sid AND UsedOnUtc IS NULL AND InvalidatedOnUtc IS NULL", c)) { q.Parameters.AddWithValue("@Sid", SessionId()); c.Open(); q.ExecuteNonQuery(); } }
        static bool HasPermission(string code) { using (SqlConnection c = new SqlConnection(SQLHelper.ConnectionString2)) using (SqlCommand q = new SqlCommand("SELECT COUNT(1) FROM dbo.CostingAuthenticatorPermission WHERE EmployeeCode=@Code AND IsAllowed=1", c)) { q.Parameters.AddWithValue("@Code", code); c.Open(); return Convert.ToInt32(q.ExecuteScalar()) > 0; } }
        static string CompanyEmail(string code) { using (SqlConnection c = new SqlConnection(SQLHelper.ConnectionString)) using (SqlCommand q = new SqlCommand("SELECT TOP 1 OfficialEmailID FROM dbo.EmployeeInfo WHERE Code=@Code", c)) { q.Parameters.AddWithValue("@Code", code); c.Open(); return Convert.ToString(q.ExecuteScalar()).Trim(); } }
        static void QueueEmail(string email, string otp)
        {
            string password = new bllTracking().GetPassword("ackdata");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HostingEnvironment.QueueBackgroundWorkItem(async token =>
            {
                try
                {
                    using (MailMessage m = new MailMessage())
                    using (SmtpClient s = new SmtpClient())
                    {
                        m.From = new MailAddress("ack@infinity-data.com", "Canopy Billing", Encoding.UTF8);
                        m.To.Add(email);
                        m.Subject = "Canopy AR Verification";
                        m.Body = "<!doctype html><html><body style='margin:0;background:#f4f7fb;font-family:Arial,sans-serif;color:#1e293b;'>" +
                            "<table role='presentation' width='100%' cellspacing='0' cellpadding='0' style='padding:28px 12px;background:#f4f7fb;'><tr><td align='center'>" +
                            "<table role='presentation' width='560' cellspacing='0' cellpadding='0' style='max-width:560px;width:100%;background:#ffffff;border:1px solid #dbe4ee;border-radius:12px;'>" +
                            "<tr><td style='padding:22px 28px;background:#0f4c81;color:#ffffff;border-radius:12px 12px 0 0;font-size:20px;font-weight:bold;'>Canopy AR Verification</td></tr>" +
                            "<tr><td style='padding:28px;'><p style='margin:0 0 14px;'>Hello,</p>" +
                            "<p style='margin:0 0 22px;color:#475569;line-height:22px;'>Your one-time verification code to access Project Costing is:</p>" +
                            "<div style='padding:18px;background:#eef4fa;border:1px solid #cbd8e6;border-radius:10px;text-align:center;font-size:32px;font-weight:bold;letter-spacing:8px;color:#0f4c81;'>" + HttpUtility.HtmlEncode(otp) + "</div>" +
                            "<p style='margin:22px 0 0;color:#64748b;font-size:13px;line-height:20px;'>This code expires in 5 minutes and can be used only once. Do not share it.</p>" +
                            "</td></tr><tr><td style='padding:16px 28px;border-top:1px solid #e2e8f0;color:#94a3b8;font-size:12px;'>Canopy Billing &bull; Automated security message</td></tr>" +
                            "</table></td></tr></table></body></html>";
                        m.IsBodyHtml = true; m.BodyEncoding = Encoding.UTF8; m.Priority = MailPriority.High;
                        s.UseDefaultCredentials = false; s.Credentials = new NetworkCredential("ack@infinity-data.com", password);
                        s.Host = "smtp.office365.com"; s.Port = 587; s.DeliveryMethod = SmtpDeliveryMethod.Network; s.EnableSsl = true;
                        await s.SendMailAsync(m);
                    }
                }
                catch (Exception ex) { System.Diagnostics.Trace.TraceError("Costing OTP email failed: {0}", ex); }
            });
        }
        static string NewOtp() { byte[] b = new byte[4]; uint v, limit = uint.MaxValue - (uint.MaxValue % 1000000); using (RandomNumberGenerator r = RandomNumberGenerator.Create()) { do { r.GetBytes(b); v = BitConverter.ToUInt32(b, 0); } while (v >= limit); } return (v % 1000000).ToString("D6"); }
        static byte[] Hash(string code, string sid, string otp, string salt) { using (HMACSHA256 h = new HMACSHA256(Key())) return h.ComputeHash(Encoding.UTF8.GetBytes(code + "|" + sid + "|" + Purpose + "|" + salt + "|" + otp)); }
        static byte[] Key() { string existingMailSecret = new bllTracking().GetPassword("ackdata"); if (string.IsNullOrWhiteSpace(existingMailSecret)) throw new InvalidOperationException("The existing ackdata email credential is not configured."); using (SHA256 h = SHA256.Create()) return h.ComputeHash(Encoding.UTF8.GetBytes("CanopyBilling|CostingEmailOtp|v1|" + existingMailSecret)); }
        static byte[] RandomBytes(int n) { byte[] b = new byte[n]; using (RandomNumberGenerator r = RandomNumberGenerator.Create()) r.GetBytes(b); return b; }
        static bool Equal(byte[] a, byte[] b) { if (a == null || b == null || a.Length != b.Length) return false; int d = 0; for (int i = 0; i < a.Length; i++) d |= a[i] ^ b[i]; return d == 0; }
        static string SessionId() { return HttpContext.Current == null || HttpContext.Current.Session == null ? null : HttpContext.Current.Session.SessionID; }
        static string Ip() { string s = HttpContext.Current.Request.UserHostAddress ?? ""; return s.Substring(0, Math.Min(45, s.Length)); }
        static string Mask(string e) { int at = e.IndexOf('@'); return at <= 1 ? "***" + (at >= 0 ? e.Substring(at) : "") : e.Substring(0, 1) + new string('*', Math.Min(6, at - 1)) + e.Substring(at); }
        static AccessState S(string status, string message) { return new AccessState { Status = status, Message = message }; }
        static void Exec(SqlConnection c, SqlTransaction t, string sql, long id) { using (SqlCommand q = new SqlCommand(sql, c, t)) { q.Parameters.AddWithValue("@Id", id); q.ExecuteNonQuery(); } }
        public sealed class AccessState { public string Status { get; set; } public string Message { get; set; } public string MaskedEmail { get; set; } public int RetryAfterSeconds { get; set; } }
    }
}

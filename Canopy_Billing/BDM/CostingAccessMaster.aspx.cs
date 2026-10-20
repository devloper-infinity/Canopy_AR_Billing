using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Vendor_Portal.App_Code.BLL;
using Vendor_Portal.App_Code.DAL;

namespace Vendor_Portal.BDM
{
 public partial class CostingAccessMaster : Page
 {
  protected void Page_Load(object sender,EventArgs e){if(!User.IsInRole("Admin"))throw new HttpException(403,"Administrator access required.");if(!IsPostBack)BindGrid();}
  protected void Assign_Click(object sender,EventArgs e){ChangeAccess(true);}
  protected void Revoke_Click(object sender,EventArgs e){ChangeAccess(false);}
  private void ChangeAccess(bool allowed){string target=EmployeeCode.Text.Trim(),actor=CostingSecurityService.CurrentEmployeeCode();if(!EmployeeExists(target)){Show("Employee code was not found.",false);return;}using(SqlConnection c=new SqlConnection(SQLHelper.ConnectionString2))using(SqlCommand x=new SqlCommand(@"MERGE dbo.CostingAuthenticatorPermission AS t USING(SELECT @Code EmployeeCode)s ON t.EmployeeCode=s.EmployeeCode WHEN MATCHED THEN UPDATE SET IsAllowed=@Allowed,ModifiedByCode=@Actor,ModifiedOnUtc=SYSUTCDATETIME() WHEN NOT MATCHED THEN INSERT(EmployeeCode,IsAllowed,ModifiedByCode)VALUES(@Code,@Allowed,@Actor);UPDATE dbo.CostingAuthorizationSession SET RevokedOnUtc=SYSUTCDATETIME() WHERE EmployeeCode=@Code AND RevokedOnUtc IS NULL;UPDATE dbo.CostingEmailOtpChallenge SET InvalidatedOnUtc=SYSUTCDATETIME() WHERE EmployeeCode=@Code AND UsedOnUtc IS NULL AND InvalidatedOnUtc IS NULL;INSERT dbo.CostingSecurityAudit(EmployeeCode,ActorEmployeeCode,ActionName,Succeeded,OccurredOnUtc,IpAddress)VALUES(@Code,@Actor,@Action,1,SYSUTCDATETIME(),@Ip)",c)){x.Parameters.AddWithValue("@Code",target);x.Parameters.AddWithValue("@Allowed",allowed);x.Parameters.AddWithValue("@Actor",actor);x.Parameters.AddWithValue("@Action",allowed?"ACCESS_ASSIGNED":"ACCESS_REVOKED");x.Parameters.AddWithValue("@Ip",Request.UserHostAddress??"");c.Open();x.ExecuteNonQuery();}BindGrid();Show(allowed?"Access assigned.":"Access removed; active sessions and pending codes were revoked.",true);}
  private bool EmployeeExists(string code){using(SqlConnection c=new SqlConnection(SQLHelper.ConnectionString))using(SqlCommand x=new SqlCommand("SELECT COUNT(1) FROM dbo.EmployeeInfo WHERE Code=@Code",c)){x.Parameters.AddWithValue("@Code",code);c.Open();return Convert.ToInt32(x.ExecuteScalar())==1;}}
  private void BindGrid(){using(SqlConnection c=new SqlConnection(SQLHelper.ConnectionString2))using(SqlDataAdapter a=new SqlDataAdapter("SELECT EmployeeCode,IsAllowed,ModifiedByCode,ModifiedOnUtc FROM dbo.CostingAuthenticatorPermission ORDER BY EmployeeCode",c)){DataTable t=new DataTable();a.Fill(t);StatusGrid.DataSource=t;StatusGrid.DataBind();}}
  private void Show(string text,bool ok){Message.Text=HttpUtility.HtmlEncode(text);Message.CssClass=ok?"text-success d-block mb-3":"text-danger d-block mb-3";}
 }
}

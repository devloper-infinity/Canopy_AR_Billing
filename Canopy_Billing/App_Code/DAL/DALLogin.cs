using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Vendor_Portal.App_Code.DAL
{
    public class DALLogin
    {
        public int ValidateUser(string Username, string Password)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_ValidateUser");
            SQLHelper.AddParamToSQLCmd(cmd, "@Username", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, Username);
            SQLHelper.AddParamToSQLCmd(cmd, "@Password", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, Password);
            SQLHelper.AddParamToSQLCmd(cmd, "@ReturnValue", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.ReturnValue, null);
            SQLHelper.ExecuteNonQueryCmd(cmd);

            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
            cmd.Dispose();
            return ReturnValue; //-1=User Not Exist, 0=Invalid Password, >0=Success

        }

        public DataTable GetUserById(int EmployeeID, string Username, string Password)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetUserById_ForOST");
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeID", System.Data.SqlDbType.BigInt, 50, System.Data.ParameterDirection.Input, EmployeeID);
            SQLHelper.AddParamToSQLCmd(cmd, "@Username", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Username);
            SQLHelper.AddParamToSQLCmd(cmd, "@Password", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, Password);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable BlockUserLogin(string Code)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_BlockUserLogin");
            SQLHelper.AddParamToSQLCmd(cmd, "@Code", System.Data.SqlDbType.NVarChar, 10, System.Data.ParameterDirection.Input, Code);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }
        public DataTable GetUserInformation(int EmployeeID)
        {
            SqlCommand cmd = SQLHelper.GetCommand(System.Data.CommandType.StoredProcedure, "usp_GetUserInformation");
            SQLHelper.AddParamToSQLCmd(cmd, "@EmployeeID", System.Data.SqlDbType.BigInt, 50, System.Data.ParameterDirection.Input, EmployeeID);
            DataTable dt = SQLHelper.ExecuteDataTableCmd(cmd);
            return dt;
        }

        public DataTable GetUserMfaSetting(int EmployeeID)
        {
            EnsureMfaTable();

            using (SqlConnection connection = new SqlConnection(SQLHelper.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT TOP 1 EmployeeID, UserName, ProtectedSecret, IsEnabled
                  FROM dbo.CanopyUserMfa
                  WHERE EmployeeID = @EmployeeID AND IsEnabled = 1", connection))
            {
                cmd.Parameters.Add("@EmployeeID", SqlDbType.BigInt).Value = EmployeeID;
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
        }

        public void SaveUserMfaSetting(int EmployeeID, string UserName, string ProtectedSecret)
        {
            EnsureMfaTable();

            using (SqlConnection connection = new SqlConnection(SQLHelper.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"IF EXISTS (SELECT 1 FROM dbo.CanopyUserMfa WHERE EmployeeID = @EmployeeID)
                  BEGIN
                      UPDATE dbo.CanopyUserMfa
                      SET UserName = @UserName,
                          ProtectedSecret = @ProtectedSecret,
                          IsEnabled = 1,
                          UpdatedOn = GETDATE()
                      WHERE EmployeeID = @EmployeeID
                  END
                  ELSE
                  BEGIN
                      INSERT INTO dbo.CanopyUserMfa (EmployeeID, UserName, ProtectedSecret, IsEnabled, CreatedOn)
                      VALUES (@EmployeeID, @UserName, @ProtectedSecret, 1, GETDATE())
                  END", connection))
            {
                cmd.Parameters.Add("@EmployeeID", SqlDbType.BigInt).Value = EmployeeID;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = UserName;
                cmd.Parameters.Add("@ProtectedSecret", SqlDbType.NVarChar, 512).Value = ProtectedSecret;
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void MarkUserMfaVerified(int EmployeeID)
        {
            EnsureMfaTable();

            using (SqlConnection connection = new SqlConnection(SQLHelper.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE dbo.CanopyUserMfa
                  SET LastVerifiedOn = GETDATE()
                  WHERE EmployeeID = @EmployeeID", connection))
            {
                cmd.Parameters.Add("@EmployeeID", SqlDbType.BigInt).Value = EmployeeID;
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void EnsureMfaTable()
        {
            using (SqlConnection connection = new SqlConnection(SQLHelper.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"IF OBJECT_ID(N'dbo.CanopyUserMfa', N'U') IS NULL
                  BEGIN
                      CREATE TABLE dbo.CanopyUserMfa
                      (
                          EmployeeID BIGINT NOT NULL CONSTRAINT PK_CanopyUserMfa PRIMARY KEY,
                          UserName NVARCHAR(100) NOT NULL,
                          ProtectedSecret NVARCHAR(512) NOT NULL,
                          IsEnabled BIT NOT NULL CONSTRAINT DF_CanopyUserMfa_IsEnabled DEFAULT (1),
                          CreatedOn DATETIME NOT NULL CONSTRAINT DF_CanopyUserMfa_CreatedOn DEFAULT (GETDATE()),
                          UpdatedOn DATETIME NULL,
                          LastVerifiedOn DATETIME NULL
                      )
                  END", connection))
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

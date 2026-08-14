using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;

namespace Vendor_Portal.App_Code.DAL
{
    public class SQLHelper
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MainCon"].ToString();
            }
        }
        public static string ConnectionString2
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["InfinityBilling"].ToString();
            }
        }
        public static string ConnectionString3
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SubCon"].ToString();
            }
        }

        /// <summary>
        /// For Other Parameters
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="paramId"></param>
        /// <param name="sqlType"></param>
        /// <param name="paramSize"></param>
        /// <param name="paramDirection"></param>
        /// <param name="paramvalue"></param>
        /// <returns></returns>
        public static bool AddParamToSQLCmd(SqlCommand sqlCmd, string paramId, SqlDbType sqlType, int paramSize, ParameterDirection paramDirection, object paramvalue)
        {
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            if (String.IsNullOrEmpty(paramId))
            {
                throw new ArgumentOutOfRangeException("paramId");
            }

            SqlParameter newSqlParam = new SqlParameter();
            newSqlParam.ParameterName = paramId;
            newSqlParam.SqlDbType = sqlType;
            newSqlParam.Direction = paramDirection;

            if (paramSize > 0)
            {
                newSqlParam.Size = paramSize;
            }

            if (sqlType == SqlDbType.Image || sqlType == SqlDbType.Binary)
            {
                newSqlParam.Value = paramvalue;
            }
            else
            {
                if ((paramvalue != null))
                {
                    StringBuilder FilteredParam = new StringBuilder(paramvalue.ToString());
                    //  FilteredParam.Replace("'", "`");
                    FilteredParam.Replace("--", "-");
                    FilteredParam.Replace("--", "-");
                    FilteredParam.Replace("--", "-");
                    // FilteredParam.Replace("%", "(per)");
                    // FilteredParam.Replace(";", "");
                    FilteredParam.Replace("/*", "");
                    FilteredParam.Replace("*/", "");
                    FilteredParam.Replace("@@", "");
                    FilteredParam.Replace("sysobjects", "");
                    FilteredParam.Replace("syscolumns", "");
                    newSqlParam.Value = FilteredParam.ToString();
                    FilteredParam = null;
                }
            }
            sqlCmd.Parameters.Add(newSqlParam);
            newSqlParam = null;
            return true;
        }

        /// <summary>
        /// For Decimal Parameters
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="paramId"></param>
        /// <param name="sqlType"></param>
        /// <param name="paramPrecision"></param>
        /// <param name="paramScale"></param>
        /// <param name="paramDirection"></param>
        /// <param name="paramvalue"></param>
        /// <returns></returns>
        public static bool AddParamToSQLCmd(SqlCommand sqlCmd, string paramId, SqlDbType sqlType, byte paramPrecision, byte paramScale, ParameterDirection paramDirection, object paramvalue)
        {
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            if (String.IsNullOrEmpty(paramId))
            {
                throw new ArgumentOutOfRangeException("paramId");
            }

            SqlParameter newSqlParam = new SqlParameter();
            newSqlParam.ParameterName = paramId;
            newSqlParam.SqlDbType = sqlType;
            newSqlParam.Direction = paramDirection;

            if (paramPrecision > 0)
            {
                newSqlParam.Size = paramPrecision;
                newSqlParam.Precision = paramPrecision;
            }
            if (paramScale > 0)
            {
                newSqlParam.Scale = paramScale;
            }

            if (sqlType == SqlDbType.Image || sqlType == SqlDbType.Binary)
            {
                newSqlParam.Value = paramvalue;
            }
            else
            {
                if ((paramvalue != null))
                {
                    StringBuilder FilteredParam = new StringBuilder(paramvalue.ToString());
                    FilteredParam.Replace("'", "`");
                    FilteredParam.Replace("--", "-");
                    FilteredParam.Replace("--", "-");
                    FilteredParam.Replace("--", "-");
                    //FilteredParam.Replace("%", "(per)");
                    FilteredParam.Replace(";", "");
                    FilteredParam.Replace("/*", "");
                    FilteredParam.Replace("*/", "");
                    FilteredParam.Replace("@@", "");
                    FilteredParam.Replace("sysobjects", "");
                    FilteredParam.Replace("syscolumns", "");
                    newSqlParam.Value = FilteredParam.ToString();
                    FilteredParam = null;
                }
            }
            sqlCmd.Parameters.Add(newSqlParam);
            newSqlParam = null;
            return true;
        }

        public static object ExecuteScalarCmd(SqlCommand sqlCmd)
        {
            object result = null;
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            using (SqlConnection MyCon = new SqlConnection(ConnectionString))
            {
                MyCon.Open();
                try
                {
                    sqlCmd.Connection = MyCon;
                    sqlCmd.CommandTimeout = 0;
                    result = sqlCmd.ExecuteScalar();
                    sqlCmd.Dispose();
                }
                catch
                {
                    return null;
                }
            }
            return result;
        }

        public static object ExecuteScalarCmdBilling(SqlCommand sqlCmd)
        {
            object result = null;
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            using (SqlConnection MyCon = new SqlConnection(ConnectionString2))
            {
                MyCon.Open();
                try
                {
                    sqlCmd.Connection = MyCon;
                    sqlCmd.CommandTimeout = 0;
                    result = sqlCmd.ExecuteScalar();
                    sqlCmd.Dispose();
                }
                catch
                {
                    return null;
                }
            }
            return result;
        }


        public static DataSet ExecuteDataSetCmd(SqlCommand sqlCmd)
        {
            DataSet dblds = new DataSet();
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            using (SqlConnection MyCon = new SqlConnection(ConnectionString))
            {
                MyCon.Open();
                try
                {
                    sqlCmd.Connection = MyCon;
                    sqlCmd.CommandTimeout = 0;

                    SqlDataAdapter dblsa = new SqlDataAdapter(sqlCmd);
                    dblsa.Fill(dblds);

                    sqlCmd.Dispose();
                }
                catch
                {
                    return null;
                }
            }
            return dblds;
        }

        public static DataSet ExecuteDataSetCmd_Billing(SqlCommand sqlCmd)
        {
            DataSet dblds = new DataSet();
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            using (SqlConnection MyCon = new SqlConnection(ConnectionString2))
            {
                MyCon.Open();
                try
                {
                    sqlCmd.Connection = MyCon;
                    sqlCmd.CommandTimeout = 0;

                    SqlDataAdapter dblsa = new SqlDataAdapter(sqlCmd);
                    dblsa.Fill(dblds);

                    sqlCmd.Dispose();
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
            return dblds;
        }

        public static SqlDataReader ExecuteReaderCmd(SqlCommand sqlCmd)
        {
            SqlDataReader temp;
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }

            SqlConnection MyCon = new SqlConnection(ConnectionString);
            MyCon.Open();
            try
            {
                sqlCmd.Connection = MyCon;
                sqlCmd.CommandTimeout = 0;
                temp = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                MyCon.Close();
                return null;
            }
            return temp;
        }

        public static bool ExecuteNonQueryCmd(SqlCommand sqlCmd)
        {
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            using (SqlConnection MyCon = new SqlConnection(ConnectionString))
            {
                MyCon.Open();
                try
                {
                    sqlCmd.Connection = MyCon;
                    sqlCmd.CommandTimeout = 0;
                    sqlCmd.ExecuteNonQuery();
                }
                catch
                {
                }
            }
            return true;
        }

        public static bool ExecuteNonQueryCmdBilling(SqlCommand sqlCmd)
        {
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            using (SqlConnection MyCon = new SqlConnection(ConnectionString2))
            {
                MyCon.Open();
                try
                {
                    sqlCmd.Connection = MyCon;
                    sqlCmd.CommandTimeout = 0;
                    sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
            }
            return true;
        }


        public static SqlConnection GetTransConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static bool ExecuteNonQueryCmd(SqlCommand sqlCmd, SqlConnection MyCon, SqlTransaction sqlTrasaction)
        {
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            try
            {
                sqlCmd.Connection = MyCon;
                sqlCmd.Transaction = sqlTrasaction;
                sqlCmd.CommandTimeout = 0;
                sqlCmd.ExecuteNonQuery();
            }
            catch
            {
            }
            return true;
        }

        public static SqlCommand GetCommand(CommandType cmdType, string cmdText)
        {
            SqlCommand sqlCmd = new SqlCommand(cmdText);
            sqlCmd.CommandType = cmdType;
            return sqlCmd;
        }

        public static DataTable ExecuteDataTableCmd(SqlCommand sqlCmd)
        {
            DataTable dbldt = new DataTable();
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            using (SqlConnection MyCon = new SqlConnection(ConnectionString))
            {
                MyCon.Open();
                try
                {
                    sqlCmd.Connection = MyCon;
                    sqlCmd.CommandTimeout = 0;

                    SqlDataAdapter dblsa = new SqlDataAdapter(sqlCmd);
                    dblsa.Fill(dbldt);

                    sqlCmd.Dispose();
                }
                catch
                {
                    return null;
                }
            }
            return dbldt;
        }

        public static DataTable ExecuteDataTableCmd_billing(SqlCommand sqlCmd)
        {
            DataTable dbldt = new DataTable();
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            using (SqlConnection MyCon = new SqlConnection(ConnectionString2))
            {
                MyCon.Open();
                try
                {
                    sqlCmd.Connection = MyCon;
                    sqlCmd.CommandTimeout = 0;

                    SqlDataAdapter dblsa = new SqlDataAdapter(sqlCmd);
                    dblsa.Fill(dbldt);

                    sqlCmd.Dispose();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return dbldt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCoreCommon
{
    public static class SQLiteHelper
    {
        public static IDbConnection OpenConnection(this SqlConnection sqlConn)
        {
            IDbConnection connection = new SqlConnection(sqlConn.ConnectionString);  //这里sqlconnection就是数据库连接字符串
            connection.Open();
            return connection;

        }
        //public static IDbConnection OpenCrmSqlConn()
        //{
        //    IDbConnection connection = Settings.CrmSetting.SqlConn;
        //    connection.Open();
        //    return connection;
        //}
        //public static IDbConnection OpenAddSqlConn()
        //{
        //    IDbConnection connection = Settings.CrmSetting.SqlAddConn;
        //    connection.Open();
        //    return connection;
        //}
    }
}

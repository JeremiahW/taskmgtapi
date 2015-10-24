using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMgt.DataAccess
{
    public static class SqlExtension
    {
        public static DataTable SqlQueryForDataTatable(this Database db, string sql)
        {

            SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            //conn.ConnectionString = db.Connection.ConnectionString;
            //if (conn.State != ConnectionState.Open)
            //{
            //    conn.Open();
            //}

            conn = (SqlConnection)db.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);

            conn.Close(); //连接需要关闭
            conn.Dispose();
            return table;
        }
    }
}

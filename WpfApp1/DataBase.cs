using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class DataBase
    {
        public static string name;
        SqlConnection conn = new SqlConnection("Data Source=ISHARDCODED; Integrated Security=SSPI; Initial Catalog=Demo");

        public void openConnection()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        public void closeConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return conn;
        }
    }
}

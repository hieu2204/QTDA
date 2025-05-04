using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Utilities
{
    public class ConnectDatabase
    {
        private readonly string connect = "Data Source=DESKTOP-VDPSTNV;Initial Catalog=POS_DB;Integrated Security=True;";
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connect);
        }
    }
}

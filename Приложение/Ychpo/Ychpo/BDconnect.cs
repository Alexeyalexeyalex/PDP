using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Ychpo
{
    class BDconnect
    {
        public static SqlConnection GetBDConnection()
        {
            string conString = "Data Source=KIRITO\\ZERGI;Initial Catalog=ychpo;Persist Security Info=True;User ID=sa;Password=Qq112233!";

            // Создание подключения
            SqlConnection con = new SqlConnection(conString);
            return con;
        }
    }
}

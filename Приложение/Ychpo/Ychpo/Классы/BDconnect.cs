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
            //string conString = "Data Source=ITACHI\\ZERGI;Initial Catalog=ychpo;Persist Security Info=True;User ID=sa;Password=Qq112233!";
            //string conString = "workstation id=YCHPROB.mssql.somee.com;packet size=4096;user id=NEOTYANKA_SQLLogin_1;pwd=k3n9n8r51e;data source=YCHPROB.mssql.somee.com;persist security info=False;initial catalog=YCHPROB";
            // Создание подключения
            SqlConnection con = new SqlConnection(conString);
            return con;
        }
    }
}

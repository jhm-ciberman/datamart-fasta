using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta
{
    public static class Config
    {
        public static string ConnectionStringFaverino = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=faverino;User ID=sa;Password=sa";

        //var connectionStringDatamart = "Data Source=CIBERMAN-PC;Initial Catalog=faverino_datamart;User ID=javier;Password=javier";
        public static string ConnectionStringDatamart = "Data Source=CIBERMAN-PC;Integrated Security=SSPI;Initial Catalog=faverino_datamart";
    }
}

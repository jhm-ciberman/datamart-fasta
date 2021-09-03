using DataMartFasta.ETL;
using DataMartFasta.Views;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DataMartFasta
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var connectionStringFaverino = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=faverino;User ID=sa;Password=sa";
            var connectionFaverino = new SqlConnection(connectionStringFaverino);

            //var connectionStringDatamart = "Data Source=CIBERMAN-PC;Initial Catalog=faverino_datamart;User ID=javier;Password=javier";

            var connectionStringDatamart = "Data Source = CIBERMAN-PC; Integrated Security = SSPI; Initial Catalog = faverino_datamart";
            var connectionDatamart = new SqlConnection(connectionStringDatamart);

            var compiler = new SqlServerCompiler();
            var dbFaverino = new QueryFactory(connectionFaverino, compiler);
            var dbDatamart = new QueryFactory(connectionDatamart, compiler);
            dbDatamart.Logger = compiled => {
                Debug.WriteLine(compiled.ToString());
            };
            var dataWarehouse = new DataWarehouse(dbFaverino, dbDatamart);

            var window = new OlapCubeWindow(dbDatamart);
            this.MainWindow = window;
            window.Show();
        }
    }
}

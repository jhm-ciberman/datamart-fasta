using DataMartFasta.Definitions;
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
        internal static DataWarehouse DataWarehouse { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var connectionFaverino = new SqlConnection(Config.ConnectionStringFaverino);
            var connectionDatamart = new SqlConnection(Config.ConnectionStringDatamart);

            var compiler = new SqlServerCompiler();
            var dbFaverino = new QueryFactory(connectionFaverino, compiler);
            var dbDatamart = new QueryFactory(connectionDatamart, compiler);
            dbDatamart.Logger = compiled => {
                Debug.WriteLine(compiled.ToString());
            };

            // Sorry for the global variable! :D 
            App.DataWarehouse = new DataWarehouse(dbFaverino, dbDatamart);

            var olapCube = new OlapCube();
            var window = new OlapCubeWindow(olapCube, dbDatamart);
            this.MainWindow = window;
            window.Show();
        }
    }
}

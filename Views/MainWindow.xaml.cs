using DataMartFasta.ETL;
using DataMartFasta.ViewModels;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataMartFasta.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var connectionStringFaverino = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=faverino;User ID=sa;Password=sa";
            var connectionFaverino = new SqlConnection(connectionStringFaverino);

            var connectionStringDatamart = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=faverino_datamart;User ID=sa;Password=sa";
            var connectionDatamart = new SqlConnection(connectionStringDatamart);

            var compiler = new SqlServerCompiler();
            var dbFaverino = new QueryFactory(connectionFaverino, compiler);
            var dbDatamart = new QueryFactory(connectionDatamart, compiler);

            var dataWarehouse = new DataWarehouse(dbFaverino, dbDatamart);

            this.stockControl.DataContext = new StockViewModel(dataWarehouse);
            this.despachosControl.DataContext = new DespachosViewModel(dataWarehouse);
        }
    }
}

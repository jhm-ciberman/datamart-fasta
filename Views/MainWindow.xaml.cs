﻿using DataMartFasta.ETL;
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
        internal MainWindow(DataWarehouse dataWarehouse)
        {
            InitializeComponent();



            this.stockControl.DataContext = new StockViewModel(dataWarehouse);
            this.despachosControl.DataContext = new DespachosViewModel(dataWarehouse);
        }
    }
}

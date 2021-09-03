﻿using DataMartFasta.ETL;
using DataMartFasta.ViewModels;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataMartFasta.Views
{
    /// <summary>
    /// Interaction logic for OlapCubeWindow.xaml
    /// </summary>
    public partial class OlapCubeWindow : Window, IRecipient<PropertyChangedMessage<bool>>, IRecipient<PropertyChangedMessage<FactTableViewModel>>
    {
        internal OlapCubeWindow(QueryFactory dw)
        {
            InitializeComponent();

            WeakReferenceMessenger.Default.RegisterAll(this);
            this.DataContext = new OlapCubeViewModel(dw);
        }

        public OlapCubeViewModel ViewModel => (OlapCubeViewModel)this.DataContext;

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var treeView = (TreeView)sender;
            if (treeView.SelectedItem is FactAttributeViewModel attribute)
            {
                attribute.IsVisible = !attribute.IsVisible;
            }
        }

        private Dictionary<FactColumnViewModel, DataGridColumn> columns = new Dictionary<FactColumnViewModel, DataGridColumn>();

        public void OnColumnVisibilityUpdated(FactColumnViewModel factColumn)
        {
            if (factColumn.IsVisible == false)
            {
                if (this.columns.TryGetValue(factColumn, out var column))
                {
                    this.columns.Remove(factColumn);
                    this.dataGrid.Columns.Remove(column);
                }
            }
            else
            {
                var dataGridColumn = new DataGridTextColumn()
                {
                    Header = factColumn.DisplayName,
                    Binding = new Binding(factColumn.UniqueAliasName) { Mode = BindingMode.OneTime },
                    
                };
                this.columns.Add(factColumn, dataGridColumn);
                this.dataGrid.Columns.Add(dataGridColumn);
            }
        }

        public void Receive(PropertyChangedMessage<bool> message)
        {
            if (message.Sender is FactColumnViewModel columnVM
                && message.PropertyName == nameof(FactColumnViewModel.IsVisible))
            {
                this.OnColumnVisibilityUpdated(columnVM);
                this.RefreshQuery();
            }
        }

        public void RefreshQuery()
        {
            this.ViewModel.PerformQueryCommand.Execute(null);
        }

        public void Receive(PropertyChangedMessage<FactTableViewModel> message)
        {
            if (message.Sender is OlapCubeViewModel vm
                    && message.PropertyName == nameof(OlapCubeViewModel.SelectedFactTable))
            {
                this.columns.Clear();
                this.dataGrid.Columns.Clear();
                var factTable = vm.SelectedFactTable;

                foreach (var measure in factTable.MeasuresList)
                    this.OnColumnVisibilityUpdated(measure);
  
                foreach (var dimension in factTable.DimensionsList)
                    foreach (var attribute in dimension.Attributes)
                        this.OnColumnVisibilityUpdated(attribute);


                this.RefreshQuery();
            }
        }
    }
}
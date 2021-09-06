using DataMartFasta.Definitions;
using DataMartFasta.ETL;
using FontAwesome.Sharp;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DataMartFasta.ViewModels
{
    public class OlapCubeViewModel : ObservableRecipient
    {
        public List<FactTableViewModel> FactTablesList { get;  } = new List<FactTableViewModel>();

        private FactTableViewModel selectedfactTable;
        public FactTableViewModel SelectedFactTable { get => this.selectedfactTable; set => this.SetProperty(ref this.selectedfactTable, value, true); }

        public IRelayCommand PerformQueryCommand { get; }

        public IRelayCommand AddConditionCommand { get; }

        private QueryFactory dw;

        internal OlapCubeViewModel(OlapCube olapCube, QueryFactory dw)
        {
            this.dw = dw;

            foreach (var fact in olapCube.FactTables)
            {
                this.FactTablesList.Add(new FactTableViewModel(this.dw, fact));
            }

            this.PerformQueryCommand = new RelayCommand(this.PerformQuery, () => this.SelectedFactTable != null);

            this.selectedfactTable = this.FactTablesList.FirstOrDefault();
        }

        protected void PerformQuery()
        {
            this.SelectedFactTable.PerformQuery();
        }
    }
}

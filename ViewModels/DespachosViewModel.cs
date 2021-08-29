using DataMartFasta.ETL;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataMartFasta.ViewModels
{
    public class DespachosViewModel : ObservableObject
    {
        public IRelayCommand RunCommand { get; }

        private DataWarehouse dw;

        public string ButtonText => "Transpasar Despachos";

        private int total = 0;
        public int Total { get => this.total; set => this.SetProperty(ref this.total, value); }

        private int current = 0;
        public int Current { get => this.current; set => this.SetProperty(ref this.current, value); }

        private string description = "";
        public string Description { get => this.description; set => this.SetProperty(ref this.description, value); }

        internal DespachosViewModel(DataWarehouse dw)
        {
            this.dw = dw;
            this.RunCommand = new RelayCommand(this.Run, () => this.etl == null);
        }

        private FactDespachosETL etl = null;

        private void Run()
        {
            Task.Run(this.RunAsync)
                .ContinueWith((t, vm) =>
                {
                    ((DespachosViewModel)vm).Description = t.Exception.Message;
                }, this, TaskContinuationOptions.OnlyOnFaulted);
        }

        public async Task RunAsync()
        {
            if (this.etl != null) return;

            this.Description = $"Empezando...";

            this.etl = new FactDespachosETL(this.dw);
            this.etl.OnProgressChange += this.OnProgressChange;

            await this.etl.Run();

            this.etl.OnProgressChange -= this.OnProgressChange;
            this.etl = null;
            this.Description = $"Finalizado";
        }

        private void OnProgressChange()
        {
            this.Total = this.etl.Total;
            this.Current = this.etl.Current;
            this.Description = $"Progreso: {this.Current} de {this.Total}";
            Debug.WriteLine(this.Description);
        }
    }
}

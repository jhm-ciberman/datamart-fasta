using DataMartFasta.Definitions;
using Humanizer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ViewModels
{
    public class FactColumnViewModel : ObservableRecipient
    {
        private bool isVisible = false;

        /// <summary>
        /// Indicates if this measure is shown in the main table
        /// </summary>
        public bool IsVisible { get => this.isVisible; set => this.SetProperty(ref this.isVisible, value, broadcast: true); }

        /// <summary>
        /// Hides the column
        /// </summary>
        public IRelayCommand HideCommand { get; }

        /// <summary>
        /// The column friendly name to show to the user
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// The alias name that will be used in the select statement. Example: "sum(ColumnName) as UniqueAliasName".
        /// This alias name must be guaranteed to be unique. Normally you wouldn't set this value manually. 
        /// It will be constructed based on the column's name and a Guid. 
        /// </summary>
        public string BindingName { get; }


        public FactColumnViewModel(string displayName, string columnName)
        {
            this.DisplayName = displayName;
            this.BindingName = columnName + "_" + Guid.NewGuid().ToString("N"); // Unique!
            this.HideCommand = new RelayCommand(this.Hide);
        }

        private void Hide()
        {
            this.IsVisible = false;
        }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}

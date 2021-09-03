using DataMartFasta.Definitions;
using Humanizer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ViewModels
{
    public class FactColumnViewModel : ObservableRecipient
    {
        /// <summary>
        /// The name of the measure column in the database
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// The friendly name that is shown to the user
        /// </summary>
        public string DisplayName { get; }


        private bool isVisible = false;

        /// <summary>
        /// Indicates if this measure is shown in the main table
        /// </summary>
        public bool IsVisible { get => this.isVisible; set => this.SetProperty(ref this.isVisible, value, broadcast: true); }

        /// <summary>
        /// The alias name that will be used in the select statement. Example: "sum(ColumnName) as UniqueAliasName"
        /// </summary>
        public string UniqueAliasName { get; }

        public FactColumnViewModel(ColumnDefinition definition)
        {
            this.ColumnName = definition.ColumnName;
            this.DisplayName = definition.DisplayName;
            this.UniqueAliasName = definition.ColumnName + "_" + Guid.NewGuid().ToString("N"); // Unique!
        }

        public FactColumnViewModel(string columnName, string displayName = null)
        {
            this.ColumnName = columnName;
            this.DisplayName = displayName ?? columnName.Humanize();
            this.UniqueAliasName = columnName + "_" + Guid.NewGuid().ToString("N"); // Unique!
        }
    }
}

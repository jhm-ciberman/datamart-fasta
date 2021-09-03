using DataMartFasta.Definitions;
using Humanizer;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataMartFasta.ViewModels
{
    public class FactTableViewModel
    {
        public string DisplayName { get; }
        public string TableName { get; }

        private List<FactDimensionViewModel> dimensionsList = new List<FactDimensionViewModel>();
        public IReadOnlyList<FactDimensionViewModel> DimensionsList => this.dimensionsList;

        private List<FactMeasureViewModel> measuresList = new List<FactMeasureViewModel>();
        public IReadOnlyList<FactMeasureViewModel> MeasuresList => this.measuresList;

        public IRelayCommand AddAttributeCommand { get; }

        public FactTableViewModel(string tableName, string displayName = null)
        {
            this.TableName = tableName;
            this.DisplayName = displayName ?? tableName.Humanize();
        }

        public FactTableViewModel WithDimension(DimensionDefinition dimension, string foreignKeyName, string displayName = null, bool addDimensionNameToAttributes = false)
        {
            this.dimensionsList.Add(new FactDimensionViewModel(this, dimension, foreignKeyName, displayName, addDimensionNameToAttributes));
            return this;
        }

        public FactTableViewModel WithMeasure(string columnName, string displayName = null)
        {
            this.measuresList.Add(new FactMeasureViewModel(columnName, displayName));
            return this;
        }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
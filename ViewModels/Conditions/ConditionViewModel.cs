using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SqlKata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DataMartFasta.ViewModels
{
    public class ConditionViewModel : ObservableValidator
    {
        
        private FactTableViewModel factTable;

        public IList<FactDimensionViewModel> DimensionsList => this.factTable.DimensionsList;


        private FactDimensionViewModel selectedDimension = null;

        [Required]
        public FactDimensionViewModel SelectedDimension 
        { 
            get => this.selectedDimension;
            set
            {
                this.SetProperty(ref this.selectedDimension, value, true);
                this.AttributesList = value.Attributes;
                this.SelectedAttribute = value.Attributes.FirstOrDefault();
                this.OnPropertyChanged(nameof(this.HasSelectedDimension));
                this.OnConditionChanged();
            }
        }

        private FactAttributeViewModel selectedAttribute = null;

        [Required]
        public FactAttributeViewModel SelectedAttribute 
        { 
            get => this.selectedAttribute;
            set
            {
                this.SetProperty(ref this.selectedAttribute, value, true);
                this.OnConditionChanged();
            }
        }

        public Operator selectedOperator = null;

        [Required]
        public Operator SelectedOperator
        {
            get => this.selectedOperator;
            set
            {
                this.SetProperty(ref this.selectedOperator, value, true);
                this.OnConditionChanged();
            }
        }

        private string filterExpression = "";
        public string FilterExpression
        {
            get => this.filterExpression;
            set
            {
                this.SetProperty(ref this.filterExpression, value, true);
                this.OnConditionChanged();
            }
        }

        private bool conditionIsEnabled = false;
        public bool ConditionIsEnabled
        {
            get => this.conditionIsEnabled;
            set
            {
                this.SetProperty(ref this.conditionIsEnabled, value);
                this.OnConditionChanged();
            }
        }

        private IEnumerable<FactAttributeViewModel> attributesList = null;
        public IEnumerable<FactAttributeViewModel> AttributesList { get => this.attributesList; protected set => this.SetProperty(ref this.attributesList, value); }

        public IRelayCommand RemoveItselfCommand { get; }

        public ConditionViewModel(FactTableViewModel factTable)
        {
            this.factTable = factTable;
            this.SelectedOperator = this.OperatorsList.FirstOrDefault();
            this.RemoveItselfCommand = new RelayCommand(this.RemoveItself);
        }


        public IList<Operator> OperatorsList { get; } = new List<Operator> 
        { 
            new Operator("Igual a", (query, column, value) => query.Where(column, "=", value)), 
            new Operator("Distinto a", (query, column, value) => query.Where(column, "<>", value)), 
            new Operator("Menor que", (query, column, value) => query.Where(column, "<", value)), 
            new Operator("Mayor que", (query, column, value) => query.Where(column, ">", value)),
            new Operator("Que empiece con", (query, column, value) => query.Where(column, "like", "%" + value)),
            new Operator("Que termine con", (query, column, value) => query.Where(column, "like", value + "%")),
            new Operator("Que contenga", (query, column, value) => query.Where(column, "like", "%" + value + "%")),
        };

        protected void OnConditionChanged()
        {
            this.factTable.PerformQuery();
        }

        public bool HasSelectedDimension => this.SelectedDimension != null;

        public void ApplyScope(Query query, List<FactDimensionViewModel> includedDimensions, string factTable)
        {
            if (! this.ConditionIsEnabled) return;

            this.ValidateAllProperties();

            if (this.HasErrors) return;

            if (! includedDimensions.Contains(this.SelectedDimension))
            {
                this.SelectedDimension.ApplyScope(query, includedDimensions, factTable);
            }

            var column = this.SelectedDimension.UniqueAliasName + "." + this.SelectedAttribute.Column.Name;

            this.SelectedOperator.Callback.Invoke(query, column, this.FilterExpression);
        }


        private void RemoveItself()
        {
            this.factTable.RemoveCondition(this);
        }


    }
}

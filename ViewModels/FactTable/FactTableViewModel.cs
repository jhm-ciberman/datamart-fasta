using DataMartFasta.Definitions;
using Humanizer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace DataMartFasta.ViewModels
{
    public class FactTableViewModel : ObservableObject
    {
        private FactTable table;

        public QueryFactory queryFactory;

        public List<FactMeasureViewModel> MeasuresList { get; } = new List<FactMeasureViewModel>();

        public List<FactDimensionViewModel> DimensionsList { get; } = new List<FactDimensionViewModel>();

        public ObservableCollection<ConditionViewModel> ConditionsList { get; } = new ObservableCollection<ConditionViewModel>();

        public IRelayCommand AddConditionCommand { get; }

        private IList<dynamic> queryResults = new List<dynamic>();
        public IList<dynamic> QueryResults { get => this.queryResults; set => this.SetProperty(ref this.queryResults, value); }

        private string queryStatus = "";
        public string QueryStatus { get => this.queryStatus; protected set => this.SetProperty(ref this.queryStatus, value); }


        public FactTableViewModel(QueryFactory queryFactory, FactTable table)
        {
            this.queryFactory = queryFactory;
            this.table = table;

            foreach (var measure in this.table.MeasuresList)
            {
                this.MeasuresList.Add(new FactMeasureViewModel(measure));
            }

            foreach (var dimension in this.table.DimensionsList)
            {
                this.DimensionsList.Add(new FactDimensionViewModel(dimension));
            }

            this.AddConditionCommand = new RelayCommand(this.AddCondition);
        }

        private void AddCondition()
        {
            this.ConditionsList.Add(new ConditionViewModel(this));
        }

        public override string ToString()
        {
            return this.table.DisplayName;
        }


        public void PerformQuery()
        {
            var query = this.queryFactory.Query(this.table.TableName);


            var visibleDimensions = this.DimensionsList.Where(dim => dim.Attributes.Any(attr => attr.IsVisible));
            var visibleMeasures = this.MeasuresList.Where(measure => measure.IsVisible);

            if (!visibleDimensions.Any() && !visibleMeasures.Any())
            {
                this.QueryResults = new dynamic[0];
                return;
            }

            var factTableName = this.table.TableName;
            var includedDimensions = new List<FactDimensionViewModel>();

            foreach (var dimension in visibleDimensions)
            {
                dimension.ApplyScope(query, includedDimensions, factTableName);
            }

            foreach (var measure in visibleMeasures)
            {
                measure.ApplyScope(query, includedDimensions, factTableName);
            }

            foreach (var condition in this.ConditionsList)
            {
                condition.ApplyScope(query, includedDimensions, factTableName);
            }

            var sw = Stopwatch.StartNew();
            var results = query.Get().ToList();
            var time = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds);

            this.QueryResults = results;
            this.QueryStatus = $"{ results.Count } resultados en {time.Humanize()}";
        }

        internal void RemoveCondition(ConditionViewModel whereConditionViewModel)
        {
            this.ConditionsList.Remove(whereConditionViewModel);
            this.PerformQuery();
        }
    }
}
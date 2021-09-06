using DataMartFasta.Definitions;
using Humanizer;
using Microsoft.Toolkit.Mvvm.Input;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataMartFasta.ViewModels
{
    public class FactTableViewModel
    {
        private FactTable table;

        public List<FactMeasureViewModel> MeasuresList { get; } = new List<FactMeasureViewModel>();

        public List<FactDimensionViewModel> DimensionsList { get; } = new List<FactDimensionViewModel>();

        public FactTableViewModel(FactTable table)
        {
            this.table = table;

            foreach (var measure in this.table.MeasuresList)
            {
                this.MeasuresList.Add(new FactMeasureViewModel(measure));
            }

            foreach (var dimension in this.table.DimensionsList)
            {
                this.DimensionsList.Add(new FactDimensionViewModel(dimension));
            }
        }

        public override string ToString()
        {
            return this.table.DisplayName;
        }


        public IEnumerable<dynamic> PerformQuery(QueryFactory queryFactory)
        {
            var query = queryFactory.Query(this.table.TableName);


            var visibleDimensions = this.DimensionsList.Where(dim => dim.Attributes.Any(attr => attr.IsVisible));
            var visibleMeasures = this.MeasuresList.Where(measure => measure.IsVisible);

            if (!visibleDimensions.Any() && !visibleMeasures.Any())
            {
                return new dynamic[0];
            }

            var factTableName = this.table.TableName;

            foreach (var dimension in visibleDimensions)
            {
                dimension.ApplyScope(query, factTableName);
            }

            foreach (var measure in visibleMeasures)
            {
                measure.ApplyScope(query, factTableName);
            }

            return query.Get();
        }
    }
}
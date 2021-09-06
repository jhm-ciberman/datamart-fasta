using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.Definitions
{
    public class FactTable : Table
    {
        /// <summary>
        /// Creates a new fact table in a fluent way.
        /// </summary>
        /// <param name="tableName">The dimension's table name</param>
        /// <param name="displayName">A friendly name to use for this dimension. if ommited it will be inferred</param>
        /// <param name="icon">An icon for this dimension. If omited, no icon will be used.</param>
        /// <returns></returns>
        public static FactTable Make(string tableName, string displayName = null, IconChar? icon = null)
        {
            return new FactTable(tableName, displayName, icon);
        }

        private List<FactDimensionColumn> dimensionsList = new List<FactDimensionColumn>();
        public IReadOnlyList<FactDimensionColumn> DimensionsList => this.dimensionsList;

        private List<FactMeasureColumn> measuresList = new List<FactMeasureColumn>();
        public IReadOnlyList<FactMeasureColumn> MeasuresList => this.measuresList;

        public FactTable(string tableName, string displayName = null, IconChar? icon = null) : base(tableName, displayName, icon)
        {

        }

        public FactTable WithDimension(DimensionTable dimension, string foreignKeyName, string displayName = null, bool addDimensionNameToAttributes = false)
        {
            this.dimensionsList.Add(new FactDimensionColumn(dimension, foreignKeyName, displayName, addDimensionNameToAttributes));
            return this;
        }

        public FactTable WithMeasure(string columnName, string displayName = null)
        {
            this.measuresList.Add(new FactMeasureColumn(columnName, displayName));
            return this;
        }
    }
}

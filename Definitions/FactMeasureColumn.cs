using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.Definitions
{
    public class FactMeasureColumn : Column
    {
        /// <summary>
        /// The SQL aggregate function used to get the results for this measure
        /// </summary>
        public string Aggregate { get; }

        public FactMeasureColumn(string columnName, string displayName = null, string aggregate = "sum") : base(columnName, displayName)
        {
            this.Aggregate = aggregate;
        }
    }
}

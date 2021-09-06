using SqlKata;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.Definitions
{
    public class DimensionAttributeColumn : Column
    {
        public DimensionTable DimensionTable { get; }

        public DimensionAttributeColumn(DimensionTable dimensionTable, string columnName, string displayName = null) : base(columnName, displayName)
        {
            this.DimensionTable = dimensionTable;
        }
    }
}

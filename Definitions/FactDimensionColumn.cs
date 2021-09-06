using SqlKata;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.Definitions
{
    public class FactDimensionColumn : Column
    {
        public DimensionTable DimensionTable { get; }

        public bool AddDimensionNameToAttributes { get; }

        public IEnumerable<DimensionAttributeColumn> AttributeColumns => this.DimensionTable.Attributes;

        public FactDimensionColumn(DimensionTable dimensionTable, string columnName, string displayName = null, bool addDimensionNamesToAttribute = false) 
            : base(columnName, displayName)
        {
            this.DimensionTable = dimensionTable;
            this.AddDimensionNameToAttributes = addDimensionNamesToAttribute;
        }
    }
}

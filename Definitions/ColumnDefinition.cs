using Humanizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.Definitions
{
    public class ColumnDefinition
    {
        public string ColumnName { get; }

        public string DisplayName { get; }

        public ColumnDefinition(string columnName, string displayName = null)
        {
            this.ColumnName = columnName;
            this.DisplayName = displayName ?? columnName.Humanize(LetterCasing.Sentence);
        }
    }
}

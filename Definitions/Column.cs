using Humanizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.Definitions
{
    public abstract class Column
    {
        /// <summary>
        /// The name of the measure column in the database
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The friendly name that is shown to the user
        /// </summary>
        public string DisplayName { get; }

        public Column(string columnName, string displayName = null)
        {
            this.Name = columnName;
            this.DisplayName = displayName ?? columnName.Humanize(LetterCasing.Sentence);
        }
    }
}

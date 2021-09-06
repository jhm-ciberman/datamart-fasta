using FontAwesome.Sharp;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.Definitions
{
    public abstract class Table
    {
        /// <summary>
        /// The table's name
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// The user friendly display name for this table
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// An icon for this table
        /// </summary>
        public IconChar Icon { get; }

        public Table(string tableName, string displayName = null, IconChar? icon = null)
        {
            this.TableName = tableName;
            this.DisplayName = displayName ?? tableName.Humanize(LetterCasing.Sentence);
            this.Icon = icon ?? IconChar.None;
        }
    }
}

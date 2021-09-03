using FontAwesome.Sharp;
using Humanizer;
using System.Collections.Generic;

namespace DataMartFasta.Definitions
{
    public class DimensionDefinition
    {
        public string DisplayName { get; }

        public IconChar Icon { get; }

        public string TableName { get; }

        private List<ColumnDefinition> attributes = new List<ColumnDefinition>();
        public IReadOnlyList<ColumnDefinition> Attributes => this.attributes;

        public string PrimaryKeyColumnName { get; internal set; } = "id";

        public DimensionDefinition(string tableName, string displayName = null, IconChar? icon = null)
        {
            this.TableName = tableName;
            this.DisplayName = displayName ?? tableName.Humanize(LetterCasing.Sentence);
            this.Icon = icon ?? IconChar.None;
        }

        public static DimensionDefinition Make(string tableName, string displayName = null, IconChar? icon = null)
        {
            return new DimensionDefinition(tableName, displayName, icon);
        }

        public DimensionDefinition WithAttribute(string columnName, string displayName = null)
        {
            this.attributes.Add(new ColumnDefinition(columnName, displayName));
            return this;
        }

    }
}
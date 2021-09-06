using DataMartFasta.Definitions;
using FontAwesome.Sharp;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ViewModels
{
    public class FactDimensionViewModel
    {
        public FactDimensionColumn Column { get; }

        public List<FactAttributeViewModel> attributes = new List<FactAttributeViewModel>();
        public IReadOnlyList<FactAttributeViewModel> Attributes => this.attributes;

        public IconChar Icon => this.Column.DimensionTable.Icon;

        public string DisplayName => this.Column.DisplayName;

        public string UniqueAliasName { get; }

        public FactDimensionViewModel(FactDimensionColumn column)
        {
            this.Column = column;
            this.UniqueAliasName = this.Column.DimensionTable.TableName + "_" + Guid.NewGuid().ToString("N"); // Unique! 

            foreach (var attribute in column.DimensionTable.Attributes)
            {
                var displayName = column.AddDimensionNameToAttributes
                    ? $"{attribute.DisplayName} ({this.Column.DisplayName})"
                    : attribute.DisplayName;

                this.attributes.Add(new FactAttributeViewModel(attribute, displayName));
            }
        }

        public void ApplyScope(Query query, List<FactDimensionViewModel> includedDimensions, string factTableName)
        {
            includedDimensions.Add(this);

            var dimensionTable = this.Column.DimensionTable;
            var tableAlias = this.UniqueAliasName;
            var foreignKey = factTableName + "." + this.Column.Name;
            var primaryKey = tableAlias + "." + dimensionTable.PrimaryKeyColumnName;

            query.Join(dimensionTable.TableName + " as " + tableAlias, foreignKey, primaryKey);

            foreach (var attribute in this.Attributes)
            {
                if (attribute.IsVisible)
                {
                    query.GroupBy(this.UniqueAliasName + "." + attribute.Column.Name);
                    query.Select(this.UniqueAliasName + "." + attribute.Column.Name + " as " + attribute.BindingName);
                }
            }
        }
    }
}

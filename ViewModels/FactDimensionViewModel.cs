using DataMartFasta.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ViewModels
{
    public class FactDimensionViewModel
    {
        public FactTableViewModel FactTable { get; }

        private DimensionDefinition definition;

        public string ForeignKeyColumnName { get; }

        public string DisplayName { get; }

        public FontAwesome.Sharp.IconChar Icon => this.definition.Icon;

        public List<FactAttributeViewModel> attributes = new List<FactAttributeViewModel>();
        public IReadOnlyList<FactAttributeViewModel> Attributes => this.attributes;

        public string PrimaryKeyColumnName => this.definition.PrimaryKeyColumnName;

        public string TableName => this.definition.TableName;

        /// <summary>
        /// The alias name that will be used in the joins statements.
        /// </summary>
        public string UniqueAliasName { get; }

        public FactDimensionViewModel(FactTableViewModel factTable, DimensionDefinition definition, string foreignKeyColumnName, string displayName = null, bool addDimensionNameToAttributes = false)
        {
            this.FactTable = factTable;
            this.definition = definition;
            this.ForeignKeyColumnName = foreignKeyColumnName;
            this.DisplayName = displayName ?? definition.DisplayName;
            this.UniqueAliasName = definition.TableName + "_" + Guid.NewGuid().ToString("N"); // Unique!

            foreach (var attribute in definition.Attributes)
            {
                var attributeDisplayName = addDimensionNameToAttributes ? $"{attribute.DisplayName} ({this.DisplayName})" : attribute.DisplayName;
                this.attributes.Add(new FactAttributeViewModel(this, attribute.ColumnName, attributeDisplayName));
            }
        }
    }
}

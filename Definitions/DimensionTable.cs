using FontAwesome.Sharp;
using Humanizer;
using System.Collections.Generic;

namespace DataMartFasta.Definitions
{
    public class DimensionTable : Table
    {
        /// <summary>
        /// Creates a new dimension table in a fluent way.
        /// </summary>
        /// <param name="tableName">The dimension's table name</param>
        /// <param name="displayName">A friendly name to use for this dimension. if ommited it will be inferred</param>
        /// <param name="icon">An icon for this dimension. If omited, no icon will be used.</param>
        /// <returns></returns>
        public static DimensionTable Make(string tableName, string displayName = null, IconChar? icon = null)
        {
            return new DimensionTable(tableName, displayName, icon);
        }


        private List<DimensionAttributeColumn> attributes = new List<DimensionAttributeColumn>();

        public DimensionTable(string tableName, string displayName = null, IconChar? icon = null) : base(tableName, displayName, icon)
        {
        }


        /// <summary>
        /// A list of the attributes of this dimension
        /// </summary>
        public IEnumerable<DimensionAttributeColumn> Attributes => this.attributes;

        /// <summary>
        /// The primary key column name of this dimensions table. Normally, is the "id" column.
        /// </summary>
        public string PrimaryKeyColumnName { get; internal set; } = "id";

        /// <summary>
        /// Adds an attribute to this dimension
        /// </summary>
        /// <param name="columnName">The attribute column's name</param>
        /// <param name="displayName">A friendly name to show to the user. This value is optional and will be inferred from the column's name if omited.</param>
        /// <returns>This same dimension, for fluent chaining.</returns>
        public DimensionTable WithAttribute(string columnName, string displayName = null)
        {
            this.attributes.Add(new DimensionAttributeColumn(this, columnName, displayName));
            return this;
        }

    }
}
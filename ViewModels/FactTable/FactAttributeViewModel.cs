using DataMartFasta.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace DataMartFasta.ViewModels
{
    public class FactAttributeViewModel : FactColumnViewModel
    {
        public DimensionAttributeColumn Column { get; }

        public FactAttributeViewModel(DimensionAttributeColumn attributeColumn, string overrideDisplayName = null)
            : base(overrideDisplayName ?? attributeColumn.DisplayName, attributeColumn.Name)
        {
            this.Column = attributeColumn;
        }
    }
}

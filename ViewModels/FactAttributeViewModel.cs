using DataMartFasta.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace DataMartFasta.ViewModels
{
    public class FactAttributeViewModel : FactColumnViewModel
    {
        /// <summary>
        /// The Fact Dimension asociated to this attribute
        /// </summary>
        public FactDimensionViewModel Dimension { get; }


        public FactAttributeViewModel(FactDimensionViewModel dimension, string columnName, string displayName = null)
            : base(columnName, displayName)
        {
            this.Dimension = dimension;
        }
    }
}

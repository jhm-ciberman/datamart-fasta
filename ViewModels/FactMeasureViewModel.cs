using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Humanizer;
using DataMartFasta.Definitions;

namespace DataMartFasta.ViewModels
{
    public class FactMeasureViewModel : FactColumnViewModel
    {
        /// <summary>
        /// The SQL aggregate function used to get the results for this measure
        /// </summary>
        public string Aggregate = "sum";

        public FactMeasureViewModel(ColumnDefinition definition) : base(definition)
        {
        }

        public FactMeasureViewModel(string columnName, string displayName = null) : base(columnName, displayName)
        {
        }
    }
}
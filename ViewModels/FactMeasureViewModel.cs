using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Humanizer;
using DataMartFasta.Definitions;
using System.Collections.ObjectModel;
using SqlKata;

namespace DataMartFasta.ViewModels
{
    public class FactMeasureViewModel : FactColumnViewModel
    {
        private FactMeasureColumn column { get; }

        public FactMeasureViewModel(FactMeasureColumn definition) : base(definition.DisplayName, definition.Name)
        {
            this.column = definition;
        }

        public void ApplyScope(Query query, string factTableName)
        {
            query.SelectRaw(this.column.Aggregate + "(" + factTableName + "." + this.column.Name + ") as " + this.BindingName);
        }
    }
}
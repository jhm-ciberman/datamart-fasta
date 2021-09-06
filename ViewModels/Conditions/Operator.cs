using SqlKata;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ViewModels
{
    public class Operator
    {
        public delegate void WhereCallback(Query query, string column, object value);

        public Operator(string displayName, WhereCallback callback)
        {
            this.DisplayName = displayName;
            this.Callback = callback;
        }

        public string DisplayName { get; }

        public WhereCallback Callback { get; }
    }
}

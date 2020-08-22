using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class DefinitionValue
    {
        public string GoodDefinition { get; protected set; }
        public string TotalDefinition { get; protected set; }

        public DefinitionValue() { }
        public DefinitionValue(string good, string total) {
            this.TotalDefinition = total;
            this.GoodDefinition = good;
        }

        public string GetTotalDefinition(string definition) {
            return string.IsNullOrWhiteSpace(this.TotalDefinition) ? definition : this.TotalDefinition;
        }
        public string GetGoodDefinition(string definition) {
            return string.IsNullOrWhiteSpace(this.GoodDefinition) ? definition : this.GoodDefinition;
        }
    }
}

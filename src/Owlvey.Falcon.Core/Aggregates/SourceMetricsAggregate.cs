using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SourceMetricsAggregate
    {
        private readonly IEnumerable<SourceEntity> Sources;
        public SourceMetricsAggregate(IEnumerable<SourceEntity> sources) {
            this.Sources = sources;
        }
        public QualityMeasureValue Execute() {

            var temp = new List<QualityMeasureValue>();

            
            foreach (var source in this.Sources)
            {
                temp.Add(source.Measure());            
            }            
            
            return QualityMeasureValue.Merge(temp); 
        }

    }
}

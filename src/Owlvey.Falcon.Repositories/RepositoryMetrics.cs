using Prometheus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Repositories
{
    public static class RepositoryMetrics
    {
        public static readonly Histogram RelationAccessDuration = Metrics
                    .CreateHistogram("owlvey_relational_access_duration_ms", 
                                     "Histogram of relational processing durations.",
                                     configuration: new HistogramConfiguration() {
                                          
                                     }
        );
    }
}

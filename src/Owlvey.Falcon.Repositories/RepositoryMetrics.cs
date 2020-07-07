using Prometheus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Repositories
{
    public static class RepositoryMetrics
    {
        public static readonly Summary RelationAccessDuration = Metrics
                    .CreateSummary("owlvey_relational_access_duration_ms", 
                                     "Summary of relational processing durations.",
                                     configuration: new SummaryConfiguration() {
                                         Objectives = new[]
                                        {
                                            new QuantileEpsilonPair(0.5, 0.05),
                                            new QuantileEpsilonPair(0.9, 0.05),
                                            new QuantileEpsilonPair(0.95, 0.01),
                                            new QuantileEpsilonPair(0.99, 0.005),
                                        }
                                     }
        );
    }
}

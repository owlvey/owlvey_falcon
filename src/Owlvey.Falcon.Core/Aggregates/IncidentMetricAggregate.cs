using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class IncidentMetricAggregate
    {

        private IEnumerable<IncidentEntity> Incidents;

        public IncidentMetricAggregate( IEnumerable<IncidentEntity> incidents) {
            this.Incidents = incidents;
        }

        public (int mttd, int mtte, int mttf, int mttm) Metrics() {

            if (this.Incidents.Count() > 0)
            {
                return (
                    (int)this.Incidents.Average(c=>c.TTD),
                    (int)this.Incidents.Average(c => c.TTE),
                    (int)this.Incidents.Average(c => c.TTF),
                    (int)this.Incidents.Average(c => c.TTM));
            }
            else {
                return (0,0,0,0); 
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Models
{
    public class DashboardProductRp
    {
        public long SourceTotal { get; set; }
        public StatsValue SourceStats { get; set; } 
        public StatsValue FeaturesStats { get; set; }
        public decimal FeaturesCoverage { get; set; }
        public StatsValue ServicesStats { get; set; }

        public int SLOFail { get; set; }
        public decimal SLOProportion { get; set; }
               
        public List<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
        public List<SourceGetListRp> Sources { get; set; } = new List<SourceGetListRp>();
        public List<ServiceGetListRp> Services { get; set; } = new List<ServiceGetListRp>();

        public List<SquadGetListRp> Squads { get; set; } = new List<SquadGetListRp>();

        

        public Dictionary<int, List<int>> ServiceMaps { get; set; } = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> FeatureMaps { get; set; } = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> SquadMaps { get; set; } = new Dictionary<int, List<int>>();

        public Dictionary<int, object> IncidentInformation { get; set; } = new Dictionary<int, object>();
    }
}

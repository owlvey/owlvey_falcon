using System.Collections.Generic;
using System.Text.Json.Serialization;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Models
{ 

    public class OperationProductDashboardRp
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

        [JsonConverter(typeof(JsonIDictionaryConverter<int, List<int>>))]
        public IDictionary<int, List<int>> ServiceMaps { get; set; } = new Dictionary<int, List<int>>();
        [JsonConverter(typeof(JsonIDictionaryConverter<int, List<int>>))]
        public IDictionary<int, List<int>> FeatureMaps { get; set; } = new Dictionary<int, List<int>>();
        [JsonConverter(typeof(JsonIDictionaryConverter<int, List<int>>))]
        public IDictionary<int, List<int>> SquadMaps { get; set; } = new Dictionary<int, List<int>>();
        [JsonConverter(typeof(JsonIDictionaryConverter<int, object>))]
        public IDictionary<int, object> IncidentInformation { get; set; } = new Dictionary<int, object>();
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Models
{ 

    public class OperationProductDashboardRp
    {        
        public StatsValue SourceStats { get; set; }
        public StatsValue FeaturesStats { get; set; }
        public decimal FeaturesCoverage { get; set; }
        public StatsValue JourneysStats { get; set; }
        public int SLOFail { get; set; }
        public decimal SLOProportion { get; set; }
        public List<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
        public List<SourceGetListRp> Sources { get; set; } = new List<SourceGetListRp>();
        public List<JourneyGetListRp> Journeys { get; set; } = new List<JourneyGetListRp>();
        public List<SquadGetListRp> Squads { get; set; } = new List<SquadGetListRp>();

        [JsonConverter(typeof(JsonIDictionaryConverter<int, List<int>>))]
        public IDictionary<int, List<int>> JourneyMaps { get; set; } = new Dictionary<int, List<int>>();
        [JsonConverter(typeof(JsonIDictionaryConverter<int, List<int>>))]
        public IDictionary<int, List<int>> FeatureMaps { get; set; } = new Dictionary<int, List<int>>();
        [JsonConverter(typeof(JsonIDictionaryConverter<int, List<int>>))]
        public IDictionary<int, List<int>> SquadMaps { get; set; } = new Dictionary<int, List<int>>();
        [JsonConverter(typeof(JsonIDictionaryConverter<int, object>))]
        public IDictionary<int, object> IncidentInformation { get; set; } = new Dictionary<int, object>();
    }
}

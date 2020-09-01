using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System;
using System.Text.Json.Serialization;

namespace Owlvey.Falcon.Models
{
    public class IndicatorBaseRp
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public int SourceId { get; set; }
    }
    public class IndicatorMigrateRp {
        public string Product { get; set; }
        public string Feature { get; set; }
        public string Source { get; set; }        
    }
    public class IndicatorGetRp : IndicatorBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }        
        public int FeatureId { get; set; }        
        public string SourceAvatar { get; set; }
        public string Feature { get; set; }
        public string FeatureAvatar { get; set; }
    }

    public class IndicatorGetListRp : IndicatorBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }        
        public int FeatureId { get; set; }
        
        
    }

    public class IndicatorDetailRp : IndicatorGetListRp
    {
        public QualityMeasureValue Measure { get; set; }
        public string ReliabilityRiskLabel { get; set; }
        public string SecurityRiskLabel { get; set; }
        public decimal SecurityRisk { get; set; }
        public decimal ReliabilityRisk { get; set; }
    }

    public class IndicatorPostRp
    {
        public int FeatureId { get; set; }
        public int SourceId { get; set; }
    }

    public class IndicatorPutRp
    {
        public string Value { get; set; }
    }
}

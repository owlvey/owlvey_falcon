﻿using Owlvey.Falcon.Core.Entities;
using System;
using System.Text.Json.Serialization;

namespace Owlvey.Falcon.Models
{
    public class IndicatorBaseRp
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SourceKindEnum Kind { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SourceGroupEnum Group { get; set; }

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

    public class IndicatorAvailabilityGetListRp : IndicatorGetListRp
    {
        public decimal Measure { get; set; }        
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

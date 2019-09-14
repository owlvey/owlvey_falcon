﻿using System;
namespace Owlvey.Falcon.Models
{
    public class IndicatorBaseRp
    {
        public int Id { get; set; }        
    }
    public class IndicatorMigrateRp {
        public string Product { get; set; }
        public string Source { get; set; }
        public string Feature { get; set; }
    }
    public class IndicatorGetRp : IndicatorBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int SourceId { get; set; }
        public int FeatureId { get; set; }
        public string Source { get; set; }
        public string SourceAvatar { get; set; }
        public string Feature { get; set; }
        public string FeatureAvatar { get; set; }
    }

    public class IndicatorGetListRp : IndicatorBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int SourceId { get; set; }
        public int FeatureId { get; set; }
        public string Source { get; set; }
        public decimal Availability { get; set; }
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

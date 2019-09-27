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
        public StatsValue ServicesStats { get; set; } 
               
        public List<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
        public List<SourceGetListRp> Sources { get; set; } = new List<SourceGetListRp>();
        public List<ServiceGetListRp> Services { get; set; } = new List<ServiceGetListRp>();        
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class DashboardProductRp
    {
        public int SourceCount { get; set; }
        public int SourceTotal { get; set; }
        public decimal SourceAvailability { get; set; }


        public List<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
        public List<SourceGetListRp> Sources { get; set; } = new List<SourceGetListRp>();
    }
}

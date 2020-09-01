using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Models
{
    public class ProductDashboardRp {

        public int SourceTotal { get; set; }
        public int SourceAssigned { get; set; }

        public decimal SourceCoverage {
            get
            {
                return QualityUtils.CalculateProportion(this.SourceTotal, this.SourceAssigned);
            }
        }

        public int FeatureTotal { get; set; }
        public int FeatureAssigned { get; set; }

        public decimal FeatureCoverage {
            get
            {
                return QualityUtils.CalculateProportion(this.FeatureTotal, this.FeatureAssigned);
            }
        }

        public List<JourneyGroupRp> Groups { get; set; } = new List<JourneyGroupRp>();

        public class JourneyGroupRp {
            public string Name { get; set; }
            public decimal Proportion {
                get
                {
                    return QualityUtils.CalculateFailProportion(this.Total, this.Fail);
                }
            }
            public int Total { get; set; }
            public int Fail { get; set; }
        }
    }    
}
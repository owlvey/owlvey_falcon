﻿using System;
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
                return AvailabilityUtils.CalculateProportion(this.SourceTotal, this.SourceAssigned);
            }
        }

        public int FeatureTotal { get; set; }
        public int FeatureAssigned { get; set; }

        public decimal FeatureCoverage {
            get
            {
                return AvailabilityUtils.CalculateProportion(this.FeatureTotal, this.FeatureAssigned);
            }
        }

        public List<ServiceGroupRp> Groups { get; set; } = new List<ServiceGroupRp>();

        public class ServiceGroupRp {
            public string Name { get; set; }
            public decimal Proportion {
                get
                {
                    return AvailabilityUtils.CalculateFailProportion(this.Total, this.Fail);
                }
            }
            public int Total { get; set; }
            public int Fail { get; set; }
        }
    }    
}
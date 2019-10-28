using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Models
{
    public class ProductDashboardRp {

        public List<ServiceGroupRp> groups { get; set; } = new List<ServiceGroupRp>();

        public class ServiceGroupRp {
            public string name { get; set; }
            public decimal proportion { get {
                    return AvailabilityUtils.CalculateFailProportion(this.total, this.fail);
                } }
            public int total { get; set; }
            public int fail { get; set; }
        }
    }

    
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class FeatureBaseRp
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class FeatureGetRp : FeatureBaseRp {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class FeatureGetListRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class FeaturePostRp {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class FeaturePutRp
    {
        public string Value { get; set; }
    }
}

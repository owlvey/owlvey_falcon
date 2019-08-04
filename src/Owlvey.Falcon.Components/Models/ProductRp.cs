using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{
    public class ProductBaseRp
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ProductGetRp : ProductBaseRp {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class ProductGetListRp : ProductBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class ProductPostRp {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ProductPutRp
    {
        public string Value { get; set; }
    }
}

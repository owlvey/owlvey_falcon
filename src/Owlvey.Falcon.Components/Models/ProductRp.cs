using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class ProductBaseRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
        public string Name { get; set; }
        public int CustomerId { get; set; }
    }

    public class ProductPutRp
    {
        public string Value { get; set; }
    }
}

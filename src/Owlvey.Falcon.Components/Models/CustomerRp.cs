using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class CustomerBaseRp
    {

    }

    public class CustomerGetRp : CustomerBaseRp {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class CustomerGetListRp : CustomerBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class CustomerPostRp {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class CustomerPutRp
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}

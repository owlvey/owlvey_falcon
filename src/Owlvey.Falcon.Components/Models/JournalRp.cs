using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{
    public class JournalBaseRp
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class JournalGetRp : JournalBaseRp {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class JournalGetListRp : JournalBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class JournalPostRp {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class JournalPutRp
    {
        public string Value { get; set; }
    }
}

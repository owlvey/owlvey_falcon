using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class MemberBaseRp
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }

    public class MemberGetRp : MemberBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class MemberGetListRp : MemberBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class MemberPostRp
    {
        public int SquadId { get; set; }
        public int UserId { get; set; }
    }

    public class MemberPutRp
    {
        public string Value { get; set; }
    }
}

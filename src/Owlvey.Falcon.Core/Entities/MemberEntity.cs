using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class MemberEntity: BaseEntity
    {        
        public virtual SquadEntity Squad { get; set; }
        public int SquadId { get; set; }
        public virtual UserEntity User { get; set; }
        public int UserId { get; set; }        
    }
}

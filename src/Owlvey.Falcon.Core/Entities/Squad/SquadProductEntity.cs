using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadProductEntity : BaseEntity
    {
        public int SquadId { get; set; }
        public virtual SquadEntity Squad { get; set; }

        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }
    }
}

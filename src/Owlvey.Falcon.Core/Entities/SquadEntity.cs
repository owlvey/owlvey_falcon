using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadEntity: BaseEntity
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Avatar { get; protected set; }

        public virtual ICollection<UserEntity> Users { get; set; }

        public virtual CustomerEntity Customer { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadEntity: BaseEntity
    {
        [Required]
        public string Name { get; protected set; }
        [Required]
        public string Description { get; protected set; }
        public string Avatar { get; protected set; }

        public virtual ICollection<UserEntity> Users { get; set; }

    }
}

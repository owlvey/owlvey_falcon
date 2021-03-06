﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity : BaseEntity
    {
        [Required]        
        public string Name { get; set; }

        [Required]
        public string Avatar { get; set; }

        public string Leaders { get; set; }

        public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
        public virtual ICollection<SquadEntity> Squads { get; set; } = new List<SquadEntity>();

        public void Update(DateTime on, string modifiedBy, string name= null, string avatar = null, string leaders= null) {
            this.Leaders = leaders ?? this.Leaders;
            this.Name = name ?? this.Name;
            this.Avatar = avatar ?? this.Avatar;
            this.ModifiedBy = modifiedBy;
            this.ModifiedOn = on;
            this.Validate();
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadEntity: BaseEntity
    {
        [Required]
        public string Name { get; protected set; }
        
        public string Description { get; protected set; }
        public string Avatar { get; protected set; }

        public virtual ICollection<UserEntity> Users { get; set; }
        
        public virtual CustomerEntity Customer { get; set; }

        public void Update(DateTime on, string modifiedBy, string name = null, string description = null, string avatar = null)
        { 
            this.Name = name ?? this.Name;
            this.Description = description ?? this.Description;
            this.Avatar = avatar ?? this.Avatar;
            this.ModifiedBy = modifiedBy;
            this.ModifiedOn = on;
            this.Validate();
        }

        public void RemoveMember(int userId) {

            if (this.Users == null)
                this.Users = new List<UserEntity>();

            if (this.Users.Any(c => c.Id.Equals(userId))) {
                this.Users.Remove(this.Users.First(c => c.Id.Equals(userId)));
            }
        }

    }
}

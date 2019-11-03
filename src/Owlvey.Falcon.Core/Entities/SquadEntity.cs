using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class SquadCompare : IEqualityComparer<SquadEntity>
    {
        public bool Equals(SquadEntity x, SquadEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(SquadEntity obj)
        {
            return obj.Id.Value;
        }
    }

    public partial class SquadEntity: BaseEntity
    {
        [Required]
        public string Name { get; protected set; }
        
        public string Description { get; protected set; }
        public string Avatar { get; protected set; }

        public string Leaders { get; set; }

        public virtual ICollection<MemberEntity> Members { get; set; } = new List<MemberEntity>();
        public virtual ICollection<SquadFeatureEntity> FeatureMaps { get; set; } = new List<SquadFeatureEntity>();
               
        
        public virtual CustomerEntity Customer { get; set; }
        public int CustomerId { get; set; }

        public void Update(DateTime on, string modifiedBy, string name, string description, 
            string avatar, string leaders)
        {
            this.Leaders = leaders ?? this.Leaders;
            this.Name = name ?? this.Name;
            this.Description = description ?? this.Description;
            this.Avatar = avatar ?? this.Avatar;
            this.ModifiedBy = modifiedBy;
            this.ModifiedOn = on;
            this.Validate();
        }

        public void RemoveMember(int userId) {            

        }
    }
}

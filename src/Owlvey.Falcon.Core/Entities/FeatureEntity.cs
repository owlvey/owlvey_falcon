using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{

    public class FeatureCompare : IEqualityComparer<FeatureEntity>
    {
        public bool Equals(FeatureEntity x, FeatureEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(FeatureEntity obj)
        {
            return obj.GetHashCode();
        }
    }


    public partial class FeatureEntity: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Avatar { get; set; }
        
        [Required]
        public decimal MTTD { get; set; }

        [Required]
        public decimal MTTR { get; set; }

        [Required]
        public decimal MTTF { get; set; }

        [NotMapped]
        public decimal MTBF
        {
            get
            {
                return this.MTTD + this.MTTR + this.MTTF;
            }
        }


        public int ProductId { get; set; }

        public int ServiceMapId { get; set; }

        public virtual ProductEntity Product { get; set; }

        public virtual IEnumerable<ServiceMapEntity> ServiceMaps { get; set; } = new List<ServiceMapEntity>();

        public virtual ICollection<IndicatorEntity> Indicators { get; set; } = new List<IndicatorEntity>();

        public virtual ICollection<SquadFeatureEntity> Squads { get; set; } = new List<SquadFeatureEntity>(); 
        
        public virtual void Update(DateTime on, string modifiedBy, string name, string avatar , string description,
            decimal? mttd, decimal? mttr)
        {
            this.Name = string.IsNullOrWhiteSpace(name) ? this.Name : name;
            this.Avatar = string.IsNullOrWhiteSpace(avatar) ? this.Avatar : avatar;
            this.Description = string.IsNullOrWhiteSpace(description) ? this.Description : description;
            this.ModifiedOn = on;
            this.ModifiedBy = modifiedBy;
            this.MTTD = mttd ?? this.MTTD;
            this.MTTR = mttr ?? this.MTTR;

        }

    }
}

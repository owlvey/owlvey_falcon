using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceEntity: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required]
        public decimal Slo { get; set; }

        [NotMapped]
        public decimal MTTD { get {
                if (this.FeatureMap.Count > 0)
                {
                    return  Math.Ceiling(this.FeatureMap.Select(c => c.Feature).Average(c => c.MTTD));
                }
                return -1;
            } }

        [NotMapped]
        public decimal MTTR { get {
                if (this.FeatureMap.Count > 0)
                {
                    return Math.Ceiling(this.FeatureMap.Select(c => c.Feature).Average(c => c.MTTR));
                }
                return -1;
            } }

        [NotMapped]
        public decimal MTTF { get {
                if (this.FeatureMap.Count > 0)
                {
                    return Math.Ceiling(this.FeatureMap.Select(c => c.Feature).Average(c => c.MTTF));
                }
                return -1;
            } }

        [NotMapped]
        public decimal MTBF { get {
                if (this.FeatureMap.Count > 0) {
                    return  Math.Ceiling(this.FeatureMap.Select(c => c.Feature).Average(c => c.MTBF));
                }
                return -1;                
           }
        }

        [Required]
        public string Avatar { get; set; }

        [NotMapped]
        public decimal Impact
        {
            get
            {
                return AvailabilityUtils.MeasureImpact(this.Slo);
            }
        }

        public int ProductId { get; set; }

        public virtual ProductEntity Product { get; set; }

        public virtual ICollection<ServiceMapEntity> FeatureMap { get; set; } = new List<ServiceMapEntity>();

        public void Update(DateTime on, string modifiedBy, string name, decimal? slo, string description = null, string avatar = null)
        {
            this.Name = name ?? this.Name;
            this.Slo = slo ?? this.Slo;
            this.Description = description ?? this.Description;
            this.Avatar = avatar ?? this.Avatar;
            this.ModifiedBy = modifiedBy;
            this.ModifiedOn = on;            
            this.Validate();
        }

    }
}

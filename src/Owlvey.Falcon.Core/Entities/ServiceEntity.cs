using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace Owlvey.Falcon.Core.Entities
{
    public class ServiceEntityCompare : IEqualityComparer<ServiceEntity>
    {
        public bool Equals(ServiceEntity x, ServiceEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ServiceEntity obj)
        {
            return obj.Id.Value;
        }
    }

    public partial class ServiceEntity: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        public decimal Slo { get; set; }       

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

        [NotMapped]
        public string Group { get {

                if (!string.IsNullOrWhiteSpace(this.Name))
                {
                    return this.Name.Split(' ').FirstOrDefault();
                }
                else {
                    return this.Name;
                }
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

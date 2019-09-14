﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

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

        public int ProductId { get; set; }

        public int ServiceMapId { get; set; }

        public virtual ProductEntity Product { get; set; }

        public virtual IEnumerable<ServiceMapEntity> ServiceMaps { get; set; } = new List<ServiceMapEntity>();

        public virtual ICollection<IndicatorEntity> Indicators { get; set; } = new List<IndicatorEntity>();

        public virtual ICollection<SquadFeatureEntity> Squads { get; set; } = new List<SquadFeatureEntity>();

        public virtual ICollection<IncidentMapEntity> IncidentMap { get; set; } = new List<IncidentMapEntity>();

        public virtual void Update(DateTime on, string modifiedBy, string name, string avatar , string description)
        {
            this.Name = string.IsNullOrWhiteSpace(name) ? this.Name : name;
            this.Avatar = string.IsNullOrWhiteSpace(avatar) ? this.Avatar : avatar;
            this.Description = string.IsNullOrWhiteSpace(description) ? this.Description : description;
            this.ModifiedOn = on;
            this.ModifiedBy = modifiedBy;            
        }

        [NotMapped]
        public decimal MTTD { get {
                if (this.IncidentMap.Count > 0)
                {
                    return Math.Ceiling(this.IncidentMap.Select(c => c.Incident).Average(c => c.MTTD));
                }
                return -1;                
            } }
        [NotMapped]
        public decimal MTTE
        {
            get
            {
                if (this.IncidentMap.Count > 0)
                {
                    return Math.Ceiling(this.IncidentMap.Select(c => c.Incident).Average(c => c.MTTE));
                }
                return -1;
            }
        }
        [NotMapped]
        public decimal MTTF
        {
            get
            {
                if (this.IncidentMap.Count > 0)
                {
                    return Math.Ceiling(this.IncidentMap.Select(c => c.Incident).Average(c => c.MTTF));
                }
                return -1;
            }
        }
        [NotMapped]
        public decimal MTTM
        {
            get
            {
                if (this.IncidentMap.Count > 0)
                {
                    return Math.Ceiling(this.IncidentMap.Select(c => c.Incident).Average(c => c.MTTM));
                }
                return -1;
            }
        }
    }
}

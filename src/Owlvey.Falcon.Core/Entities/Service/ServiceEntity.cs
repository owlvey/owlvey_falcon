﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;

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
        public decimal AvailabilitySlo { get; set; } 

        [Required]
        public decimal ExperienceSlo { get; set; }

        [Required]
        public decimal LatencySlo { get; set; }

        [Required]
        public decimal AvailabilitySla {get; set;}

        [Required]
        public decimal LatencySla { get; set; }

        public SLOValue GetSLO (){
            return new SLOValue( this.AvailabilitySlo, this.LatencySlo, this.ExperienceSlo);
        }

        public SLAValue GetSLA (){
            return new SLAValue( this.AvailabilitySla, this.LatencySla);
        }

        [Required]
        public string Avatar { get; set; }

        public string Leaders { get; set; }

        [NotMapped]
        public decimal AvailabilityImpact
        {
            get
            {
                return QualityUtils.MeasureImpact(this.AvailabilitySlo);
            }
        }

        [Required]
        public string Group { get; set; }

        
        public int ProductId { get; set; }

        public virtual ProductEntity Product { get; set; }

        public virtual ICollection<ServiceMapEntity> FeatureMap { get; set; } = new List<ServiceMapEntity>();

        public void Update(DateTime on, string modifiedBy, string name, 
            decimal? availabilitySlo,
            decimal? latencySlo,
            decimal? experienceSlo,
            SLAValue slaValue,
            string description, string avatar,
            string leaders, string group)
        {
            this.Leaders = leaders ?? this.Leaders;
            this.Name = name ?? this.Name;
            this.AvailabilitySlo = availabilitySlo ?? this.AvailabilitySlo;
            this.ExperienceSlo = experienceSlo ?? this.ExperienceSlo;
            this.LatencySlo = latencySlo ?? this.LatencySlo;
            this.Description = description ?? this.Description;
            this.Avatar = avatar ?? this.Avatar;
            this.ModifiedBy = modifiedBy;            
            this.ModifiedOn = on;
            this.Group = string.IsNullOrWhiteSpace(group) ? this.Group : group;

            if (slaValue != null){
                this.AvailabilitySla = slaValue.Availability ?? this.AvailabilitySla;
                this.LatencySla = slaValue.Latency ?? this.LatencySla;
            }

            this.Validate();
        }

        public bool ValidateLeader(string email) {
            return !string.IsNullOrWhiteSpace(this.Leaders) && this.Leaders.Contains(email);
        }

    }
}

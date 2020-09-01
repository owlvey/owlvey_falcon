using Owlvey.Falcon.Core.Entities.Source;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ProductEntity : BaseEntity
    {
        [Required]        
        public string Name { get; set; }
        [Required]
        public string Avatar { get; set; }
        [Required]
        public string Description { get; set; }

        public string Leaders { get; set; }
        public CustomerEntity Customer { get; set; }

        public int CustomerId { get; set; }
        
        public virtual ICollection<JourneyEntity> Journeys { get; set; } = new List<JourneyEntity>();
        public virtual ICollection<FeatureEntity> Features { get; set; } = new List<FeatureEntity>();
        public virtual SourceCollection Sources { get; set; } = new SourceCollection();
        public virtual ICollection<IncidentEntity> Incidents { get; set; } = new List<IncidentEntity>();
        public virtual ICollection<AnchorEntity> Anchors { get; set; } = new List<AnchorEntity>(); 

        public void AddJourney(JourneyEntity entity) {
            entity.Product = this;
            this.Journeys.Add(entity);
        }
        
        public void AddFeature(FeatureEntity entity) {
            entity.Product = this;
            this.Features.Add(entity); 
        }

        public virtual void Update(DateTime on, string modifiedBy, string name, string description, 
            string avatar, string leaders)
        {
            this.Leaders = leaders ?? this.Leaders;
            this.Name = name ?? this.Name;
            this.Description = description ?? this.Description;
            this.Avatar = avatar ?? this.Avatar;
            this.ModifiedOn = on;
            this.ModifiedBy = modifiedBy;
        }

        public bool ValidateLeader(string email) {
            return !string.IsNullOrWhiteSpace(this.Leaders) && this.Leaders.Contains(email);
        }

        public void ClearSourceItems() {
            foreach (var journey in this.Journeys)
            {
                foreach (var map in journey.FeatureMap)
                {
                    foreach (var indicator in map.Feature.Indicators)
                    {
                        indicator.Source.SourceItems = new List<SourceItemEntity>();
                    }
                }
            }

            foreach (var item in this.Sources)
            {
                item.SourceItems = new List<SourceItemEntity>();
            }
        }


        #region metrics



        #endregion

    }
}

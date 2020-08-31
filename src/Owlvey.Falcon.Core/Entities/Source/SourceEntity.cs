using Owlvey.Falcon.Core.Aggregates;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity: BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Tags { get; set; }

        [Required]
        public string GoodDefinitionAvailability { get; set; }

        [Required]
        public string TotalDefinitionAvailability { get; set; }

        public DefinitionValue AvailabilityDefinition
        {
            get {
                return new DefinitionValue(this.GoodDefinitionAvailability, this.TotalDefinitionAvailability);
            }
        }

        [Required]
        public string GoodDefinitionLatency { get; set; }

        [Required]
        public string TotalDefinitionLatency { get; set; }

        public DefinitionValue LatencyDefinition
        {
            get
            {
                return new DefinitionValue(this.GoodDefinitionLatency, this.TotalDefinitionLatency);
            }
        }

        [Required]
        public string GoodDefinitionExperience { get; set; }

        [Required]
        public string TotalDefinitionExperience { get; set; }

        public DefinitionValue ExperienceDefinition
        {
            get
            {
                return new DefinitionValue(this.GoodDefinitionExperience, this.TotalDefinitionExperience);
            }
        }

        public string Avatar { get; set; }

        public string Description { get; set; }                

        public decimal Percentile { get; set; } = 0.95m;

        public virtual ProductEntity Product { get; set; }

        public int ProductId { get; set; }
        
        public virtual ICollection<IndicatorEntity> Indicators { get; set; } = new List<IndicatorEntity>();

        public virtual ICollection<SourceItemEntity> SourceItems { get; set; } = new List<SourceItemEntity>();

        public virtual void Update(
         string name, string avatar,
         DefinitionValue availabilityDefinition,
         DefinitionValue latencyDefinition,
         DefinitionValue experienceDefinition,
         DateTime on, string modifiedBy, 
         string tags, string description, decimal percentile)
        {
            this.Name = string.IsNullOrWhiteSpace(name) ? this.Name : name;
            this.Avatar = string.IsNullOrWhiteSpace(avatar) ? this.Avatar : avatar;

            this.GoodDefinitionAvailability = availabilityDefinition.GetGoodDefinition(this.GoodDefinitionAvailability);
            this.TotalDefinitionAvailability = availabilityDefinition.GetTotalDefinition(this.TotalDefinitionAvailability);

            this.GoodDefinitionLatency = latencyDefinition.GetGoodDefinition(this.GoodDefinitionAvailability);
            this.TotalDefinitionLatency = latencyDefinition.GetTotalDefinition(this.TotalDefinitionAvailability);

            this.GoodDefinitionExperience = experienceDefinition.GetGoodDefinition(this.GoodDefinitionAvailability);
            this.TotalDefinitionExperience = experienceDefinition.GetTotalDefinition(this.TotalDefinitionAvailability);

            this.Description = string.IsNullOrWhiteSpace(description) ? this.Description : description;
            this.ModifiedOn = on;
            this.Percentile = percentile;
            this.Tags = tags ?? this.Tags;
            this.ModifiedBy = modifiedBy;
        }

        public SourceItemEntity AddSourceItem(int good, int total, DateTime target, DateTime on, string createdBy, SourceGroupEnum group)
        {
            
            var result = Factory.CreateItem(this, target, good, total, on, createdBy, group);
            this.SourceItems.Add(result);
            return result;
        }

        public Dictionary<string, int> FeaturesToDictionary() {
            return this.Indicators.ToDictionary(d => d.Feature.Name, c => c.Feature.Id.Value); 
        }

        public SourceGroupEnum ParseGroup(string value) {
            return (SourceGroupEnum)Enum.Parse(typeof(SourceGroupEnum), value);
        }

        public IEnumerable<DayMeasureValue> GetDailySeries(DatePeriodValue period) {
            var items = new SourceDailyAggregate(this, period).MeasureAvailability();
            return items; 
        }
    }
}

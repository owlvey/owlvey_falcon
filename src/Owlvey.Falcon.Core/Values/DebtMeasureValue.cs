using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class DebtMeasureValue
    {
        public decimal Availability { get; set; }
        public decimal Latency { get; set; }
        public decimal Experience { get; set; }
        public DebtMeasureValue()
        {
            
        }
        public DebtMeasureValue(decimal availability, decimal latency, decimal experience)
        {
            this.Availability = availability;
            this.Latency = latency;
            this.Experience = experience;
        }
        public DebtMeasureValue(IEnumerable<DebtMeasureValue> value)
        {
            this.Availability = value.Sum(c=>c.Availability);
            this.Latency = value.Sum(c => c.Latency);
            this.Experience = value.Sum(c => c.Experience);
        }
        public void Add(DebtMeasureValue value) {
            this.Availability += value.Availability;
            this.Latency += value.Latency;
            this.Experience += value.Experience;
        }

        public void AddRange(IEnumerable<DebtMeasureValue> values)
        {
            foreach (var item in values)
            {
                this.Add(item);
            }            
        }
        public void Add(JourneyQualityMeasureValue value)
        {
            this.Availability += value.AvailabilityDebt;
            this.Latency += value.LatencyDebt;
            this.Experience += value.ExperienceDebt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class BudgetMeasureValue
    {

        public decimal AvailabilityDebt { get; protected  set; }
        public decimal LatencyDebt { get; protected set; }
        public decimal ExperienceDebt { get; protected set; }
        public decimal AvailabilityAsset { get; protected set; }
        public decimal ExperienceAsset { get; protected set; }
        public decimal LatencyAsset { get; protected set; }

        public BudgetMeasureValue(
            decimal availabilityDebt,
            decimal latencyDebt,
            decimal experienceDebt,
            decimal availabilityAsset,
            decimal latencyAsset,
            decimal experienceAsset) {
            this.AvailabilityDebt = Math.Round(availabilityDebt, 3);
            this.LatencyDebt = Math.Round(latencyDebt, 3);
            this.ExperienceDebt = Math.Round(experienceDebt, 3);
            this.AvailabilityAsset = Math.Round(availabilityAsset, 3);
            this.LatencyAsset = Math.Round(latencyAsset, 3);
            this.ExperienceAsset = Math.Round(experienceAsset, 3);
        }
    }
}

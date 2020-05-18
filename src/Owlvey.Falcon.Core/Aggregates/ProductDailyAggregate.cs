using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class ProductDailyAggregate
    {
        public ProductEntity Product { get; protected set; }
        public DatePeriodValue Period { get; set; }
        

        public ProductDailyAggregate(ProductEntity product, DatePeriodValue period) {
            this.Product = product;
            this.Period = period;
        }
        public IEnumerable<DayBudgetMeasure> MeasureQuality() {
            var result = new List<DayBudgetMeasure>();
            var totalServices = this.Product.Services.Count;
            if (totalServices == 0)
            {
                return result;
            }                        
            foreach (var period in this.Period.GetDatesPeriods())
            {
                decimal Avadebt = 0, LatDebt = 0, ExpDebt = 0;
                decimal Avaasset = 0 , LatAsset = 0, ExpAsset = 0;                
                bool hasData = false;
                foreach (var service in this.Product.Services)
                {                    
                    var measure = service.Measure(period);
                    if (measure.HasData) {
                        Avaasset += measure.AvailabilityAsset;
                        LatAsset += measure.LatencyAsset;
                        ExpAsset += measure.ExperienceAsset;
                        Avadebt += measure.AvailabilityDebt;
                        LatDebt += measure.LatencyDebt;
                        ExpDebt += measure.ExperienceDebt;
                        hasData = true;
                    }                    
                }
                if (hasData) {
                    result.Add(
                        new DayBudgetMeasure(period.Start,
                        new BudgetMeasureValue( Avadebt, LatDebt, ExpDebt, Avaasset, LatAsset, ExpAsset)));
                }                
            }
            return result;
        }
    }
}

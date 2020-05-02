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
                decimal debt = 0;
                decimal asset= 0;                
                bool hasData = false;
                foreach (var service in this.Product.Services)
                {                    
                    var measure = service.MeasureQuality(period);
                    if (measure.HasData) {
                        asset += measure.Asset;                        
                        debt += measure.Debt;
                        hasData = true;
                    }                    
                }
                if (hasData) {
                    result.Add(
                        new DayBudgetMeasure(period.Start,
                        new BudgetMeasureValue(debt, asset)));
                }                
            }
            return result;
        }
    }
}

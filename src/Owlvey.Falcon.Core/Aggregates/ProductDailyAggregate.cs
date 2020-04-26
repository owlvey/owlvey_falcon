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
        public IEnumerable<DayMeasureValue> MeasureQuality() {
            var result = new List<DayMeasureValue>();
            var totalServices = this.Product.Services.Count;
            if (totalServices == 0)
            {
                return result;
            }            
            
            foreach (var item in this.Period.GetDatesIntervals())
            {
                var quality = 0;
                var availability = 0;
                var latency = 0;
                bool hasData = false;
                foreach (var service in this.Product.Services)
                {
                    var agg = new ServiceQualityAggregate(service);
                    var measure = agg.MeasureQuality(item.start, item.end);
                    if (measure.HasData) {
                        if (measure.Quality >= service.Slo) quality += 1;
                        if (measure.Availability >= service.Slo) availability += 1;
                        if (measure.Latency >= service.Slo) latency += 1;
                        hasData = true;
                    }                    
                }
                if (hasData) {
                    result.Add(new DayMeasureValue(item.start,
                    new QualityMeasureValue(
                        QualityUtils.CalculateAvailability(totalServices, quality),
                        QualityUtils.CalculateAvailability(totalServices, availability),
                        QualityUtils.CalculateAvailability(totalServices, latency)
                    )));
                }
                
            }
            return result;
        }
    }
}

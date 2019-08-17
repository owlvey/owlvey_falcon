using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
namespace Owlvey.Falcon.Core.Aggregates
{
    public class ProductAvailabilityAggregate
    {
        public ProductEntity Product { get; protected set; }        
        private readonly DateTime Start;
        private readonly DateTime End;
        public ProductAvailabilityAggregate(ProductEntity entity,            
            DateTime start, DateTime end)
        {
            this.Product = entity;            
            this.Start = start;
            this.End = end;
        }
        private IEnumerable<(ServiceEntity service, IEnumerable<DayAvailabilityEntity> availabilities)> GetServiceAvailabilities() {
            var result = new List<(ServiceEntity, IEnumerable<DayAvailabilityEntity>)>();
            foreach (var item in this.Product.Services)
            {
                var agg = new ServiceAvailabilityAggregate(item, this.Start, this.End);
                var (ser, availabilities, features)  = agg.MeasureAvailability();
                result.Add((ser, availabilities));                
            }
            return result;
        }
        public (ProductEntity product, IEnumerable<DayAvailabilityEntity> availabilities,
            IEnumerable<(ServiceEntity service, IEnumerable<DayAvailabilityEntity> availabilities)> services) MeasureAvailability()
        {
            List<DayAvailabilityEntity> result = new List<DayAvailabilityEntity>();

            var indicators = this.GetServiceAvailabilities();

            var data = indicators.SelectMany(c => c.availabilities).ToList();

            var days = DateTimeUtils.DaysDiff(this.End, this.Start);

            var pivot = this.Start;

            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Date, pivot)).Select(c => c.Availability).ToList();

                var availability = sample.Aggregate((a, x) => a * x);
                var minimun = sample.Min();
                var maximun = sample.Max();
                var average = sample.Average();

                result.Add(new DayAvailabilityEntity(pivot, availability, minimun, maximun, average));

                pivot = pivot.AddDays(1);
            }

            return (this.Product, result, indicators);
        }
    }
}

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

            var serviceIndicators = this.GetServiceAvailabilities();

            var data = serviceIndicators.SelectMany(c => c.availabilities).ToList();

            var days = DateTimeUtils.DaysDiff(this.End, this.Start);

            var pivot = this.Start;

            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Date, pivot)).ToList();
                if (sample.Count > 0) {
                    var availability = AvailabilityUtils.CalculateAvailability(sample.Select(c => c.Availability));
                    var minimun = sample.Min(c => c.Availability);
                    var maximun = sample.Max(c => c.Availability);
                    var average = AvailabilityUtils.CalculateAvailability(sample.Select(c => c.Availability));
                    result.Add(new DayAvailabilityEntity(pivot, availability, minimun, maximun, average));
                }
                pivot = pivot.AddDays(1);
            }

            return (this.Product, result, serviceIndicators);
        }
    }
}

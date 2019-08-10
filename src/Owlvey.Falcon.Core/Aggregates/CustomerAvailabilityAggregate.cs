using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class CustomerAvailabilityAggregate
    {
        public CustomerEntity Customer { get; protected set; }        
        private readonly DateTime Start;
        private readonly DateTime End;        

        public CustomerAvailabilityAggregate(CustomerEntity entity,            
            DateTime start, DateTime end)
        {
            this.Customer = entity;            
            this.Start = start;
            this.End = end;
        }
        private IEnumerable<(ProductEntity service, IEnumerable<DayAvailabilityEntity> availabilities)> GetAvailabilities()
        {
            var result = new List<(ProductEntity, IEnumerable<DayAvailabilityEntity>)>();
            foreach (var item in this.Customer.Products)
            {
                var agg = new ProductAvailabilityAggregate(item, this.Start, this.End);
                var tmp = agg.MeasureAvailability();
                result.Add(tmp);
            }
            return result;
        }
        public (CustomerEntity customer, IEnumerable<DayAvailabilityEntity> availabilities) MeasureAvailability()
        {
            List<DayAvailabilityEntity> result = new List<DayAvailabilityEntity>();

            var indicators = this.GetAvailabilities();
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

            return (this.Customer, result);
        }        
    }
}

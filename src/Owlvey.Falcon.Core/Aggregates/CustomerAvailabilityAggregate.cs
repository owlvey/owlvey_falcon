using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class CustomerAvailabilityAggregate
    {
        public CustomerEntity Customer { get; protected set; }
        private IEnumerable<(ProductEntity service, IEnumerable<DayAvailabilityEntity> availabilities)> Indicators;
        private readonly DateTime Start;
        private readonly DateTime End;
        public CustomerAvailabilityAggregate(CustomerEntity entity,
            IEnumerable<(ProductEntity product, IEnumerable<DayAvailabilityEntity> availabilities)> indicators,
            DateTime start, DateTime end)
        {
            this.Customer = entity;
            this.Indicators = indicators;
            this.Start = start;
            this.End = end;
        }
        public (CustomerEntity customer, IEnumerable<DayAvailabilityEntity> availabilities) MeasureAvailability()
        {
            List<DayAvailabilityEntity> result = new List<DayAvailabilityEntity>();

            var data = this.Indicators.SelectMany(c => c.availabilities).ToList();

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

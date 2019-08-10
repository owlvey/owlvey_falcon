using System;
namespace Owlvey.Falcon.Core.Entities
{
    public class DayAvailabilityEntity
    {

        public DateTime Date { get; protected set; }
        public decimal Availability { get; protected set; }
        public decimal Minimun { get; protected set; }
        public decimal Maximun { get; protected set; }
        public decimal Average { get; protected set; }

        public DayAvailabilityEntity(DateTime date, decimal availability, decimal minimun, decimal maximun, decimal average)
        {
            this.Minimun = minimun;
            this.Maximun = maximun;
            this.Average = average;
            this.Date = date;
            this.Availability = availability;
        }

        internal void ChangeAvailability(decimal availability) {
            this.Minimun = availability;
            this.Maximun = availability;
            this.Average = availability;
            this.Availability = availability;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Series
{
    public class DatetimeSerieitemModel
    {
        public string Title { get; set; }
        public DateTime Date { get; protected set; }
        public decimal OAve { get; protected set; }

        public DatetimeSerieitemModel() { }
        public DatetimeSerieitemModel(DateTime target, decimal value) {
            this.Date = target;
            this.OAve = value;
        }
    }
}

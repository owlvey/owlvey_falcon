using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Series
{
    public class DatetimeSerieItemModel
    {
        public string Title { get; set; }
        public DateTime Date { get; protected set; }
        public decimal OAve { get; protected set; }

        public DatetimeSerieItemModel() { }
        public DatetimeSerieItemModel(DateTime target, decimal value) {
            this.Date = target;
            this.OAve = value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Series
{
    public class DateInteractionModel
    {
        public DateTime Date { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }
        public int Bad
        {
            get { return this.Total - this.Good; }
        }

        public DateInteractionModel() { }
        public DateInteractionModel(DateTime date, int total, int good)
        {
            this.Date = date;
            this.Total = total;
            this.Good = good;
        }
    }
}

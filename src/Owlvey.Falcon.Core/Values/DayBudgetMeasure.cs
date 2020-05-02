using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class DayBudgetMeasure
    {
        public DateTime Date { get; protected set; }
        public BudgetMeasureValue Measure { get; protected set; }

        public DayBudgetMeasure(DateTime date, BudgetMeasureValue measure)
        {
            this.Date = date;
            this.Measure = measure;
        }
    }
}

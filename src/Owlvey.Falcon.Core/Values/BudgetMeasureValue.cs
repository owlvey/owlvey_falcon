using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class BudgetMeasureValue
    {

        public decimal Debt { get; protected  set; }
        public decimal Asset { get; protected set; }

        public BudgetMeasureValue(decimal debt, decimal asset) {
            this.Debt = Math.Round(debt, 3);
            this.Asset = Math.Round(asset, 3);
        }
    }
}

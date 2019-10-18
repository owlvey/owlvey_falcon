using System;
using System.Collections.Generic;
using System.Linq;
namespace Owlvey.Falcon.Core
{
    public class AvailabilityUtils
    {        
        public static decimal CalculateAverageAvailability(IEnumerable<decimal> availabilities) {
            if (availabilities.Count() > 0)
            {
                return Math.Round(availabilities.Average(c => c), 5);
            }
            else {
                return 1;
            }            
        }

        // https://www.thoughtco.com/exponential-decay-definition-2312215
        public static decimal MeasureImpact(decimal SLO) {
            if (SLO < 1) {
                SLO *= 100;
            }
            double a = 1000;
            double b = 0.05;
            double x = (100 - (double)SLO)/0.1;
            double impact = a * Math.Pow((1 - b), x);
            return Math.Round((decimal)impact, 0);
        }

        public static decimal MeasurePoints(decimal SLO, decimal availability) {
            return Math.Round((availability - SLO) * MeasureImpact(SLO), 2);
        }

        public static decimal MeasureBudgetInMinutes(decimal budget, DateTime start, DateTime end) {
            if (budget <= 0) return 0;

            var minutes = (decimal)end.Subtract(start).TotalMinutes;                                  

            return Math.Floor( (minutes * budget) / 100m);
        }
        public static decimal MeasureBudget(decimal avaialbility, decimal slo) {
            return avaialbility - slo;
        }

        public static int MeanTimeInMinutes(IEnumerable<int> input) {
            if (input.Count() == 0) {
                return 0;
            }
            return (int)Math.Round(input.Average());
        }
        public static decimal CalculateDotAvailability(IEnumerable<decimal> availabilities)
        {
            return Math.Round(availabilities.Aggregate((a, x) => a * x),5);            
        }

        public static decimal CalculateFailProportion(decimal total, decimal fail, decimal defaultValue = 1) {            
            return CalculateProportion(total, total - fail, defaultValue);
        }
        public static decimal CalculateProportion(decimal total, decimal good, decimal defaultValue = 1)
        {
            if (total == 0)
            {
                return defaultValue;
            }
            else
            {
                return Math.Round(good / total, 1);
            }
        }
        public static decimal CalculateAvailability(decimal total, decimal good, decimal defaultValue = 1) {
            if (total == 0)
            {   
                return defaultValue;
            }
            else {
                if (good == 0){
                    return 0.01M;
                }
                else {
                    return Math.Round(good / total, 5);
                }                
            }            
        }
    }
}

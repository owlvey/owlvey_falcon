using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace Owlvey.Falcon.Core
{
    public class QualityUtils
    {        
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

        public static string BudgetToAction(JourneyQualityMeasureValue measure) {
            if (measure.AvailabilityErrorBudget > 0 && measure.LatencyErrorBudget > 0 && measure.ExperienceErrorBudget > 0)
            {
                return "innovate";                
            }
            else
            {
                return "improve";                
            }            
        }
        public static decimal MeasureBudget(decimal avaialbility, decimal slo) {
            return avaialbility - slo;
        }
        public static decimal MeasureDebt(decimal availability, decimal slo) {
            var budget = MeasureBudget(availability, slo);
            if (budget >= 0) {
                return 0;
            }
            return -1 * budget;
        }
        public static decimal MeasureLatencyBudget(decimal latency, decimal slo) {
            
            return latency - slo;
        }
        public static decimal MeasureLatencyDebt(decimal latency,
            decimal slo)
        {
            var budget = MeasureLatencyBudget(latency, slo);
            if (budget <= 0)
            {
                return 0;
            }
            return budget;
        }

        public static int MeanTimeInMinutes(IEnumerable<int> input) {
            if (input.Count() == 0) {
                return 0;
            }
            return (int)Math.Round(input.Average());
        }        

        public static decimal CalculateMinimum(IEnumerable<decimal> availabilities, int round = 3, decimal defaultValue = 1)
        {
            if (availabilities.Count() == 0)
            {
                return defaultValue;
            }
            else {
                return Math.Round(availabilities.Min(), round);
            }            
        }

        public static decimal CalculateMaximum(IEnumerable<decimal> availabilities, int round = 3, decimal defaultValue = 0)
        {
            if (availabilities.Count() == 0)
            {
                return defaultValue;
            }
            else
            {
                return Math.Round(availabilities.Max(), round);
            }
        }

        public static decimal CalculateAverage(IEnumerable<decimal> availabilities, int round = 3)
        {
            if (availabilities.Count() == 0) {
                return 0;
            }
            return Math.Round(availabilities.Average(), round);
        }

        public static decimal CalculateFailProportion(decimal total, decimal fail, decimal defaultValue = 1) {            
            return CalculateProportion(total, total - fail, defaultValue);
        }

        internal static (double b0, double b1, double r2) LinealRegression(int[] xdata, int[] ydata) {
            var x = xdata.Select(c => (double)c).ToArray();
            var y = ydata.Select(c => (double)c).ToArray();
            Tuple<double, double> p = Fit.Line(x, y);
            double a = Math.Round(p.Item1, 5);
            double b = Math.Round(p.Item2, 5);
            var r2 = GoodnessOfFit.RSquared(xdata.Select(x => a + b * x), y);
            return (a, b, Math.Round(r2, 4));
        }
        internal static double MeasureCorrelation(int[] totals, int[] goods)
        {
            var total = totals.Select(c => (double)c).ToArray();
            var good = goods.Select(c => (double)c).ToArray();
            return MeasureCorrelation(total, good);
        }
        internal static double MeasureCorrelation(double[] totals, double[] goods)
        {
            if (totals.Length <= 1 || goods.Length <= 1) {
                return 0;
            }
            var result = Math.Round(Correlation.Pearson(totals, goods), 3);
            if (double.IsNaN(result))
                return 0;
            return result;
        }

        public static decimal CalculateProportion(decimal total, decimal good, decimal defaultValue = 1)
        {
            if (total == 0)
            {
                return defaultValue;
            }
            else
            {
                return Math.Round(good / total, 3);
            }
        }
        public static decimal CalculateProportion(int total, int good, decimal defaultValue = 1)
        {
            return CalculateProportion((decimal)total, (decimal)good, defaultValue);
        }
        public static decimal CalculateProportion(int? total, int? good, decimal defaultValue = 1)
        {
            return CalculateProportion((decimal)total.GetValueOrDefault(), (decimal)good.GetValueOrDefault(), defaultValue);
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

        public static decimal CalculateFeatureSlo(decimal slo, int items) {

            if (items == 0)
            {
                return slo;
            }
            return Math.Round((decimal)Math.Pow((double)slo, 1 / (double)items), 4);
        }

        public static (int good, int total) ProportionToNumbers(decimal availability) {
            if (availability >= 1) {
                return (1, 1);
            }
            if (availability <= 0) {
                return (0, 0);
            }
            int total = 1;
            decimal good = availability;

            while (good % 1 != 0) {
                total *= 10;
                good *= 10;
            }

            if (total < 100) {
                good *= 10;
                total *= 10;
            }
            return ((int)good, total); 
        }

        public static (int good, int total) ProportionToMinutes(DateTime start, DateTime end, 
            decimal availability) {
                       

            if (availability > 1)
            {
                availability = 1;
            }
            if (availability < 0) {
                availability = 0;
            }

            var total = end.Subtract(start).TotalMinutes;

            if (total <= 0.001)
            {
                total = 1440;                
            }
            var good = (decimal)total * availability;
            
            return ((int)Math.Ceiling(good), (int)Math.Ceiling(total));
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
namespace Owlvey.Falcon.Core
{
    public class AvailabilityUtils
    {        
        public static decimal CalculateAvailability(IEnumerable<decimal> availabilities) {
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
            double a = 50;
            double b = 0.05;
            double x = (100 - (double)SLO)/0.1;
            double impact = a * Math.Pow((1 - b), x);
            return Math.Round((decimal)impact, 2);
        }

        public static decimal MeasureBudgetInMinutes(decimal budget) {
            if (budget <= 0) return 0;

            decimal minutes = 60 * 24;

            return Math.Floor(budget * minutes);
        }
        public static decimal MeasureBudget(decimal avaialbility, decimal slo) {
            return avaialbility - slo;
        }

        public static decimal CalculateDotAvailability(IEnumerable<decimal> availabilities)
        {
            return Math.Round(availabilities.Aggregate((a, x) => a * x),5);            
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

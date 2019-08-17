using System;
namespace Owlvey.Falcon.Core
{
    public class AvailabilityUtils
    {
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
                    return Math.Round(good / total, 3);
                }
                
            }            
        }
    }
}

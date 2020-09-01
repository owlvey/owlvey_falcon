using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Models.Exports;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class AnnualAvailabilityInteractionsAggregate
    {
        private readonly ProductEntity Product;
        private readonly DatePeriodValue Period;

        public AnnualAvailabilityInteractionsAggregate(ProductEntity product,
            DateTime target) {
            this.Product = product;            
            this.Period = DatePeriodValue.ToYearFromStart(target);
        }

        public (IEnumerable<AnnualAvailabilityInteractionsItemModel> journeys, IEnumerable<AnnualAvailabilityInteractionsItemModel> sources) Execute()
        {
            var periods = Period.ToYearPeriods();
            var journeysResult = new List<AnnualAvailabilityInteractionsItemModel>();            
            var sourceResult = new List<AnnualAvailabilityInteractionsItemModel>();
            foreach (var journey in this.Product.Journeys)
            {
                var temp = new AnnualAvailabilityInteractionsItemModel(journey.Id.Value, journey.Name);
                foreach (var period in periods)
                {
                    int total = 0;
                    int good = 0;                    
                    foreach (var map in journey.FeatureMap)
                    {
                        foreach (var indicator in map.Feature.Indicators)
                        {
                            var target = indicator.Source.SourceItems
                                .Where(c => period.Contains(c.Target))
                                .ToList();

                            good += target.Sum(c => c.Good.GetValueOrDefault());
                            total += target.Sum(c => c.Total.GetValueOrDefault());                            
                        }
                    }
                    temp.LoadData(period.Start.Month, good, total);                    
                }
                journeysResult.Add(temp);
            }

            foreach (var source in this.Product.Sources)
            {
                var temp = new AnnualAvailabilityInteractionsItemModel(source.Id.Value, source.Name);
                foreach (var period in periods)
                {
                    int total = 0;
                    int good = 0;                    
                        
                    var target = source.SourceItems
                        .Where(c =>                        
                        period.Contains(c.Target))
                        .ToList();

                    good += target.Sum(c => c.Good.GetValueOrDefault());
                    total += target.Sum(c => c.Total.GetValueOrDefault());
                    temp.LoadData(period.Start.Month, good, total);
                }

                sourceResult.Add(temp);
            }
            return (journeysResult, sourceResult);            
        }
    }
}

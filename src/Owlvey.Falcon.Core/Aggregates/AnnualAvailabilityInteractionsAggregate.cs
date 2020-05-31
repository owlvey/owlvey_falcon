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

        public ( IEnumerable<AnnualAvailabilityInteractionsItemModel> services, IEnumerable<AnnualAvailabilityInteractionsItemModel>  sources ) Execute() {
            var periods = Period.ToYearPeriods();
            var servicesResult = new List<AnnualAvailabilityInteractionsItemModel>();            
            var sourceResult = new List<AnnualAvailabilityInteractionsItemModel>();
            foreach (var service in this.Product.Services)
            {
                var temp = new AnnualAvailabilityInteractionsItemModel(service.Id.Value, service.Name);
                foreach (var period in periods)
                {
                    int total = 0;
                    int good = 0;                    
                    foreach (var map in service.FeatureMap)
                    {
                        foreach (var indicator in map.Feature.Indicators)
                        {
                            var target = indicator.Source.SourceItems
                                .Where(c => 
                                c.Source.Group == SourceGroupEnum.Availability &&
                                c.Source.Kind == SourceKindEnum.Interaction && period.Contains(c.Target))
                                .ToList();

                            good += target.Sum(c => c.Good.GetValueOrDefault());
                            total += target.Sum(c => c.Total.GetValueOrDefault());                            
                        }
                    }
                    temp.LoadData(period.Start.Month, good, total);                    
                }
                servicesResult.Add(temp);
            }

            foreach (var source in this.Product.Sources.Where(c=>c.Group == SourceGroupEnum.Availability && c.Kind == SourceKindEnum.Interaction))
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
            return (servicesResult, sourceResult);            
        }
    }
}

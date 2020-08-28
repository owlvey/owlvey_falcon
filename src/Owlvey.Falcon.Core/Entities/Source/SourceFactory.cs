using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity
    {
        public static class Factory {

            public static IEnumerable<SourceItemEntity> CreateItemsFromRange(SourceEntity source,
            DateTime start, DateTime end, int? good, int? total, 
            DateTime on, string createdBy, SourceGroupEnum group)
            {
                return CreateItemsFromRange(source, start, end , good, total, null, on , createdBy, group);
            }
            public static IEnumerable<SourceItemEntity> CreateItemsFromRangeByMeasure(SourceEntity source,
                DateTime start, DateTime end, decimal measure,
                DateTime on, string createdBy, SourceGroupEnum group)
            {
                return CreateItemsFromRange(source, start, end, null, null, measure, on, createdBy, group);
            }
            public static IEnumerable<SourceItemEntity> CreateItemsFromRange(SourceEntity source,
            DateTime start, DateTime end, int? good, int? total, decimal? measure,
            DateTime on, string createdBy, SourceGroupEnum group)
            {
                var result = new List<SourceItemEntity>();
                var days = (decimal)DateTimeUtils.DaysDiff(end, start);
                if (good.HasValue && total.HasValue)
                {
                    if (good < 0 || total < 0)
                    {
                        throw new ApplicationException(
                            string.Format("good {0} and total {1} must be greater equal than zero", good, total));
                    }
                    var target_good = (int)Math.Ceiling(good.Value / days);
                    var target_total = (int)Math.Ceiling(total.Value / days);

                    for (int i = 0; i < days; i++)
                    {
                        var target = start.AddDays(i);
                        var entity = Factory.CreateItem(source,
                            target, target_good, target_total, on, createdBy, group);
                        result.Add(entity);
                    }
                }
                else if (measure.HasValue)
                {
                    for (int i = 0; i < days; i++)
                    {
                        var target = start.AddDays(i);
                        var entity = Factory.CreateItem(source,
                            target, 0, 0, measure.Value, on, createdBy, group);
                        result.Add(entity);
                    }
                }
                else {
                    throw new ApplicationException($" no values for measures ${total}, ${good}, ${measure}");
                }

                return result;

            }

            public static SourceItemEntity CreateItem(SourceEntity source,
                    DateTime target, int good, int total,
                    DateTime on, string createdBy, SourceGroupEnum group)
            {
                return CreateItem(source, target, good, total, null, on, createdBy, group);
            }
            public static SourceItemEntity CreateItemByMeasure(SourceEntity source,
                    DateTime target, decimal measure,
                    DateTime on, string createdBy, SourceGroupEnum group)
            {
                return CreateItem(source, target, null, null, measure, on, createdBy, group);
            }            

            public static SourceItemEntity CreateItem(SourceEntity source,
                    DateTime target, int? good, int? total, decimal? measeure,
                    DateTime on, string createdBy, SourceGroupEnum group)
            {                
                if (measeure.HasValue)
                {
                    if (measeure < 0)
                    {
                        throw new ApplicationException(string.Format("invalid value measure {0} ", measeure));
                    }
                    if (group != SourceGroupEnum.Latency)
                    {
                        if (measeure > 1 || measeure < 0)
                        {
                            throw new ApplicationException(string.Format("proportion {0} must be between 0 and 1", measeure));
                        }
                    }
                }
                else if (good.HasValue && total.HasValue) {
                    if (good < 0 || total < 0)
                    {
                        throw new ApplicationException(
                            string.Format("good {0} and total {1} must be greater equal than zero", good, total));
                    }
                    if (total < good)
                    {
                        throw new ApplicationException(
                              string.Format("good {0} is greater than total {1}", good, total));
                    }
                    measeure = QualityUtils.CalculateProportion(total, good);
                }
                else
                {
                    throw new ApplicationException($" no values for measures ${total}, ${good}, ${measeure}");
                }

                var entity = new SourceItemEntity()
                {
                    Target = target,
                    Good = good,
                    Total = total,
                    Measure = measeure.Value,
                    Source = source,
                    CreatedBy = createdBy,
                    ModifiedBy = createdBy,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Group = group
                };

                return entity;
            }            
            
            public static SourceEntity Create(ProductEntity product,  
                string name, DateTime on, string user)
            {
                string goodDefinitionAvailability = "e.g. successful requests, as measured from the laod balancer metrics, Any HTTP status othen than 500-599 is considered successful.";
                string totalDefinitionAvailability = "e.g. All requests measured from the load balancer.";

                string goodDefinitionLatency = "e.g.  request that return in minus specific time";
                string totalDefinitionLatency = "e.g. All requests measured from the load balancer.";

                string goodDefinitionExperience = "e.g. successful requests, as measured from the laod balancer metrics, means application errors ";
                string totalDefinitionExperience = "e.g. All requests measured from the load balancer.";

                SourceEntity entity = new SourceEntity();                                
                entity.Name = name;
                entity.GoodDefinitionAvailability = goodDefinitionAvailability;
                entity.TotalDefinitionAvailability = totalDefinitionAvailability;
                entity.GoodDefinitionLatency = goodDefinitionLatency;
                entity.TotalDefinitionLatency = totalDefinitionLatency;
                entity.GoodDefinitionExperience = goodDefinitionExperience;
                entity.TotalDefinitionExperience = totalDefinitionExperience;
                entity.CreatedBy = user;
                entity.ModifiedBy = user;
                entity.CreatedOn = on;
                entity.ModifiedOn = on;
                entity.Description = name;                              
                entity.Product = product;                
                entity.Avatar = "https://d2.alternativeto.net/dist/icons/restpack-html-to-pdf-api_135030.png?width=128&height=128&mode=crop&upscale=false";
                entity.Validate();
                return entity;
            }
        }
    }
}

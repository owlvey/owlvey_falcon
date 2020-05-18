using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity
    {
        public static class Factory {

            public static IEnumerable<SourceItemEntity> CreateInteractionsFromRange(SourceEntity source,
            DateTime start, DateTime end, int good, int total,
            DateTime on, string createdBy)
            {

                if (good < 0 || total < 0)
                {
                    throw new ApplicationException(
                        string.Format("good {0} and total {1} must be greater equal than zero", good, total));
                }

                var result = new List<SourceItemEntity>();

                var days = (decimal)DateTimeUtils.DaysDiff(end, start);

                var target_good = (int)Math.Ceiling(good / days);
                var target_total = (int)Math.Ceiling(total / days);

                for (int i = 0; i < days; i++)
                {
                    var target = start.AddDays(i);
                    var entity = Factory.CreateInteraction(source,
                        target, target_good, target_total, on, createdBy);
                    result.Add(entity);
                }

                return result;

            }


            public static SourceItemEntity CreateInteraction(SourceEntity source,
                    DateTime target, int good, int total,
                    DateTime on, string createdBy)
            {
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

                var entity = new SourceItemEntity()
                {
                    Target = target,
                    Good = good,
                    Total = total,
                    Measure = QualityUtils.CalculateProportion(total, good),
                    Source = source,
                    CreatedBy = createdBy,
                    ModifiedBy = createdBy,
                    CreatedOn = on,
                    ModifiedOn = on,
                };

                return entity;
            }
            public static IEnumerable<SourceItemEntity> CreateProportionFromRange(SourceEntity source,
              DateTime start, DateTime end, decimal proportion,
              DateTime on, string createdBy)
            {
                var result = new List<SourceItemEntity>();
                var days = (decimal)DateTimeUtils.DaysDiff(end, start);
                for (int i = 0; i < days; i++)
                {
                    var target = start.AddDays(i);
                    var entity = Factory.CreateProportion(source,
                        target, proportion, on, createdBy);
                    result.Add(entity);
                }
                return result;
            }


            public static IEnumerable<SourceItemEntity> CreateLatencyFromRange(SourceEntity source,
              DateTime start, DateTime end, decimal proportion,
              DateTime on, string createdBy)
            {
                var result = new List<SourceItemEntity>();
                var days = (decimal)DateTimeUtils.DaysDiff(end, start);
                for (int i = 0; i < days; i++)
                {
                    var target = start.AddDays(i);
                    var entity = Factory.CreateLatency(source,
                        target, proportion, on, createdBy);
                    result.Add(entity);
                }
                return result;
            }

            public static SourceItemEntity CreateProportion(SourceEntity source,
                  DateTime target, decimal proportion,
                  DateTime on, string createdBy)
            {

                if (proportion > 1 || proportion < 0) {
                    throw new ApplicationException(string.Format("proportion {0} must be between 0 and 1", proportion));
                }

                var entity = new SourceItemEntity()
                {
                    Target = target,
                    Measure = proportion,
                    Source = source,
                    CreatedBy = createdBy,
                    ModifiedBy = createdBy,
                    CreatedOn = on,
                    ModifiedOn = on,
                };

                return entity;
            }

            public static SourceItemEntity CreateLatency(SourceEntity source,
                  DateTime target, decimal latency,
                  DateTime on, string createdBy)
            {

                if (source.Group != SourceGroupEnum.Latency) {
                    throw new ApplicationException(string.Format("this isn't a latency source", latency));
                }
                if (latency < 0)
                {
                    throw new ApplicationException(string.Format("latency {0} must be greater than 0", latency));
                }

                var entity = new SourceItemEntity()
                {
                    Target = target,
                    Measure = latency,
                    Source = source,
                    CreatedBy = createdBy,
                    ModifiedBy = createdBy,
                    CreatedOn = on,
                    ModifiedOn = on,
                };

                return entity;
            }
            public static SourceEntity Create(ProductEntity product,  
                string name, DateTime on, string user, SourceKindEnum kind,
                SourceGroupEnum group)
            {
                string goodDefinition = "e.g. successful requests, as measured from the laod balancer metrics, Any HTTP status othen than 500-599 is considered successful.";
                string totalDefinition = "e.g. All requests measured from the load balancer.";
                SourceEntity entity = new SourceEntity();
                if (group == SourceGroupEnum.Latency) {
                    kind = SourceKindEnum.MiliSeconds;
                    
                }
                entity.Kind = kind;
                entity.Name = name;
                entity.GoodDefinition = goodDefinition;
                entity.TotalDefinition = totalDefinition;
                entity.CreatedBy = user;
                entity.ModifiedBy = user;
                entity.CreatedOn = on;
                entity.ModifiedOn = on;
                entity.Description = name;
                entity.Kind = kind;
                entity.Group = group;
                entity.Product = product;                
                entity.Avatar = "https://d2.alternativeto.net/dist/icons/restpack-html-to-pdf-api_135030.png?width=128&height=128&mode=crop&upscale=false";
                entity.Validate();
                return entity;
            }
        }
    }
}

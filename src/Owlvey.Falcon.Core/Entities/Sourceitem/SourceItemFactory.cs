using Owlvey.Falcon.Core.Entities.Sourceitem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceItemEntity
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

                //if (source.Kind == SourceKindEnum.Proportion)
                //{
                //    for (int i = 0; i < days; i++)
                //    {
                //        var target = start.AddDays(i);
                //        var entity = SourceItemEntity.Factory.CreateInteraction(source, target, good, total, on, createdBy);
                //        result.Add(entity);
                //    }
                //}
                //else {
                var target_good = (int)Math.Ceiling(good / days);
                var target_total = (int)Math.Ceiling(total / days);

                for (int i = 0; i < days; i++)
                {
                    var target = start.AddDays(i);
                    var entity = SourceItemEntity.Factory.CreateInteraction(source,
                        target, target_good, target_total, on, createdBy);
                    result.Add(entity);
                }
                //}

                return result;

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
                    var entity = SourceItemEntity.Factory.CreateProportion(source, 
                        target, proportion, on, createdBy);
                    result.Add(entity);
                }       
                return result;
            }


            public static SourceItemEntity CreateInteraction(SourceEntity source, 
                DateTime target, int good, int total,
                DateTime on, string createdBy) {

                if (good < 0 || total < 0) {
                    throw new ApplicationException(
                        string.Format("good {0} and total {1} must be greater equal than zero", good, total));
                }

                SourceItemEntity entity = null;                                 

                entity = new InteractionSourceItemEntity()
                {         
                    Target = target,
                    Good = good,
                    Total = total,
                    Proportion = QualityUtils.CalculateProportion(total, good),
                    Source = source,
                    CreatedBy = createdBy,
                    ModifiedBy = createdBy,
                    CreatedOn = on,
                    ModifiedOn = on,
                };
                
                return entity;
            }

            public static SourceItemEntity CreateProportion(SourceEntity source,
              DateTime target, decimal proportion,
              DateTime on, string createdBy)
            {                

                SourceItemEntity entity = null;

                entity = new ProportionSourceItemEntity()
                {
                    Target = target,                    
                    Proportion = proportion,
                    Source = source,
                    CreatedBy = createdBy,
                    ModifiedBy = createdBy,
                    CreatedOn = on,
                    ModifiedOn = on,
                };

                return entity;
            }
        }
    }
}

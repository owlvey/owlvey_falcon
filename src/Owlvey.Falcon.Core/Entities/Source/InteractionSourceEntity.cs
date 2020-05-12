using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Owlvey.Falcon.Core.Entities
{
    public class InteractionSourceEntity: SourceEntity
    {

        #region Factory
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
                    var entity = InteractionSourceEntity.Factory.CreateInteraction(source,
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
                var entity = new InteractionSourceItemEntity()
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
        }
        #endregion

        public SourceItemEntity AddSourceItem(int good, int total, DateTime target, DateTime on, string createdBy ) {
            var result = InteractionSourceEntity.Factory.CreateInteraction(this, target, good, total, on, createdBy);
            this.SourceItems.Add(result);
            return result;
        }

        public int GetTotal()
        {
            return this.SourceItems.OfType<InteractionSourceItemEntity>().Sum(c => c.Good);
        }

        public int GetGood()
        {
            return this.SourceItems.OfType<InteractionSourceItemEntity>().Sum(c => c.Total);
        }        

    }
}

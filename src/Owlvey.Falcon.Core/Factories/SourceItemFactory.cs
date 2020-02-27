using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceItemEntity
    {
        public static class Factory {


            public static IEnumerable<SourceItemEntity> CreateFromRange(SourceEntity source, DateTime start, DateTime end, int good, int total,
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
                    var entity = SourceItemEntity.Factory.Create(source, target, target_good, target_total, on, createdBy);
                    result.Add(entity);
                }
                return result;

            }


            public static SourceItemEntity Create(SourceEntity source, 
                DateTime target, int good, int total,
                DateTime on, string createdBy) {


                if (good < 0 || total < 0) {
                    throw new ApplicationException(
                        string.Format("good {0} and total {1} must be greater equal than zero", good, total));
                }


                var entity = new SourceItemEntity()
                {         
                    Target = target,
                    Good = good,
                    Total = total,
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class ProportionSourceEntity : SourceEntity
    {        
        #region  Factory
        public static class Factory
        {
            public static IEnumerable<SourceItemEntity> CreateProportionFromRange(SourceEntity source,
                DateTime start, DateTime end, decimal proportion,
                DateTime on, string createdBy)
            {
                var result = new List<SourceItemEntity>();
                var days = (decimal)DateTimeUtils.DaysDiff(end, start);
                for (int i = 0; i < days; i++)
                {
                    var target = start.AddDays(i);
                    var entity = ProportionSourceEntity.Factory.CreateProportion(source,
                        target, proportion, on, createdBy);
                    result.Add(entity);
                }
                return result;
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
        

        #endregion


    }
}

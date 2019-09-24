using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceItemEntity
    {
        public static class Factory {

            public static SourceItemEntity Create(SourceEntity source, 
                DateTime start, DateTime end, int good, int total,
                DateTime on, string createdBy) {


                if (good < 0 || total < 0) {
                    throw new ApplicationException(
                        string.Format("good {0} and total {1} must be greater equal than zero", good, total));
                }


                var entity = new SourceItemEntity()
                {         
                    Start = start,
                    End = end,
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

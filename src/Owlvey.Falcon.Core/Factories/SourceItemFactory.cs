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

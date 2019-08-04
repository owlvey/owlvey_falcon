using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class JournalEntity
    {
        public static class Factory {

            public static JournalEntity Create(string goodDefinition, string totalDefinition, string createdBy)
            {
                var entity = new JournalEntity()
                {
                    GoodDefinition = goodDefinition,
                    TotalDefinition = totalDefinition,
                    CreatedBy = createdBy
                };

                entity.Create(createdBy, DateTime.UtcNow);

                return entity;
            }
        }
    }
}

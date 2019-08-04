using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class JournalEntity
    {
        public static class Factory {

            public static JournalEntity Create(string goodDefinition, string totalDefinition, DateTime on, string user)
            {
                var entity = new JournalEntity()
                {
                    GoodDefinition = goodDefinition,
                    TotalDefinition = totalDefinition,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                };

                return entity;
            }
        }
    }
}

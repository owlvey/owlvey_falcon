using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity
    {
        public static class Factory {

            public static SourceEntity Create(string goodDefinition, string totalDefinition, DateTime on, string user)
            {
                var entity = new SourceEntity()
                {
                    GoodDefinition = goodDefinition,
                    TotalDefinition = totalDefinition,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                };
                entity.Validate();
                return entity;
            }
        }
    }
}

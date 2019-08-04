using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadEntity
    {
        public static class Factory {

            public static SquadEntity Create(string name, string description, string createdBy)
            {
                var entity = new SquadEntity()
                {
                    Name = name,
                    Description = description,
                    CreatedBy = createdBy
                };

                entity.Create(createdBy, DateTime.UtcNow);

                return entity;
            }
        }
    }
}

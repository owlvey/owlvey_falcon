using System;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core
{
    public class AnchorFactory
    {
        public static class Factory {
            public static AnchorEntity Create(string name, DateTime on, string user)
            {
                var entity = new AnchorEntity()
                {
                    Name = name,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Target = new DateTime(on.Year, 1, 1)
                };                
                entity.Validate();
                return entity;
            }
        }
    }
}

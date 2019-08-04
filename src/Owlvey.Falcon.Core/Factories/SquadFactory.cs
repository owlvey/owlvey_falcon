using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadEntity
    {
        public static class Factory {
            public static SquadEntity Create(string user, DateTime on, string name) {
                var result = new SquadEntity
                {
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Name = name                    
                };
                return result;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class MemberEntity
    {
        public static class Factory {

            public static MemberEntity Create(SquadEntity squad, UserEntity user, DateTime on, string createdBy) {
                var entity = new MemberEntity()
                {
                     CreatedBy = createdBy,
                     ModifiedBy = createdBy,
                     CreatedOn = on,
                     ModifiedOn = on,
                     Squad = squad,
                     User = user
                };
                entity.Validate();
                return entity;
            }
        }
    }
}

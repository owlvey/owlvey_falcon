using System.Collections.Generic;

namespace Owlvey.Falcon.Core.Entities
{
    public class UserEntityCompare : IEqualityComparer<UserEntity>
    {
        public bool Equals(UserEntity x, UserEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(UserEntity obj)
        {
            return obj.Id.Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class SourceEntityComparer : IEqualityComparer<SourceEntity>
    {
        public bool Equals(SourceEntity x, SourceEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(SourceEntity obj)
        {
            return obj.Id.Value;
        }
    }

}

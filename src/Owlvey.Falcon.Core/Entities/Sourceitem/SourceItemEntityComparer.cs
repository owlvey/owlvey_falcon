using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class SourceItemEntityComparer : IEqualityComparer<SourceItemEntity>
    {
        public bool Equals(SourceItemEntity x, SourceItemEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(SourceItemEntity obj)
        {
            return obj.Id.Value;
        }
    }
}

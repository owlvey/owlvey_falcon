using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Owlvey.Falcon.Core.Entities
{
    public class InteractionSourceEntity: SourceEntity
    {
        [NotMapped]
        public int Good
        {
            get
            {
                return this.SourceItems.Sum(c => c.Good);
            }
        }
        [NotMapped]
        public int Total
        {
            get
            {
                return this.SourceItems.Sum(c => c.Total);
            }
        }


    }
}

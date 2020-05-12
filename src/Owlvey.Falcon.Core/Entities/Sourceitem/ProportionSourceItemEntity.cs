using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class ProportionSourceItemEntity : SourceItemEntity
    {

      
        public void Update(decimal proportion, DateTime target)
        {            
            this.Proportion = proportion;
            this.Target = target;
        }
    }
}

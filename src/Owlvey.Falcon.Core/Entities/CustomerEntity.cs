using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class CustomerEntity : BaseEntity
    {
        public string Name { get; set; }        
        
        internal virtual ICollection<ProductEntity> Products { get; set; }
        
    }
}

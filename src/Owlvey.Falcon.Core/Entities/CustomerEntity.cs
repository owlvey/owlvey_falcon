using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }        
        
        internal virtual ICollection<ProductEntity> Products { get; set; }
    }
}

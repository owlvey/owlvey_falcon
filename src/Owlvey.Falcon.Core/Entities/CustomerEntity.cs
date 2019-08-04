using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity : BaseEntity
    {
<<<<<<< HEAD
        public CustomerEntity() {
            this.Products = new List<ProductEntity>();
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Avatar { get; set; }
=======
        [Required]
        public string Name { get; set; }        
>>>>>>> 4c019572161af3f9d8d9106d964c228d54e41492
        
        internal virtual ICollection<ProductEntity> Products { get; set; }
    }
}

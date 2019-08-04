using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity : BaseEntity
    {
        public CustomerEntity() {
            this.Products = new List<ProductEntity>();
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Avatar { get; set; }

        public virtual ICollection<ProductEntity> Products { get; set; }

        public void AddProduct(ProductEntity entity) {
            this.Products.Add(entity); 
        }
    }
}

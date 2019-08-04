using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity : BaseEntity
    {
        public CustomerEntity() {
            this.Products = new List<ProductEntity>();
        }

        public string Name { get; set; }                
        
        public virtual ICollection<ProductEntity> Products { get; set; }

        public void AddProduct(ProductEntity product) {
            this.Products.Add(product);
        }
    }
}

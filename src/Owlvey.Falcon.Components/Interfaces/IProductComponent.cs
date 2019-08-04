using Owlvey.Falcon.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Interfaces
{
    public interface IProductComponent
    {
        Task<BaseComponentResultRp> CreateProduct(ProductPostRp model);
        Task<BaseComponentResultRp> UpdateProduct(int id, ProductPutRp model);
        Task<BaseComponentResultRp> DeleteProduct(int id);
    }
}

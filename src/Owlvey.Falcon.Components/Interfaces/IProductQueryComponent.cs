using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface IProductQueryComponent
    {
        Task<IEnumerable<ProductGetListRp>> GetProducts();
        Task<ProductGetRp> GetProductById(int id);
    }
}

using Owlvey.Falcon.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Interfaces
{
    public interface IProductQueryComponent
    {
        Task<IEnumerable<ProductGetListRp>> GetProducts();
        Task<ProductGetRp> GetProductById(int id);
    }
}

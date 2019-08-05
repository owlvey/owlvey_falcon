using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class ProductQueryComponent : BaseComponent, IProductQueryComponent
    {
        private readonly FalconDbContext _dbContext;

        public ProductQueryComponent(FalconDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Get Product by id
        /// </summary>
        /// <param name="key">Product Id</param>
        /// <returns></returns>
        public async Task<ProductGetRp> GetProductById(int id)
        {
            var entity = await this._dbContext.Products.FirstOrDefaultAsync(c=> c.Id.Equals(id));

            if (entity == null)
                return null;

            return new ProductGetRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }

        /// <summary>
        /// Get All Product
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductGetListRp>> GetProducts()
        {
            var entities = await this._dbContext.Products.ToListAsync();

            return entities.Select(entity => new ProductGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}

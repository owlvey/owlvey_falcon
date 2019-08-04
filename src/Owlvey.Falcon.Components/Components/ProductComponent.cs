using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class ProductComponent : BaseComponent, IProductComponent
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserIdentityService _identityService;

        public ProductComponent(IProductRepository productRepository,
            IUserIdentityService identityService)
        {
            this._productRepository = productRepository;
            this._identityService = identityService;
        }

        /// <summary>
        /// Create a new Product
        /// </summary>
        /// <param name="model">Product Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> CreateProduct(ProductPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();


            return result;
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="key">Product Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteProduct(int id)
        {
            var result = new BaseComponentResultRp();

            return result;
        }
        
        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="model">Product Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> UpdateProduct(int id, ProductPutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            return result;
        }
    }
}

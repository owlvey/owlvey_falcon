using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Owlvey.Falcon.Components
{
    public class ProductComponent : BaseComponent, IProductComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly IUserIdentityGateway _identityService;        

        public ProductComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper)
        {
            this._dbContext = dbContext;
            this._identityService = identityService;            
        }
        
        public async Task<BaseComponentResultRp> CreateProduct(ProductPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var customer = await this._dbContext.Customers.SingleAsync(c => c.Id == model.CustomerId);

            var entity = ProductEntity.Factory.Create(model.Name, 
                this._datetimeGateway.GetCurrentDateTime(),
                createdBy, customer);

            this._dbContext.Products.Add(entity);

            await this._dbContext.SaveChangesAsync();

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

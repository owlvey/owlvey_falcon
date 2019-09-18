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
using Owlvey.Falcon.Repositories.Products;

namespace Owlvey.Falcon.Components
{
    public class ProductComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        

        public ProductComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
            
        }

        public async Task<ProductGetListRp> CreateOrUpdate(CustomerEntity customer, string name, string description, string avatar) {
            var createdBy = this._identityService.GetIdentity();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var entity = await this._dbContext.Products.Where(c => c.CustomerId == customer.Id && c.Name == name).SingleOrDefaultAsync();
            if (entity == null)
            {
                entity = ProductEntity.Factory.Create(name, this._datetimeGateway.GetCurrentDateTime(), createdBy, customer);
            }
            entity.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, name, description, avatar);
            this._dbContext.Products.Update(entity);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<ProductGetListRp>(entity);
        }

        public async Task<ProductGetListRp> CreateProduct(ProductPostRp model)
        {            
            var createdBy = this._identityService.GetIdentity();
            var entity = await this._dbContext.GetProduct(model.CustomerId, model.Name);
            if (entity == null) {
                var customer = await this._dbContext.Customers.SingleAsync(c => c.Id == model.CustomerId);
                entity = ProductEntity.Factory.Create(model.Name,
                    this._datetimeGateway.GetCurrentDateTime(),
                    createdBy, customer);
                this._dbContext.Products.Add(entity);
                await this._dbContext.SaveChangesAsync();
            }

            return this._mapper.Map<ProductGetListRp>(entity);
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="key">Product Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteProduct(int id)
        {
            var result = new BaseComponentResultRp();
            var modifiedBy = this._identityService.GetIdentity();

            var product = await this._dbContext.Products.SingleAsync(c => c.Id == id);

            if (product == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }

            product.Delete(this._datetimeGateway.GetCurrentDateTime(), modifiedBy);

            this._dbContext.Products.Remove(product);

            await this._dbContext.SaveChangesAsync();

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

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var product = await this._dbContext.Products.Include(c=> c.Customer).SingleAsync(c => c.Id == id);

            if (product == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }

            // Validate if the resource exists.
            if (!product.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase)) {

                var customer = await this._dbContext.Customers.Include(c=> c.Products).SingleAsync(c => c.Id.Equals(product.Customer.Id));
                
                if (customer.Products.Any(c => c.Name.Equals(model.Name)))
                {
                    result.AddConflict($"The Resource {model.Name} has already been taken.");
                    return result;
                }
            }
                       
            product.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, name: model.Name, description: model.Description, avatar: null);

            this._dbContext.Update(product);

            await this._dbContext.SaveChangesAsync();

            return result;
        }
    }
}

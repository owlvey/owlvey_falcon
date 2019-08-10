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
    public class ProductComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        

        public ProductComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
            
        }
        
        public async Task<BaseComponentResultRp> CreateProduct(ProductPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var customer = await this._dbContext.Customers.Include(c=> c.Products).SingleAsync(c => c.Id == model.CustomerId);

            // Validate if the resource exists.
            if (customer.Products.Any(c => c.Name.Equals(model.Name))) {
                result.AddConflict($"The Resource {model.Name} has already been taken.");
                return result;
            }

            var entity = ProductEntity.Factory.Create(model.Name, 
                this._datetimeGateway.GetCurrentDateTime(),
                createdBy, customer);
            
            this._dbContext.Products.Add(entity);

            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

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
           
            product.Name = model.Name ?? product.Name;
            product.Description = model.Description ?? product.Description;
            product.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy);

            this._dbContext.Update(product);

            await this._dbContext.SaveChangesAsync();

            return result;
        }
    }
}

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
    public class CustomerComponent : BaseComponent, ICustomerComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly IUserIdentityGateway _identityService;

        public CustomerComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService)
        {
            this._dbContext = dbContext;
            this._identityService = identityService;
        }

        /// <summary>
        /// Create a new Customer
        /// </summary>
        /// <param name="model">Customer Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> CreateCustomer(CustomerPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();
            var entity = CustomerEntity.Factory.Create(createdBy, DateTime.Now, model.Name);
            await this._dbContext.AddAsync(entity);
            await this._dbContext.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="key">Customer Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteCustomer(int id)
        {
            var result = new BaseComponentResultRp();

            return result;
        }
        
        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="model">Customer Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> UpdateCustomer(int id, CustomerPutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            return result;
        }
    }
}

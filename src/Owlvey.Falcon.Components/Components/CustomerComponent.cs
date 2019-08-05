using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Owlvey.Falcon.Components
{
    public class CustomerComponent : BaseComponent, ICustomerComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly IUserIdentityGateway _identityService;        

        public CustomerComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper): base(dateTimeGateway, mapper)
        {
            this._dbContext = dbContext;            
            this._identityService = identityService;
        }

        public async Task<BaseComponentResultRp> CreateCustomer(CustomerPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();
            var entity = CustomerEntity.Factory.Create(createdBy, DateTime.Now, model.Name);
            await this._dbContext.AddAsync(entity);
            await this._dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<BaseComponentResultRp> DeleteCustomer(int id)
        {
            var result = new BaseComponentResultRp();
            var modifiedBy = this._identityService.GetIdentity();

            var customer = await this._dbContext.Customers.SingleAsync(c=>c.Id == id);

            customer.Delete(this._datetimeGateway.GetCurrentDateTime(), modifiedBy);

            this._dbContext.Customers.Remove(customer);

            await this._dbContext.SaveChangesAsync();

            return result;
        }
        
        public async Task<BaseComponentResultRp> UpdateCustomer(int id, CustomerPutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var customer = await this._dbContext.Customers.SingleAsync(c => c.Id == id);

            customer.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, model.Name, model.Avatar);

            this._dbContext.Update(customer);

            await this._dbContext.SaveChangesAsync();
            
            return result;
        }
    }
}

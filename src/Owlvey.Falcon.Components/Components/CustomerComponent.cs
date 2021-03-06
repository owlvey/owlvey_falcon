using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Owlvey.Falcon.Repositories.Customers;
using Owlvey.Falcon.Repositories.Features;
using Owlvey.Falcon.Repositories.Journeys;
using Polly;

namespace Owlvey.Falcon.Components
{
    public class CustomerComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        

        public CustomerComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityGateway, IDateTimeGateway dateTimeGateway, IMapper mapper,
            ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityGateway, configuration)
        {
            this._dbContext = dbContext;            
        }

        public async Task<CustomerGetListRp> CreateCustomer(CustomerPostRp model)
        {            
            var createdBy = this._identityGateway.GetIdentity();

            var retryPolicy = Policy.Handle<DbUpdateException>()
                .WaitAndRetryAsync(this._configuration.DefaultRetryAttempts,
                i => this._configuration.DefaultPauseBetweenFails);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var entity = await this._dbContext.GetCustomer(model.Name);
                if (entity == null)
                {
                    entity = CustomerEntity.Factory.Create(createdBy,
                        this._datetimeGateway.GetCurrentDateTime()
                        , model.Name, defaultValue : model.Default);
                    await this._dbContext.AddAsync(entity);
                    await this._dbContext.SaveChangesAsync();
                }
                return this._mapper.Map<CustomerGetListRp>(entity);
            });            
        }

        public async Task<CustomerGetRp> CreateOrUpdate(string name,
            string avatar, string leaders)
        {
            var createdBy = this._identityGateway.GetIdentity();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var entity = await this._dbContext.Customers.Where(c => c.Name == name).SingleOrDefaultAsync();
            if (entity == null)
            {
                entity = CustomerEntity.Factory.Create(name, this._datetimeGateway.GetCurrentDateTime(), createdBy, defaultValue: false);
            }
            entity.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, name, avatar, leaders);
            this._dbContext.Customers.Update(entity);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<CustomerGetRp>(entity);
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await this._dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);
            if (customer != null) {
                var modifiedBy = this._identityGateway.GetIdentity();
                this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;                
                var journeys = await this._dbContext.Journeys.Where(c => c.Product.CustomerId == id).ToListAsync();
                var features = await this._dbContext.Features.Where(c => c.Product.CustomerId == id).ToListAsync();

                foreach (var journey in journeys)
                {
                    await this._dbContext.RemoveJourney(journey.Id.Value);
                }

                foreach (var feature in features)
                {
                    await this._dbContext.RemoveFeature(feature.Id.Value);
                }

                this._dbContext.Customers.Remove(customer);

                await this._dbContext.SaveChangesAsync();
            }
        }
        
        public async Task<BaseComponentResultRp> UpdateCustomer(int id, CustomerPutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityGateway.GetIdentity();

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var customer = await this._dbContext.Customers.SingleAsync(c => c.Id == id);

            if (customer == null) {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }
            
            customer.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, 
                model.Name, 
                model.Avatar,
                model.Leaders);

            this._dbContext.Update(customer);

            await this._dbContext.SaveChangesAsync();
            
            return result;
        }
    }
}

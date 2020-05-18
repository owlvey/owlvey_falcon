using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public class UserQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        public UserQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper,
            IUserIdentityGateway identityService, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, 
                identityService, configuration)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<UserGetListRp>> GetUsers()
        {
            var entities = await this._dbContext.Users.ToListAsync();

            return this._mapper.Map<IEnumerable<UserGetListRp>>(entities);
        }

        public async Task<UserGetRp> GetUserByEmail(string email)
        {
            var entity = await this._dbContext.Users.SingleAsync(c => c.Email == email);
            return this._mapper.Map<UserGetRp>(entity);
        }

        public async Task<UserGetRp> GetUserById(int id)
        {
            var entity = await this._dbContext.Users.SingleOrDefaultAsync(c => c.Id == id);

            if (entity == null)
                return null;

            var customers = await this._dbContext.Customers
                .Include(c=>c.Products)
                .ThenInclude(c=>c.Services)                
                .ToListAsync();            

            var customersSquads = await this._dbContext.Customers
                .Include(c => c.Products)
                .ThenInclude(c=>c.Features)
                .ThenInclude(c=>c.Squads)
                .ThenInclude(c=>c.Squad)                
                .ThenInclude(c => c.Members)                
                .ToListAsync();
            
            var result = this._mapper.Map<UserGetRp>(entity);
            
            foreach (var customer in customers)
            {
                foreach (var product in customer.Products)
                {
                    if ( product.ValidateLeader(entity.Email)) {
                        var temp = this._mapper.Map<ProductGetListRp>(product);                        
                        result.Products.Add(temp);
                    }

                    foreach (var service in product.Services)
                    {
                        if (service.ValidateLeader(entity.Email))
                        {
                            var temp = new Dictionary<string, object>();
                            temp["customerId"] = customer.Id;
                            temp["customer"] = customer.Name;
                            temp["productId"] = product.Id;
                            temp["product"] = product.Name;
                            temp["serviceId"] = service.Id;
                            temp["service"] = service.Name;
                            temp["slo"] = service.AvailabilitySlo;
                            result.Services.Add(temp);
                        }                            
                    }
                }
            }

            foreach (var customer in customersSquads)
            {
                foreach (var product in customer.Products)
                {
                    foreach (var feature in product.Features)
                    {
                        foreach (var squad in feature.Squads)
                        {
                            foreach (var member in squad.Squad.Members)
                            {
                                if (member.UserId == id) {
                                    var temp = new Dictionary<string, object>
                                    {
                                        ["customerId"] = customer.Id,
                                        ["customer"] = customer.Name,
                                        ["productId"] = product.Id,
                                        ["product"] = product.Name,
                                        ["featureId"] = feature.Id,
                                        ["feature"] = feature.Name,
                                        ["squadId"] = squad.Id,
                                        ["squad"] = squad.Squad.Name,
                                    };
                                    result.Features.Add(temp);
                                }
                            }
                        }                                                
                    }
                }
            }
            return result;
        }

    }
}


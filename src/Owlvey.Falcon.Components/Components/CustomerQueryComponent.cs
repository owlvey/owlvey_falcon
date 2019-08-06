using Owlvey.Falcon.Components;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Gateways;

namespace Owlvey.Falcon.Components
{
    public class CustomerQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;        
        public CustomerQueryComponent(FalconDbContext dbContext, IMapper mapper, IDateTimeGateway dateTimeGateway, IUserIdentityGateway identityService) : base(dateTimeGateway, mapper, identityService)
        {            
            this._dbContext = dbContext;
        }


        public async Task<CustomerGetRp> GetCustomerById(int id)
        {
            var entities = await this._dbContext.Customers.Where(c => c.Id.Equals(id)).ToListAsync();

            var entity = entities.FirstOrDefault();            

            if (entity == null)
                return null;

            return this._mapper.Map<CustomerGetRp>(entity);
        }

        public async Task<CustomerGetRp> GetCustomerByName(string name)
        {
            var entity = await this._dbContext.Customers.SingleAsync(c => c.Name.Equals(name));
            return this._mapper.Map<CustomerGetRp>(entity);
        }



        /// <summary>
        /// Get All Customer
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerGetListRp>> GetCustomers()
        {            
            var entities = await this._dbContext.Customers.ToListAsync();

            return entities.Select(entity => new CustomerGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}

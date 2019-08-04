using Owlvey.Falcon.Components.Interfaces;
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Components.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Services
{
    public class CustomerQueryComponent : BaseComponent, ICustomerQueryComponent
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerQueryComponent(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        /// <summary>
        /// Get customer by id
        /// </summary>
        /// <param name="key">Customer Id</param>
        /// <returns></returns>
        public async Task<CustomerGetRp> GetCustomerById(int id)
        {
            var entity = await this._customerRepository.FindFirst(c=> c.Id.Equals(id));

            if (entity == null)
                return null;

            return new CustomerGetRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }

        /// <summary>
        /// Get All Customer
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerGetListRp>> GetCustomers()
        {
            var entities = await this._customerRepository.GetAll();

            return entities.Select(entity => new CustomerGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}

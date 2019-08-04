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
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserIdentityGateway _identityService;

        public CustomerComponent(ICustomerRepository customerRepository,
            IUserIdentityGateway identityService)
        {
            this._customerRepository = customerRepository;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("customers")]
    public class CustomerController : BaseController
    {
        private readonly CustomerQueryComponent _customerQueryService;
        private readonly CustomerComponent _customerService;
        
        public CustomerController(CustomerQueryComponent CustomerQueryService,
                                    CustomerComponent CustomerService) : base()
        {
            this._customerQueryService = CustomerQueryService;
            this._customerService = CustomerService;
        }

        /// <summary>
        /// Get Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(CustomerPostRp), 200)]
        public async Task<IActionResult> Get()
        {
            var model = await this._customerQueryService.GetCustomers();
            return this.Ok(model);
        }

        /// <summary>
        /// Get Customer by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetCustomerId")]
        [ProducesResponseType(typeof(CustomerGetRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this._customerQueryService.GetCustomerById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        /// <summary>
        /// Create a new Customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Customers
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]CustomerPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._customerService.CreateCustomer(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");
            var newResource = await this._customerQueryService.GetCustomerById(id);

            return this.Created(Url.RouteUrl("GetCustomerId", new { id = id }), newResource);
        }

        /// <summary>
        /// Update an Customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, [FromBody]CustomerPutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._customerService.UpdateCustomer(id, resource);

            if (response.HasNotFounds())
            {
                return this.NotFound(response.GetNotFounds());
            }

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            return this.Ok();
        }

        /// <summary>
        /// Delete an Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._customerService.DeleteCustomer(id);

            if (response.HasNotFounds())
            {
                return this.NotFound(response.GetNotFounds());
            }

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            return this.NoContent();
        }


    }
}

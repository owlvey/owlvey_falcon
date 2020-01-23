using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ProductQueryComponent _productQueryComponent;

        public CustomerController(CustomerQueryComponent CustomerQueryService,
                                  CustomerComponent CustomerService,
                                  ProductQueryComponent productQueryComponent) : base()
        {
            this._productQueryComponent = productQueryComponent;
            this._customerQueryService = CustomerQueryService;
            this._customerService = CustomerService;
        }

        /// <summary>
        /// Get Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerGetListRp>), 200)]
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
            CustomerGetRp model;            
            model = await this._customerQueryService.GetCustomerById(id);
            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            model.Products = await this._productQueryComponent.GetProducts(id);           

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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody]CustomerPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._customerService.CreateCustomer(resource);            

            return this.Created(Url.RouteUrl("GetCustomerId", new { id = response.Id }), response);
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
        [Authorize(Policy = "RequireAdminRole")]
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
        [Authorize(Policy = "RequireAdminRole")]
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

        [HttpGet("{id}/squads/reports/graph")]
        [ProducesResponseType(typeof(GraphGetRp), 200)]
        public async Task<IActionResult> GetSquadGraph(int id, DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                GraphGetRp result = await this._customerQueryService.GetSquadsGraph(id, start.Value, end.Value);
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }

        }

        #region Reports

        [HttpGet("dashboard/products/services")]
        [ProducesResponseType(typeof(CustomerDashboardRp), 200)]
        public async Task<IActionResult> GetDashboardProductServices(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                var result = await this._customerQueryService.GetCustomersDashboardProductServices(start, end);
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }
        }

        #endregion

    }
}
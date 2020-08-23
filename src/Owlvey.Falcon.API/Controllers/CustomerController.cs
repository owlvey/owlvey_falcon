using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("customers")]
    public class CustomerController : BaseController
    {
        private readonly CustomerQueryComponent _customerQueryComponent;
        private readonly CustomerComponent _customerComponent;
        private readonly ProductQueryComponent _productQueryComponent;

        public CustomerController(CustomerQueryComponent CustomerQuery,
                                  CustomerComponent CustomerComponent,
                                  ProductQueryComponent productQueryComponent) : base()
        {
            this._productQueryComponent = productQueryComponent;
            this._customerQueryComponent = CustomerQuery;
            this._customerComponent = CustomerComponent;
        }

        /// <summary>
        /// Get Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerGetListRp>), 200)]
        public async Task<IActionResult> Get(DateTime? start, DateTime? end)
        {
            var model = await this._customerQueryComponent.GetCustomersQuality(new Core.Values.DatePeriodValue(start, end));
            return this.Ok(model);
        }
        [HttpGet("lite")]
        [ProducesResponseType(typeof(IEnumerable<CustomerLiteRp>), 200)]
        public async Task<IActionResult> GetLite(int customerId)
        {
            var model = await this._customerQueryComponent.GetCustomers();
            return this.Ok(model);
        }

        /// <summary>
        /// Get Customer by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetCustomerId")]
        [ProducesResponseType(typeof(CustomerBaseRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            CustomerGetRp model;            
            model = await this._customerQueryComponent.GetCustomerById(id);
            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");                        

            return this.Ok(model);
        }

        [HttpGet("{id}/quality")]
        [ProducesResponseType(typeof(CustomerGetRp), 200)]
        public async Task<IActionResult> GetByIdQuality(int id, DateTime? start, DateTime? end)
        {
            CustomerGetRp model;
            model = await this._customerQueryComponent.GetCustomerByIdWithQuality(id, new DatePeriodValue(start, end));
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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody]CustomerPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._customerComponent.CreateCustomer(resource);            

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

            var response = await this._customerComponent.UpdateCustomer(id, resource);

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
            await this._customerComponent.DeleteCustomer(id);           

            return this.Ok();
        }

        [HttpGet("{id}/squads/reports/graph")]
        [ProducesResponseType(typeof(GraphGetRp), 200)]
        public async Task<IActionResult> GetSquadGraph(int id, DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                GraphGetRp result = await this._customerQueryComponent.GetSquadsGraph(id, new DatePeriodValue(start.Value, end.Value));
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }

        }

        #region Reports

        [HttpGet("dashboard/products/journeys")]
        [ProducesResponseType(typeof(CustomerDashboardRp), 200)]
        public async Task<IActionResult> GetDashboardProductJourneys(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                var result = await this._customerQueryComponent.GetCustomersDashboardProductJourneys(start, end);
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
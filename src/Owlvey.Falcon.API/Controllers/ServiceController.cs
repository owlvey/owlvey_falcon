using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("services")]
    public class ServiceController : BaseController
    {
        private readonly ServiceQueryComponent _serviceQueryService;
        private readonly ServiceComponent _serviceService;
        
        public ServiceController(ServiceQueryComponent serviceQueryService,
                                 ServiceComponent serviceService) : base()
        {
            this._serviceQueryService = serviceQueryService;
            this._serviceService = serviceService;
        }

        /// <summary>
        /// Get Services
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServicePostRp), 200)]
        public async Task<IActionResult> Get(int? productId)
        {
            if (!productId.HasValue) {
                return this.BadRequest("product id is requerid");
            }

            var model = await this._serviceQueryService.GetServices(productId.Value);
            return this.Ok(model);
        }

        /// <summary>
        /// Get Service by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetServiceId")]
        [ProducesResponseType(typeof(ServiceGetRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this._serviceQueryService.GetServiceById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        /// <summary>
        /// Create a new Service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Services
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]ServicePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._serviceService.CreateService(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");
            var newResource = await this._serviceQueryService.GetServiceById(id);

            return this.Created(Url.RouteUrl("GetServiceId", new { id = id }), newResource);
        }

        /// <summary>
        /// Update an Service
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, [FromBody]ServicePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._serviceService.UpdateService(id, resource);

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
        /// Delete an Service
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._serviceService.DeleteService(id);

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

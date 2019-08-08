using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    public partial class CustomerController : BaseController
    {
        /// <summary>
        /// Get Services
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/products/{productId}/services")]
        [ProducesResponseType(typeof(ServicePostRp), 200)]
        public async Task<IActionResult> Get(int? productId)
        {
            if (!productId.HasValue) {
                return this.BadRequest("product id is requerid");
            }
            var model = await this._serviceQueryService.GetServices(productId.Value);
            return this.Ok(model);
        }
        
        [HttpGet("{id}/products/{productId}/services/{serviceId}", Name = "GetServiceId")]
        [ProducesResponseType(typeof(ServiceGetRp), 200)]
        public async Task<IActionResult> GetserviceById(int id, int productId, int serviceId)
        {
            var model = await this._serviceQueryService.GetServiceById(serviceId);

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
        /// <param name="productId">Product Id</param>
        /// <param name="id">Customer Id</param>
        /// <returns></returns>
        [HttpPost("{id}/products/{productId}/services")]
        [ProducesResponseType(typeof(ServiceGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostService(int id, int productId, [FromBody]ServicePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._serviceService.CreateService(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            var serviceId = response.GetResult<int>("Id");
            var newResource = await this._serviceQueryService.GetServiceById(id);

            return this.Created(Url.RouteUrl("GetServiceId", new { id = id, productId = productId, serviceId = serviceId }), newResource);
        }

        /// <summary>
        /// Update an Service
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="serviceId">ServiceId Id</param>
        /// <param name="productId">Product Id</param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}/products/{productId}/services/{serviceId}")]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutService(int id, int productId,  int serviceId, [FromBody]ServicePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._serviceService.UpdateService(serviceId, resource);

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
        /// <param name="id">Customer Id</param>
        /// <param name="serviceId">ServiceId Id</param>
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        [HttpDelete("{id}/products/{productId}/services/{serviceId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteService(int id, int productId, int serviceId)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._serviceService.DeleteService(serviceId);

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

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
        /// Get Features
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/products/{productId}/features")]
        [ProducesResponseType(typeof(FeaturePostRp), 200)]
        public async Task<IActionResult> Get(int id, int productId)
        {
            var model = await this._featureQueryService.GetFeatures(productId);
            return this.Ok(model);
        }

        /// <summary>
        /// Get Feature by id 
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="productId">Product Id</param>
        /// <param name="featureId">Feature Id</param>
        /// <returns></returns>
        [HttpGet("{id}/products/{productId}/features/{featureId}", Name = "GetFeatureId")]
        [ProducesResponseType(typeof(FeatureGetRp), 200)]
        public async Task<IActionResult> GetFeatureById(int id, int productId, int featureId)
        {
            var model = await this._featureQueryService.GetFeatureById(featureId);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        /// <summary>
        /// Create a new Feature
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Features
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Customer Id</param>
        /// <param name="productId">Product Id</param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost("{id}/products/{productId}/features")]
        [ProducesResponseType(typeof(FeatureGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post(int id, int productId, [FromBody]FeaturePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._featureService.CreateFeature(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            var featureId = response.GetResult<int>("Id");
            var newResource = await this._featureQueryService.GetFeatureById(id);

            return this.Created(Url.RouteUrl("GetFeatureId", new { id = id, productId = productId, featureId = featureId }), newResource);
        }

        /// <summary>
        /// Update an Feature
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="featureId">Feature Id</param>
        /// <param name="productId">Product Id</param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}/products/{productId}/features/{featureId}")]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, int productId, int featureId, [FromBody]FeaturePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._featureService.UpdateFeature(featureId, resource);

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
        /// Delete an Feature
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="productId">Product Id</param>
        /// <param name="featureId">Feature Id</param>
        /// <returns></returns>
        [HttpDelete("{id}/products/{productId}/features/{featureId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id, int productId, int featureId)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._featureService.DeleteFeature(featureId);

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

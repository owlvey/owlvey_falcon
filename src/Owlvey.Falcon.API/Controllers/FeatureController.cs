using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("features")]
    public class FeatureController : BaseController
    {
        private readonly FeatureQueryComponent _featureQueryService;
        private readonly FeatureComponent _featureService;
        
        public FeatureController(FeatureQueryComponent featureQueryService,
                                    FeatureComponent featureService) : base()
        {
            this._featureQueryService = featureQueryService;
            this._featureService = featureService;
        }

        /// <summary>
        /// Get Features
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(FeaturePostRp), 200)]
        public async Task<IActionResult> Get()
        {
            var model = await this._featureQueryService.GetFeatures();
            return this.Ok(model);
        }

        /// <summary>
        /// Get Feature by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FeatureGetRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this._featureQueryService.GetFeatureById(id);

            if (model == null)
                return this.NotFound($"The Key {id} doesn't exists.");

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
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FeaturePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._featureService.CreateFeature(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            return this.Ok();
        }

        /// <summary>
        /// Update an Feature
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]FeaturePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._featureService.UpdateFeature(id, resource);

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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._featureService.DeleteFeature(id);

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

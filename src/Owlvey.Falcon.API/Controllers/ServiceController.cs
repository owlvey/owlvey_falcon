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
        private readonly IServiceQueryComponent _ServiceQueryService;
        private readonly IServiceComponent _ServiceService;
        
        public ServiceController(IServiceQueryComponent ServiceQueryService,
                                    IServiceComponent ServiceService) : base()
        {
            this._ServiceQueryService = ServiceQueryService;
            this._ServiceService = ServiceService;
        }

        /// <summary>
        /// Get Services
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServicePostRp), 200)]
        public async Task<IActionResult> Get()
        {
            var model = await this._ServiceQueryService.GetServices();
            return this.Ok(model);
        }

        /// <summary>
        /// Get Service by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceGetRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this._ServiceQueryService.GetServiceById(id);

            if (model == null)
                return this.NotFound($"The Key {id} doesn't exists.");

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
        public async Task<IActionResult> Post([FromBody]ServicePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._ServiceService.CreateService(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            return this.Ok();
        }

        /// <summary>
        /// Update an Service
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ServicePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._ServiceService.UpdateService(id, resource);

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

            var response = await this._ServiceService.DeleteService(id);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components.Interfaces;
using Owlvey.Falcon.Components.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("squads")]
    public class SquadController : BaseController
    {
        private readonly ISquadQueryComponent _squadQueryService;
        private readonly ISquadComponent _squadService;
        
        public SquadController(ISquadQueryComponent squadQueryService,
                                    ISquadComponent squadService) : base()
        {
            this._squadQueryService = squadQueryService;
            this._squadService = squadService;
        }

        /// <summary>
        /// Get Squads
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(SquadPostRp), 200)]
        public async Task<IActionResult> Get()
        {
            var model = await this._squadQueryService.GetSquads();
            return this.Ok(model);
        }

        /// <summary>
        /// Get Squad by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this._squadQueryService.GetSquadById(id);

            if (model == null)
                return this.NotFound($"The Key {id} doesn't exists.");

            return this.Ok(model);
        }

        /// <summary>
        /// Create a new Squad
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Squads
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SquadPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._squadService.CreateSquad(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            return this.Ok();
        }

        /// <summary>
        /// Update an Squad
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]SquadPutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._squadService.UpdateSquad(id, resource);

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
        /// Delete an Squad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._squadService.DeleteSquad(id);

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

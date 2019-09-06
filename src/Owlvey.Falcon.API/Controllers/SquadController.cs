using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("squads")]
    public partial class SquadController : BaseController
    {
        private readonly SquadQueryComponent _squadQueryService;
        private readonly SquadComponent _squadService;        

        public SquadController(SquadQueryComponent squadQueryService, 
            SquadComponent squadService) 
        {
            this._squadQueryService = squadQueryService;
            this._squadService = squadService;            
        }

        /// <summary>
        /// Get Squads
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(SquadPostRp), 200)]
        public async Task<IActionResult> Get([FromQuery(Name = "customerId")]int customerId)
        {
            var model = await this._squadQueryService.GetSquads(customerId);
            return this.Ok(model);
        }

        /// <summary>
        /// Get Squad by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetSquadId")]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this._squadQueryService.GetSquadById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

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
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]SquadPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._squadService.CreateSquad(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");
            var newResource = await this._squadQueryService.GetSquadById(id);

            return this.Created(Url.RouteUrl("GetSquadId", new { id = id }), newResource);
        }


        [HttpPut("{id}/members/{userId}")]
        [ProducesResponseType(typeof(object), 200)]                
        public async Task<IActionResult> PutMember(int id, int userId)
        {
            await this._squadService.RegisterMember(id, userId);
            return this.Ok();            
        }

        [HttpDelete("{id}/members/{userId}")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> DeleteMember(int id, int userId)
        {
            await this._squadService.UnRegisterMember(id, userId);
            return this.Ok();
        }

        /// <summary>
        /// Update an Squad
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("squads")]
    public class SquadController : BaseController
    {
        private readonly SquadQueryComponent _squadQueryService;
        private readonly SquadComponent _squadService;        

        public SquadController(SquadQueryComponent squadQueryService, 
            SquadComponent squadService) 
        {
            this._squadQueryService = squadQueryService;
            this._squadService = squadService;            
        }

        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SquadGetListRp>), 200)]
        public async Task<IActionResult> Get([FromQuery]int customerId, 
            [FromQuery] DateTime? start,
            [FromQuery] DateTime? end)
        {
            if (start.HasValue)
            {
                var model = await this._squadQueryService.GetSquadsWithPoints(customerId, 
                    new DatePeriodValue(start.Value, end.Value));
                return this.Ok(model);
            }
            else {
                var model = await this._squadQueryService.GetSquads(customerId);
                return this.Ok(model);
            }
            
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
        [HttpGet("{id}/quality")]
        [ProducesResponseType(typeof(SquadQualityGetRp), 200)]
        public async Task<IActionResult> GetQualityById(int id, DateTime? start, DateTime? end)
        {
            if (start.HasValue)
            {
                var model = await this._squadQueryService.GetSquadByIdWithQuality(id, 
                    new DatePeriodValue( start.Value, end.Value));
                if (model == null)
                    return this.NotFound($"The Resource {id} doesn't exists.");

                return this.Ok(model);
            }
            return this.BadRequest();            
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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody]SquadPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._squadService.CreateSquad(resource);            
            
            return this.Created(Url.RouteUrl("GetSquadId", new { id = response.Id }), response);
        }


        [HttpPut("{id}/members/{userId}")]
        [ProducesResponseType(typeof(object), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> PutMember(int id, int userId)
        {
            await this._squadService.RegisterMember(id, userId);
            return this.Ok();            
        }

        [HttpDelete("{id}/members/{userId}")]
        [ProducesResponseType(typeof(object), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteMember(int id, int userId)
        {
            await this._squadService.UnRegisterMember(id, userId);
            return this.Ok();
        }

        [HttpGet("{id}/members/complement")]
        [ProducesResponseType(typeof(IEnumerable<UserGetListRp>), 200)]
        public async Task<IActionResult> ComplementMembers(int id)
        {
            var result = await this._squadService.GetUsersComplement(id);
            return this.Ok(result);
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
        [Authorize(Policy = "RequireAdminRole")]
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
        [Authorize(Policy = "RequireAdminRole")]
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

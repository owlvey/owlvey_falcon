using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    public partial class SquadController : BaseController
    {
        /// <summary>
        /// Get Members
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/members")]
        [ProducesResponseType(typeof(SquadPostRp), 200)]
        public async Task<IActionResult> GetMembers(int id)
        {
            var model = await this._memberQueryComponent.GetMembers(id);
            return this.Ok(model);
        }

        /// <summary>
        /// Get Members by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="memberId">Member Id</param>
        /// <returns></returns>
        [HttpGet("{id}/members/{memberId}", Name = "GetSquadMemberId")]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        public async Task<IActionResult> GetMembersById(int id, int memberId)
        {
            var model = await this._squadQueryService.GetSquadById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        /// <summary>
        /// Create a new Squad Member
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
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost("{id}/members")]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostMember(int id, [FromBody]MemberPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._memberComponent.CreateMember(resource);

            if (response.HasNotFounds())
            {
                return this.Conflict(response.GetNotFounds());
            }

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            var memberId = response.GetResult<int>("Id");
            var members = await this._memberQueryComponent.GetMembers(id);
            var newResource = members.FirstOrDefault(c => c.Id.Equals(memberId));

            return this.Created(Url.RouteUrl("GetSquadMemberId", new { id = id, memberId = memberId }), newResource);
        }

        /// <summary>
        /// Delete an Squad Member
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/members/{memberId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteMember(int id, int memberId)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._memberComponent.DeleteMember(id, memberId);

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

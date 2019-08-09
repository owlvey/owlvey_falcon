using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("members")]
    public class MemberController : BaseController
    {
        private readonly MemberQueryComponent _memberQueryComponent;
        private readonly MemberComponent _memberComponent;

        public MemberController(MemberQueryComponent memberQueryComponent, MemberComponent memberComponent) 
        {
            this._memberQueryComponent = memberQueryComponent;
            this._memberComponent = memberComponent;
        }
        /// <summary>
        /// Get Members
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(MemberGetRp), 200)]
        public async Task<IActionResult> GetMember(int squadId)
        {
            var model = await this._memberQueryComponent.GetMembersBySquad(squadId);
            return this.Ok(model);
        }

        /// <summary>
        /// Get Members by id 
        /// </summary>
        /// <param name="id">id</param>        
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetSquadMemberId")]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        public async Task<IActionResult> GetSquadMemberId(int id)
        {
            var model = await this._memberQueryComponent.GetMember(id);

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
        [HttpPost()]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostMember([FromBody]MemberPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._memberComponent.CreateMember(resource);

            if (response.HasNotFounds())
            {
                return this.Conflict(response.GetNotFounds());
            }

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");            
            var newResource = await this._memberQueryComponent.GetMember(id);            

            return this.Created(Url.RouteUrl("GetSquadMemberId", new { id }), newResource);
        }

        /// <summary>
        /// Delete an Squad Member
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteMember(int id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._memberComponent.DeleteMember(id);

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

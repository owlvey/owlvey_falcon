using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("squadProducts")]
    public class SquadProductController : BaseController
    {
        private SquadProductComponent _squadProductComponent;

        public SquadProductController(SquadProductComponent squadProductComponent)
        {
            this._squadProductComponent = squadProductComponent;
        }

        [HttpGet("{id}", Name = "GetSquadProductById")]
        [ProducesResponseType(typeof(SquadProductGetRp), 200)]
        public async Task<IActionResult> GetSquadProductById(int id)
        {
            var model = await this._squadProductComponent.GetId(id);
            return this.Ok(model);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<SquadProductGetListRp>), 200)]
        public async Task<IActionResult> GetAll([FromQuery(Name = "squadId")]int squadId)
        {
            var model = await this._squadProductComponent.GetBySquad(squadId);
            return this.Ok(model);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]SquadProductPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._squadProductComponent.CreateSquadProduct(resource);

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");
            var newResource = await this._squadProductComponent.GetId(id);


            return this.Created(Url.RouteUrl("GetSquadProductById", new { id }), newResource);
        }
    }
}

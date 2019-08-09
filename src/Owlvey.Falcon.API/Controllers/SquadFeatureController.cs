using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("squadFeatures")]
    public class SquadFeatureController : BaseController
    {
        private SquadFeatureComponent _squadFeatureComponent;

        public SquadFeatureController(SquadFeatureComponent squadFeatureComponent) {
            this._squadFeatureComponent = squadFeatureComponent;
        }
        
        [HttpGet("{id}", Name = "GetSquadFeatureById")]
        [ProducesResponseType(typeof(SquadFeatureGetRp), 200)]
        public async Task<IActionResult> GetSquadFeatureById(int id)
        {
            var model = await this._squadFeatureComponent.GetId(id);
            return this.Ok(model);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<SquadFeatureGetListRp>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var model = await this._squadFeatureComponent.GetAll();
            return this.Ok(model);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        [ProducesResponseType(409)]        
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post( [FromBody]SquadFeaturePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._squadFeatureComponent.CreateSquadFeature(resource);
            
            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");
            var newResource = await this._squadFeatureComponent.GetId(id);
            

            return this.Created(Url.RouteUrl("GetSquadFeatureById", new { id }), newResource);
        }
    }
}

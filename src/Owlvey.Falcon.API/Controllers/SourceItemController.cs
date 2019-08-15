using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("sourceItems")]
    public class SourceItemController: BaseController
    {
        private readonly SourceItemComponent _sourceItemComponent;

        public SourceItemController(SourceItemComponent sourceItemComponent)
        {
            this._sourceItemComponent = sourceItemComponent;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SourceItemGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]SourceItemPostRp model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var response = await this._sourceItemComponent.Create(model);

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");

            var newResource = await this._sourceItemComponent.GetById(id);

            return this.Created(Url.RouteUrl("GetBySourceItemId", new { id }), newResource);
        }
                
        [HttpGet("{id}", Name = "GetBySourceItemId")]
        [ProducesResponseType(typeof(SourceItemGetRp), 200)]
        public async Task<IActionResult> GetBySourceItemId(int id)
        {
            var model = await this._sourceItemComponent.GetById(id);
            return this.Ok(model);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<SourceItemGetRp>), 200)]
        public async Task<IActionResult> GetBySourceId(int? sourceId)
        {
            if (sourceId.HasValue)
            {
                var model = await this._sourceItemComponent.GetBySource(sourceId.Value);
                return this.Ok(model);
            }
            else {
                var model = await this._sourceItemComponent.GetAll();
                return this.Ok(model);
            }            
        }
    }
}

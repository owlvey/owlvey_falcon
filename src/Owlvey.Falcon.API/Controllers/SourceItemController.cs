﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core.Values;
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

            return this.Created(Url.RouteUrl("GetBySourceItemId", new { response.Id }), response);
        }

        [HttpPost("uptime")]
        [ProducesResponseType(typeof(SourceItemGetRp), 200)]        
        public async Task<IActionResult> PostUptime([FromBody]SourceItemUptimePostRp model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var response = await this._sourceItemComponent.Create(model);            

            return this.Created(Url.RouteUrl("GetBySourceItemId", new { response.Id }), response);
        }

        [HttpGet("{id}", Name = "GetBySourceItemId")]
        [ProducesResponseType(typeof(SourceItemGetRp), 200)]
        public async Task<IActionResult> GetBySourceItemId(int id)
        {
            var model = await this._sourceItemComponent.GetById(id);
            return this.Ok(model);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SourceItemGetRp), 200)]
        public async Task<IActionResult> DeleteSourceItem(int id)
        {
            var model = await this._sourceItemComponent.Delete(id);
            return this.Ok(model);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<SourceItemGetListRp>), 200)]
        public async Task<IActionResult> GetBySourceId(int? sourceId, DateTime? start, DateTime? end)
        {
            IEnumerable<SourceItemGetListRp> model = new List<SourceItemGetListRp>();
            if (sourceId.HasValue && start.HasValue && end.HasValue) {
                model = this._sourceItemComponent.GetBySourceIdAndDateRange(sourceId.Value, start.Value, end.Value);
            }
            else if (sourceId.HasValue)
            {
                model = await this._sourceItemComponent.GetBySource(sourceId.Value);                
            }
            else {
                model = await this._sourceItemComponent.GetAll();                
            }
            return this.Ok(model);
        }
        
    }
}

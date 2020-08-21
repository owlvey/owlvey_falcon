using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Components.Models;
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

        [HttpGet("{id}", Name = "GetBySourceItemId")]
        [ProducesResponseType(typeof(InteractiveSourceItemGetRp), 200)]
        public async Task<IActionResult> GetBySourceItemId(int id)
        {
            var model = await this._sourceItemComponent.GetById(id);
            return this.Ok(model);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(InteractiveSourceItemGetRp), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteSourceItem(int id)
        {
            var model = await this._sourceItemComponent.Delete(id);
            return this.Ok(model);
        }

        [HttpDelete()]
        [ProducesResponseType(typeof(InteractiveSourceItemGetRp), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteAllSourceItems(int sourceId)
        {
            var model = await this._sourceItemComponent.DeleteSource(sourceId);
            return this.Ok(model);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<SourceItemGetListRp>), 200)]
        public async Task<IActionResult> GetBySourceId(int? sourceId, DateTime? start, DateTime? end)
        {
            IEnumerable<SourceItemGetListRp> model = new List<SourceItemGetListRp>();
            if (sourceId.HasValue && start.HasValue && end.HasValue) {
                model = await this._sourceItemComponent.GetBySourceIdAndDateRange(sourceId.Value, start.Value, end.Value);
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

        [HttpGet("interactions")]
        [ProducesResponseType(typeof(IEnumerable<InteractionSourceItemGetListRp>), 200)]
        public async Task<IActionResult> GetInteractionsBySourceId(int? sourceId, DateTime? start, DateTime? end)
        {            
            if (sourceId.HasValue && start.HasValue && end.HasValue)
            {
                var model = await this._sourceItemComponent.GetInteractionsBySourceIdAndDateRange(sourceId.Value, start.Value, end.Value);
                return this.Ok(model);
            }            
            else
            {
                return this.BadRequest();
            }            
        }

    }
}

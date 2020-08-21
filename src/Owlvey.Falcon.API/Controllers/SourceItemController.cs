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
        [ProducesResponseType(typeof(SourceItemGetListRp), 200)]
        public async Task<IActionResult> GetBySourceItemId(int id)
        {
            var model = await this._sourceItemComponent.GetById(id);
            return this.Ok(model);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SourceItemGetListRp), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteSourceItem(int id)
        {
            var model = await this._sourceItemComponent.Delete(id);
            return this.Ok(model);
        }

        [HttpDelete()]
        [ProducesResponseType(typeof(SourceItemGetListRp), 200)]
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

        

        [HttpPost("latency")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(IEnumerable<SourceItemBaseRp>), 200)]
        public async Task<IActionResult> PostLatencyItem([FromBody]SourceItemLatencyPostRp model)
        {
            var result = await this._sourceItemComponent.CreateLatency(model);
            return this.Ok(result);
        }

   

        [HttpGet("latency/items")]
        [ProducesResponseType(typeof(IEnumerable<LatencySourceItemGetRp>), 200)]
        public async Task<IActionResult> GetLatencyItems(int id, DateTime? start, DateTime? end)
        {
            IEnumerable<LatencySourceItemGetRp> model = new List<LatencySourceItemGetRp>();
            if (start.HasValue && end.HasValue)
            {
                model = await this._sourceItemComponent.GetLatencyItems(id, new DatePeriodValue(start, end));
            }
            return this.Ok(model);
        }

        [HttpPost("experience/items")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(IEnumerable<SourceItemBaseRp>), 200)]
        public async Task<IActionResult> PostProportion([FromBody]SourceItemExperiencePostRp model)
        {
            var result = await this._sourceItemComponent.CreateExperienceItem(model);
            return this.Ok(result);
        }


        [HttpPost("availability/items")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(IEnumerable<SourceItemBaseRp>), 200)]
        public async Task<IActionResult> PostAvailability([FromBody]SourceItemAvailabilityPostRp model)
        {
            var result = await this._sourceItemComponent.CreateAvailabilityItem(model);
            return this.Ok(result);
        }

        [HttpGet("{id}/scalability")]
        [ProducesResponseType(typeof(ScalabilitySourceGetRp), 200)]
        public async Task<IActionResult> GetScalability(int id, 
            DateTime? start, DateTime? end)
        {            
            var model = await this._sourceItemComponent.GetScalability(id, 
                new DatePeriodValue(start, end));            
            return this.Ok(model);
        }

    }
}

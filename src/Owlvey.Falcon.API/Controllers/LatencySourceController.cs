using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("sources/latency")]
    public class LatencySourceController : BaseController
    {
        private LatencySourceComponent _sourceComponent;
        public LatencySourceController(LatencySourceComponent sourceComponent) : base()
        {
            this._sourceComponent = sourceComponent;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LatencySourceGetRp), 200)]
        public async Task<IActionResult> GetById(int id, DateTime? start, DateTime? end)
        {
            LatencySourceGetRp model = null;
            if (end.HasValue)
            {
                model = await this._sourceComponent.GetByIdWithDetail(id, new DatePeriodValue(start.Value, end.Value));
            }
            else {
                model = await this._sourceComponent.GetById(id);
            }
            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        [HttpGet("{id}/items")]
        [ProducesResponseType(typeof(IEnumerable<LatencySourceItemGetRp>), 200)]
        public async Task<IActionResult> GetProportionsBySourceId(int id, DateTime? start, DateTime? end)
        {
            IEnumerable<LatencySourceItemGetRp> model = new List<LatencySourceItemGetRp>();
            if (start.HasValue && end.HasValue)
            {
                model = await this._sourceComponent.GetItems(id, new DatePeriodValue(start, end));
            }
            return this.Ok(model);
        }
        [HttpPost("items")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(IEnumerable<SourceItemBaseRp>), 200)]
        public async Task<IActionResult> Post([FromBody]LatencySourceItemPostRp model)
        {
            var result = await this._sourceComponent.CreateLatency(model);
            return this.Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LatencySourceGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]LatencySourcePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._sourceComponent.Update(id, resource);            

            return this.Ok(response);
        }
    }
}

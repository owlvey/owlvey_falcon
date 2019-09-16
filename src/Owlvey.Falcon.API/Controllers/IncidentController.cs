using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("incidents")]
    public class IncidentController : BaseController
    {
        private readonly IncidentComponent _incidentComponent;
        public IncidentController(IncidentComponent incidentComponent ) {
            this._incidentComponent = incidentComponent;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IncidentGetListRp>), 200)]
        public async Task<IActionResult> Get(int productId, DateTime? start ,DateTime? end)
        {
            var model = await this._incidentComponent.Get(productId, new PeriodValue(start.Value, end.Value));
            return this.Ok(model);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<IncidentDetailtRp>), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var model = await this._incidentComponent.Get(id);
            return this.Ok(model);
        }

        [HttpGet("{id}/features/complement")]
        [ProducesResponseType(typeof(IEnumerable<FeatureLiteRp>), 200)]
        public async Task<IActionResult> GetFeatureComplement(int id)
        {
            var model = await this._incidentComponent.GetFeatureComplement(id);
            return this.Ok(model);
        }

        [HttpPut("{id}/features/{featureId}")]        
        public async Task<IActionResult> GetFeatureComplement(int id, int featureId)
        {
            await this._incidentComponent.RegisterFeature(id, featureId);
            return this.Ok();
        }

        [HttpDelete("{id}/features/{featureId}")]        
        public async Task<IActionResult> DeleteFeatureComplement(int id, int featureId)
        {
            await this._incidentComponent.UnRegisterFeature(id, featureId);
            return this.Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IEnumerable<IncidentPutRp>), 200)]
        public async Task<IActionResult> Put(int id, [FromBody] IncidentPutRp model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var result = await this._incidentComponent.Put(model);
            return this.Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<IndicatorGetListRp>), 200)]
        public async Task<IActionResult> Post([FromBody]IncidentPostRp model)
        {
            var result = await this._incidentComponent.Post(model);
            return this.Ok(result);
        }


    }
}

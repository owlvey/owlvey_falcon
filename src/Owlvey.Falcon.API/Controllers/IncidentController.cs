using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetList(int productId)
        {
            var model = await this._incidentComponent.GetByProduct(productId);
            return this.Ok(model);
        }

        [HttpGet("{id}", Name = "GetIncidentId")]
        [ProducesResponseType(typeof(IEnumerable<IncidentDetailtRp>), 200)]
        public async Task<IActionResult> GetIncidentId(int id)
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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetFeatureComplement(int id, int featureId)
        {
            await this._incidentComponent.RegisterFeature(id, featureId);
            return this.Ok();
        }

        [HttpDelete("{id}/features/{featureId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteFeatureComplement(int id, int featureId)
        {
            await this._incidentComponent.UnRegisterFeature(id, featureId);
            return this.Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IEnumerable<IncidentPutRp>), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put(int id, [FromBody] IncidentPutRp model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var result = await this._incidentComponent.Put(id, model);
            return this.Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<IndicatorGetListRp>), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody]IncidentPostRp model)
        {
            var (result, created) = await this._incidentComponent.Post(model);
            if (created)
            {
                return this.Created(Url.RouteUrl("GetIncidentId", new { id = result.Id }), result);                
            }
            else {
                return this.Ok(result);
            }
        }


    }
}

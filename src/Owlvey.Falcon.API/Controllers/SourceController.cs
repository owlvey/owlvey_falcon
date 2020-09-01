using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("sources")]
    public class SourceController : BaseController
    {        
        private readonly SourceComponent _sourceComponent;        

        public SourceController(SourceComponent sourceComponent) : base()
        {
            this._sourceComponent = sourceComponent;            
        }

        [HttpGet]
        [ProducesResponseType(typeof(SourcesGetRp), 200)]
        public async Task<IActionResult> Get(int? productId, int? indicatorId,
            DateTime? start, DateTime? end)
        {
            if (productId.HasValue && end.HasValue && start.HasValue)
            {
                var model = await this._sourceComponent.GetByProductIdWithAvailability(productId.Value, start.Value, end.Value);
                return this.Ok(model);
            }
            else if (productId.HasValue)
            {
                var model = await this._sourceComponent.GetByProductId(productId.Value);
                return this.Ok(model);
            }
            else if (indicatorId.HasValue)
            {
                var model = await this._sourceComponent.GetByIndicatorId(indicatorId.Value);
                return this.Ok(model);
            }
            else
            {
                return this.NotFound($"The Resource doesn't exists.");
            }            
        }
        

        [HttpGet("{id}", Name = "GetSourceById")]
        [ProducesResponseType(typeof(SourceLiteRp), 200)]
        public async Task<IActionResult> GetSourceById(int id)
        {
            var model = await this._sourceComponent.GetById(id);
            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");
            return this.Ok(model);
        }

        [HttpGet("{id}/detail", Name = "GetSourceDetailById")]
        [ProducesResponseType(typeof(SourceGetRp), 200)]
        public async Task<IActionResult> GetSourceDetailById(int id, DateTime? start, DateTime? end)
        {            
            var model = await this._sourceComponent.GetByIdWithDetail(id, 
                new DatePeriodValue(start.Value, end.Value));            

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }




        [HttpGet("{id}/anchor")]
        [ProducesResponseType(typeof(SourceAnchorRp), 200)]
        public async Task<IActionResult> GetSourceAnchor(int id)
        {            
            var model = await this._sourceComponent.GetAnchor(id);         
            return this.Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SourceGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody]SourcePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);
            var response = await this._sourceComponent.Create(resource);            
            return this.Created(Url.RouteUrl("GetSourceById", new { response.Id }), response);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SourceGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put(int id, [FromBody] SourcePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);
            var response = await this._sourceComponent.Update(id, resource);
            return this.Created(Url.RouteUrl("GetSourceById", new { response.Id }), response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int id)
        {            
            await this._sourceComponent.Delete(id);            
            return this.Ok();
        }        

    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{    
    [Route("V2/sourceItems")]
    public class SourceItemComtrollerV2 : BaseController
    {
        private readonly SourceItemComponent _sourceItemComponent;

        public SourceItemComtrollerV2(SourceItemComponent sourceItemComponent)
        {
            this._sourceItemComponent = sourceItemComponent;
        } 
        [HttpPost]
        [ProducesResponseType(typeof(SourceItemGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> PostV2([FromBody]SourceItemPostV2Rp model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var response = await this._sourceItemComponent.Create(model);
            return this.Created(Url.RouteUrl("GetBySourceItemId", new { response.Id }), response);
        }
    }
}

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
        [ProducesResponseType(typeof(IEnumerable<IndicatorGetListRp>), 200)]
        public async Task<IActionResult> Get(int productId, DateTime? start ,DateTime? end)
        {
            var model = await this._incidentComponent.Get(productId, new PeriodValue(start.Value, end.Value));
            return this.Ok(model);
        }

    }
}

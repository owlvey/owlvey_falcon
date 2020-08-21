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
    [Route("sourceItems/experience")]
    public class ExperienceSourceController : BaseController
    {
        private ExperienceSourceComponent _sourceComponent;
        public ExperienceSourceController(ExperienceSourceComponent sourceComponent) : base()
        {
            this._sourceComponent = sourceComponent;
        }
        

        [HttpGet("{id}/interaction/items")]
        [ProducesResponseType(typeof(IEnumerable<InteractiveSourceItemGetRp>), 200)]
        public async Task<IActionResult> GetBySourceId(int id, DateTime? start, DateTime? end)
        {
            IEnumerable<InteractiveSourceItemGetRp> model = new List<InteractiveSourceItemGetRp>();
            if (start.HasValue && end.HasValue)
            {
                model = await this._sourceComponent.GetInteractionItems(id, new DatePeriodValue(start, end));
            }
            return this.Ok(model);
        }

        [HttpGet("{id}/proportion/items")]
        [ProducesResponseType(typeof(IEnumerable<ProportionSourceItemGetRp>), 200)]
        public async Task<IActionResult> GetProportionsBySourceId(int id, DateTime? start, DateTime? end)
        {
            IEnumerable<ProportionSourceItemGetRp> model = new List<ProportionSourceItemGetRp>();
            if (start.HasValue && end.HasValue)
            {
                model = await this._sourceComponent.GetProportionItems(id, new DatePeriodValue(start, end));
            }
            return this.Ok(model);
        }

        [HttpPost("items")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(IEnumerable<SourceItemBaseRp>), 200)]
        public async Task<IActionResult> PostProportion([FromBody]SourceItemExperiencePostRp model)
        {
            var result = await this._sourceComponent.CreateItem(model);
            return this.Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SourceGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put(int id, [FromBody]SourcePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._sourceComponent.Update(id, resource);

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            return this.Ok();
        }
    }
}

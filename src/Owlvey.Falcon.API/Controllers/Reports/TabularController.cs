using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers.Reports
{
    [Route("reports/tabular")]
    public class TabularController : BaseController
    {
        private readonly SourceComponent _sourceComponent;
        private readonly IndicatorComponent _indicatorComponent; 

        public TabularController(SourceComponent sourceComponent,
            IndicatorComponent indicatorComponent) 
        {
            this._sourceComponent = sourceComponent;
            this._indicatorComponent = indicatorComponent;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TabularGetRp), 200)]
        public async Task<IActionResult> Get(int? sourceId, int? indicatorId, DateTime? start, DateTime? end, int period=1)
        {               
            if (!start.HasValue) {
                return this.BadRequest("start is required");
            }
            if (!end.HasValue)
            {
                return this.BadRequest("end is required");
            }
            TabularGetRp result = new TabularGetRp();            

            if (sourceId.HasValue && indicatorId.HasValue) {
                return this.BadRequest("multiple ids are not allowed");
            }
            else if (sourceId.HasValue){
                result = await this._sourceComponent.GetTabularBySourceId(sourceId.Value, start.Value, end.Value);
            }
            else if (indicatorId.HasValue) {
                result = await this._indicatorComponent.GetTabularBySourceId(indicatorId.Value, start.Value, end.Value);
            }

            return this.Ok(result);
        }

    }
}

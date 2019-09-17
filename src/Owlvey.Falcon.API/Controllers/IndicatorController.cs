using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("indicators")]
    public class IndicatorController : BaseController
    {
        private readonly IndicatorComponent _indicatorComponent;
        public IndicatorController(IndicatorComponent indicatorComponent) : base()
        {
            this._indicatorComponent = indicatorComponent;
        }
                

        [HttpGet("{id}", Name = "GetIndicatorById")]
        [ProducesResponseType(typeof(IndicatorGetRp), 200)]
        public async Task<IActionResult> GetIndicatorById(int id)
        {
            var model = await this._indicatorComponent.GetById(id);
            
            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(IndicatorGetRp), 200)]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await this._indicatorComponent.Delete(id);
            return this.Ok(model);
        }

        //TODO Refactor and delete
        [HttpPost]
        [ProducesResponseType(typeof(IndicatorGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]IndicatorPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._indicatorComponent.Create(resource);

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");
            var newResource = await this._indicatorComponent.GetById(id);

            return this.Created(Url.RouteUrl("GetIndicatorById", new { id }), newResource);
        }

        #region reports

        [HttpGet("{id}/reports/daily/series")]
        [ProducesResponseType(typeof(SeriesGetRp), 200)]
        public async Task<IActionResult> ReportSeries(int id, DateTime? start, DateTime? end, int period = 1)
        {
            if (!start.HasValue)
            {
                return this.BadRequest("start is required");
            }
            if (!end.HasValue)
            {
                return this.BadRequest("end is required");
            }
            var result = await this._indicatorComponent.GetDailySeriesById(id, start.Value, end.Value);
            return this.Ok(result);
        }
        #endregion

    }
}

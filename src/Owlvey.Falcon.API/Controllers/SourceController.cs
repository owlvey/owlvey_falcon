using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
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
        [ProducesResponseType(typeof(IEnumerable<SourceGetListRp>), 200)]
        public async Task<IActionResult> Get(int? productId, int? indicatorId, int? customerId)
        {
            IEnumerable<SourceGetListRp> model = new List<SourceGetListRp>();

            if (productId.HasValue)
            {
                model = await this._sourceComponent.GetByProductId(productId.Value);
            }
            else if (indicatorId.HasValue)
            {
                model = await this._sourceComponent.GetByIndicatorId(indicatorId.Value);
            }            
            else {
                return this.NotFound($"The Resource doesn't exists.");
            }
            
            return this.Ok(model);
        }

        [HttpGet("{id}", Name = "GetSourceById")]
        [ProducesResponseType(typeof(SquadGetRp), 200)]
        public async Task<IActionResult> GetSourceById(int id)
        {
            var model = await this._sourceComponent.GetById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SourceGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]        
        public async Task<IActionResult> Post([FromBody]SourcePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._sourceComponent.Create(resource);

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");
            var newResource = await this._sourceComponent.GetById(id);

            return this.Created(Url.RouteUrl("GetSourceById", new { id }), newResource);
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
            var result = await this._sourceComponent.GetDailySeriesById(id, start.Value, end.Value, period);
            return this.Ok(result);
        }
        #endregion
    }
}

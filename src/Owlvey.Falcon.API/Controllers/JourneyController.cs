using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core.Models.Series;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("journeys")]
    public class JourneyController : BaseController
    {
        private readonly JourneyQueryComponent _journeyQueryComponent;
        private readonly JourneyComponent _journeyComponent;        
        private readonly JourneyMapComponent _journeyMapComponent;

        public JourneyController(JourneyQueryComponent journeyQueryComponent,
                                 JourneyComponent journeyComponent,                                 
                                 JourneyMapComponent journeyMapComponent) 
        {
            this._journeyQueryComponent = journeyQueryComponent;
            this._journeyComponent = journeyComponent;            
            this._journeyMapComponent = journeyMapComponent;
        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(JourneyGetListRp), 200)]
        public async Task<IActionResult> Get(int productId, DateTime? start, DateTime? end, string group = null)
        {
            IEnumerable<JourneyGetListRp> model = new List<JourneyGetListRp>();
            if (start.HasValue && end.HasValue) {
                model = await this._journeyQueryComponent.GetJourneysWithAvailability(productId, start.Value, end.Value);
                if (!string.IsNullOrWhiteSpace(group))
                {
                    model = model.Where(c => c.Group == group).ToList();                    
                }
            }
            else {
                model = await this._journeyQueryComponent.GetListByProductId(productId);
            }            
            return this.Ok(model);
        }

        /// <summary>
        /// Get  by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetJourneyId")]
        [ProducesResponseType(typeof(JourneyGetRp), 200)]
        public async Task<IActionResult> GetById(int id, DateTime? start, DateTime? end)
        {
            JourneyGetRp model = null;
            if (start.HasValue && end.HasValue) {
                model = await this._journeyQueryComponent.GetJourneyByIdWithAvailabilities(id, start.Value, end.Value);
            }
            else {
                model = await this._journeyQueryComponent.GetJourneyById(id);
            }

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");
            
            return this.Ok(model);
        }

        /// <summary>
        /// Create a new 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /journeys
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(JourneyGetListRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody]JourneyPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);
            var response = await this._journeyComponent.Create(resource);
            return this.Created(Url.RouteUrl("GetJourneyId", new { id = response.Id }), response);
        }

        /// <summary>
        /// Update an journey
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put(int id, [FromBody]JourneyPutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._journeyComponent.Update(id, resource);

            if (response.HasNotFounds())
            {
                return this.NotFound(response.GetNotFounds());
            }

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            return this.Ok();
        }

        /// <summary>
        /// Delete an journey
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._journeyComponent.Delete(id);

            if (response.HasNotFounds())
            {
                return this.NotFound(response.GetNotFounds());
            }

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            return this.NoContent();
        }

        #region Features
        [HttpPut("{id}/features/{featureId}")]
        [ProducesResponseType(typeof(JourneyMapPostRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> PutFeature(int? id, int? featureId)
        {       
            if (id.HasValue && featureId.HasValue)
            {
                await this._journeyMapComponent.CreateMap(new JourneyMapPostRp()
                {
                    FeatureId = featureId,
                    JourneyId = id
                });
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            }            
        }

        [HttpDelete("{id}/features/{featureId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int? id, int? featureId)
        {
            if (id.HasValue && featureId.HasValue)
            {
                var result = await this._journeyMapComponent.DeleteMap(id.Value, featureId.Value);
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpGet("{id}/features/complement")]
        [ProducesResponseType(typeof(IEnumerable<FeatureGetListRp>), 200)]
        public async Task<IActionResult> GetIndicatorsComplement(int id)
        {
            var model = await this._journeyQueryComponent.GetFeaturesComplement(id);
            return this.Ok(model);
        }
        #endregion



        #region journeyGroups
        [HttpGet("journeyGroup")]
        [ProducesResponseType(typeof(JourneyGroupListRp), 200)]
        public async Task<IActionResult> GetGroups(int productId, DateTime? start, DateTime? end)
        {
            if (!start.HasValue || !end.HasValue)
            {
                return this.BadRequest("start is required");
            }
            var period = new DatePeriodValue(start.Value, end.Value);
            var model = await this._journeyQueryComponent.GetJourneyGroupReport(productId, period);
            return this.Ok(model);
        }




        #endregion

        #region reports        


        [HttpGet("reports/annual")]
        [ProducesResponseType(typeof(AnnualJourneyListRp), 200)]
        public async Task<IActionResult> ReportAnnual(int productId, DateTime? start)
        {
            if (!start.HasValue)
            {
                return this.BadRequest("start is required");
            }
            var model = await this._journeyQueryComponent.GetAnnualReport(productId, start.Value);
            return this.Ok(model);
        }
        

        [HttpGet("reports/journeyGroup/annual")]
        [ProducesResponseType(typeof(AnnualJourneyGroupListRp), 200)]
        public async Task<IActionResult> ReportAnnualJourneyGroup(int productId, DateTime? start)
        {
            if (!start.HasValue)
            {
                return this.BadRequest("start is required");
            }            
            var model = await this._journeyQueryComponent.GetAnnualGroupReport(productId, start.Value);
            return this.Ok(model);
        }

        [HttpGet("{id}/reports/journeyGroup/annual/calendar")]
        [ProducesResponseType(typeof(MultiSerieItemGetRp), 200)]
        public async Task<IActionResult> ReportAnnualGroupCalendar(int id, DateTime? start)
        {
            if (!start.HasValue)
            {
                return this.BadRequest("start is required");
            }
            var model = await this._journeyQueryComponent.GetAnnualJourneyGroupCalendarReport(id, start.Value);
            return this.Ok(model);
        }

        [HttpGet("reports/journeyGroup/annual/calendar")]
        [ProducesResponseType(typeof(IEnumerable<MultiSerieItemGetRp>), 200)]
        public async Task<IActionResult> ReportAnnualGroupCalendar(int productId, string group, DateTime? start)
        {
            if (!start.HasValue)
            {
                return this.BadRequest("start is required");
            }
            var model = await this._journeyQueryComponent.GetAnnualJourneyGroupCalendarReport(productId, group, start.Value);
            return this.Ok(model);
        }

        [HttpGet("{id}/reports/daily/series")]
        [ProducesResponseType(typeof(DatetimeSerieModel), 200)]
        public async Task<IActionResult> ReportSeries(int id, DateTime? start, DateTime? end)
        {
            if (!start.HasValue)
            {
                return this.BadRequest("start is required");
            }
            if (!end.HasValue)
            {
                return this.BadRequest("end is required");
            }
            var result = await this._journeyQueryComponent.GetDailySeriesById(id, new DatePeriodValue(start.Value, end.Value));

            return this.Ok(result);
        }

        [HttpGet("reports/debt/daily/series")]
        [ProducesResponseType(typeof(DatetimeSerieModel), 200)]
        public async Task<IActionResult> GetDailyGroups(int productId, string group, DateTime? start, DateTime? end)
        {
            if (!start.HasValue || !end.HasValue)
            {
                return this.BadRequest("start is required");
            }
            var period = new DatePeriodValue(start.Value, end.Value);
            var model = await this._journeyQueryComponent.GetJourneyGroupDailyErrorBudget(productId,  period, group);
            return this.Ok(model);
        }

        #endregion

        #region graph

        [HttpGet("{id}/reports/graph")]
        [ProducesResponseType(typeof(GraphGetRp), 200)]
        public async Task<IActionResult> GetGraph(int id, DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                GraphGetRp result = await this._journeyQueryComponent.GetGraph(id, new DatePeriodValue(start.Value, end.Value));
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }
        }


        #endregion


    }
}
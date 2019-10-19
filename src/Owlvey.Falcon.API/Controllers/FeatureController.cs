using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("features")]
    public class FeatureController : BaseController
    {
        private readonly FeatureQueryComponent _featureQueryService;
        private readonly IndicatorComponent _indicatorComponent;
        private readonly FeatureComponent _featureComponent;        
        private readonly SquadComponent _squadComponent; 

        public FeatureController(FeatureQueryComponent featureQueryService,
            IndicatorComponent indicatorComponent,            
            SquadComponent squadComponent,
            FeatureComponent featureService) : base()
        {
            this._indicatorComponent = indicatorComponent;            
            this._featureQueryService = featureQueryService;
            this._featureComponent = featureService;
            this._squadComponent = squadComponent;
        }

        /// <summary>
        /// Get Features
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FeatureGetListRp>), 200)]
        public async Task<IActionResult> Get(int productId, DateTime? start, DateTime? end)
        {
            object model = null;            
            if (start.HasValue && end.HasValue)
            {
                model = await this._featureQueryService.GetFeaturesWithAvailability(productId, start.Value, end.Value);
            }
            else
            {
                model = await this._featureQueryService.GetFeatures(productId);
            }            
            return this.Ok(model);
        }

        /// <summary>
        /// Get Feature by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetFeatureId")]
        [ProducesResponseType(typeof(FeatureGetRp), 200)]
        public async Task<IActionResult> GetFeatureId(int id, DateTime? start, DateTime? end)
        {
            object model = null;
            if (end.HasValue)
            {
                model = await this._featureQueryService.GetFeatureByIdWithAvailability(id, start.Value,  end.Value);
            }
            else {
                model = await this._featureQueryService.GetFeatureById(id);
            }

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");            

            return this.Ok(model);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<FeatureGetListRp>), 200)]
        public async Task<IActionResult> Search(int productId, string name = null)
        {
            IEnumerable<FeatureGetListRp> result = new List<FeatureGetListRp>();

            if (!string.IsNullOrWhiteSpace(name))
            {
                result = await this._featureQueryService.SearchFeatureByName(productId, name);
            }
            else {
                return BadRequest("name is required");
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Create a new Feature
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Features
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(FeatureGetListRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]FeaturePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._featureComponent.CreateFeature(resource);            

            return this.Created(Url.RouteUrl("GetFeatureId", new { id = response.Id }), response);
        }

        /// <summary>
        /// Update an Feature
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, [FromBody]FeaturePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._featureComponent.UpdateFeature(id, resource);

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
        /// Delete an Feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await this._featureComponent.DeleteFeature(id);           
            return this.Ok();
        }


        #region Indicators

        [HttpPut("{id}/indicators/{sourceId}")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> GetIndicators(int id, int sourceId)
        {
            await this._indicatorComponent.Create(id, sourceId);            
            return this.Ok();
        }

        [HttpGet("{id}/indicators")]
        [ProducesResponseType(typeof(IEnumerable<IndicatorGetListRp>), 200)]
        public async Task<IActionResult> GetIndicators(int id)
        {
            var result = await this._indicatorComponent.GetByFeature(id);
            return this.Ok(result);
        }

        [HttpGet("{id}/indicators/complement")]
        [ProducesResponseType(typeof(IEnumerable<SourceGetListRp>), 200)]
        public async Task<IActionResult> GetIndicatorsComplement(int id)
        {            
            var result = await this._indicatorComponent.GetSourcesComplement(id);
            return this.Ok(result);
        }
        [HttpDelete("{id}/indicators/{sourceId}")]
        public async Task<IActionResult> DeleteIndicator(int id, int sourceId)
        {
            await this._indicatorComponent.Delete(id, sourceId);
            return this.Ok();
        }
        #endregion


        #region Squads

        [HttpPut("{id}/squads/{squadId}")]
        [ProducesResponseType(typeof(IEnumerable<SourceGetListRp>), 200)]
        public async Task<IActionResult> PutSquad(int id, int squadId)
        {
            await this._featureComponent.RegisterSquad(new SquadFeaturePostRp()
            {
                 FeatureId = id, SquadId = squadId
            });            
            return this.Ok();
        }

        [HttpDelete("{id}/squads/{squadId}")]
        [ProducesResponseType(typeof(IEnumerable<SourceGetListRp>), 200)]
        public async Task<IActionResult> DeleteSquad(int id, int squadId)
        {
            await this._featureComponent.UnRegisterSquad(squadId, id);
            return this.Ok();
        }

        [HttpGet("{id}/squads/complement")]
        [ProducesResponseType(typeof(IEnumerable<SourceGetListRp>), 200)]
        public async Task<IActionResult> GetSquadsComplement(int id)
        {            
            var result  =await this._squadComponent.GetSquadComplementByFeature(id);
            return this.Ok(result);
        }

        #endregion

        #region

        [HttpGet("{id}/reports/daily/series")]
        [ProducesResponseType(typeof(MultiSeriesGetRp), 200)]
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
            var result = await this._featureQueryService.GetDailySeriesById(id, start.Value, end.Value);

            return this.Ok(result);
        }
        
        #endregion

    }
}
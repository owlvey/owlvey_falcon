using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("services")]
    public class ServiceController : BaseController
    {
        private readonly ServiceQueryComponent _serviceQueryService;
        private readonly ServiceComponent _serviceService;
        private readonly FeatureQueryComponent _featureQueryComponent; 

        public ServiceController(ServiceQueryComponent serviceQueryService,
                                 ServiceComponent serviceService,
                                 FeatureQueryComponent featureQueryComponent) 
        {
            this._serviceQueryService = serviceQueryService;
            this._serviceService = serviceService;
            this._featureQueryComponent = featureQueryComponent;
        }

        /// <summary>
        /// Get Services
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceGetListRp), 200)]
        public async Task<IActionResult> Get(int productId, DateTime? start, DateTime? end)
        {
            IEnumerable<ServiceGetListRp> model = new List<ServiceGetListRp>();
            if (start.HasValue && end.HasValue) {
                model = await this._serviceQueryService.GetServicesWithAvailability(productId, start.Value, end.Value);
            }
            else {
                model = await this._serviceQueryService.GetServices(productId);
            }            
            return this.Ok(model);
        }

        /// <summary>
        /// Get Service by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetServiceId")]
        [ProducesResponseType(typeof(ServiceGetRp), 200)]
        public async Task<IActionResult> GetById(int id, DateTime? start, DateTime? end)
        {
            ServiceGetRp model = null;
            if (end.HasValue)
            {
                model = await this._serviceQueryService.GetServiceByIdWithAvailabilities(id, start.Value, end.Value);
            }
            else {
                model = await this._serviceQueryService.GetServiceById(id);
            }

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            if (end.HasValue)
            {
                model.Features = await this._featureQueryComponent.GetFeaturesByServiceIdWithAvailability(id, start.Value, end.Value);
            }
            else {
                model.Features = await this._featureQueryComponent.GetFeaturesByServiceId(id);
            }
            
            return this.Ok(model);
        }

        /// <summary>
        /// Create a new Service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Services
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceGetListRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]ServicePostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);
            var response = await this._serviceService.CreateService(resource);
            return this.Created(Url.RouteUrl("GetServiceId", new { id = response.Id }), response);
        }

        /// <summary>
        /// Update an Service
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, [FromBody]ServicePutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._serviceService.UpdateService(id, resource);

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
        /// Delete an Service
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._serviceService.DeleteService(id);

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


        [HttpGet("{id}/features/complement")]
        [ProducesResponseType(typeof(IEnumerable<FeatureGetListRp>), 200)]
        public async Task<IActionResult> GetIndicatorsComplement(int id)
        {
            var model = await this._serviceQueryService.GetFeaturesComplement(id);
            return this.Ok(model);
        }


        #region reports
        [HttpGet("{id}/reports/daily/series")]
        [ProducesResponseType(typeof(SeriesGetRp), 200)]
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
            var result = await this._serviceQueryService.GetDailySeriesById(id, start.Value, end.Value);

            return this.Ok(result);
        }
        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("appsettings")]
    public class AppSettingController : BaseController
    {
        private readonly AppSettingQueryComponent _appSettingQueryService;
        private readonly AppSettingComponent _appSettingService;
        
        public AppSettingController(AppSettingQueryComponent appSettingQueryService,
                                    AppSettingComponent appSettingService) : base()
        {
            this._appSettingQueryService = appSettingQueryService;
            this._appSettingService = appSettingService;
        }

        /// <summary>
        /// Get appsettings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(AppSettingPostRp), 200)]
        public async Task<IActionResult> Get()
        {
            var model = await this._appSettingQueryService.GetSettings();
            return this.Ok(model);
        }

        /// <summary>
        /// Get appsetting by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AppSettingGetRp), 200)]
        public async Task<IActionResult> GetById(string id)
        {
            var model = await this._appSettingQueryService.GetAppSettingById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        /// <summary>
        /// Create a new appsetting
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /appsettings
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AppSettingPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._appSettingService.CreateAppSetting(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            return this.Ok();
        }

        /// <summary>
        /// Update an appsetting
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]AppSettingPutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._appSettingService.UpdateAppSetting(id, resource);

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
        /// Delete an appsetting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._appSettingService.DeleteAppSetting(id);

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


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("settings")]
    public class SettingController : BaseController
    {
        readonly IConfiguration _configuration;
        public SettingController(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get appsettings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var settings = new {
                authority = _configuration["Authentication__Authority"],
                api = _configuration["Settings__Api"],
            };

            return this.Ok(settings);
        }
    }
}

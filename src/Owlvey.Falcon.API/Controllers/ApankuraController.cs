using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apankura.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("apankura")]
    public class ApankuraController : ControllerBase
    {        

        private readonly ILogger<ApankuraController> _logger;

        public ApankuraController(ILogger<ApankuraController> logger)
        {
            _logger = logger;
        }

        [HttpGet("liveness")]        
        public Dictionary<string,string> Get()
        {
            var date = DateTime.Now;            
            var map = new Dictionary<string, string>();
            map.Add("date", date.ToString("dd-MM-yyyy HH:mm:ss"));            
            return map;            
        }
    }
}

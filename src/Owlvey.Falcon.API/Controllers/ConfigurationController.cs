using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("configuration")]
    [AllowAnonymous]
    public class ConfigurationController : Controller
    {
        [HttpPost("exception")]
        public async Task<IActionResult> PostException([FromQuery]string message)
        {
            throw new ApplicationException(message);
        }
    }
}

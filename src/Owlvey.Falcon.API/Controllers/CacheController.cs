using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;

namespace Owlvey.Falcon.API.Controllers
{

    [Route("cache")]
    public class CacheController : BaseController
    {

        private CacheComponent CacheComponent;

        public CacheController(CacheComponent cache) {
            this.CacheComponent = cache;
        }

        [HttpGet("last")]
        public async Task<IActionResult> GetLastModified()
        {
            var modified = await this.CacheComponent.GetLastModified();
            return this.Ok(new {
                modified
            });                        
        }
    }
}

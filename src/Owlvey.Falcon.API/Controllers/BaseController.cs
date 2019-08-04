using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Controllers
{
    public abstract class BaseController : Controller
    {
        
        /// <summary>
        /// Conflict Result Model
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Conflict(object value)
        {
            return this.StatusCode(StatusCodes.Status409Conflict, value);
        }

        /// <summary>
        /// Error Result Model
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error(object value)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, value);
        }

    }
}

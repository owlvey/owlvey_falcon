using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("accounts")]
    public class AccountController : BaseController
    {
        /// <summary>
        /// User Info
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var name = this.User.FindFirst(c => c.Type == "name").Value;
            var email = this.User.FindFirst(c => c.Type == "email").Value;
            return this.Ok(new { name = name, email = email });
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("users")]
    public class UserController : BaseController
    {
        private readonly UserQueryComponent _userQueryService;
        private readonly UserComponent _userService;
        
        public UserController(UserQueryComponent userQueryService, 
            UserComponent userService) : base()
        {
            this._userQueryService = userQueryService;
            this._userService = userService;
        }

        /// <summary>
        /// Get Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserPostRp), 200)]
        public async Task<IActionResult> Get([FromQuery(Name = "email")]string email)
        {
            var model = new List<UserGetListRp>();

            if (string.IsNullOrEmpty(email))
            {
                model = (await this._userQueryService.GetUsers()).ToList();
            }
            else {
                var user = await this._userQueryService.GetUserByEmail(email);

                if (user != null) {
                    model.Add(new UserGetListRp {
                        Email = user.Email,
                        CreatedBy = user.CreatedBy,
                        CreatedOn = user.CreatedOn,
                        Id = user.Id
                    });
                }
            }

            return this.Ok(model);
        }

        /// <summary>
        /// Get User by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUserId")]
        [ProducesResponseType(typeof(UserGetRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this._userQueryService.GetUserById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Users
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]UserPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._userService.CreateUser(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");
            var newResource = await this._userQueryService.GetUserById(id);

            return this.Created(Url.RouteUrl("GetUserId", new { id = id }), newResource);
        }

    }
}

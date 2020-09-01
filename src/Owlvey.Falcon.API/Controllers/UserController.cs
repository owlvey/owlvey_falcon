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
        private readonly UserQueryComponent _userQueryComponent;
        private readonly UserComponent _userComponent;
        
        public UserController(UserQueryComponent userQueryComponent, 
            UserComponent userComponent) : base()
        {
            this._userQueryComponent = userQueryComponent;
            this._userComponent = userComponent;
        }


        /// <summary>
        /// Get Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserGetListRp), 200)]
        public async Task<IActionResult> Get([FromQuery(Name = "email")]string email)
        {
            IEnumerable<UserGetListRp> model = new List<UserGetListRp>();

            if (string.IsNullOrEmpty(email))
            {
                model = await this._userQueryComponent.GetUsers();
            }
            else {
                var user = await this._userQueryComponent.GetUserByEmail(email);
                if (user != null) {
                    model = new List<UserGetListRp>() {
                    new UserGetListRp {
                        Email = user.Email,
                        CreatedBy = user.CreatedBy,
                        CreatedOn = user.CreatedOn,
                        Id = user.Id
                    }};
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
            var model = await this._userQueryComponent.GetUserById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserGetRp), 200)]
        public async Task<IActionResult> Put(int id, [FromBody]UserPutRp model)
        {
            var result = await this._userComponent.PutUser(id, model);
            return this.Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserGetRp), 200)]
        public async Task<IActionResult> Delete(int id)
        {
            await this._userComponent.DeleteUser(id);
            return this.Ok();
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

            var response = await this._userComponent.CreateUser(resource);
            return this.Created(Url.RouteUrl("GetUserId", new { id = response.Id }), response);
        }

    }
}

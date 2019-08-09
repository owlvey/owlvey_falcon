using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("products")]
    public class ProductController : BaseController
    {
        private readonly ProductQueryComponent _productQueryService;
        private readonly ProductComponent _productService;

        public ProductController(ProductQueryComponent productQueryService,
                                 ProductComponent productService) : base()
        {
            this._productQueryService = productQueryService;
            this._productService = productService;
        }

        /// <summary>
        /// Get Products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ProductPostRp), 200)]
        public async Task<IActionResult> Get(int customerId)
        {
            var model = await this._productQueryService.GetProducts(customerId);
            return this.Ok(model);
        }

        /// <summary>
        /// Get Product by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetProductId")]
        [ProducesResponseType(typeof(ProductGetRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this._productQueryService.GetProductById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            return this.Ok(model);
        }

        /// <summary>
        /// Create a new Product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Products
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]ProductPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._productService.CreateProduct(resource);

            if (response.HasConflicts())
            {
                return this.Conflict(response.GetConflicts());
            }

            var id = response.GetResult<int>("Id");
            var newResource = await this._productQueryService.GetProductById(id);

            return this.Created(Url.RouteUrl("GetProductId", new { id = id }), newResource);
        }

        /// <summary>
        /// Update an Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, [FromBody]ProductPutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._productService.UpdateProduct(id, resource);

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
        /// Delete an Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._productService.DeleteProduct(id);

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
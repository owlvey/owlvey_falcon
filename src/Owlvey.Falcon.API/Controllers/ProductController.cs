using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    public partial class CustomerController
    {
        
        /// <summary>
        /// Get Products
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/products")]
        [ProducesResponseType(typeof(ProductPostRp), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var model = await this._productQueryService.GetProducts(id);
            return this.Ok(model);
        }

        /// <summary>
        /// Get Product by id 
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        [HttpGet("{id}/products/{productId}", Name = "GetProductId")]
        [ProducesResponseType(typeof(ProductGetRp), 200)]
        public async Task<IActionResult> GetProductById(int id, int productId)
        {
            var model = await this._productQueryService.GetProductById(productId);

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
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost("{id}/products")]
        [ProducesResponseType(typeof(ProductGetRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post(int id, [FromBody]ProductPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._productService.CreateProduct(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            var productId = response.GetResult<int>("Id");
            var newResource = await this._productQueryService.GetProductById(id);

            return this.Created(Url.RouteUrl("GetProductId", new { id = id, productId = productId }), newResource);
        }

        /// <summary>
        /// Update an Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}/products/{productId}")]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, int productId, [FromBody]ProductPutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._productService.UpdateProduct(productId, resource);

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
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        [HttpDelete("{id}/products/{productId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id, int productId)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._productService.DeleteProduct(productId);

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

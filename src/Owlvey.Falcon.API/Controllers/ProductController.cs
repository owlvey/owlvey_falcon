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
        private readonly ServiceQueryComponent _serviceQueryComponent;
        

        public ProductController(ProductQueryComponent productQueryService,
                                 ProductComponent productService,
                                 ServiceQueryComponent serviceQueryComponent)
        {            
            this._productQueryService = productQueryService;
            this._productService = productService;
            this._serviceQueryComponent = serviceQueryComponent;
        }

        /// <summary>
        /// Get Products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductGetListRp>), 200)]
        public async Task<IActionResult> Get(int customerId, string expand)
        {
            IEnumerable<ProductGetListRp> model = new List<ProductGetListRp>();            
            model = await this._productQueryService.GetProductsWithServices(customerId);            
            return this.Ok(model);
        }

        /// <summary>
        /// Get Product by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetProductId")]
        [ProducesResponseType(typeof(ProductGetRp), 200)]
        public async Task<IActionResult> GetProductId(int id)
        {
            var model = await this._productQueryService.GetProductById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            model.Services = await this._serviceQueryComponent.GetServices(id);

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
            return this.Created(Url.RouteUrl("GetProductId", new { response.Id }), response);
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
            await this._productService.DeleteProduct(id);
            return this.Ok();
        }

        [HttpGet("{id}/dashboard")]
        [ProducesResponseType(typeof(DashboardProductRp), 200)]
        public async Task<IActionResult> GetDashboard(int id, DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                var result = await this._productQueryService.GetProductDashboard(id, start.Value, end.Value);
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }
        }

        #region graph

        [HttpGet("{id}/reports/graph")]
        [ProducesResponseType(typeof(GraphGetRp), 200)]
        public async Task<IActionResult> GetGraph(int id, DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                GraphGetRp result = await this._productQueryService.GetGraph(id, start.Value, end.Value);
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }
        }


        #endregion




        #region Anchors

        [HttpGet("{id}/sync")]
        [ProducesResponseType(typeof(AnchorRp), 200)]
        public async Task<IActionResult> GetAnchors(int id)
        {
            var entities = await this._productQueryService.GetAnchors(id);
            return this.Ok(entities);
        }

        [HttpGet("{id}/sync/{name}")]
        [ProducesResponseType(typeof(AnchorRp), 200)]
        public async Task<IActionResult> GetAnchor(int id, string name)
        {
            var result = await this._productQueryService.GetAnchor(id, name);
            return this.Ok(result);
        }

        [HttpPut("{id}/sync/{name}")]
        [ProducesResponseType(typeof(DateTime), 200)]
        public async Task<IActionResult> SetAnchor(int id, string name, [FromBody]AnchorPutRp model)
        {
            await this._productService.PutAnchor(id, name, model);
            return this.Ok();
        }


        [HttpPost("{id}/sync/{name}")]
        [ProducesResponseType(typeof(AnchorRp), 200)]
        public async Task<IActionResult> PostAnchor(int id, string name)
        {
            var result = await this._productService.PostAnchor(id, name);
            return this.Ok(result);
        }

        [HttpDelete("{id}/sync/{name}")]        
        public async Task<IActionResult> Delete(int id, string name)
        {
            await this._productService.DeleteAnchor(id, name);
            return this.Ok();
        }

        #endregion



        #region reports

        [HttpGet("{id}/reports/daily/services/series")]
        [ProducesResponseType(typeof(SeriesGetRp), 200)]
        public async Task<IActionResult> ReportSeries(int id, DateTime? start, DateTime? end, int period = 1)
        {
            if (!start.HasValue)
            {
                return this.BadRequest("start is required");
            }
            if (!end.HasValue)
            {
                return this.BadRequest("end is required");
            }
            var result = await this._productQueryService.GetDailyServiceSeriesById(id, start.Value, end.Value);

            return this.Ok(result);
        }


        [HttpGet("{id}/reports/daily/features/series")]
        [ProducesResponseType(typeof(SeriesGetRp), 200)]
        public async Task<IActionResult> ReportFeaturesSeries(int id,
            DateTime? start, DateTime? end)
        {
            if (!start.HasValue)
            {
                return this.BadRequest("start is required");
            }
            if (!end.HasValue)
            {
                return this.BadRequest("end is required");
            }
            var result = await this._productQueryService.GetDailyFeaturesSeriesById(id, start.Value, end.Value);

            return this.Ok(result);
        }
        #endregion

    }
}
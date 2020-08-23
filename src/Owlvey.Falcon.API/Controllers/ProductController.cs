using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("products")]
    public class ProductController : BaseController
    {
        private readonly ProductQueryComponent _productQueryComponent;
        private readonly ProductComponent _productComponent;
        private readonly JourneyQueryComponent _journeyQueryComponent;
        public ProductController(ProductQueryComponent productQueryComponent,
                                 ProductComponent productComponent,
                                 JourneyQueryComponent journeyQueryComponent)
        {
            this._productQueryComponent = productQueryComponent;
            this._productComponent = productComponent;
            this._journeyQueryComponent = journeyQueryComponent;
        }

        /// <summary>
        /// Get Products
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(ProductGetListRp), 200)]
        public async Task<IActionResult> Get(int customerId, DateTime? start, DateTime? end)
        {            

            var model = await this._productQueryComponent.GetProductsWithInformation(customerId, new DatePeriodValue(start, end));
            return this.Ok(model);
        }

        [HttpGet("lite")]
        [ProducesResponseType(typeof(IEnumerable<ProductBaseRp>), 200)]
        public async Task<IActionResult> GetLite(int customerId)
        {
            var model = await this._productQueryComponent.GetProducts(customerId);
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
            var model = await this._productQueryComponent.GetProductById(id);

            if (model == null)
                return this.NotFound($"The Resource {id} doesn't exists.");

            model.Journeys = await this._journeyQueryComponent.GetListByProductId(id);

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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody]ProductPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);
            var response = await this._productComponent.CreateProduct(resource);
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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put(int id, [FromBody]ProductPutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._productComponent.UpdateProduct(id, resource);

            return this.Ok();
        }

        /// <summary>
        /// Delete an Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int id)
        {
            await this._productComponent.DeleteProduct(id);
            return this.Ok();
        }

        [HttpGet("{id}/dashboard")]
        [ProducesResponseType(typeof(OperationProductDashboardRp), 200)]
        public async Task<IActionResult> GetDashboard(int id, DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                var result = await this._productQueryComponent.GetProductDashboard(id, start.Value, end.Value);
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpGet("{id}/dashboard/journeys/groups")]
        [ProducesResponseType(typeof(ProductDashboardRp), 200)]
        public async Task<IActionResult> GetDashboardGroups(int id, DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                var result = await this._productQueryComponent.GetJourneyGroupDashboard(id, start.Value, end.Value);
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }
        }



        #region graph

        [HttpGet("{id}/reports/graph/availability")]
        [ProducesResponseType(typeof(GraphGetRp), 200)]
        public async Task<IActionResult> GetGraphAvailability(int id, DateTime? start, DateTime? end)
        {
            DatePeriodValue period = new DatePeriodValue(start.Value, end.Value);
            if (period.IsValid())
            {
                GraphGetRp result = await this._productQueryComponent.GetGraphAvailability(id, period);
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpGet("{id}/reports/graph/latency")]
        [ProducesResponseType(typeof(GraphGetRp), 200)]
        public async Task<IActionResult> GetGraphLatency(int id, DateTime? start, DateTime? end)
        {
            DatePeriodValue period = new DatePeriodValue(start.Value, end.Value);
            if (period.IsValid())
            {
                GraphGetRp result = await this._productQueryComponent.GetGraphLatency(id, period);
                return this.Ok(result);
            }
            else
            {
                return this.BadRequest();
            }
        }
        [HttpGet("{id}/reports/graph/experience")]
        [ProducesResponseType(typeof(GraphGetRp), 200)]
        public async Task<IActionResult> GetGraphExperience(int id, DateTime? start, DateTime? end)
        {
            DatePeriodValue period = new DatePeriodValue(start.Value, end.Value);
            if (period.IsValid())
            {
                GraphGetRp result = await this._productQueryComponent.GetGraphExperience(id, period);
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
            var entities = await this._productQueryComponent.GetAnchors(id);
            return this.Ok(entities);
        }

        [HttpGet("{id}/sync/{name}")]
        [ProducesResponseType(typeof(AnchorRp), 200)]
        public async Task<IActionResult> GetAnchor(int id, string name)
        {
            var result = await this._productQueryComponent.GetAnchor(id, name);
            return this.Ok(result);
        }

        [HttpPut("{id}/sync/{name}")]
        [ProducesResponseType(typeof(DateTime), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SetAnchor(int id, string name, [FromBody]AnchorPutRp model)
        {
            await this._productComponent.PutAnchor(id, name, model);
            return this.Ok();
        }


        [HttpPost("{id}/sync/{name}")]
        [ProducesResponseType(typeof(AnchorRp), 200)]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> PostAnchor(int id, string name)
        {
            var result = await this._productComponent.PostAnchor(id, name);
            return this.Ok(result);
        }

        [HttpDelete("{id}/sync/{name}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int id, string name)
        {
            await this._productComponent.DeleteAnchor(id, name);
            return this.Ok();
        }

        #endregion


        #region exports

        [HttpGet("{id}/exports/items")]
        public async Task<IActionResult> Exportitems(int id, DateTime? start, DateTime? end)
        {
            var stream = await this._productQueryComponent.ExportItems(id, new DatePeriodValue(start, end));

            string excelName = $"owlvey-items-{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("{id}/exports/availability/interactions")]
        public async Task<IActionResult> ExportAnnualAvailabilityInteractions(int id) {
            var stream = await this._productQueryComponent.ExportAnnualAvailabilityInteractions(id);

            string excelName = $"owlvey-annual-availability-interactions-{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpPost("{id}/imports/items")]
        [Authorize(Policy = "RequireAdminRole")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostBackupAsync([FromRoute]int id, [FromForm(Name = "data")] IFormFile file)
        {
            using (MemoryStream excelStream = new MemoryStream())
            {
                file.CopyTo(excelStream);
                var logs = await this._productComponent.ImportsItems(id, excelStream);
                return Ok(logs);
            }
        }
        #endregion


        #region reports



        [HttpGet("{id}/reports/excel")]
        [ProducesResponseType(typeof(SeriesGetRp), 200)]
        public async Task<IActionResult> ExportExcel(int id, DateTime? start, DateTime? end)
        {
            if (!start.HasValue || !end.HasValue)
            {
                return this.BadRequest("start is required");
            }

            var result = await this._productQueryComponent.GetProductExportToExcel(id, start.Value, end.Value);
            string excelName = $"{result.Item1.Customer.Name}-{result.Item1.Name}-excel-{start.Value.ToString("yyyyMMdd")}-{end.Value.ToString("yyyyMMdd")}.xlsx";
            return File(result.Item2, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);                       
        }

               


        [HttpGet("{id}/reports/daily/journeys/series")]
        [ProducesResponseType(typeof(SeriesGetRp), 200)]
        public async Task<IActionResult> ReportSeries(int id, DateTime? start, DateTime? end, string group = null)
        {
            if (!start.HasValue)
            {
                return this.BadRequest("start is required");
            }
            if (!end.HasValue)
            {
                return this.BadRequest("end is required");
            }
            MultiSeriesGetRp result;
            var period = new DatePeriodValue(start.Value, end.Value);  
            if (string.IsNullOrWhiteSpace(group))
            {
                result = await this._productQueryComponent.GetDailyJourneysSeriesById(id, period);
            }
            else {
                result = await this._productQueryComponent.GetDailyJourneysSeriesByIdAndGroup(id, period, group);
            }            

            return this.Ok(result);
        }


        #endregion

    }
}
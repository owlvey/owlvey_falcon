using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components.Interfaces;
using Owlvey.Falcon.Components.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("journals")]
    public class JournalController : BaseController
    {
        private readonly IJournalQueryComponent _journalQueryService;
        private readonly IJournalComponent _journalService;
        
        public JournalController(IJournalQueryComponent journalQueryService,
                                    IJournalComponent journalService) : base()
        {
            this._journalQueryService = journalQueryService;
            this._journalService = journalService;
        }

        /// <summary>
        /// Get Journals
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(JournalPostRp), 200)]
        public async Task<IActionResult> Get()
        {
            var model = await this._journalQueryService.GetJournals();
            return this.Ok(model);
        }

        /// <summary>
        /// Get Journal by id 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(JournalGetRp), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this._journalQueryService.GetJournalById(id);

            if (model == null)
                return this.NotFound($"The Key {id} doesn't exists.");

            return this.Ok(model);
        }

        /// <summary>
        /// Create a new Journal
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Journals
        ///     {
        ///        "id": "key1",
        ///        "value": "Value1"
        ///     }
        ///
        /// </remarks>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]JournalPostRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._journalService.CreateJournal(resource);

            if (response.HasConflicts()) {
                return this.Conflict(response.GetConflicts());
            }

            return this.Ok();
        }

        /// <summary>
        /// Update an Journal
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]JournalPutRp resource)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._journalService.UpdateJournal(id, resource);

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
        /// Delete an Journal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var response = await this._journalService.DeleteJournal(id);

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

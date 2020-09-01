using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("risks/reliability")]
    public class ReliabilityRiskController : BaseController
    {
        readonly ReliabilityRiskComponent _ReliabilityRiskComponent;
        public ReliabilityRiskController(ReliabilityRiskComponent component)
        {
            _ReliabilityRiskComponent = component;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<ReliabilityRiskRp>), 200)]
        public async Task<IActionResult> GetRisks(int? sourceId)
        {
            var response = await this._ReliabilityRiskComponent.GetRisks(sourceId);
            return this.Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReliabilityRiskGetRp), 200)]
        public async Task<IActionResult> GetRisk(int id)
        {
            var response = await this._ReliabilityRiskComponent.GetRiskById(id);
            return this.Ok(response);
        }
        [HttpPost("threats/default")]
        [ProducesResponseType(typeof(SecurityThreatGetRp), 200)]
        public async Task<IActionResult> PostThreatDefault()
        {
            await this._ReliabilityRiskComponent.CreateDefault();
            return this.Ok();
        }

        [HttpPost()]
        [ProducesResponseType(typeof(ReliabilityRiskGetRp), 200)]
        public async Task<IActionResult> PostRisk([FromBody] ReliabilityRiskPostRp model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var response = await this._ReliabilityRiskComponent.Create(model);
            return this.Ok(response);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReliabilityRiskGetRp), 200)]
        public async Task<IActionResult> PutRisk(int id,
            [FromBody] ReliabilityRiskPutRp model)
        {
            var response = await this._ReliabilityRiskComponent.UpdateRisk(id, model);
            return this.Ok(response);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> DeleteRisk(int id)
        {
            await this._ReliabilityRiskComponent.DeleteRisk(id);
            return this.Ok();
        }

        #region threats 
        [HttpGet("threats")]
        [ProducesResponseType(typeof(IEnumerable<ReliabilityThreatGetRp>), 200)]
        public async Task<IActionResult> GetThreats()
        {
            var response = await this._ReliabilityRiskComponent.GetThreats();
            return this.Ok(response);
        }
        [HttpGet("threats/{id}")]
        [ProducesResponseType(typeof(ReliabilityThreatGetRp), 200)]
        public async Task<IActionResult> GetThreat(int id)
        {
            var response = await this._ReliabilityRiskComponent.GetThreat(id);
            return this.Ok(response);
        }
        [HttpPost("threats")]
        [ProducesResponseType(typeof(ReliabilityThreatGetRp), 200)]
        public async Task<IActionResult> PostThreat([FromBody] ReliabilityThreatPostRp model)
        {
            var response = await this._ReliabilityRiskComponent.CreateThreat(model);
            return this.Ok(response);
        }
        [HttpPut("threats/{id}")]
        [ProducesResponseType(typeof(ReliabilityThreatGetRp), 200)]
        public async Task<IActionResult> PutThreat(int id, 
            [FromBody] ReliabilityThreatPutRp model)
        {
            var response = await this._ReliabilityRiskComponent.UpdateThreat(id,
                model);
            return this.Ok(response);
        }
        [HttpDelete("threats/{id}")]
        [ProducesResponseType(typeof(JourneyGetListRp), 200)]
        public async Task<IActionResult> DeleteThreat(int id)
        {
            await this._ReliabilityRiskComponent.DeleteThreat(id);
            return this.Ok();
        }
        #endregion

    }
}

using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("risks/security")]
    public class SecurityRiskController : BaseController
    {
        readonly SecurityRiskComponent _SecurityRiskComponent;
        public SecurityRiskController(SecurityRiskComponent component)
        {
            _SecurityRiskComponent = component;
        }
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<SecurityRiskRp>), 200)]
        public async Task<IActionResult> GetRisks(int? sourceId)
        {
            var response = await this._SecurityRiskComponent.GetRisks(sourceId);
            return this.Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SecurityRiskRp), 200)]
        public async Task<IActionResult> GetRisk(int id)
        {
            var response = await this._SecurityRiskComponent.GetRiskById(id);
            return this.Ok(response);            
        }
        [HttpPost()]
        [ProducesResponseType(typeof(SecurityRiskRp), 200)]
        public async Task<IActionResult> PostRisk([FromBody]SecurityRiskPost model)
        {
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }
            var response = await this._SecurityRiskComponent.Create(model);
            return this.Ok(response);            
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SecurityRiskRp), 200)]
        public async Task<IActionResult> PutRisk(int id, 
            [FromBody]SecurityRiskPut model)
        {
            var response = await this._SecurityRiskComponent.UpdateRisk(id, model);
            return this.Ok(response);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> DeleteRisk(int id)
        {
            await this._SecurityRiskComponent.DeleteRisk(id);
            return this.Ok();
        }

        #region threats 
        [HttpGet("threats")]
        [ProducesResponseType(typeof(IEnumerable<SecurityThreatGetRp>), 200)]
        public async Task<IActionResult> GetThreats()
        {
            var response = await this._SecurityRiskComponent.GetThreats();
            return this.Ok(response);
        }
        [HttpGet("threats/{id}")]
        [ProducesResponseType(typeof(SecurityThreatGetRp), 200)]
        public async Task<IActionResult> GetThreat(int id)
        {
            var response = await this._SecurityRiskComponent.GetThreat(id);
            return this.Ok(response);            
        }
        [HttpPost("threats/default")]
        [ProducesResponseType(typeof(SecurityThreatGetRp), 200)]
        public async Task<IActionResult> PostThreatDefault()
        {
            await this._SecurityRiskComponent.CreateDefault();
            return this.Ok();
        }

        [HttpPost("threats")]
        [ProducesResponseType(typeof(SecurityThreatGetRp), 200)]
        public async Task<IActionResult> PostThreat([FromBody] SecurityThreatPostRp model)
        {
            var response = await this._SecurityRiskComponent.CreateThreat(model);
            return this.Ok(response);
        }
        [HttpPut("threats/{id}")]
        [ProducesResponseType(typeof(SecurityThreatGetRp), 200)]
        public async Task<IActionResult> PutThreat(int id, [FromBody] SecurityThreatPutRp model)
        {
            var response = await  this._SecurityRiskComponent.UpdateThreat(id, model);
            return this.Ok(response);
        }        
        [HttpDelete("threats/{id}")]
        [ProducesResponseType(typeof(JourneyGetListRp), 200)]
        public async Task<IActionResult> PostThreat(int id)
        {
            await this._SecurityRiskComponent.DeleteThreat(id);
            return this.Ok();
        }
        #endregion



    }
}

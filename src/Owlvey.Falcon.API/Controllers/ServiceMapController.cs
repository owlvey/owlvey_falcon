using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("servicesMaps")]
    public class ServiceMapController: BaseController
    {        
        private readonly ServiceMapComponent _serviceMapComponent;

        public ServiceMapController(ServiceMapComponent serviceMapComponent) 
        {
            this._serviceMapComponent = serviceMapComponent;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceMapPostRp), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]ServiceMapPostRp model)
        {
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }
            var result = await this._serviceMapComponent.CreateServiceMap(new ServiceMapPostRp() {
                 FeatureId = model.FeatureId,
                 ServiceId = model.ServiceId
            });
            return this.Ok(result);
        }
    }
}

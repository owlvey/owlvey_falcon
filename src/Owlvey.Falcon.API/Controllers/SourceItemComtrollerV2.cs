using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.API.Controllers
{    
    [Route("V2/sourceItems")]
    public class SourceItemComtrollerV2 : BaseController
    {
        private readonly SourceItemComponent _sourceItemComponent;

        public SourceItemComtrollerV2(SourceItemComponent sourceItemComponent)
        {
            this._sourceItemComponent = sourceItemComponent;
        }         
    }
}

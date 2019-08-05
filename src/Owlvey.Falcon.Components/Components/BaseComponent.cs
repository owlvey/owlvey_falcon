using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Owlvey.Falcon.Gateways;

namespace Owlvey.Falcon.Components
{
    public abstract class BaseComponent
    {
        protected readonly IDateTimeGateway _datetimeGateway;
        protected readonly IMapper _mapper;

        protected BaseComponent(IDateTimeGateway dataTimeGateway, IMapper mapper) {
            this._mapper = mapper;
            this._datetimeGateway = dataTimeGateway;
        }
    }
}

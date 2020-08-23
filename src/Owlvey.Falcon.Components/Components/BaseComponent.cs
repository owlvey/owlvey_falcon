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
        protected readonly IUserIdentityGateway _identityGateway;
        protected readonly ConfigurationComponent _configuration; 

        protected BaseComponent(IDateTimeGateway dataTimeGateway, IMapper mapper,
            IUserIdentityGateway identityGateway, ConfigurationComponent configuration) {
            this._mapper = mapper;
            this._datetimeGateway = dataTimeGateway;
            this._identityGateway = identityGateway;
            this._configuration = configuration;
        }
    }
}

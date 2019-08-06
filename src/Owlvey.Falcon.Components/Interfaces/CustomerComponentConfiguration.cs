using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public static class CustomerComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg ) {
            cfg.CreateMap<CustomerEntity, Models.CustomerGetRp>();
            cfg.CreateMap<CustomerEntity, Models.CustomerGetListRp>();
        }
    }
}

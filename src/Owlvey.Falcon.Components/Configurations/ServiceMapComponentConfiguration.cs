using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Interfaces
{
    public class ServiceMapComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ServiceMapEntity, Models.ServiceGetListRp>();
            cfg.CreateMap<ServiceMapEntity, Models.ServiceMapGetRp>();
        }
    }
}

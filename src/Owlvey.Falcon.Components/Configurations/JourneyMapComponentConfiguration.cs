using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Interfaces
{
    public class JourneyMapComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<JourneyMapEntity, Models.JourneyGetListRp>();
            cfg.CreateMap<JourneyMapEntity, Models.JourneyMapGetRp>();
        }
    }
}

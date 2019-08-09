using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public class SquadFeatureComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SquadFeatureEntity, Models.SquadFeatureGetListRp>();
            cfg.CreateMap<SquadFeatureEntity, Models.SquadFeatureGetRp>();
        }
    }
}

using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.Components
{
    public class SquadFeatureComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SquadFeatureEntity, SquadFeatureGetListRp>();
            cfg.CreateMap<SquadFeatureEntity, SquadFeatureGetRp>();
        }
    }
}

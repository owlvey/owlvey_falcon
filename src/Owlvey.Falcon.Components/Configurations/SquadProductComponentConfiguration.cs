using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public class SquadProductComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SquadProductEntity, Models.SquadProductGetListRp>().ForMember(c => c.Name,
                opt => opt.MapFrom(d => d.Squad.Name )); 
            cfg.CreateMap<SquadProductEntity, Models.SquadProductGetRp>();
        }
    }
}

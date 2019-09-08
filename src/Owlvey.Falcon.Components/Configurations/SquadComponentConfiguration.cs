using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Owlvey.Falcon.Components
{
    public class SquadComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SquadEntity, Models.SquadGetListRp>();
            cfg.CreateMap<SquadEntity, Models.SquadGetRp>()
                .ForMember(d=>d.Members, m=> m.MapFrom(c=>c.Members.Select(d=>d.User)))
                .ForMember(d => d.Features, m => m.MapFrom(c => c.Features.Select(d => d.Feature)));
        }
    }
}

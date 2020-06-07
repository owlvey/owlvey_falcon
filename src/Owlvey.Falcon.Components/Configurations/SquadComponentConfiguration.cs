using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.Components
{
    public class SquadComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SquadEntity, SquadGetListRp>()
                .ForMember(d => d.Features, m => m.Ignore())
                .ForMember(d => d.Members, m => m.MapFrom(c=>c.Members.Count()))
                .ForMember(d => d.Debt, m => m.Ignore());
            cfg.CreateMap<SquadEntity, SquadGetRp>()
                .ForMember(d=>d.Members, m=> m.MapFrom(c=>c.Members.Select(d=>d.User)))
                .ForMember(d => d.Features, m => m.MapFrom(c => c.FeatureMaps.Select(d => d.Feature)));

            cfg.CreateMap<SquadEntity, SquadGetDetailRp>()
                .ForMember(d => d.Members, m => m.MapFrom(c => c.Members.Select(d => d.User)))
                .ForMember(d => d.Points, m => m.Ignore())
                .ForMember(d => d.Features, m => m.Ignore());

            cfg.CreateMap<SquadEntity, SquadMigrationRp>();

            cfg.CreateMap<SquadEntity, SquadQualityGetRp>()
                .ForMember(d => d.Features, m => m.Ignore())
                .ForMember(d => d.Members, m => m.Ignore())
                .ForMember(d => d.Debt, m => m.Ignore()); 

            

        }
    }
}

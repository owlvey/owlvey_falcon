using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Components
{
    public static class SourceItemComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SourceItemEntity, Models.SourceItemGetListRp>();
            cfg.CreateMap<SourceItemEntity, Models.SourceItemGetRp>()
                .ForMember(m => m.Clues, ope => ope.MapFrom( c=> c.ExportClues()));
            cfg.CreateMap<DayPointValue, Models.SeriesItemGetRp>()
                .ForMember(m => m.OMax, ope => ope.MapFrom(c => c.Maximun))
                .ForMember(m => m.OMin, ope => ope.MapFrom(c => c.Minimun))
                .ForMember(m => m.OAve, ope => ope.MapFrom(c => c.Average));
        }
    }
}

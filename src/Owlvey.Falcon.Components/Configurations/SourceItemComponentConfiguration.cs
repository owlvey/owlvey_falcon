using System;
using AutoMapper;
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.Components
{
    public static class SourceItemComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SourceItemEntity, SourceItemGetListRp>();
            cfg.CreateMap<ProportionSourceItemGetRp, ProportionSourceItemGetRp>();

            cfg.CreateMap<DayPointValue, SeriesItemGetRp>()
                .ForMember(m => m.OMax, ope => ope.MapFrom(c => c.Maximun))
                .ForMember(m => m.OMin, ope => ope.MapFrom(c => c.Minimun))
                .ForMember(m => m.OAve, ope => ope.MapFrom(c => c.Average));
        }
    }
}

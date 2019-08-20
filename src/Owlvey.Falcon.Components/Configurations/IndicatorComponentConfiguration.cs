using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public class IndicatorComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<IndicatorEntity, Models.IndicatorGetRp>()
                .ForMember(m => m.Source, opt => opt.MapFrom(src => src.Source.Name))
                .ForMember(m => m.SourceAvatar, opt => opt.MapFrom(src => src.Source.Avatar))
                .ForMember(m=> m.Feature, opt => opt.MapFrom(src=> src.Feature.Name))
                .ForMember(m => m.FeatureAvatar, opt => opt.MapFrom(src => src.Feature.Avatar));


            cfg.CreateMap<IndicatorEntity, Models.IndicatorGetListRp>()
                .ForMember(c => c.Availability, opt => opt.Ignore())
                .ForMember(c => c.Source, opt => opt.MapFrom(d=>d.Source.Name));                
        }
    }
}

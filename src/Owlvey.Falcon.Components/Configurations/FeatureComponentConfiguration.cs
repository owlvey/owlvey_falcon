using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Owlvey.Falcon.Components
{
    public class FeatureComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<FeatureEntity, Models.FeatureGetListRp>()
                .ForMember(c => c.IndicatorsCount,opt => opt.MapFrom(d => d.Indicators.Count))
                .ForMember(c => c.Availability, opt => opt.Ignore())
                .ForMember(c => c.Quality, opt => opt.Ignore())
                .ForMember(c => c.Latency, opt => opt.Ignore())
                .ForMember(c => c.MapId, opt => opt.MapFrom(d => d.ServiceMapId))
                .ForMember(c => c.ServiceCount, opt => opt.Ignore())                
                .ForMember(c => c.Product, opt => opt.MapFrom(c=> c.Product == null ? "": c.Product.Name));

            cfg.CreateMap<FeatureEntity, Models.SequenceFeatureGetListRp>()
                .ForMember(c => c.IndicatorsCount, opt => opt.MapFrom(d => d.Indicators.Count))
                .ForMember(c => c.Availability, opt => opt.Ignore())
                .ForMember(c => c.Quality, opt => opt.Ignore())
                .ForMember(c => c.Latency, opt => opt.Ignore())
                .ForMember(c => c.MapId, opt => opt.MapFrom(d=>d.ServiceMapId))
                .ForMember(c => c.Sequence, opt => opt.Ignore())                
                .ForMember(c => c.ServiceCount, opt => opt.Ignore())                                
                .ForMember(c => c.Product, opt => opt.MapFrom(c => c.Product == null ? "" : c.Product.Name));


            cfg.CreateMap<FeatureEntity, Models.FeatureAvailabilityGetListRp>()
                .ForMember(c => c.IndicatorsCount, opt => opt.MapFrom(d => d.Indicators.Count))
                .ForMember(c => c.Availability, opt => opt.Ignore())
                .ForMember(c => c.Quality, opt => opt.Ignore())
                .ForMember(c => c.Latency, opt => opt.Ignore())
                .ForMember(c => c.Total, opt => opt.Ignore())
                .ForMember(c => c.Good, opt => opt.Ignore())
                .ForMember(c => c.Squads, opt => opt.Ignore())
                .ForMember(c => c.ServiceCount, opt => opt.Ignore())                                
                .ForMember(c => c.Product, opt => opt.MapFrom(c => c.Product == null ? "" : c.Product.Name));

            cfg.CreateMap<FeatureEntity, Models.FeatureGetRp>()                
                .ForMember(c => c.MTTM, opt => opt.Ignore() )
                .ForMember(c => c.MTTD, opt => opt.Ignore())
                .ForMember(c => c.MTTE, opt => opt.Ignore())                
                .ForMember(c => c.MTTF, opt => opt.Ignore())
                .ForMember(c => c.Services, opt => opt.Ignore())
                .ForMember(c => c.Incidents, opt => opt.Ignore())
                .ForMember(c => c.Indicators, opt => opt.Ignore())
                .ForMember(c => c.Squads, opt => opt.MapFrom(c=>c.Squads.Select(d=>d.Squad)));

            cfg.CreateMap<FeatureEntity, Models.FeatureQualityGetRp>()
                .ForMember(c => c.Availability, opt => opt.Ignore())
                .ForMember(c => c.Quality, opt => opt.Ignore())
                .ForMember(c => c.Latency, opt => opt.Ignore())
                .ForMember(c => c.MTTM, opt => opt.Ignore())
                .ForMember(c => c.MTTD, opt => opt.Ignore())
                .ForMember(c => c.MTTE, opt => opt.Ignore())
                .ForMember(c => c.MTTF, opt => opt.Ignore())
                .ForMember(c => c.Services, opt => opt.Ignore())
                .ForMember(c => c.Incidents, opt => opt.Ignore())
                .ForMember(c => c.Indicators, opt => opt.Ignore())
                .ForMember(c => c.Squads, opt => opt.MapFrom(c => c.Squads.Select(d => d.Squad)));


            

            cfg.CreateMap<FeatureEntity, Models.FeatureLiteRp>();

            cfg.CreateMap<FeatureEntity, Models.FeatureMigrateRp>();


        }
    }
}

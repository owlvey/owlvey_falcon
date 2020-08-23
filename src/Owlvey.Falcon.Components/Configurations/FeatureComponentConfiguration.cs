using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.Components
{
    public class FeatureComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<FeatureEntity, FeatureGetListRp>()
                .ForMember(c => c.IndicatorsCount,opt => opt.MapFrom(d => d.Indicators.Count))
                .ForMember(c => c.Availability, opt => opt.Ignore())                
                .ForMember(c => c.Latency, opt => opt.Ignore())
                .ForMember(c => c.Experience, opt => opt.Ignore())
                .ForMember(c => c.Debt, opt => opt.Ignore())
                .ForMember(c => c.MapId, opt => opt.MapFrom(d => d.JourneyMapId))
                .ForMember(c => c.JourneyCount, opt => opt.Ignore())                
                .ForMember(c => c.Product, opt => opt.MapFrom(c=> c.Product == null ? "": c.Product.Name));

            cfg.CreateMap<FeatureEntity, SequenceFeatureGetListRp>()
                .ForMember(c => c.IndicatorsCount, opt => opt.MapFrom(d => d.Indicators.Count))
                .ForMember(c => c.Availability, opt => opt.Ignore())                
                .ForMember(c => c.Experience, opt => opt.Ignore())
                .ForMember(c => c.Debt, opt => opt.Ignore())
                .ForMember(c => c.Latency, opt => opt.Ignore())
                .ForMember(c => c.MapId, opt => opt.MapFrom(d=>d.JourneyMapId))
                .ForMember(c => c.Sequence, opt => opt.Ignore())                
                .ForMember(c => c.JourneyCount, opt => opt.Ignore())                                
                .ForMember(c => c.Product, opt => opt.MapFrom(c => c.Product == null ? "" : c.Product.Name));


            cfg.CreateMap<FeatureEntity, FeatureAvailabilityGetListRp>()
                .ForMember(c => c.IndicatorsCount, opt => opt.MapFrom(d => d.Indicators.Count))
                .ForMember(c => c.Quality, opt => opt.Ignore())                                
                .ForMember(c => c.Squads, opt => opt.Ignore())
                .ForMember(c => c.Debt, opt => opt.Ignore())
                .ForMember(c => c.JourneyCount, opt => opt.Ignore())                                
                .ForMember(c => c.Product, opt => opt.MapFrom(c => c.Product == null ? "" : c.Product.Name));

            cfg.CreateMap<FeatureEntity, FeatureGetRp>()                
                .ForMember(c => c.MTTM, opt => opt.Ignore() )
                .ForMember(c => c.MTTD, opt => opt.Ignore())
                .ForMember(c => c.MTTE, opt => opt.Ignore())                
                .ForMember(c => c.MTTF, opt => opt.Ignore())
                .ForMember(c => c.Journeys, opt => opt.Ignore())
                .ForMember(c => c.Incidents, opt => opt.Ignore())
                .ForMember(c => c.Indicators, opt => opt.Ignore())
                .ForMember(c => c.Squads, opt => opt.MapFrom(c=>c.Squads.Select(d=>d.Squad)));

            cfg.CreateMap<FeatureEntity, FeatureQualityGetRp>()
                .ForMember(c => c.Availability, opt => opt.Ignore())                
                .ForMember(c => c.Latency, opt => opt.Ignore())
                .ForMember(c => c.Experience, opt => opt.Ignore())
                .ForMember(c => c.Debt, opt => opt.Ignore())                
                .ForMember(c => c.Journeys, opt => opt.Ignore())
                .ForMember(c => c.Incidents, opt => opt.Ignore())
                .ForMember(c => c.Indicators, opt => opt.Ignore())
                .ForMember(c => c.Squads, opt => opt.MapFrom(c => c.Squads.Select(d => d.Squad)));


            

            cfg.CreateMap<FeatureEntity, FeatureLiteRp>();

            cfg.CreateMap<FeatureEntity, FeatureMigrateRp>();


        }
    }
}

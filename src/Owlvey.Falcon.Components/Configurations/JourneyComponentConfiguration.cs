using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class JourneyComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<JourneyEntity, JourneyGetListRp>()
                .ForMember(c=>c.FeaturesCount,opt=> opt.MapFrom(d=>d.FeatureMap.Count))                
                .ForMember(m => m.Deploy, ope => ope.Ignore())                
                .ForMember(m => m.Availability, ope => ope.Ignore())
                .ForMember(m => m.Experience, ope => ope.Ignore())
                .ForMember(m => m.Latency, ope => ope.Ignore())
                .ForMember(m => m.SLAValue, ope =>  ope.MapFrom(c=>c.GetSLA()) )
                .ForMember(m => m.AvailabilityErrorBudget, ope => ope.Ignore())
                .ForMember(m => m.AvailabilityDebt, ope => ope.Ignore())
                .ForMember(m => m.LatencyErrorBudget, ope => ope.Ignore())
                .ForMember(m => m.LatencyDebt, ope => ope.Ignore())
                .ForMember(m => m.ExperienceDebt, ope => ope.Ignore())
                .ForMember(m => m.ExperienceErrorBudget, ope => ope.Ignore());


            cfg.CreateMap<JourneyEntity, JourneyGetRp>()
                .ForMember(m => m.Features, ope=> ope.Ignore())                
                .ForMember(m => m.PreviousAvailability, ope => ope.Ignore())
                .ForMember(m => m.PreviousLatency, ope => ope.Ignore())
                .ForMember(m => m.PreviousExperience, ope => ope.Ignore())                

                .ForMember(m => m.SLAValue, ope => ope.MapFrom(d=> d.GetSLA()))                

                .ForMember(m => m.BeforeAvailability, ope => ope.Ignore())
                .ForMember(m => m.BeforeLatency, ope => ope.Ignore())
                .ForMember(m => m.BeforeExperience, ope => ope.Ignore())

                .ForMember(m => m.AvailabilityErrorBudget, ope => ope.Ignore())
                .ForMember(m => m.LatencyErrorBudget, ope => ope.Ignore())
                .ForMember(m => m.ExperienceErrorBudget, ope => ope.Ignore())

                .ForMember(m => m.Experience, ope => ope.Ignore())
                .ForMember(m => m.Availability, ope => ope.Ignore())
                .ForMember(m => m.Latency, ope => ope.Ignore());

            cfg.CreateMap<JourneyEntity, JourneyMigrateRp>()                
                .ForMember(m => m.AvailabilitySLO, ope => ope.Ignore())
                .ForMember(m => m.ExperienceSLO, ope => ope.Ignore())
                .ForMember(m => m.LatencySLO, ope => ope.Ignore())
                .ForMember(m => m.AvailabilitySLA, ope =>  ope.MapFrom(c=>c.AvailabilitySla)) 
                .ForMember(m => m.LatencySLA, ope =>  ope.MapFrom(c=>c.LatencySla));
        }
    }
}

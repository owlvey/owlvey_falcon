using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class ServiceComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ServiceEntity, ServiceGetListRp>()
                .ForMember(c=>c.FeaturesCount,opt=> opt.MapFrom(d=>d.FeatureMap.Count))                
                .ForMember(m => m.Deploy, ope => ope.Ignore())
                .ForMember(m => m.MTTD, ope => ope.Ignore())
                .ForMember(m => m.MTTE, ope => ope.Ignore())
                .ForMember(m => m.MTTF, ope => ope.Ignore())                                
                .ForMember(m => m.MTTM, ope => ope.Ignore())                
                .ForMember(m => m.Availability, ope => ope.Ignore())
                .ForMember(m => m.Experience, ope => ope.Ignore())
                .ForMember(m => m.Latency, ope => ope.Ignore())
                .ForMember(m => m.AvailabilityErrorBudget, ope => ope.Ignore())
                .ForMember(m => m.AvailabilityDebt, ope => ope.Ignore())
                .ForMember(m => m.LatencyErrorBudget, ope => ope.Ignore())
                .ForMember(m => m.LatencyDebt, ope => ope.Ignore())
                .ForMember(m => m.ExperienceDebt, ope => ope.Ignore())
                .ForMember(m => m.ExperienceErrorBudget, ope => ope.Ignore());


            cfg.CreateMap<ServiceEntity, ServiceGetRp>()
                .ForMember(m => m.Features, ope=> ope.Ignore())
                .ForMember(m => m.MTTD, ope => ope.Ignore())
                .ForMember(m => m.MTTE, ope => ope.Ignore())
                .ForMember(m => m.MTTF, ope => ope.Ignore())
                .ForMember(m => m.MTTM, ope => ope.Ignore())                
                .ForMember(m => m.PreviousAvailability, ope => ope.Ignore())
                .ForMember(m => m.PreviousLatency, ope => ope.Ignore())
                .ForMember(m => m.PreviousExperience, ope => ope.Ignore())

                .ForMember(m => m.BeforeAvailability, ope => ope.Ignore())
                .ForMember(m => m.BeforeLatency, ope => ope.Ignore())
                .ForMember(m => m.BeforeExperience, ope => ope.Ignore())

                .ForMember(m => m.AvailabilityErrorBudget, ope => ope.Ignore())
                .ForMember(m => m.LatencyErrorBudget, ope => ope.Ignore())
                .ForMember(m => m.ExperienceErrorBudget, ope => ope.Ignore())

                .ForMember(m => m.Experience, ope => ope.Ignore())
                .ForMember(m => m.Availability, ope => ope.Ignore())
                .ForMember(m => m.Latency, ope => ope.Ignore());

            cfg.CreateMap<ServiceEntity, ServiceMigrateRp>()
                .ForMember(m => m.AvailabilitySLO, ope => ope.Ignore())
                .ForMember(m => m.ExperienceSLO, ope => ope.Ignore())
                .ForMember(m => m.LatencySLO, ope => ope.Ignore());
        }
    }
}

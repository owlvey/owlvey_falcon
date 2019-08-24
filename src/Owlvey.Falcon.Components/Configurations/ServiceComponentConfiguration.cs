﻿using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class ServiceComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ServiceEntity, Models.ServiceGetListRp>()
                .ForMember(c=>c.FeaturesCount,opt=> opt.MapFrom(d=>d.FeatureMap.Count))
                .ForMember(m => m.Availability, ope => ope.Ignore())
                .ForMember(m => m.Deploy, ope => ope.Ignore())
                .ForMember(m => m.Risk, ope => ope.Ignore());


            cfg.CreateMap<ServiceEntity, Models.ServiceGetRp>()
                .ForMember(m=>m.Features, ope=> ope.Ignore())
                .ForMember(m => m.Availability, ope => ope.Ignore());
        }
    }
}

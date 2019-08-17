﻿using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class FeatureComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<FeatureEntity, Models.FeatureGetListRp>().ForMember(c => c.IndicatorsCount,
                opt => opt.MapFrom(d => d.Indicators.Count)); ;
            cfg.CreateMap<FeatureEntity, Models.FeatureGetRp>();
            cfg.CreateMap<FeatureEntity, Models.FeatureLiteRp>();            
        }
    }
}

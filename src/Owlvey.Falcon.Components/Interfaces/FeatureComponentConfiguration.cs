using AutoMapper;
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
            cfg.CreateMap<FeatureEntity, Models.FeatureGetListRp>();
            cfg.CreateMap<FeatureEntity, Models.FeatureGetRp>();
        }
    }
}

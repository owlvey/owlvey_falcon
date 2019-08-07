using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public class IndicatorComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<IndicatorEntity, Models.IndicatorGetRp>();
            cfg.CreateMap<IndicatorEntity, Models.IndicatorGetListRp>();                
        }
    }
}

using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public static class SourceItemComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SourceItemEntity, Models.SourceItemGetListRp>();
            cfg.CreateMap<SourceItemEntity, Models.SourceItemGetRp>();
            cfg.CreateMap<DayAvailabilityEntity, Models.SeriesItemGetRp>();
        }
    }
}

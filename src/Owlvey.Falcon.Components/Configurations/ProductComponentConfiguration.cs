using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.Components
{
    public class ProductComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ProductEntity, ProductGetListRp>()
                .ForMember(c => c.ServicesCount, opt => opt.MapFrom(d => d.Services.Count))
                .ForMember(c => c.FeaturesCount, opt => opt.MapFrom(d => d.Features.Count))
                .ForMember(c => c.SourcesCount, opt => opt.MapFrom(d => d.Sources.Count))
                .ForMember(c => c.Coverage, opt => opt.MapFrom(d => d.MeasureCoverage().assigned))
                .ForMember(c => c.Ownership, opt => opt.Ignore())
                .ForMember(c => c.Utilization, opt => opt.MapFrom(d => d.MeasureUtilization().assigned)); 
            cfg.CreateMap<ProductEntity, ProductGetRp>();
            cfg.CreateMap<ProductEntity, ProductMigrationRp>();

            cfg.CreateMap<AnchorEntity, AnchorRp>();
        }
    }
}


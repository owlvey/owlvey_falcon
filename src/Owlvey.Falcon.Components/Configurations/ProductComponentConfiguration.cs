using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public class ProductComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ProductEntity, Models.ProductGetListRp>().ForMember(c => c.ServicesCount,
                opt => opt.MapFrom(d => d.Services.Count)); 
            cfg.CreateMap<ProductEntity, Models.ProductGetRp>();
            cfg.CreateMap<ProductEntity, Models.ProductMigrationRp>();

            cfg.CreateMap<AnchorEntity, Models.AnchorRp>();
        }
    }
}


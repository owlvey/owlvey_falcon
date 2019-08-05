using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
namespace Owlvey.Falcon.Interfaces
{
    public class ProductComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ProductEntity, Models.ProductGetListRp>();
            cfg.CreateMap<ProductEntity, Models.ProductGetRp>();
        }
    }
}


using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public static class CustomerComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg ) {
            cfg.CreateMap<CustomerEntity, Models.CustomerGetRp>().ForMember(c => c.Availability, opt => opt.Ignore()); 
            cfg.CreateMap<CustomerEntity, Models.CustomerGetListRp>()
                .ForMember(c => c.ProductsCount, opt => opt.MapFrom(d => d.Products.Count))
                .ForMember(c=>c.Availability, opt=>opt.Ignore()); 
        }
    }
}

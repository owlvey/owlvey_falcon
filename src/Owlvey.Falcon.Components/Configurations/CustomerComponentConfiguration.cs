using System;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.Components
{
    public static class CustomerComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg ) {
            cfg.CreateMap<CustomerEntity, CustomerGetRp>().ForMember(c => c.Availability, opt => opt.Ignore()); 
            cfg.CreateMap<CustomerEntity, CustomerGetListRp>()
                .ForMember(c => c.ProductsCount, opt => opt.MapFrom(d => d.Products.Count))
                .ForMember(c=>c.Availability, opt=>opt.Ignore()); 
        }
    }
}

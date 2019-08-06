using AutoMapper;
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
            cfg.CreateMap<ServiceEntity, Models.ServiceGetListRp>();
            cfg.CreateMap<ServiceEntity, Models.ServiceGetRp>();
        }
    }
}

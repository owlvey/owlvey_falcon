
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class SourceComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SourceEntity, Models.SourceGetListRp>();
            cfg.CreateMap<SourceEntity, Models.SourceGetRp>();
        }
    }
}


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
            cfg.CreateMap<SourceEntity, Models.SourceGetListRp>().ForMember(c => c.Availability, opt => opt.Ignore());
            cfg.CreateMap<SourceEntity, Models.SourceGetRp>().ForMember(c => c.Availability, opt => opt.Ignore());  
            cfg.CreateMap<SourceEntity, Models.SourceLitRp>();
            cfg.CreateMap<SourceEntity, Models.SourceMigrateRp>();
        }
    }
}

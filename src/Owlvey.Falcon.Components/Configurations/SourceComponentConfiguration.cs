
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class SourceComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SourceEntity, SourceGetListRp>()
                .ForMember(c => c.Debt, opt => opt.Ignore())
                .ForMember(c => c.Measure, opt => opt.Ignore())
                .ForMember(c => c.Correlation, opt => opt.Ignore())                
                .ForMember(c => c.References, opt => opt.Ignore());
            cfg.CreateMap<SourceEntity, SourceGetRp>()
                .ForMember(c => c.Debt, opt => opt.Ignore())
                .ForMember(c => c.Clues, opt => opt.Ignore())
                .ForMember(c => c.Features, opt => opt.Ignore())
                .ForMember(c => c.Daily, opt => opt.Ignore())
                .ForMember(c => c.Quality, opt => opt.Ignore());  
            cfg.CreateMap<SourceEntity, SourceLiteRp>();
            cfg.CreateMap<SourceEntity, SourceMigrateRp>();
        }
    }
}

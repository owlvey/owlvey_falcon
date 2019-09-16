using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Owlvey.Falcon.Components
{    
    public class IncidentComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<IncidentEntity, Models.IncidentGetListRp>();
                
            cfg.CreateMap<IncidentEntity, Models.IncidentDetailtRp>()
                .ForMember(c => c.Features, opt => opt.MapFrom(c=>c.FeatureMaps.Select(d=>d.Feature).ToList()));
        }
    }
}

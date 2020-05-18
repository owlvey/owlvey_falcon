using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.Components
{    
    public class IncidentComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<IncidentEntity, IncidentGetListRp>()
                    .ForMember(c => c.FeaturesCount, opt => opt.MapFrom(c => c.FeatureMaps.Count()));

            cfg.CreateMap<IncidentEntity, IncidentDetailtRp>()
                .ForMember(c => c.Features, opt => opt.MapFrom(c=>c.FeatureMaps.Select(d=>d.Feature).ToList()));
        }
    }
}

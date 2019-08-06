using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Interfaces
{
    public class SquadComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SquadEntity, Models.SquadGetListRp>();
            cfg.CreateMap<SquadEntity, Models.SquadGetRp>();
        }
    }
}

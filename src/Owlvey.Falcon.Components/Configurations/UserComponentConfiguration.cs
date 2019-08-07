using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class UserComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserEntity, Models.UserGetListRp>();
            cfg.CreateMap<UserEntity, Models.UserGetRp>();
        }
    }
}

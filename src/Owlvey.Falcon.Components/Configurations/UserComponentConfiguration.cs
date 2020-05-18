using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class UserComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserEntity, UserGetListRp>();
            cfg.CreateMap<UserEntity, UserGetRp>()
                .ForMember(m => m.Products, ope => ope.Ignore())
                .ForMember(m => m.Services, ope => ope.Ignore())
                .ForMember(m => m.Features, ope => ope.Ignore());
        }
    }
}

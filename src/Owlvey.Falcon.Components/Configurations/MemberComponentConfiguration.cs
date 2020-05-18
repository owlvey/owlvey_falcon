using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class MemberComponentConfiguration
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<MemberEntity, MemberGetRp>()
                .ForMember(d=> d.Email , m=> m.MapFrom(src=> src.User.Email));
            cfg.CreateMap<MemberEntity, MemberGetListRp>()
                .ForMember(d => d.Email, m => m.MapFrom(src => src.User.Email));


            


        }
    }
}

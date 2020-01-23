﻿using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class ServiceModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public decimal SLO { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Leaders { get; set; }

        public void Load(string organization, string product, ServiceEntity entity)
        {            
            this.Name = entity.Name;
            this.Description = entity.Description;
            this.Avatar = entity.Avatar;
            this.Description = entity.Description;
            this.Group = entity.Group;
            this.SLO = entity.Slo;
            this.Leaders = entity.Leaders;
            this.Organization = organization;
            this.Product = product;
        }
        public static IEnumerable<UserModel> Load(IEnumerable<UserEntity> entities)
        {
            var result = new List<UserModel>();
            foreach (var item in entities)
            {
                var model = new UserModel();
                model.Load(item);
                result.Add(model);
            }
            return result;
        }

    }
}

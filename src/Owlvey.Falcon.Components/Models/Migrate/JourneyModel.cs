﻿using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class JourneyModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public decimal AvailabilitySlo { get; set; }
        public decimal LatencySlo { get; set; }
        public decimal ExperienceSlo { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Leaders { get; set; }
        public decimal AvailabilitySLA { get; set; }
        public decimal LatencySLA { get; set; }

        

        public void Load(string organization, string product, JourneyEntity entity)
        {            
            this.Name = entity.Name;
            this.Description = entity.Description;
            this.Avatar = entity.Avatar;
            this.Description = entity.Description;
            this.Group = entity.Group;
            this.AvailabilitySlo = entity.AvailabilitySlo;
            this.ExperienceSlo = entity.ExperienceSlo;
            this.LatencySlo = entity.LatencySlo;
            this.AvailabilitySLA = entity.AvailabilitySla;
            this.LatencySLA = entity.LatencySla;            
            this.Leaders = entity.Leaders;
            this.Organization = organization;
            this.Product = product;            
        }
        public static IEnumerable<JourneyModel> Load(string organization, string product, IEnumerable<JourneyEntity> entities)
        {
            var result = new List<JourneyModel>();
            foreach (var item in entities)
            {
                var model = new JourneyModel();
                model.Load(organization, product, item);
                result.Add(model);
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SquadModel
    {

        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Leaders { get; set; }
        public string Organization { get; set; }
        

        public void Load(string organization, SquadEntity entity)
        {
            this.Name = entity.Name;
            this.Avatar = entity.Avatar;
            this.Leaders = entity.Leaders;
            this.Description = entity.Description;
            this.Organization = organization;
            
        }
        public static IEnumerable<SquadModel> Load(OrganizationModel organization, IEnumerable<SquadEntity> entities)
        {
            var result = new List<SquadModel>();
            foreach (var item in entities)
            {
                var model = new SquadModel();                
                model.Load(organization.Name, item);
                result.Add(model);
            }
            return result;
        }
    }
}

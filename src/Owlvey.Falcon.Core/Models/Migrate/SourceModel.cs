using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SourceModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
        public string Kind { get; set; }
        

        public void Load(string organization, string product, SourceEntity entity)
        {
            this.Organization = organization;
            this.Product = product;
            this.Name = entity.Name;
            this.Avatar = entity.Avatar;
            this.Description = entity.Description;
            this.Group = entity.Group.ToString();
            this.GoodDefinition = entity.GoodDefinition;
            this.TotalDefinition = entity.TotalDefinition;
            this.Kind = entity.Kind.ToString();
        }
        public static IEnumerable<SourceModel> Load(string organization, string product, IEnumerable<SourceEntity> entities)
        {
            var result = new List<SourceModel>();
            foreach (var item in entities)
            {
                var model = new SourceModel();
                model.Load(organization, product, item);
                result.Add(model);
            }
            return result;
        }
    }
}

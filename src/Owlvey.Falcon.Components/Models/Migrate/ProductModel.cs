using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class ProductModel
    {
        public string Organization { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Leaders { get; set; }
        public string Avatar { get; set; }

        public void Load(string organization, ProductEntity entity)
        {
            this.Organization = organization;
            this.Name = entity.Name;
            this.Description = entity.Description;
            this.Leaders = entity.Leaders;
            this.Avatar = entity.Avatar;
        }
        public static IEnumerable<ProductModel> Load(string organization, IEnumerable<ProductEntity> entities)
        {
            var result = new List<ProductModel>();
            foreach (var item in entities)
            {
                var model = new ProductModel();
                model.Load(organization, item);
                result.Add(model);
            }
            return result;
        }
    }
}

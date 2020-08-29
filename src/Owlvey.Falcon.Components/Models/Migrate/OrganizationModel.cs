using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class OrganizationModel
    {

        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Leaders { get; set; }


        public void Load(CustomerEntity entity) {
            this.Name = entity.Name;
            this.Avatar = entity.Avatar;
            this.Leaders = entity.Leaders;
        }
        public static IEnumerable<OrganizationModel> Load(IEnumerable<CustomerEntity> entities) {
            var result = new List<OrganizationModel>();
            foreach (var item in entities)
            {
                var model = new OrganizationModel();
                model.Load(item);
                result.Add(model);
            }
            return result;
        }
    }
}

using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SourceLiteModel
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
        public string Kind { get; set; }

        public decimal Percentile { get; set; }

        public void Load(SourceEntity entity)
        {            
            this.Name = entity.Name;
            this.Avatar = entity.Avatar;
            this.Description = entity.Description;
            this.Group = entity.Group.ToString();
            this.GoodDefinition = entity.GoodDefinition;
            this.TotalDefinition = entity.TotalDefinition;
            this.Kind = entity.Kind.ToString();
            this.Percentile = entity.Percentile;
        }
        public static IEnumerable<SourceLiteModel> Load(IEnumerable<SourceEntity> entities)
        {
            var result = new List<SourceLiteModel>();
            foreach (var item in entities)
            {
                var model = new SourceLiteModel();
                model.Load(item);
                result.Add(model);
            }
            return result;
        }
    }
}

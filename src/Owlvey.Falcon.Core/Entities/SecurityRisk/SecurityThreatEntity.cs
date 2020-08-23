using System;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SecurityThreatEntity: BaseEntity
    {        
        public string Name { get; set;}
        public string Avatar { get; set; }
        public string Description {get; set;}
        public string Reference { get; set;}        
        public string Tags {get ; set;}

        public void Update(DateTime on, string ModifiedBy, string name, string description, string tags, string reference) {
            this.Name =  string.IsNullOrWhiteSpace( name) ? this.Name: name;
            this.Description = string.IsNullOrWhiteSpace(description) ? this.Description : description;
            this.Tags = string.IsNullOrWhiteSpace(tags) ? this.Tags : tags;
            this.Reference = string.IsNullOrWhiteSpace(reference) ? this.Reference: reference;
            this.ModifiedBy = ModifiedBy;
            this.ModifiedOn = on;
        }
    }
}

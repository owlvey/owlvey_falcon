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

        public void Update(DateTime on, string ModifiedBy, string name) {
            this.Name = name;
            this.ModifiedBy = ModifiedBy;
            this.ModifiedOn = on;
        }
    }
}

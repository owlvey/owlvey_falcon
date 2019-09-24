using System;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Core.Entities
{
    public class AnchorEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Target { get; set; }

        public virtual ProductEntity Product { get; set; }

        public int ProductId { get; set; }

        public void Update(DateTime target, DateTime on, string user)
        {
            this.Target = target;
            this.ModifiedOn = on;
            this.ModifiedBy = user;
            this.Validate();
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Models
{
    public class EntityBase
    {
        public EntityBase()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.Status = EntityStatus.Active;
        }

        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public EntityStatus Status { get; set; }

        public string Version { get; set; }
    }
    
}

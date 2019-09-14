using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class IncidentEntity : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }        
        public string Tags { get; set; }        
        [Required]
        public decimal MTTD { get; set; }
        [Required]
        public decimal MTTE { get; set; }
        [Required]
        public decimal MTTF { get; set; }

        [Required]
        public decimal MTTM { get; set; }

        
        public virtual ProductEntity Product { get; set; }

        public int ProductId { get; set; }


    }
}

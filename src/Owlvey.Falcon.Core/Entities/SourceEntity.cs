using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity: BaseEntity
    {
        [Required]
        public string Key { get; set; }

        public string Tags { get; set; }

        [Required]
        public string GoodDefinition { get; set; }
        [Required]
        public string TotalDefinition { get; set; }
        public string Avatar { get; set; }               


        public virtual ICollection<IndicatorEntity> Indicators { get; set; }
        public virtual ICollection<SourceItemEntity> SourceItems { get; set; }
    }
}

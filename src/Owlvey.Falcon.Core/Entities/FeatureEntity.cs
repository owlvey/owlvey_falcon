using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class FeatureEntity: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Avatar { get; set; }

        public virtual ProductEntity Product { get; set; }

        public virtual ICollection<IndicatorEntity> Indicators { get; set; } = new List<IndicatorEntity>();        
    }
}

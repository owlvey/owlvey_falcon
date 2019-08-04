using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class JournalEntity: BaseEntity
    {
        [Required]
        public string GoodDefinition { get; set; }
        [Required]
        public string TotalDefinition { get; set; }
<<<<<<< HEAD
        public string Avatar { get; set; }                
=======
        public string Avatar { get; set; }
        
        //public virtual ICollection<FeatureEntity> Features { get; set; }
>>>>>>> 4c019572161af3f9d8d9106d964c228d54e41492
        public virtual ICollection<JournalItemEntity> JournalItems { get; set; }
    }
}

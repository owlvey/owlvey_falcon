using System;
using System.Collections.Generic;

namespace Owlvey.Falcon.Core.Entities
{
    public class JournalEntity: BaseEntity
    {
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<FeatureEntity> Features { get; set; }
        public virtual ICollection<JournalItemEntity> JournalItems { get; set; }
    }
}

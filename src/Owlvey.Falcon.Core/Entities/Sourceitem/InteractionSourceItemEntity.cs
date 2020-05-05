using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class InteractionSourceItemEntity : SourceItemEntity
    {
        public InteractionSourceItemEntity() {
            this.Kind = SourceKindEnum.Interaction;
        }
    }
}

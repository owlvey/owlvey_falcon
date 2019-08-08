using System;
namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadFeatureEntity : BaseEntity
    {
        public virtual FeatureEntity Feature { get; set; }
        public virtual SquadEntity Squad { get; set; }


    }
}

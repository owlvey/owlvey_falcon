using System;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Models
{
    public class SquadFeatureBaseRp
    {
        public int Id { get; set; }
    }

    public class SquadFeatureGetRp : SquadFeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SquadFeatureGetListRp : SquadFeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SquadFeaturePostRp
    {
        [Required]
        public int SquadId { get; set; }
        [Required]
        public int FeatureId { get; set; }
    }

    public class SquadFeaturePutRp
    {
    }
}

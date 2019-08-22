using System;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Models
{
    public class SquadProductBaseRp
    {
        public int Id { get; set; }
    }

    public class SquadProductGetRp : SquadProductBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SquadProductGetListRp : SquadProductBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SquadProductPostRp
    {
        [Required]
        public int SquadId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }

    public class SquadProductPutRp
    {
    }
}

using System;
namespace Owlvey.Falcon.Models
{
    public class SourceBaseRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; } = "";
    }

    public class SourceGetRp : SourceBaseRp
    {
        public string Avatar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }

        public decimal Availability { get; set; }
    }
    public class SourceLitRp: SourceBaseRp {
        public string Avatar { get; set; }
    }

    public class SourceGetListRp : SourceBaseRp
    {
        public string CreatedBy { get; set; }        
        public DateTime? CreatedOn { get; set; }
        public decimal Availability { get; set; }
    }

    public class SourcePostRp
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }

    public class SourcePutRp
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
    }
}

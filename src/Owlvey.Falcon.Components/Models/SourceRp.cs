using System;
namespace Owlvey.Falcon.Models
{
    public class SourceBaseRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SourceGetRp : SourceBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SourceGetListRp : SourceBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SourcePostRp
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }

    public class SourcePutRp
    {
        public string Value { get; set; }
    }
}

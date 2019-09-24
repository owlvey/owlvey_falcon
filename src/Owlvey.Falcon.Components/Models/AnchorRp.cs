using System;
namespace Owlvey.Falcon.Models
{
    public class AnchorRp
    {
        public DateTime Target { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class AnchorPutRp {
        public DateTime Target { get; set; }
    }
}

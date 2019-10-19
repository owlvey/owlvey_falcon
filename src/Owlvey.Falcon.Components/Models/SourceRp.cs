using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Owlvey.Falcon.Core.Entities;
using System;
namespace Owlvey.Falcon.Models
{
    public class SourceLitRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; } = "";
        public string Avatar { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SourceKindEnum Kind { get; set; }
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
    }    

    public class SourceGetRp : SourceLitRp
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Availability { get; set; }
        public int Total { get; set; }
        public int Good { get; set; }
        public int Delta { get { return this.Total - Good; } }
    }

    public class SourceMigrateRp {
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; } = "";        
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }        
        public string Avatar { get; set; }
        public string Description { get; set; }
    }

    public class SourceGetListRp : SourceLitRp
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Availability { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }
    }

    public class SourcePostRp
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public SourceKindEnum Kind { get; set; } = SourceKindEnum.Interaction;
    }

    public class SourcePutRp
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
    }
}

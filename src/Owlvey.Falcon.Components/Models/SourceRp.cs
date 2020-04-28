using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Owlvey.Falcon.Models
{
    public class SourceLitRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; } = "";
        public string Avatar { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SourceKindEnum Kind { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SourceGroupEnum Group { get; set; }

        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
    }    

    public class SourceGetRp : SourceLitRp
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Quality { get; set; }
        public int Total { get; set; }
        public int Good { get; set; }
        public int Delta { get { return this.Total - Good; } }

        public Dictionary<string, int> Features  { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, decimal> Clues { get; set; } = new Dictionary<string, decimal>();
    }

    public class SourceMigrateRp {
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; } = "";        
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }        
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }
        public string Group { get; set; }
    }

    public class SourceGetListRp : SourceLitRp
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Availability { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }
        public int References { get; set; }
    }

    public class SourcePostRp
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SourceKindEnum Kind { get; set; } = SourceKindEnum.Interaction;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SourceGroupEnum Group { get; set; } = SourceGroupEnum.Availability;
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

using System;
namespace Owlvey.Falcon.Models
{
    public class IndicatorBaseRp
    {
        public int Id { get; set; }        
    }

    public class IndicatorGetRp : IndicatorBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class IndicatorGetListRp : IndicatorBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class IndicatorPostRp
    {
        public int FeatureId { get; set; }
        public int SourceId { get; set; }
    }

    public class IndicatorPutRp
    {
        public string Value { get; set; }
    }
}

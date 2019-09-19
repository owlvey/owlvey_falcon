using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class IncidentBaseRp
    {
        public int Id { get; set; }        
        public string Key { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Affected { get; set; }
        public int TTD { get; set; }
        public int TTE { get; set; }
        public int TTF { get; set; }
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int TTM { get {
                return this.TTD + this.TTE + this.TTF;
         } }
    }


    public class IncidentGetListRp : IncidentBaseRp
    {
        
    }

    public class IncidentDetailtRp : IncidentBaseRp
    {
        public List<FeatureLiteRp> Features { get; set; }
    }

    public class IncidentPostRp 
    {
        public int ProductId { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
    }

    public class IncidentMigrationRp {
        public string Product { get; set; }
        public string Key { get; set; }                
        public string Title { get; set; }
        public int Affected { get; set; }        
        public int TTD { get; set; }        
        public int TTE { get; set; }        
        public int TTF { get; set; }        
        public string URL { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }

    public class IncidentFeatureMigrationRp
    {
        public string Product { get; set; }
        public string IncidentKey { get; set; }
        public string Feature { get; set; }

    }
    public class IncidentPutRp
    {
        [Required]
        public int? Affected { get; set; }

        [Required]
        public string Title { get; set; }        

        [Required]
        public int? TTD { get; set; }

        [Required]
        public int? TTE { get; set; }

        [Required]
        public int? TTF { get; set; }

        [Required]
        public string URL { get; set; }

        [Required]
        public DateTime? End { get; set; }

    }


}

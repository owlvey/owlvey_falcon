using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{    
    public class IncidentBaseRp
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }


    public class IncidentGetListRp : IncidentBaseRp
    {
        
    }


}

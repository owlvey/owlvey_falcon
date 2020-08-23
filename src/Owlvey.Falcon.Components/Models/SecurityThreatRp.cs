using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    
    public abstract class SecurityThreatRp{
        
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string Tags { get; set; }
    }
    public class SecurityThreatGetRp: SecurityThreatRp
    {
        public int Id { get; set; }
    }

    public class SecurityThreatPutRp : SecurityThreatRp
    {
        
    }
    public class SecurityThreatPostRp {
        public string Name { get; set; }        
    }
}

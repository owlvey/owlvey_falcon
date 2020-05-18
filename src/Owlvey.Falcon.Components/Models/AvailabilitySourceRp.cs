using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class AvailabilitySourceGetListRp
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
    }
}

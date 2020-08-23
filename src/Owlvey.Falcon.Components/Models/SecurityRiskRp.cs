using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public abstract class SecurityRiskRp
    {
        public int AgentSkillLevel { get; set; }
    }
    public class SecurityRiskGetRp: SecurityRiskRp
    {
        public int Id { get; set; }
    }
    public class SecurityRiskPost { 
        [Required]
        public int SourceId { get; set; }
        [Required]
        public int ThreatId { get; set; }
    }
    public class SecurityRiskPut: SecurityRiskRp
    {
        

    }
}

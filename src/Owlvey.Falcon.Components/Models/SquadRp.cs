using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Models
{

    public class SquadFeatureMigrationRp
    {
        public string Product { get; set; }
        public string Squad { get; set; }
        public string Feature { get; set; }

        public class Comparer : IEqualityComparer<SquadFeatureMigrationRp>
        {
            public bool Equals(SquadFeatureMigrationRp x, SquadFeatureMigrationRp y)
            {
                return String.Equals(x.Product, y.Product, StringComparison.InvariantCultureIgnoreCase)
                    && String.Equals(x.Feature, y.Feature, StringComparison.InvariantCultureIgnoreCase)
                    && String.Equals(x.Squad, y.Squad, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(SquadFeatureMigrationRp obj)
            {
                return obj.GetHashCode();
            }
        }
    }

    public class SquadBaseRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Leaders { get; set; }        
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string Avatar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SquadMigrationRp {
        public string Name { get; set; }
        public string Description { get; set; }                   
        public string Avatar { get; set; }
        public string Leaders { get; set; }
    }

    public class SquadGetRp : SquadBaseRp {
        public IEnumerable<UserGetListRp> Members { get; set; } = new List<UserGetListRp>();
        public IEnumerable<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
    }

    public class SquadQualityGetRp : SquadBaseRp {
        public IEnumerable<UserGetListRp> Members { get; set; } = new List<UserGetListRp>();
        public List<FeatureBySquadRp> Features { get; set; } = new List<FeatureBySquadRp>();
                
        public DebtMeasureValue Debt {
            get {
                return new DebtMeasureValue(this.Features.Select(c=>c.Debt));
            } 
        }
    }

    public class SquadGetDetailRp : SquadBaseRp
    {
        public decimal Points { get; set; }
        public IEnumerable<UserGetListRp> Members { get; set; } = new List<UserGetListRp>();
        public List<FeatureBySquadRp> Features { get; set; } = new List<FeatureBySquadRp>();
    }

    public class SquadGetListRp : SquadBaseRp
    {
        public DebtMeasureValue Debt { get; set; }
        public int Features { get; set; }
        public int Members { get; set; }
    }

    public class SquadPostRp {
        [Required]
        public string Name { get; set; }
        [Required]
        public int CustomerId { get; set; }        
    }

    public class SquadPutRp
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Leaders { get; set; }
    }
}

using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class ProductBaseRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public string Leaders { get; set; }
    }

    public class ProductMigrationRp {        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; } 
        public string Leaders { get; set; }
    }

    public class ProductGetRp : ProductBaseRp {
        public IEnumerable<JourneyGetListRp> Journeys { get; set; } = new List<JourneyGetListRp>();
    }

    public class ProductGetListRp  {

        public DebtMeasureValue Debt { get {
                return new DebtMeasureValue() { 
                     Availability = this.Items.Sum(c => c.Debt.Availability),
                     Latency = this.Items.Sum(c => c.Debt.Latency),
                     Experience = this.Items.Sum(c => c.Debt.Experience)
                };
            }
        }
        public DebtMeasureValue PreviousDebt
        {
            get
            {
                return new DebtMeasureValue()
                {
                    Availability = this.Items.Sum(c => c.PreviousDebt.Availability),
                    Latency = this.Items.Sum(c => c.PreviousDebt.Latency),
                    Experience = this.Items.Sum(c => c.PreviousDebt.Experience)
                };
            }
        }
        public DebtMeasureValue BeforeDebt
        {
            get
            {
                return new DebtMeasureValue()
                {
                    Availability = this.Items.Sum(c => c.BeforeDebt.Availability),
                    Latency = this.Items.Sum(c => c.BeforeDebt.Latency),
                    Experience = this.Items.Sum(c => c.BeforeDebt.Experience)
                };
            }
        }
        public List<ProductGetListItemRp> Items { get; set; } = new List<ProductGetListItemRp>();
    }
    public class ProductGetListItemRp : ProductBaseRp
    {
        public int JourneysCount { get; set; }
        public int FeaturesCount { get; set; }
        public int SourcesCount { get; set; }

        public decimal Coverage { get; set; }
        public decimal Ownership { get; set; }
        public decimal Utilization { get; set; }

        public DebtMeasureValue Debt { get; set; }
        public DebtMeasureValue PreviousDebt { get; set; }
        public DebtMeasureValue BeforeDebt { get; set; }
    }

    public class ProductPostRp {
        [Required]
        public string Name { get; set; }        
        [Required]
        public int CustomerId { get; set; }
    }

    public class ProductPutRp
    {        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Avatar { get; set; }

        public string Leaders { get; set; }
    }



    
}

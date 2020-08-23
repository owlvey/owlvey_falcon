using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Entities;
using Newtonsoft.Json;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Models
{
    public class CustomerBaseRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Leaders { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class CustomerGetRp : CustomerBaseRp {        
        public ProductGetListRp Products { get; set; } 
    }

    public class CustomerLiteRp : CustomerBaseRp { 
    }
    public class CustomerGetListRp : CustomerBaseRp
    {
        public int ProductsCount { get; set; }
        public DebtMeasureValue Debt { get; set; } = new DebtMeasureValue();        
        public DebtMeasureValue PreviousDebt { get; set; } = new DebtMeasureValue();
        public DebtMeasureValue BeforeDebt { get; set; } = new DebtMeasureValue();
    }

    public class CustomerPostRp {
        [Required]
        public string Name { get; set; }

        public bool Default { get; set; } = false;
        
    }

    public class CustomerPutRp
    {        
        public string Name { get; set; }

        public string Avatar { get; set; }
        public string Leaders { get; set; }
    }


    public class CustomerDashboardRp {

        public List<string> Categories { get; set;  } = new List<string>() { "10%", "50%", "60%", "70%", "80%", "85%", "90%", "95%", "96%", "97%", "98%", "99%", "99.9%", "100%" };


        public class CustomerProductRp {
            public int CustomerId { get; set; }
            public string Customer { get; set; }
            public int ProductId { get; set; }
            public string Product { get; set; }


            [JsonIgnore]
            public List<CustomerJourneyRp> Journeys = new List<CustomerJourneyRp>();

            public decimal Effectiveness { get {                    
                    
                    return QualityUtils.CalculateProportion(this.Journeys.Count, this.Journeys.Where(c => c.AvailabilityErrorBudget >= 0).Count());                    
                }
            }

            public decimal Previous { get; set; }
            public decimal Before { get; set; }

            public int Total {
                get {
                    return this.Journeys.Count();
                }
            }



            

            public CustomerProductRp() { }
            public CustomerProductRp(ProductEntity product) {
                this.CustomerId = product.CustomerId;
                this.Customer = product.Customer.Name;
                this.ProductId = product.Id.Value;
                this.Product = product.Name;
            }
            public List<object> Groups {
                get {
                    var result = new List<object>();
                    
                    var targets = new List<IList<CustomerJourneyRp>>() {
                        this.Journeys.Where(c => c.AvailabilitySLO > 0 && c.AvailabilitySLO <= 0.10m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.10m  && c.AvailabilitySLO <= 0.80m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.80m  && c.AvailabilitySLO <= 0.85m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.85m  && c.AvailabilitySLO <= 0.90m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.90m  && c.AvailabilitySLO <= 0.95m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.95m  && c.AvailabilitySLO <= 0.96m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.96m  && c.AvailabilitySLO <= 0.97m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.97m  && c.AvailabilitySLO <= 0.98m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.98m  && c.AvailabilitySLO <= 0.99m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.99m  && c.AvailabilitySLO <= 0.999m).ToList(),
                        this.Journeys.Where(c => c.AvailabilitySLO > 0.999m && c.AvailabilitySLO <= 100).ToList()
                    };
                    for (int i = 0; i < targets.Count; i++)
                    {
                        var total = targets[i].Count();
                        var good = targets[i].Where(c => c.AvailabilityErrorBudget >= 0).Count();
                        
                        result.Add(new {
                            index = i,
                            title = string.Format("Total {0}, Good: {1}", total, good),
                            count = targets[i].Count(),
                            budget = targets[i].Sum(c => c.AvailabilityErrorBudget),
                            status = targets[i].Where(c => c.AvailabilityErrorBudget  < 0).Count() == 0 ? "success" : targets[i].Where(c => c.AvailabilityErrorBudget >= 0).Count() == 0 ? "danger" : "warning",
                            tags = targets[i].Select(c => string.Format("{0}, SLO: {1}, Ava: {2}, Budget: {3}", c.Journey, c.AvailabilitySLO, c.Availability, c.AvailabilityErrorBudget))
                        }); 
                    }                   

                    return result;
                }
            }
            
        }
        public class CustomerJourneyRp {
            public int JourneyId { get; set; }
            public string Journey { get; set; }
            public decimal AvailabilitySLO { get; set; }
            public decimal Availability { get; set; }
            public decimal AvailabilityErrorBudget { get; set; }

            public CustomerJourneyRp() { }
            public CustomerJourneyRp(JourneyEntity journey) {
                this.JourneyId = journey.Id.Value;
                this.Journey = journey.Name;
                this.AvailabilitySLO = journey.AvailabilitySlo;
                var measure= journey.Measure();
                this.Availability = measure.Availability;
                this.AvailabilityErrorBudget = measure.AvailabilityErrorBudget;
            }
        }

        
        public List<CustomerProductRp> Products { get; set; } = new List<CustomerProductRp>();
               
   
          

    }
}

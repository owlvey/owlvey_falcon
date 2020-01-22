using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Entities;
using Newtonsoft.Json;

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
        public decimal Availability { get; set; }
        public IEnumerable<ProductGetListRp> Products { get; set; } = new List<ProductGetListRp>();
    }

    public class CustomerGetListRp : CustomerBaseRp
    {
        public int ProductsCount { get; set; }
        public decimal Availability { get; set; }
        public List<ProductGetListRp> Products { get; set; }
    }

    public class CustomerPostRp {
        [Required]
        public string Name { get; set; }
        
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
            public List<CustomerServiceRp> Services = new List<CustomerServiceRp>();
            public decimal Effectiveness { get {                    
                    return QualityUtils.CalculateProportion(this.Services.Count, this.Services.Where(c => c.Budget >= 0).Count());                    
                }
            }

            public int Total {
                get {
                    return this.Services.Count();
                }
            }

            public int Good {
                get {
                    return this.Services.Where(c => c.Budget >= 0).Count();
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
                    
                    var targets = new List<IList<CustomerServiceRp>>() {
                        this.Services.Where(c => c.SLO > 0 && c.SLO <= 0.10m).ToList(),
                        this.Services.Where(c => c.SLO > 0.10m && c.SLO <= 0.80m).ToList(),
                        this.Services.Where(c => c.SLO > 0.80m && c.SLO <= 0.85m).ToList(),
                        this.Services.Where(c => c.SLO > 0.85m && c.SLO <= 0.90m).ToList(),
                        this.Services.Where(c => c.SLO > 0.90m && c.SLO <= 0.95m).ToList(),
                        this.Services.Where(c => c.SLO > 0.95m && c.SLO <= 0.96m).ToList(),
                        this.Services.Where(c => c.SLO > 0.96m && c.SLO <= 0.97m).ToList(),
                        this.Services.Where(c => c.SLO > 0.97m && c.SLO <= 0.98m).ToList(),
                        this.Services.Where(c => c.SLO > 0.98m && c.SLO <= 0.99m).ToList(),
                        this.Services.Where(c => c.SLO > 0.99m && c.SLO <= 0.999m).ToList(),
                        this.Services.Where(c => c.SLO > 0.999m && c.SLO <= 100).ToList()
                    };
                    for (int i = 0; i < targets.Count; i++)
                    {
                        var total = targets[i].Count();
                        var good = targets[i].Where(c => c.Budget >= 0).Count();
                        
                        result.Add(new {
                            index = i,
                            title = string.Format("Total {0}, Good: {1}", total, good),
                            count = targets[i].Count(),
                            budget = targets[i].Sum(c => c.Budget),
                            status = targets[i].Where(c => c.Budget < 0).Count() == 0 ? "success" : targets[i].Where(c => c.Budget >= 0).Count() == 0 ? "danger" : "warning",
                            tags = targets[i].Select(c => string.Format("{0}, SLO: {1}, Ava: {2}, Budget: {3}", c.Service, c.SLO, c.Availability, c.Budget))
                        }); 
                    }                   

                    return result;
                }
            }
            
        }
        public class CustomerServiceRp {
            public int ServiceId { get; set; }
            public string Service { get; set; }
            public decimal SLO { get; set; }
            public decimal Availability { get; set; }
            public decimal Budget { get; set; }

            public CustomerServiceRp() { }
            public CustomerServiceRp(ServiceEntity service) {
                this.ServiceId = service.Id.Value;
                this.Service = service.Name;
                this.SLO = service.Slo;
                this.Availability = service.Availability;
                this.Budget = QualityUtils.MeasureBudget(this.Availability, this.SLO);
            }
        }

        
        public List<CustomerProductRp> Products { get; set; } = new List<CustomerProductRp>();
               
        public decimal Effectiveness {
            get
            {
                var target = this.Products.SelectMany(c => c.Services);
                return QualityUtils.CalculateProportion(target.Count(), target.Where(c => c.Budget >= 0).Count());
            }
        }
        public int Total
        {
            get
            {
                return this.Products.SelectMany(c=>c.Services).Count();
            }
        }
        public int Good
        {
            get
            {
                return this.Products.SelectMany(c=>c.Services).Where(c => c.Budget >= 0).Count();
            }
        }

    }
}

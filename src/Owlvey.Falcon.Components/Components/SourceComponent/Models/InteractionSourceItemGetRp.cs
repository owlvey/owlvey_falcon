using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{
    public class InteractiveSourceItemGetRp : SourceItemBaseRp
    {
        public InteractiveSourceItemGetRp() { }
        public InteractiveSourceItemGetRp(SourceItemEntity entity)
        {
            this.Good = entity.Good.GetValueOrDefault();
            this.Total = entity.Total.GetValueOrDefault();
            this.Target = entity.Target;
            this.CreatedBy = entity.CreatedBy;
            this.CreatedOn = entity.CreatedOn;
            this.Measure = entity.Measure;
        }
        public int Good { get; set; }
        public int Total { get; set; }
        public int Delta { get { return Total - Good; } }
        public decimal Measure { get; set; }
    }
}

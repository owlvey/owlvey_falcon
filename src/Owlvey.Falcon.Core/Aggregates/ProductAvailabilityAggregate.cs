using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class ProductAvailabilityAggregate
    {
        public ProductEntity Product { get; protected set; }
        private IEnumerable<(ServiceEntity service, decimal Availability)> Services;
        public ProductAvailabilityAggregate(ProductEntity entity,
            IEnumerable<(ServiceEntity service, decimal Availability)> services)
        {
            this.Product = entity;
            this.Services = services;
        }
        public (ProductEntity product, decimal availability) MeasureAvailability()
        {
            decimal availability = 1;
            foreach (var (_, item) in this.Services)
            {
                availability *= item;
            }
            return (this.Product, availability);
        }
    }
}

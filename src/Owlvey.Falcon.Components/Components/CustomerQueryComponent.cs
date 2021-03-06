using Owlvey.Falcon.Components;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using System.IO;
using OfficeOpenXml;
using System.Data;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Repositories.Products;

namespace Owlvey.Falcon.Components
{
    public class CustomerQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly SquadQueryComponent _squadQueryComponent;
        private readonly ProductQueryComponent _productQueryComponent;
        public CustomerQueryComponent(FalconDbContext dbContext, IMapper mapper,
            IDateTimeGateway dateTimeGateway, IUserIdentityGateway identityGateway,
            SquadQueryComponent squadQueryComponent,
            ConfigurationComponent configuration, ProductQueryComponent productQueryComponent) : base(dateTimeGateway, mapper, identityGateway, configuration)
        {
            this._squadQueryComponent = squadQueryComponent;
            this._productQueryComponent = productQueryComponent;
            this._dbContext = dbContext;
        }


        public async Task<CustomerGetRp> GetCustomerById(int id)
        {
            var entities = await this._dbContext.Customers.Where(c => c.Id.Equals(id)).ToListAsync();

            var entity = entities.FirstOrDefault();            

            if (entity == null)
                return null;

            var result = this._mapper.Map<CustomerGetRp>(entity);            
            return result;
        }

        public async Task<CustomerGetRp> GetCustomerByIdWithQuality(int id, DatePeriodValue period)
        {
            var entities = await this._dbContext.Customers.Where(c => c.Id.Equals(id)).ToListAsync();

            var entity = entities.FirstOrDefault();

            if (entity == null)
                return null;

            var result = this._mapper.Map<CustomerGetRp>(entity);
            result.Products = await this._productQueryComponent.GetProductsWithInformation(id, period);
            return result;
        }

        public async Task<CustomerGetRp> GetCustomerByName(string name)
        {
            var entity = await this._dbContext.Customers.SingleAsync(c => c.Name.Equals(name));
            return this._mapper.Map<CustomerGetRp>(entity);
        }



        /// <summary>
        /// Get All Customer
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerLiteRp>> GetCustomers()
        {
            var entities = await this._dbContext.Customers.Include(c=>c.Products).ToListAsync();




            var result = this._mapper.Map<IEnumerable<CustomerLiteRp>>(entities);

            return result;
        }

        public async Task<IEnumerable<CustomerGetListRp>> GetCustomersQuality(DatePeriodValue period)
        {
            var entities = await this._dbContext.Customers.Include(c => c.Products).ToListAsync();
            var result = new List<CustomerGetListRp>();            
            foreach (var customer in entities)
            {
                var t = this._mapper.Map<CustomerGetListRp>(customer);                
                var temp = await this._productQueryComponent.GetProductsWithInformation(customer.Id.Value, period);
                t.Debt.Add(temp.Debt);
                t.PreviousDebt.Add(temp.PreviousDebt);
                t.BeforeDebt.Add(temp.BeforeDebt);                
                result.Add(t);
            }
            return result;
        }


        public async Task<GraphGetRp> GetSquadsGraph(int customerId, DatePeriodValue period)
        {
            GraphGetRp result = new GraphGetRp();
            var root = await this._dbContext.Customers.Include(c=>c.Products).Where(c=>c.Id == customerId).SingleOrDefaultAsync();
            var temp = new List<ProductEntity>();
            foreach (var item in root.Products)
            {
                var p = await this._dbContext.FullLoadProductWithSourceItems(item.Id.Value, period.Start, period.End);
                temp.Add(p);
            }
            root.Products = temp;

            result.Name = root.Name;
            result.Id = root.Id.Value;
            result.Avatar = root.Avatar;
            
            var squads = await this._dbContext.Squads.Where(c => c.Customer.Id.Equals(customerId)).ToListAsync();
            var squadsDetail = new List<SquadQualityGetRp>();

            foreach (var item in squads)
            {
                var detail = await this._squadQueryComponent.GetSquadByIdWithQuality(item.Id.Value, 
                    period);
                squadsDetail.Add(detail);
            }                       

            foreach (var node in squadsDetail)
            {
                var snode = new GraphNode
                {
                    Id = string.Format("squad_{0}", node.Id),
                    Avatar = node.Avatar,
                    Name = node.Name,
                    Value = node.Debt.Availability,
                    Group = "squads",                    
                };
                result.Nodes.Add(snode);
                                               
                foreach (var feature in node.Features)
                {
                    var feature_id = string.Format("feature_{0}", feature.Id);
                    var fnode = result.Nodes.SingleOrDefault(c => c.Id == feature_id);
                    if (fnode == null)
                    {
                        fnode = new GraphNode
                        {
                            Id = feature_id,
                            Avatar = feature.Avatar,
                            Name = feature.Name,
                            Value = feature.Debt.Availability,
                            Group = string.Format("features_{0}", feature.Product),                            
                        };
                        result.Nodes.Add(fnode);

                    }                    
                    var edge_squad_feature = result.Edges.Where(c => c.From == snode.Id && c.To == fnode.Id).SingleOrDefault();
                    if (edge_squad_feature == null) {
                        var fedge = new GraphEdge()
                        {
                            From = snode.Id,
                            To = fnode.Id,
                            Value = feature.Debt.Availability
                        };
                        result.Edges.Add(fedge);
                    }
                }
            }
            return result;
        }

        private async Task<CustomerDashboardRp> InternalGetDashboardCustomersProductJourneys(DateTime? start, DateTime? end)
        {
            var result = new CustomerDashboardRp();
            var customers = await this._dbContext.Customers
                .Include(c => c.Products)
                .ThenInclude(c => c.Journeys)
                .ThenInclude(c => c.FeatureMap)
                .ThenInclude(c => c.Feature)
                .ThenInclude(c => c.Indicators)
                .ThenInclude(c => c.Source)
                .ToListAsync();

            var sourceItems = await this._dbContext.GetSourceItems(start.Value, end.Value);

            foreach (var customer in customers)
            {
                foreach (var product in customer.Products)
                {
                    var productResult = new CustomerDashboardRp.CustomerProductRp(product);

                    foreach (var journey in product.Journeys)
                    {
                        foreach (var featureMap in journey.FeatureMap)
                        {
                            foreach (var indicator in featureMap.Feature.Indicators)
                            {
                                var target = sourceItems.Where(c => c.SourceId == indicator.SourceId).ToList();
                                indicator.Source.SourceItems = target;
                            }
                        }
                        journey.Measure();
                        var journeyResult = new CustomerDashboardRp.CustomerJourneyRp(journey);
                        productResult.Journeys.Add(journeyResult);
                    }
                    result.Products.Add(productResult);
                }
            }
            return result;
        }
        public async Task<CustomerDashboardRp> GetCustomersDashboardProductJourneys(DateTime? start, DateTime? end)
        {
            var result = new CustomerDashboardRp();

            result = await this.InternalGetDashboardCustomersProductJourneys(start, end);

            var (bs, be, ps, pe) = DateTimeUtils.CalculateBeforePreviousDates(start, end);

            var before = await this.InternalGetDashboardCustomersProductJourneys(bs, be);
            var previous = await this.InternalGetDashboardCustomersProductJourneys(ps, pe);

            foreach (var product in result.Products)
            {
                var target_product = previous.Products.Where(c => c.ProductId == product.ProductId).SingleOrDefault();
                if (target_product != null) {
                    product.Previous = target_product.Effectiveness;
                }
                var target_before = before.Products.Where(c => c.ProductId == product.ProductId).SingleOrDefault();
                if (target_before != null)
                {
                    product.Before = target_before.Effectiveness;
                }
            }
            return result;
        }
    }
}

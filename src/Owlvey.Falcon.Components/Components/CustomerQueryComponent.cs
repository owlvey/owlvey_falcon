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

namespace Owlvey.Falcon.Components
{
    public class CustomerQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly SquadQueryComponent _squadQueryComponent;
        public CustomerQueryComponent(FalconDbContext dbContext, IMapper mapper,
            IDateTimeGateway dateTimeGateway, IUserIdentityGateway identityService,
            SquadQueryComponent squadQueryComponent,
            ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityService, configuration)
        {
            this._squadQueryComponent = squadQueryComponent;
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

        public async Task<CustomerGetRp> GetCustomerByName(string name)
        {
            var entity = await this._dbContext.Customers.SingleAsync(c => c.Name.Equals(name));
            return this._mapper.Map<CustomerGetRp>(entity);
        }



        /// <summary>
        /// Get All Customer
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerGetListRp>> GetCustomers()
        {
            var entities = await this._dbContext.Customers.Include(c=>c.Products).ToListAsync();

            var result = this._mapper.Map<IEnumerable<CustomerGetListRp>>(entities);

            return result;
        }   
    

        public async Task<GraphGetRp> GetSquadsGraph(int customerId, DateTime start, DateTime end)
        {
            GraphGetRp result = new GraphGetRp();
            var root = await this.GetCustomerById(customerId);
            result.Name = root.Name;
            result.Id = root.Id;
            result.Avatar = root.Avatar;
            
            var squads = await this._dbContext.Squads.Where(c => c.Customer.Id.Equals(customerId)).ToListAsync();
            var squadsDetail = new List<SquadGetDetailRp>();

            foreach (var item in squads)
            {
                var detail = await this._squadQueryComponent.GetSquadByIdWithAvailability(item.Id.Value, start, end);
                squadsDetail.Add(detail);
            }                       

            foreach (var node in squadsDetail)
            {
                var snode = new GraphNode
                {
                    Id = string.Format("squad_{0}", node.Id),
                    Avatar = node.Avatar,
                    Name = node.Name,
                    Value = node.Points,
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
                            Value = feature.Availability,
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
                            Value = feature.Points
                        };
                        result.Edges.Add(fedge);
                    }
                }
            }
            return result;
        }

        private async Task<CustomerDashboardRp> InternalGetDashboardCustomersProductServices(DateTime? start, DateTime? end)
        {
            var result = new CustomerDashboardRp();
            var customers = await this._dbContext.Customers
                .Include(c => c.Products)
                .ThenInclude(c => c.Services)
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

                    foreach (var service in product.Services)
                    {
                        foreach (var featureMap in service.FeatureMap)
                        {
                            foreach (var indicator in featureMap.Feature.Indicators)
                            {
                                var target = sourceItems.Where(c => c.SourceId == indicator.SourceId).ToList();
                                indicator.Source.SourceItems = target;
                            }
                        }
                        service.MeasureQuality();
                        var serviceResult = new CustomerDashboardRp.CustomerServiceRp(service);
                        productResult.Services.Add(serviceResult);
                    }
                    result.Products.Add(productResult);
                }
            }
            return result;
        }
        public async Task<CustomerDashboardRp> GetCustomersDashboardProductServices(DateTime? start, DateTime? end)
        {
            var result = new CustomerDashboardRp();

            result = await this.InternalGetDashboardCustomersProductServices(start, end);

            var (bs, be, ps, pe) = DateTimeUtils.CalculateBeforePreviousDates(start, end);

            var before = await this.InternalGetDashboardCustomersProductServices(bs, be);
            var previous = await this.InternalGetDashboardCustomersProductServices(ps, pe);

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

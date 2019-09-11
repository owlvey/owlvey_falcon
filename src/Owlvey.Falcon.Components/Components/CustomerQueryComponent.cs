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

namespace Owlvey.Falcon.Components
{
    public class CustomerQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly SquadQueryComponent _squadQueryComponent;
        public CustomerQueryComponent(FalconDbContext dbContext, IMapper mapper,
            IDateTimeGateway dateTimeGateway, IUserIdentityGateway identityService,
            SquadQueryComponent squadQueryComponent) : base(dateTimeGateway, mapper, identityService)
        {
            this._squadQueryComponent = squadQueryComponent;
            this._dbContext = dbContext;
        }


        public async Task<CustomerGetRp> GetCustomerById(int id)
        {
            var entities = await this._dbContext.Customers.Where(c => c.Deleted == false && c.Id.Equals(id)).ToListAsync();

            var entity = entities.FirstOrDefault();            

            if (entity == null)
                return null;

            var result = this._mapper.Map<CustomerGetRp>(entity);            

            return result;
        }

        public async Task<CustomerGetRp> GetCustomerByIdWithAvailability(int id, DateTime target)
        {
            var entities = await this._dbContext.Customers.Where(c => c.Deleted == false && c.Id.Equals(id)).ToListAsync();

            var entity = entities.FirstOrDefault();

            if (entity == null)
                return null;

            var result = this._mapper.Map<CustomerGetRp>(entity);

            var tmp = await this.GetAvailabilityByDateRange(id, target, target);

            result.Availability = tmp.availabilities.Single().Availability;

            return result;
        }

        public async Task<CustomerGetRp> GetCustomerByName(string name)
        {
            var entity = await this._dbContext.Customers.SingleAsync(c => c.Deleted == false && c.Name.Equals(name));
            return this._mapper.Map<CustomerGetRp>(entity);
        }



        /// <summary>
        /// Get All Customer
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerGetListRp>> GetCustomers()
        {
            var entities = await this._dbContext.Customers.Include(c=>c.Products).Where(c=>c.Deleted==false).ToListAsync();

            var result = this._mapper.Map<IEnumerable<CustomerGetListRp>>(entities);

            return result;
        }

        private async Task<(CustomerEntity customer, IEnumerable<DayAvailabilityEntity> availabilities, IEnumerable<(ProductEntity service, IEnumerable<DayAvailabilityEntity> availabilities)> products)> GetAvailabilityByDateRange(int customerId, DateTime start, DateTime end) {

            var customer = await this._dbContext.Customers.Include(c => c.Products).ThenInclude(c => c.Services).Where(c => c.Id == customerId).SingleAsync();
            foreach (var product in customer.Products)
            {
                foreach (var service in product.Services)
                {
                    var serviceMaps = await this._dbContext.ServiceMaps.Include(c => c.Feature).ThenInclude(c => c.Indicators).Where(c => c.Service.Id == service.Id).ToListAsync();
                    await this._dbContext.LoadIndicators(serviceMaps);
                    foreach (var map in serviceMaps)
                    {
                        foreach (var indicator in map.Feature.Indicators)
                        {
                            var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                            indicator.Source.SourceItems = sourceItems;
                        }
                    }
                }
            }
            
            var aggregator = new CustomerAvailabilityAggregate(customer, start, end);

            return aggregator.MeasureAvailability();

        }
        public async Task<MultiSeriesGetRp> GetDailySeriesById(int customerId, DateTime start, DateTime end)
        {
            var (customer, availability, features) =  await this.GetAvailabilityByDateRange(customerId, start, end);
            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = customer.Name,
                Avatar = customer.Avatar
            };
            result.Series.Add(new MultiSerieItemGetRp()
            {
                Name = "Availability",
                Avatar = customer.Avatar,
                Items = availability.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
            });

            foreach (var indicator in features)
            {
                result.Series.Add(new MultiSerieItemGetRp()
                {
                    Name = string.Format("Product:{0}", indicator.Item1.Id),
                    Avatar = indicator.Item1.Avatar,
                    Items = indicator.Item2.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
                });
            }
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


    }
}

using Owlvey.Falcon.Core.Models.Migrate;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using AutoMapper;
using System.Linq;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities.Source;

namespace Owlvey.Falcon.Builders
{
    public class SystemModelBuilder
    {
        private static void PrepareData(
            IEnumerable<UserEntity> users,
            IEnumerable<CustomerEntity> customers,            
            IEnumerable<SecurityRiskEntity> securityRisks,
            IEnumerable<ReliabilityRiskEntity> reliabilityRisks,
            IEnumerable<JourneyEntity> journeys,
            IEnumerable<FeatureEntity> features,
            IEnumerable<SourceEntity> sources,
            IEnumerable<SourceItemEntity> sourceItems)
        {
            foreach (var customer in customers)
            {
                foreach (var squad in customer.Squads)
                {
                    foreach (var member in squad.Members)
                    {
                        member.User = users.Where(c => c.Id == member.UserId).Single();
                    }
                }

                foreach (var product in customer.Products)
                {
                    product.Features = features.Where(c => c.ProductId == product.Id).ToList();
                    product.Journeys = journeys.Where(c => c.ProductId == product.Id).ToList();
                    product.Sources = new SourceCollection(sources.Where(c => c.ProductId == product.Id).ToList());

                    foreach (var journey in product.Journeys)
                    {
                        foreach (var item in journey.FeatureMap)
                        {
                            item.Feature = product.Features.Where(c => c.Id == item.FeatureId).Single();
                        }
                    }

                    foreach (var feature in product.Features)
                    {
                        foreach (var indicator in feature.Indicators)
                        {
                            indicator.Source = product.Sources.Where(c => c.Id == indicator.SourceId).Single();
                        }
                        foreach (var squad in feature.Squads)
                        {
                            squad.Squad = customer.Squads.Single(c => c.Id == squad.SquadId);
                        }
                    }
                    foreach (var source in product.Sources)
                    {
                        source.SourceItems = sourceItems.Where(c => c.SourceId == source.Id).ToList();

                        foreach (var security in securityRisks.Where(c => c.SourceId == source.Id)) {
                            security.Source = source;
                        }
                        foreach (var reliability in reliabilityRisks.Where(c => c.SourceId == source.Id)) {
                            reliability.Source = source;
                        }
                    }

                }
            }
        }

        public static async Task<SystemModel> Build(IMapper mapper, FalconDbContext dbContext, bool includeData) {

            SystemModel result = new SystemModel();

            var users = await dbContext.Users.ToListAsync();
            var customers = await dbContext.Customers
                .Include(c => c.Products).ThenInclude(c => c.Anchors)
                .Include(c => c.Squads).ThenInclude(d => d.Members)
                .ToListAsync();
            var journeys = await dbContext.Journeys.Include(c => c.FeatureMap).ToListAsync();
            var features = await dbContext.Features
                .Include(c => c.Indicators)
                .Include(c => c.Squads)
                .ToListAsync();
            var sources = await dbContext.Sources.ToListAsync();
            var sourceItems = new List<SourceItemEntity>();
            if (includeData)
            {
                sourceItems = await dbContext.SourcesItems.ToListAsync();
            }

            var securityThreats = await dbContext.SecurityThreats.ToListAsync();
            
            var securityRisks = await dbContext.SecurityRisks.ToListAsync();
            
            var reliabilityThreats = await dbContext.ReliabilityThreats.ToListAsync();
            var reliabilityRisks = await dbContext.ReliabilityRisks.ToListAsync();            

            SystemModelBuilder.PrepareData(users, customers,                
                securityRisks, reliabilityRisks,
                journeys, features, sources, sourceItems);

            result.AddUsers(users);

            var securityThreatModels = mapper.Map<IEnumerable<SecurityThreatModel>>(securityThreats);
            result.AddSecurityThreats(securityThreatModels);

            var securityRiskModels = mapper.Map<IEnumerable<SecurityRiskModel>>(securityRisks);
            result.AddSecurityRisks(securityRiskModels);

            var reliabilityThreatModels = mapper.Map<IEnumerable<ReliabilityThreatModel>>(reliabilityThreats);
            result.AddReliabilityThreats(reliabilityThreatModels);

            var reliabilityRiskModels = mapper.Map<IEnumerable<ReliabilityRiskModel>>(reliabilityRisks);
            result.AddReliabilityRisks(reliabilityRiskModels);

            foreach (var customer in customers)
            {
                result.AddOrganization(customer);
                result.AddSquads(customer.Name, customer.Squads);
                result.AddProducts(customer.Name, customer.Products);

                foreach (var squad in customer.Squads)
                {
                    result.AddMembers(customer.Name, squad.Name, squad.Members);
                }
                foreach (var product in customer.Products)
                {
                    result.AddAnchors(customer.Name, product.Name, product.Anchors);
                    result.AddJourneys(customer.Name, product.Name, product.Journeys);
                    foreach (var journey in product.Journeys)
                    {
                        result.AddJourneyMaps(customer.Name, product.Name, journey.Name, journey.FeatureMap);
                    }
                    result.AddFeatures(customer.Name, product.Name, product.Features);
                    foreach (var feature in product.Features)
                    {
                        result.AddIndicator(customer.Name, product.Name, feature.Name, feature.Indicators);
                        result.AddSquadFeature(customer.Name, product.Name, feature.Name, feature.Squads);
                    }
                    result.AddSources(customer.Name, product.Name, product.Sources);
                    foreach (var source in product.Sources)
                    {
                        result.AddSourceItem(customer.Name, product.Name, source.Name, source.SourceItems);
                    }
                }
            }

            return result;
            

        }

    }
}

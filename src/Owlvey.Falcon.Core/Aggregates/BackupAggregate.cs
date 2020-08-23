using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Models.Migrate;
using System.Linq;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities.Source;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class BackupAggregate
    {
        private IEnumerable<CustomerEntity> Customers;        
        private IEnumerable<SourceEntity> Sources;
        private IEnumerable<SourceItemEntity> SourceItems;
        private IEnumerable<JourneyEntity> Journeys;
        private IEnumerable<FeatureEntity> Features;
        private IEnumerable<UserEntity> Users;
        public BackupAggregate(
            IEnumerable<UserEntity> users,
            IEnumerable<CustomerEntity> customers,
            IEnumerable<JourneyEntity> journeys,
            IEnumerable<FeatureEntity> features,
            IEnumerable<SourceEntity> sources,
            IEnumerable<SourceItemEntity> sourceItems) {
            this.Journeys = journeys;
            this.Features = features; 
            this.Customers = customers;
            this.SourceItems = sourceItems;
            this.Users = users;
            this.Sources = sources;
        }

        private void PrepareData() {

            foreach (var customer in this.Customers)
            {
                foreach (var squad in customer.Squads)
                {
                    foreach (var member in squad.Members)
                    {
                        member.User = this.Users.Where(c => c.Id == member.UserId).Single();
                    }
                }                

                foreach (var product in customer.Products)
                {
                    product.Features = this.Features.Where(c => c.ProductId == product.Id).ToList();
                    product.Journeys = this.Journeys.Where(c => c.ProductId == product.Id).ToList();
                    product.Sources = new SourceCollection(this.Sources.Where(c => c.ProductId == product.Id).ToList());
                    
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
                        source.SourceItems = this.SourceItems.Where(c => c.SourceId == source.Id).ToList();
                    }
                }
            }
        }

        public SystemModel Execute() {
            this.PrepareData();

            SystemModel result = new SystemModel();            

            result.AddUsers(this.Users);

            foreach (var customer in this.Customers)
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

using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SystemModel
    {
        public ICollection<OrganizationModel> Organizations { get; protected set; } = new List<OrganizationModel>();
        public ICollection<UserModel> Users { get; protected set; } = new List<UserModel>();
        public ICollection<SquadModel> Squads { get; protected set; } =  new List<SquadModel>();
        public ICollection<AnchorModel> Anchors { get; protected set; } = new List<AnchorModel>();
        public ICollection<MemberModel> Members { get; protected set; } =  new List<MemberModel>();
        public ICollection<ProductModel> Products { get; protected set; } = new List<ProductModel>();
        public ICollection<ServiceModel> Services { get; protected set; } = new List<ServiceModel>();
        public ICollection<ServiceMapModel> ServiceMaps { get; protected set; } = new List<ServiceMapModel>();
        public ICollection<FeatureModel> Features { get; protected set; } = new List<FeatureModel>();
        public ICollection<IndicatorModel> Indicators { get; protected set; } = new List<IndicatorModel>();
        public ICollection<SourceModel> Sources { get; protected set; } = new List<SourceModel>();
        public ICollection<SourceItemModel> SourceItems { get; protected set; } = new List<SourceItemModel>();
        
        public void AddUsers(IEnumerable<UserEntity> users)
        {
            foreach (var item in users)
            {
                var model = new UserModel();
                model.Load(item);
                this.Users.Add(model);
            }            
        }

        public void AddOrganization(CustomerEntity organization) {
            var org = new OrganizationModel();
            org.Load(organization);
            this.Organizations.Add(org);
        }

        public void AddSquads(string organization, IEnumerable<SquadEntity> squads)
        {
            var items = SquadModel.Load(organization, squads);
            foreach (var item in items)
            {
                this.Squads.Add(item);
            }
        }

        public void AddMembers(string organization, string squad, IEnumerable<MemberEntity> members)
        {
            var items = MemberModel.Load(organization, squad, members);
            foreach (var item in items)
            {
                this.Members.Add(item);
            }
        }
        public void AddProducts(string organization, IEnumerable<ProductEntity> products) {
            var items = ProductModel.Load(organization,  products);
            foreach (var item in items)
            {
                this.Products.Add(item);
            }
        }
        public void AddAnchors(string organization, string product, IEnumerable<AnchorEntity> anchors)
        {
            var items = AnchorModel.Load(organization, product, anchors);
            foreach (var item in items)
            {
                this.Anchors.Add(item);
            }
        }
        public void AddServices(string organization, string product, IEnumerable<ServiceEntity> services)
        {
            var items = ServiceModel.Load(organization,  product, services);
            foreach (var item in items)
            {
                this.Services.Add(item);
            }
        }
        public void AddServiceMaps(string organization, string product, string service, IEnumerable<ServiceMapEntity> maps)
        {
            var items = ServiceMapModel.Load(organization, product, service, maps);
            foreach (var item in items)
            {
                this.ServiceMaps.Add(item);
            }
        }

        public void AddFeatures(string organization, string product, IEnumerable<FeatureEntity> features)
        {
            var items = FeatureModel.Load(organization, product, features);
            foreach (var item in items)
            {
                this.Features.Add(item);
            }
        }
        public void AddIndicator(string organization, string product, string feature, IEnumerable<IndicatorEntity> indicators)
        {
            var items = IndicatorModel.Load(organization, product, feature, indicators);
            foreach (var item in items)
            {
                this.Indicators.Add(item);
            }
        }
        public void AddSources(string organization, string product, IEnumerable<SourceEntity> sources)
        {
            var items = SourceModel.Load(organization, product, sources);
            foreach (var item in items)
            {
                this.Sources.Add(item);
            }
        }
        public void AddSourceItem(string organization, string product, string source, IEnumerable<SourceItemEntity> sourceItems)
        {
            var items = SourceItemModel.Load(organization, product, source, sourceItems);
            foreach (var item in items)
            {
                this.SourceItems.Add(item);
            }
        }
    }
}

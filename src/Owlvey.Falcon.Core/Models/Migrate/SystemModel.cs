using System;
using System.Collections.Generic;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SystemModel
    {
        public IEnumerable<OrganizationModel> Organizations { get; protected set; }
        public IEnumerable<UserModel> Users { get; protected set; }
        public IEnumerable<SquadModel> Squads { get; protected set; }
        public IEnumerable<MemberModel> Members { get; protected set; }
        public IEnumerable<ProductModel> Products { get; protected set; }
        public IEnumerable<ServiceModel> Services { get; protected set; }
        public IEnumerable<ServiceMapModel> ServiceMaps { get; protected set; }
        public IEnumerable<FeatureModel> Features { get; protected set; }
        public IEnumerable<IndicatorModel> Indicators { get; protected set; }
        public IEnumerable<SourceModel> Sources { get; protected set; }
        public IEnumerable<SourceItemModel> SourceItems { get; protected set; }
    }
}

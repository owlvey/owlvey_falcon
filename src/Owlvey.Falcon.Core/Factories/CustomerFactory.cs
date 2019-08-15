using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity
    {

        public static class Factory{
            public static CustomerEntity Create(string user, DateTime on, 
                string name,
                string avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png")
            {
                var result = new CustomerEntity
                {
                    Name = name,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,                    
                    Avatar = avatar,
                    Deleted = false
                };
                result.Validate();

                var defaultProduct = ProductEntity.Factory.Create("sample product", on, user, result);
                result.Products.Add(defaultProduct);

                var defaultFeature = FeatureEntity.Factory.Create("feature sample", "default feature", on, user, defaultProduct);
                defaultProduct.Features.Add(defaultFeature);

                var defaultService = ServiceEntity.Factory.Create("service sample", 0.99f, on, user, defaultProduct);
                defaultProduct.Services.Add(defaultService);

                var defaultMap = ServiceMapEntity.Factory.Create(defaultService, defaultFeature, on, user);
                defaultService.FeatureMap.Add(defaultMap);                                

                var defaultSquad = SquadEntity.Factory.Create("squad sample ", on, user, result);
                result.Squads.Add(defaultSquad);

                var defaultSource = SourceEntity.Factory.Create(defaultProduct, "sample source", on, user);
                defaultProduct.Sources.Add(defaultSource);

                var year = DateTime.Now.Year;
                var random = new System.Random();
                for (int i = 1; i < 13; i++)
                {
                    var defaultSourceItem = SourceItemEntity.Factory.Create(defaultSource,
                        new DateTime(year, i, 1), new DateTime(2019, i, 27), random.Next(800, 1000), 1000, on, user);
                    defaultSource.SourceItems.Add(defaultSourceItem);
                }

                var defaultIndicator = IndicatorEntity.Factory.Create(defaultFeature, defaultSource, on, user);

                defaultFeature.Indicators.Add(defaultIndicator);                

                return result;
            }
        }
    }
}

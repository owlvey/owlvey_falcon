using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity
    {

        public static class Factory{

            private static void DefaultValues(CustomerEntity result, DateTime on , string user) {
                var defaultProduct = ProductEntity.Factory.Create("Awesome Product", on, user, result);
                result.Products.Add(defaultProduct);                

                var defaultFeature = FeatureEntity.Factory.Create("Awesome Login", on, user, defaultProduct);
                defaultProduct.Features.Add(defaultFeature);

                var defaultRegistration = FeatureEntity.Factory.Create("Awesome Registration", on, user, defaultProduct);
                defaultProduct.Features.Add(defaultRegistration);

                var paymentFeature = FeatureEntity.Factory.Create("Awesome Payment", on, user, defaultProduct);
                defaultProduct.Features.Add(paymentFeature);

                var defaultService = ServiceEntity.Factory.Create("Awesome Onboarding", on, user, defaultProduct);
                defaultService.Slo = 0.999m;
                defaultProduct.Services.Add(defaultService);

                var defaultPaymentService = ServiceEntity.Factory.Create("Awesome Payment Service", on, user, defaultProduct);
                defaultPaymentService.Slo = 0.99m;
                defaultProduct.Services.Add(defaultPaymentService);

                var defaultMap = ServiceMapEntity.Factory.Create(defaultService, defaultFeature, on, user);
                defaultService.FeatureMap.Add(defaultMap);

                defaultMap = ServiceMapEntity.Factory.Create(defaultService, defaultRegistration, on, user);
                defaultService.FeatureMap.Add(defaultMap);

                defaultMap = ServiceMapEntity.Factory.Create(defaultPaymentService, paymentFeature, on, user);
                defaultPaymentService.FeatureMap.Add(defaultMap);

                var defaultSquad = SquadEntity.Factory.Create("Spartans", on, user, result);
                result.Squads.Add(defaultSquad);

                var defaultSquadFeature = SquadFeatureEntity.Factory.Create(defaultSquad, defaultFeature, on, user);
                defaultSquad.FeatureMaps.Add(defaultSquadFeature);

                defaultSquadFeature = SquadFeatureEntity.Factory.Create(defaultSquad, defaultRegistration, on, user);
                defaultSquad.FeatureMaps.Add(defaultSquadFeature);


                var AllBlacksSquad = SquadEntity.Factory.Create("All Blacks", on, user, result);
                result.Squads.Add(AllBlacksSquad);

                defaultSquadFeature = SquadFeatureEntity.Factory.Create(AllBlacksSquad, paymentFeature, on, user);
                AllBlacksSquad.FeatureMaps.Add(defaultSquadFeature);                

                var defaultSource = SourceEntity.Factory.Create(defaultProduct, "login requests", on, user, SourceKindEnum.Interaction, SourceGroupEnum.Availability);
                defaultProduct.Sources.Add(defaultSource);

                var registrationSource = SourceEntity.Factory.Create(defaultProduct, "registration requests", on, user, SourceKindEnum.Interaction, SourceGroupEnum.Availability);
                defaultProduct.Sources.Add(registrationSource);

                var paymentSource = SourceEntity.Factory.Create(defaultProduct, "payment requests", on, user, SourceKindEnum.Interaction, SourceGroupEnum.Availability);
                defaultProduct.Sources.Add(paymentSource);

                var year = on.Year;
                var random = new System.Random();
                for (int i = 1; i < 13; i++)
                {
                    var defaultSourceItem = SourceItemEntity.Factory.Create(defaultSource, new DateTime(year, i, 1), new DateTime(year, i, 27), random.Next(800, 1000), 1000, on, user);
                    defaultSource.SourceItems.Add(defaultSourceItem);

                    var registrationSourceItem = SourceItemEntity.Factory.Create(registrationSource, new DateTime(year, i, 1), new DateTime(year, i, 27), random.Next(800, 1000), 1000, on, user);
                    registrationSource.SourceItems.Add(registrationSourceItem);

                    var paymentSourceItem = SourceItemEntity.Factory.Create(paymentSource, new DateTime(year, i, 1), new DateTime(year, i, 27), random.Next(800, 1000), 1000, on, user);
                    paymentSource.SourceItems.Add(paymentSourceItem);
                }

                var defaultIndicator = IndicatorEntity.Factory.Create(defaultFeature, defaultSource, on, user);
                defaultFeature.Indicators.Add(defaultIndicator);

                var registrationIndicator = IndicatorEntity.Factory.Create(defaultRegistration, registrationSource, on, user);
                defaultRegistration.Indicators.Add(registrationIndicator);

                var paymentIndicator = IndicatorEntity.Factory.Create(paymentFeature, paymentSource, on, user);
                paymentFeature.Indicators.Add(paymentIndicator);

                var incident = IncidentEntity.Factory.Create("AWC01","Awful incident", on, user, defaultProduct);
                defaultProduct.Incidents.Add(incident);

                var incidentMap = IncidentMapEntity.Factory.Create(on, user, defaultFeature, incident);
                defaultFeature.IncidentMap.Add(incidentMap);
            }

            public static CustomerEntity Create(string user, DateTime on, 
                string name,
                string avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png",
                bool defaultValue = true)
            {
                var result = new CustomerEntity
                {
                    Name = name,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,                    
                    Avatar = avatar                    
                };
                result.Validate();
                if (defaultValue) {
                    DefaultValues(result, on, user);
                }                

                return result;
            }
        }
    }
}

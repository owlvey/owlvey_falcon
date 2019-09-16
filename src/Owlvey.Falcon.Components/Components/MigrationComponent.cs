﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OfficeOpenXml;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class MigrationComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly SquadComponent _squadComponent;
        private readonly ProductComponent _productComponent;
        private readonly ServiceComponent _serviceComponent;
        private readonly SquadQueryComponent _squadQueryComponent;
        private readonly SourceComponent _sourceComponent;
        private readonly FeatureComponent _featureComponent;
        private readonly ServiceMapComponent _serviceMapComponent;
        private readonly IndicatorComponent _indicatorComponent;
        public MigrationComponent(FalconDbContext dbContext,
            ProductComponent productComponent,
            ServiceComponent serviceComponent,
            FeatureComponent featureComponent,
            SourceComponent sourceComponent,
            ServiceMapComponent serviceMapComponent,
            IndicatorComponent indicatorComponent,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper,
            SquadComponent squadComponent, SquadQueryComponent squadQueryComponent) : base(dateTimeGateway, mapper, identityService)
        {
            this._sourceComponent = sourceComponent;
            this._dbContext = dbContext;
            this._squadComponent = squadComponent;
            this._squadQueryComponent = squadQueryComponent;
            this._productComponent = productComponent;
            this._serviceComponent = serviceComponent;
            this._featureComponent = featureComponent;
            this._serviceMapComponent = serviceMapComponent;
            this._indicatorComponent = indicatorComponent;
        }


        #region exports

        public async Task<List<string>> ImportMetadata(int customerId, Stream input)
        {
            var logs = new List<string>(); 

            var customer = await this._dbContext.Customers.Where(c => c.Id == customerId).SingleAsync();
            var squads = await this._dbContext.Squads.Where(c => c.CustomerId == customerId).ToListAsync();

            using (var package = new ExcelPackage(input))
            {
                var squadSheet = package.Workbook.Worksheets["Squads"];

                for (int row = 2; row <= squadSheet.Dimension.Rows; row++)
                {                    
                    var name = squadSheet.Cells[row, 1].GetValue<string>();
                    var description = squadSheet.Cells[row, 2].GetValue<string>();
                    var avatar = squadSheet.Cells[row, 3].GetValue<string>();
                    logs.Add(" add update " + name);
                    await this._squadComponent.CreateOrUpdate(customer, name, description, avatar);
                }

                var productSheet = package.Workbook.Worksheets["Products"];

                for (int row = 2; row <= productSheet.Dimension.Rows; row++)
                {
                    var name = productSheet.Cells[row, 1].GetValue<string>();
                    var description = productSheet.Cells[row, 2].GetValue<string>();
                    var avatar = productSheet.Cells[row, 3].GetValue<string>();
                    await this._productComponent.CreateOrUpdate(customer, name, description, avatar);
                }

                var serviceSheet = package.Workbook.Worksheets["Services"];

                for (int row = 2; row <= serviceSheet.Dimension.Rows; row++)
                {
                    var name = serviceSheet.Cells[row, 1].GetValue<string>();
                    var description = serviceSheet.Cells[row, 2].GetValue<string>();
                    var avatar = serviceSheet.Cells[row, 3].GetValue<string>();
                    var slo = serviceSheet.Cells[row, 4].GetValue<decimal>();
                    var product = serviceSheet.Cells[row, 5].GetValue<string>();
                    await this._serviceComponent.CreateOrUpdate(customer, product, name, description, avatar, slo);
                }

                var featureSheet = package.Workbook.Worksheets["Features"];
                
                for (int row = 2; row <= featureSheet.Dimension.Rows; row++)
                {
                    var name = featureSheet.Cells[row, 1].GetValue<string>();
                    var description = featureSheet.Cells[row, 2].GetValue<string>();
                    var avatar = featureSheet.Cells[row, 3].GetValue<string>();                    
                    var product = featureSheet.Cells[row, 4].GetValue<string>();
                    await this._featureComponent.CreateOrUpdate(customer, product, name, description, avatar);
                }

                var sourceSheet = package.Workbook.Worksheets["Sources"];                
                for (int row = 2; row <= sourceSheet.Dimension.Rows; row++)
                {
                    var name = sourceSheet.Cells[row, 1].GetValue<string>();
                    var tags = sourceSheet.Cells[row, 2].GetValue<string>();
                    var avatar = sourceSheet.Cells[row, 3].GetValue<string>();
                    var good = sourceSheet.Cells[row, 4].GetValue<string>();
                    var total = sourceSheet.Cells[row, 5].GetValue<string>();
                    var product = sourceSheet.Cells[row, 6].GetValue<string>();
                    await this._sourceComponent.CreateOrUpdate(customer, product, name, tags, avatar, good, total);
                }

                var serviceMapSheet = package.Workbook.Worksheets["ServicesMap"];
                for (int row = 2; row <= serviceMapSheet.Dimension.Rows; row++)
                {
                    var product = serviceMapSheet.Cells[row, 1].GetValue<string>();
                    var service = serviceMapSheet.Cells[row, 2].GetValue<string>();
                    var feature = serviceMapSheet.Cells[row, 3].GetValue<string>();
                    await this._serviceMapComponent.CreateServiceMap(customerId, product, service, feature);                    
                }

                var indicatorSheet = package.Workbook.Worksheets["Indicators"];
                for (int row = 2; row <= serviceMapSheet.Dimension.Rows; row++)
                {                    
                    var product = indicatorSheet.Cells[row, 1].GetValue<string>();
                    var source = indicatorSheet.Cells[row, 2].GetValue<string>();
                    var feature = indicatorSheet.Cells[row, 3].GetValue<string>();
                    await this._indicatorComponent.Create(customerId, product, source, feature);                    
                }               
            }
            return logs;
        }

        public async Task<(CustomerEntity entity, MemoryStream stream)> ExportMetadataExcel(int customerId)
        {

            var customer = await this._dbContext.Customers.Include(c => c.Products).ThenInclude(c => c.Services).Where(c => c.Id == customerId).SingleAsync();
            var squads = await this._dbContext.Squads.Include(c => c.Members).ThenInclude(c => c.User).Where(c => c.CustomerId == customerId).ToListAsync();
            var squadLites = this._mapper.Map<IEnumerable<SquadMigrationRp>>(squads);
            var productLites = this._mapper.Map<IEnumerable<ProductMigrationRp>>(customer.Products);
            var serviceLites = this._mapper.Map<IEnumerable<ServiceMigrateRp>>(customer.Products.SelectMany(c => c.Services));

            var memberLites = new List<MemberMigrateRp>();
            foreach (var squad in squads)
            {
                foreach (var item in squad.Members)
                {
                    memberLites.Add(new MemberMigrateRp()
                    {
                        Email = item.User.Email,
                        Squad = squad.Name
                    });
                }
            }

            var features = await this._dbContext.Features.Include(c=>c.Product).Include(c => c.ServiceMaps).ThenInclude(c => c.Service).Where(c => c.Product.CustomerId == customerId).ToListAsync();
            var featureLites = new List<FeatureMigrateRp>();
            var serviceMapLites = new List<ServiceMapMigrateRp>();

            foreach (var item in features)
            {
                var tmp = this._mapper.Map<FeatureMigrateRp>(item);
                featureLites.Add(tmp);

                foreach (var serv in item.ServiceMaps)
                {
                    serviceMapLites.Add(new ServiceMapMigrateRp()
                    {
                        Product = item.Product.Name,
                        Feature = item.Name,
                        Service = serv.Service.Name
                    });
                }
            }

            var sources = await this._dbContext.Sources.Include(c=>c.Product).Include(c => c.Indicators).ThenInclude(c => c.Feature).Where(c => c.Product.CustomerId == customerId).ToListAsync();
            var sourceLites = this._mapper.Map<IEnumerable<SourceMigrateRp>>(sources);

            var indicatorLites = new List<IndicatorMigrateRp>();

            foreach (var item in sources)
            {
                foreach (var indicator in item.Indicators)
                {
                    indicatorLites.Add(new IndicatorMigrateRp()
                    {
                        Product = item.Product.Name,
                        Feature = indicator.Feature.Name,
                        Source = item.Name
                    });
                }
            }

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var squadSheet = package.Workbook.Worksheets.Add("Squads");
                squadSheet.Cells.LoadFromCollection(squadLites, true);

                var membersSheet = package.Workbook.Worksheets.Add("Members");
                membersSheet.Cells.LoadFromCollection(memberLites, true);

                var productSheet = package.Workbook.Worksheets.Add("Products");
                productSheet.Cells.LoadFromCollection(productLites, true);

                var servicesSheet = package.Workbook.Worksheets.Add("Services");
                servicesSheet.Cells.LoadFromCollection(serviceLites, true);

                var servicesMapSheet = package.Workbook.Worksheets.Add("ServicesMap");
                servicesMapSheet.Cells.LoadFromCollection(serviceMapLites, true);

                var featuresSheet = package.Workbook.Worksheets.Add("Features");
                featuresSheet.Cells.LoadFromCollection(featureLites, true);

                var indicatorsSheet = package.Workbook.Worksheets.Add("Indicators");
                indicatorsSheet.Cells.LoadFromCollection(indicatorLites, true);

                var sourcesSheet = package.Workbook.Worksheets.Add("Sources");
                sourcesSheet.Cells.LoadFromCollection(sourceLites, true);

                package.Save();
            }
            stream.Position = 0;
            return (customer, stream);
        }

        #endregion

    }
}

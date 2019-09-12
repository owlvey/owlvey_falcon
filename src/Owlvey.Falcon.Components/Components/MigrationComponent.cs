using AutoMapper;
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
        private readonly SquadQueryComponent _squadQueryComponent;
        public MigrationComponent(FalconDbContext dbContext, 
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper,
            SquadComponent squadComponent, SquadQueryComponent squadQueryComponent) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
            this._squadComponent = squadComponent;
            this._squadQueryComponent = squadQueryComponent;

        }


        #region exports

        public async Task ImportMetadata(int customerId, Stream input)
        {
            var customer = await this._dbContext.Customers.Where(c => c.Id == customerId).SingleAsync();
            var squads = await this._dbContext.Squads.Where(c => c.CustomerId == customerId).ToListAsync();

            using (var package = new ExcelPackage(input))
            {
                var squadSheet = package.Workbook.Worksheets["Squads"];

                for (int row = 2; row < squadSheet.Dimension.Rows; row++)
                {                    
                    var name = squadSheet.Cells[row, 1].GetValue<string>();
                    var description = squadSheet.Cells[row, 2].GetValue<string>();
                    var avatar = squadSheet.Cells[row, 3].GetValue<string>();
                    await this._squadComponent.CreateOrUpdate(customer, name, description, avatar);
                }
            }            
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

            var features = await this._dbContext.Features.Include(c => c.ServiceMaps).ThenInclude(c => c.Service).Where(c => c.Product.CustomerId == customerId).ToListAsync();
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
                        Feature = item.Name,
                        Service = serv.Service.Name
                    });
                }
            }

            var sources = await this._dbContext.Sources.Include(c => c.Indicators).ThenInclude(c => c.Feature).Where(c => c.Product.CustomerId == customerId).ToListAsync();
            var sourceLites = this._mapper.Map<IEnumerable<SourceMigrateRp>>(sources);

            var indicatorLites = new List<IndicatorMigrateRp>();

            foreach (var item in sources)
            {
                foreach (var indicator in item.Indicators)
                {
                    indicatorLites.Add(new IndicatorMigrateRp()
                    {
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

using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Repositories.Products;
using Owlvey.Falcon.Repositories.Features;
using Owlvey.Falcon.Repositories.Services;
using Polly;
using System.IO;
using OfficeOpenXml;
using Owlvey.Falcon.Core.Models.Migrate;
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Builders;
using Owlvey.Falcon.Core.Entities.Source;

namespace Owlvey.Falcon.Components
{
    public class ProductComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly SourceItemComponent _sourceItemComponent;
        private readonly SourceComponent _sourceComponent;

        public ProductComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, 
            IMapper mapper, ConfigurationComponent configuration,
            SourceItemComponent sourceItemComponent,
            SourceComponent sourceComponent) : base(dateTimeGateway, mapper, identityService, configuration)
        {
            this._sourceItemComponent = sourceItemComponent;
            this._dbContext = dbContext;
            this._sourceComponent = sourceComponent;
        }

        public async Task<AnchorRp> PostAnchor(int productId, string name)
        {
            var createdBy = this._identityService.GetIdentity();
            var entity = await this._dbContext.Anchors.Where(c => c.ProductId == productId && c.Name == name).SingleOrDefaultAsync();
            if (entity == null) {
                var product = await this._dbContext.Products.Where(c => c.Id == productId).SingleAsync();
                entity = AnchorEntity.Factory.Create(name, this._datetimeGateway.GetCurrentDateTime(), createdBy, product);
                this._dbContext.Anchors.Add(entity);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<AnchorRp>(entity);
        }


        

        public async Task<AnchorRp> CreateOrUpdateAnchor(int productId, string name, DateTime target) {
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var createdBy = this._identityService.GetIdentity();
            var entity = await this._dbContext.Anchors.Where(c => c.ProductId == productId && c.Name == name).SingleOrDefaultAsync();
            if (entity == null)
            {
                var product = await this._dbContext.Products.Where(c => c.Id == productId).SingleAsync();
                entity = AnchorEntity.Factory.Create(name, this._datetimeGateway.GetCurrentDateTime(), createdBy, product);                
                this._dbContext.Anchors.Add(entity);
                await this._dbContext.SaveChangesAsync();
            }
            entity.Update(target, this._datetimeGateway.GetCurrentDateTime(), createdBy);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<AnchorRp>(entity);

        }

        public async Task<AnchorRp> DeleteAnchor(int productId, string name)
        {
            var createdBy = this._identityService.GetIdentity();
            var entity = await this._dbContext.Anchors.Where(c => c.ProductId == productId && c.Name == name).SingleOrDefaultAsync();
            if (entity != null)
            {
                this._dbContext.Anchors.Remove(entity);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<AnchorRp>(entity);
        }

        public async Task PutAnchor(int productId, string name, AnchorPutRp model)
        {
            var createdBy = this._identityService.GetIdentity();
            var entity = await this._dbContext.Anchors.Where(c => c.ProductId == productId && c.Name == name).SingleAsync();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            entity.Update(model.Target, this._datetimeGateway.GetCurrentDateTime(), createdBy);
            this._dbContext.Anchors.Update(entity);
            await this._dbContext.SaveChangesAsync();
        }
               
        public async Task<ProductGetListItemRp> CreateOrUpdate(CustomerEntity customer, string name, string description, 
            string avatar, string leaders) {
            var createdBy = this._identityService.GetIdentity();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var entity = await this._dbContext.Products.Where(c => c.CustomerId == customer.Id && c.Name == name).SingleOrDefaultAsync();
            if (entity == null)
            {
                entity = ProductEntity.Factory.Create(name, this._datetimeGateway.GetCurrentDateTime(), createdBy, customer);
            }
            entity.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, name, description, avatar, leaders);
            this._dbContext.Products.Update(entity);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<ProductGetListItemRp>(entity);
        }

        public async Task<ProductGetListItemRp> CreateProduct(ProductPostRp model)
        {            
            var createdBy = this._identityService.GetIdentity();

            var retryPolicy = Policy.Handle<DbUpdateException>()
                .WaitAndRetryAsync(this._configuration.DefaultRetryAttempts,
                i => this._configuration.DefaultPauseBetweenFails);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var entity = await this._dbContext.GetProduct(model.CustomerId, model.Name);
                if (entity == null)
                {
                    var customer = await this._dbContext.Customers.SingleAsync(c => c.Id == model.CustomerId);
                    entity = ProductEntity.Factory.Create(model.Name,
                        this._datetimeGateway.GetCurrentDateTime(),
                        createdBy, customer);
                    this._dbContext.Products.Add(entity);
                    await this._dbContext.SaveChangesAsync();
                }
                return this._mapper.Map<ProductGetListItemRp>(entity);
            });
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="key">Product Id</param>
        /// <returns></returns>
        public async Task DeleteProduct(int id)
        {
            var result = new BaseComponentResultRp();
            var modifiedBy = this._identityService.GetIdentity();

            var product = await this._dbContext.Products
                .Include(c=>c.Services)
                .Include(c=>c.Features)
                .Include(c=>c.Incidents)
                .Include(c=>c.Sources)
                .SingleAsync(c => c.Id == id);

            var squads = await this._dbContext.Squads
                .Include(c=>c.FeatureMaps)
                .Where(c => c.CustomerId == product.CustomerId).ToListAsync();

            

            if (product != null)
            {

                foreach (var service in product.Services.Select(c=>c.Id.Value).ToList())
                {
                    await this._dbContext.RemoveService(service);                    
                }

                await this._dbContext.SaveChangesAsync();                

                foreach (var feature in product.Features.Select(c=>c.Id.Value).ToList())
                {
                    await this._dbContext.RemoveFeature(feature);                    
                }
                
                await this._dbContext.SaveChangesAsync();                                    

                this._dbContext.Products.Remove(product);
                await this._dbContext.SaveChangesAsync();
            }
            
        }


        public async Task<IEnumerable<string>> ImportsItems(int productId, MemoryStream input)
        {

            var createdOn = this._datetimeGateway.GetCurrentDateTime();
            var createdBy = this._identityService.GetIdentity();
            var product = await this._dbContext.Products.Include(c=>c.Customer)
                .Where(c => c.Id == productId).SingleAsync();

            var logs = new List<string>();

            var sources =  this._dbContext.Sources.Where(c => c.ProductId == productId).ToList();

            var productInstance = await this._dbContext.Products.Include( c=>c.Customer)
                    .Where(e => e.Id == productId).SingleAsync();

            using (var package = new ExcelPackage(input))
            {
                var sourceSheet = package.Workbook.Worksheets["Sources"];
                var sourcesInstances = SourceLiteModel.Build(productInstance, createdOn, createdBy, 
                new  Builders.SheetRowAdapter( sourceSheet));                
                foreach (var item in sourcesInstances){
                    if (!sources.Exists(c=>c.Name == item.Name)) {                        
                        this._dbContext.Sources.Add(item);                        
                    }   
                }
                await this._dbContext.SaveChangesAsync();
                productInstance.Sources = new SourceCollection(await this._dbContext.Sources.Where(c=>c.ProductId == productId).ToListAsync());
                                  
                var sourceItemsSheet = package.Workbook.Worksheets["SourceItems"];
                var sourceItems = SourceItemLiteModel.Build(productInstance, createdOn, createdBy, 
                    new SheetRowAdapter(sourceItemsSheet));
                
                await this._sourceItemComponent.BulkInsert(sourceItems);                
            }

            return logs;
        }


        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="model">Product Model</param>
        /// <returns></returns>
        public async Task<ProductGetRp> UpdateProduct(int id, ProductPutRp model)
        {
            
            var createdBy = this._identityService.GetIdentity();

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var product = await this._dbContext.Products.Include(c=> c.Customer).SingleAsync(c => c.Id == id);

            if (product != null)
            {
                product.Update(this._datetimeGateway.GetCurrentDateTime(),
                createdBy, name: model.Name,
                description: model.Description,
                avatar: model.Avatar, leaders: model.Leaders);
                this._dbContext.Update(product);
                await this._dbContext.SaveChangesAsync();
            }

            return this._mapper.Map<ProductGetRp>(product);
        }
    }
}

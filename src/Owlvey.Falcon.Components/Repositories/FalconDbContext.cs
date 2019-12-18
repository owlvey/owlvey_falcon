using Owlvey.Falcon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Owlvey.Falcon.Core;
using System.Threading;

namespace Owlvey.Falcon.Repositories
{
    public class FalconDbContext : DbContext
    {
        public FalconDbContext(DbContextOptions options) :
           base(options)
        {

        }



        
        public DbSet<AppSettingEntity> AppSettings { get; set; }
        public DbSet<SquadEntity> Squads { get; set; }
        public DbSet<SquadFeatureEntity> SquadFeatures { get; set; }        

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<ServiceMapEntity> ServiceMaps { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<FeatureEntity> Features { get; set; }

        public DbSet<SourceEntity> Sources { get; set; }

        public DbSet<SourceItemEntity> SourcesItems { get; set; }

        public DbSet<IndicatorEntity> Indicators { get; set; }

        public DbSet<IncidentEntity> Incidents { get; set; }

        public DbSet<IncidentMapEntity> IncidentMaps { get; set; }
        
        public DbSet<MemberEntity> Members { get; set; }

        public DbSet<AnchorEntity> Anchors { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }

            // Customer Name Unique
            modelBuilder.Entity<SourceItemEntity>().HasIndex(p => p.Start);
            modelBuilder.Entity<SourceItemEntity>().HasIndex(p => p.End);
            modelBuilder.Entity<CustomerEntity>().HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<SquadFeatureEntity>().HasKey(x => new { x.Id });

            
            modelBuilder.Entity<SquadFeatureEntity>()
               .HasOne(pt => pt.Squad)
               .WithMany(p => p.FeatureMaps)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(pt => pt.SquadId);

            modelBuilder.Entity<SquadFeatureEntity>()
               .HasOne(pt => pt.Feature)
               .WithMany(p => p.Squads)
               .OnDelete(DeleteBehavior.Cascade)
               .HasForeignKey(pt => pt.FeatureId);
            

            modelBuilder.Entity<IncidentMapEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<IncidentMapEntity>()
               .HasOne(pt => pt.Feature)
               .WithMany(p => p.IncidentMap)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(pt => pt.FeatureId);

            modelBuilder.Entity<IncidentMapEntity>()
               .HasOne(pt => pt.Incident)
               .WithMany(p => p.FeatureMaps)
               .OnDelete(DeleteBehavior.Cascade)
               .HasForeignKey(pt => pt.IncidentId);

            modelBuilder.Entity<ServiceMapEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<ServiceMapEntity>()
               .HasOne(pt => pt.Feature)
               .WithMany(p => p.ServiceMaps)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(pt => pt.FeatureId);

            modelBuilder.Entity<ServiceMapEntity>()
               .HasOne(pt => pt.Service)
               .WithMany(p => p.FeatureMap)
               .OnDelete(DeleteBehavior.Cascade)
               .HasForeignKey(pt => pt.ServiceId);

            modelBuilder.Entity<IndicatorEntity>().HasKey(x => new { x.Id });
            
            modelBuilder.Entity<IndicatorEntity>()
               .HasOne(pt => pt.Source)
               .WithMany(p => p.Indicators)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(pt => pt.SourceId);

            modelBuilder.Entity<IndicatorEntity>()
               .HasOne(pt => pt.Feature)
               .WithMany(p => p.Indicators)
               .OnDelete(DeleteBehavior.Cascade)
               .HasForeignKey(pt => pt.FeatureId);        

            base.OnModelCreating(modelBuilder);
        }                       

        

        private void AssignLastModified() {
            this.ChangeTracker.AutoDetectChangesEnabled = true;
            var setting = this.AppSettings.SingleOrDefault(c => c.Key == AppSettingEntity.AppLastModifiedVersion);
            var value = Guid.NewGuid().ToString();
            if (setting != null)
            {

                setting.Value = value;
            }
            else
            {
                setting = AppSettingEntity.Factory.Create("AppLastModifiedVersion", value, true, DateTime.Now, "system");
                this.AppSettings.Add(setting);
            }
        }
        public override int SaveChanges()
        {
            this.AssignLastModified();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {            
            this.AssignLastModified();
            return base.SaveChangesAsync(cancellationToken);
        }

        internal async Task<ICollection<SourceItemEntity>> GetSourceItems(int sourceId,
            DateTime start, DateTime end) {
            start = start.Date;
            end = end.Date;
            List<SourceItemEntity> result = new List<SourceItemEntity>();

            var startTask = await this.SourcesItems.Where(c => c.SourceId == sourceId && c.Start >= start && c.Start <= end).ToListAsync();
            var endTask = await this.SourcesItems.Where(c => c.SourceId == sourceId && c.End >= start && c.End <= end).ToListAsync();
            var midTask = await this.SourcesItems.Where(c => c.SourceId == sourceId && c.Start >= start && c.End <= end).ToListAsync();
            var involveTask = await this.SourcesItems.Where(c => c.SourceId == sourceId && start >= c.Start && end <= c.End).ToListAsync();

            // Task.WaitAll(startTask, endTask, midTask, involveTask);
            result =  result.Union(startTask.Union(endTask).Union(midTask).Union(involveTask).Distinct(new SourceItemEntityComparer())).ToList();                        
            return result;
        }

        internal ICollection<SourceItemEntity> GetSourceItems(DateTime start, DateTime end)
        {
            start = start.Date;
            end = end.Date;
            List<SourceItemEntity> result = new List<SourceItemEntity>();
            var startTask = this.SourcesItems.Where(c => c.Start >= start && c.Start <= end).ToListAsync();
            var endTask = this.SourcesItems.Where(c =>  c.End >= start && c.End <= end).ToListAsync();
            var midTask = this.SourcesItems.Where(c =>  c.Start >= start && c.End <= end).ToListAsync();
            var involveTask = this.SourcesItems.Where(c => start >= c.Start && end <= c.End).ToListAsync();

            Task.WaitAll(startTask, endTask, midTask, involveTask);
            result = result.Union(startTask.Result.Union(endTask.Result).Union(midTask.Result).Union(involveTask.Result).Distinct(new SourceItemEntityComparer())).ToList();
            return result;
        }


        internal ICollection<SourceItemEntity> GetSourceItemsByProduct(IEnumerable<int> productIds, DateTime start, DateTime end)
        {
            start = start.Date;
            end = end.Date;
            List<SourceItemEntity> result = new List<SourceItemEntity>();
            var startTask = this.SourcesItems.Where(c => productIds.Contains(c.Source.ProductId) && c.Start >= start && c.Start <= end).ToListAsync();
            var endTask = this.SourcesItems.Where(c => productIds.Contains(c.Source.ProductId) && c.End >= start && c.End <= end).ToListAsync();
            var midTask = this.SourcesItems.Where(c => productIds.Contains(c.Source.ProductId) && c.Start >= start && c.End <= end).ToListAsync();
            var involveTask = this.SourcesItems.Where(c => productIds.Contains(c.Source.ProductId) && start >= c.Start && end <= c.End).ToListAsync();

            Task.WaitAll(startTask, endTask, midTask, involveTask);
            result = result.Union(startTask.Result.Union(endTask.Result).Union(midTask.Result).Union(involveTask.Result).Distinct(new SourceItemEntityComparer())).ToList();
            return result;
        }

        internal ICollection<SourceItemEntity> GetSourceItemsByProduct(int productId, DateTime start, DateTime end)
        {
            return this.GetSourceItemsByProduct(new List<int>() { productId }, start, end);
        }

        internal async Task LoadIndicators(IEnumerable<ServiceMapEntity> serviceMaps) {
            var sourceIds = serviceMaps.SelectMany(c => c.Feature.Indicators.Select(d => d.SourceId)).Distinct().ToList();
            var sources = await this.Sources.Where(c => sourceIds.Contains(c.Id.Value)).ToListAsync();
            foreach (var item in serviceMaps)
            {
                foreach (var indicator in item.Feature.Indicators)
                {
                    var source = sources.Single(c => c.Id == indicator.SourceId);
                    indicator.Source = source; 
                }
            }            
        }      
    }
}

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
using Prometheus;

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
        public DbSet<JourneyEntity> Journeys { get; set; }
        public DbSet<JourneyMapEntity> JourneyMaps { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<FeatureEntity> Features { get; set; }

        public DbSet<SourceEntity> Sources { get; set; }        

        public DbSet<SourceItemEntity> SourcesItems { get; set; }

        public DbSet<IndicatorEntity> Indicators { get; set; }

        public DbSet<IncidentEntity> Incidents { get; set; }

        public DbSet<IncidentMapEntity> IncidentMaps { get; set; }
        
        public DbSet<MemberEntity> Members { get; set; }

        public DbSet<AnchorEntity> Anchors { get; set; } 

        public DbSet<ClueEntity> Clues { get; set; }

        #region Security  Risk

        public DbSet<SecurityRiskEntity> SecurityRisks { get; set; }
        public DbSet<SecurityThreatEntity> SecurityThreats { get; set; }

        #endregion

        #region Reliability Risk

        public DbSet<ReliabilityRiskEntity> ReliabilityRisks { get; set; }
        public DbSet<ReliabilityThreatEntity> ReliabilityThreats { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }

            // Customer Name Unique
            modelBuilder.Entity<SourceItemEntity>().HasIndex(p => p.Target);

            //modelBuilder.Entity<SourceItemEntity>()
            //    .HasDiscriminator<SourceKindEnum>("Kind")
            //    .HasValue<SourceItemEntity>(SourceKindEnum.Interaction)
            //    .HasValue<SourceItemEntity>(SourceKindEnum.Proportion);

            modelBuilder.Entity<CustomerEntity>().HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<SquadFeatureEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<ProductEntity>().HasIndex(c => new { c.CustomerId, c.Name }).IsUnique();

            modelBuilder.Entity<JourneyEntity>()
                .Property(c => c.AvailabilitySlo).HasColumnType("decimal(5,3)");
            modelBuilder.Entity<JourneyEntity>()
                .Property(c => c.LatencySlo).HasColumnType("decimal(12,3)");
            modelBuilder.Entity<JourneyEntity>()
                .Property(c => c.ExperienceSlo).HasColumnType("decimal(5,3)");


            modelBuilder.Entity<JourneyEntity>()                
                .HasIndex(c => new { c.ProductId, c.Name }).IsUnique();
            modelBuilder.Entity<FeatureEntity>().HasIndex(c => new { c.ProductId, c.Name }).IsUnique();

            //modelBuilder.Entity<SourceEntity>()
            //    .HasDiscriminator<SourceKindEnum>("Kind")
            //    .HasValue<InteractionSourceEntity>(SourceKindEnum.Interaction)
            //    .HasValue<ProportionSourceEntity>(SourceKindEnum.Proportion);

            modelBuilder.Entity<SourceEntity>().HasIndex(c => new { c.ProductId, c.Name }).IsUnique();

            //modelBuilder.Entity<ProportionSourceEntity>().HasBaseType<SourceEntity>();
            //modelBuilder.Entity<InteractionSourceEntity>().HasBaseType<SourceEntity>();

            modelBuilder.Entity<SquadEntity>().HasIndex(c => new { c.CustomerId, c.Name }).IsUnique();
            
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

            modelBuilder.Entity<JourneyMapEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<JourneyMapEntity>()
               .HasOne(pt => pt.Feature)
               .WithMany(p => p.JourneyMaps)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(pt => pt.FeatureId);

            modelBuilder.Entity<JourneyMapEntity>()
               .HasOne(pt => pt.Journey)
               .WithMany(p => p.FeatureMap)
               .OnDelete(DeleteBehavior.Cascade)
               .HasForeignKey(pt => pt.JourneyId);


            modelBuilder.Entity<JourneyMapEntity>().HasIndex(c => new { c.JourneyId, c.FeatureId }).IsUnique();

            modelBuilder.Entity<IndicatorEntity>().HasKey(x => new { x.Id });

            modelBuilder.Entity<IndicatorEntity>().HasIndex(x => new { x.FeatureId, x.SourceId }).IsUnique();

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
        
        public override int SaveChanges()
        {            
             return base.SaveChanges();                            
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {       
            return base.SaveChangesAsync(cancellationToken);        
        }

        public async Task<ICollection<SourceItemEntity>> GetSourceItems(int sourceId,
            DateTime start, DateTime end) {            
            List<SourceItemEntity> result = new List<SourceItemEntity>();
            result = await this.SourcesItems.Where(c => c.SourceId == sourceId && c.Target >= start && c.Target <= end).ToListAsync();                        
            return result;
        }
        public async Task<ICollection<SourceItemEntity>> GetSourceItems(int sourceId, SourceGroupEnum group,
            DateTime start, DateTime end)
        {            
            List<SourceItemEntity> result = new List<SourceItemEntity>();
            result = await this.SourcesItems.Where(c => c.SourceId == sourceId && c.Group == group &&  c.Target >= start && c.Target <= end).ToListAsync();
            return result;
        }

        public async Task<ICollection<SourceItemEntity>> GetSourceItems(IEnumerable<int> sources,
            DateTime start, DateTime end)
        {            
            List<SourceItemEntity> result = new List<SourceItemEntity>();
            result = await this.SourcesItems.Where(c => sources.Contains(c.SourceId) && c.Target >= start && c.Target <= end).ToListAsync();            
            return result;
        }

        public async Task<ICollection<SourceItemEntity>> GetSourceItems(DateTime start, DateTime end)
        {            
            List<SourceItemEntity> result = new List<SourceItemEntity>();
            result = await this.SourcesItems.Where(c=>c.Target >= start && c.Target <= end).ToListAsync();
            return result;
        }


        public async Task<ICollection<SourceItemEntity>> GetSourceItemsByProduct(IEnumerable<int> productIds, DateTime start, DateTime end)
        {            
            List<SourceItemEntity> result = new List<SourceItemEntity>();
            result = await this.SourcesItems.Where(c => productIds.Contains(c.Source.ProductId) && c.Target >= start && c.Target <= end).ToListAsync();
            return result;
        }

        public async Task<ICollection<SourceItemEntity>> GetSourceItemsByProduct(int productId, DateTime start, DateTime end)
        {
            return await this.GetSourceItemsByProduct(new List<int>() { productId }, start, end);
        }

        public async Task LoadIndicators(IEnumerable<JourneyMapEntity> journeyMaps) {
            var sourceIds = journeyMaps.SelectMany(c => c.Feature.Indicators.Select(d => d.SourceId)).Distinct().ToList();
            var sources = await this.Sources.Where(c => sourceIds.Contains(c.Id.Value)).ToListAsync();
            foreach (var item in journeyMaps)
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

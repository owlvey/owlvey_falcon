using Owlvey.Falcon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Owlvey.Falcon.Core;

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
        public DbSet<MemberEntity> Members { get; set; }
 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            // Customer Name Unique
            modelBuilder.Entity<SourceItemEntity>().HasIndex(p => p.Start);
            modelBuilder.Entity<SourceItemEntity>().HasIndex(p => p.End);
            modelBuilder.Entity<CustomerEntity>().HasIndex(c => c.Name).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
                
        internal async Task<ICollection<SourceItemEntity>> GetSourceItems(int sourceId, DateTime start, DateTime end) {
            //var result = await this.SourcesItems.Where(c => c.SourceId == sourceId && (c.Start >= start && c.Start <= end) || c.End >= start && c.End <= end).ToListAsync();
            List<SourceItemEntity> result = new List<SourceItemEntity>();
            var lastTask = this.SourcesItems.Where(c => c.SourceId == sourceId && c.End <= end).OrderByDescending(c => c.End).FirstOrDefaultAsync();
            //
            if (DateTimeUtils.CompareDates(start, end))
            {
                end = DateTimeUtils.DatetimeToMidDate(end);
                var startTask = this.SourcesItems.Where(c => c.SourceId == sourceId &&  end >= c.Start  && end <= c.End).ToListAsync();
                Task.WaitAll(startTask);
                result = startTask.Result;
            }
            else {
                var startTask = this.SourcesItems.Where(c => c.SourceId == sourceId && c.Start >= start && c.Start <= end).ToListAsync();
                var endTask = this.SourcesItems.Where(c => c.SourceId == sourceId && c.End >= start && c.End <= end).ToListAsync();
                var midTask = this.SourcesItems.Where(c => c.SourceId == sourceId && c.Start >= start && c.End <= end).ToListAsync();
                Task.WaitAll(startTask, endTask, midTask, lastTask);
                result = startTask.Result.Union(endTask.Result).Union(midTask.Result).Distinct(new SourceItemEntity.EqualityComparer()).ToList();
            }
            //if no data , try to recreate information from the last information
            if (result.Count == 0) {
                var tmpEnd = lastTask.Result;
                if (tmpEnd != null) {
                    result = await this.SourcesItems.Where(c => c.SourceId == sourceId && c.End == tmpEnd.End).ToListAsync();                    
                    foreach (var item in result)
                    {
                        item.Id = int.MaxValue;                        
                        item.Start = end;
                        item.End = end;                           
                    }                                       
                }                
            }
            return result;
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

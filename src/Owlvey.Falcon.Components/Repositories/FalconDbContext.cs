using Owlvey.Falcon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            base.OnModelCreating(modelBuilder);
        }
    }
}

//using Owlvey.Falcon.Data.SQLite.Extensions;
//using Owlvey.Falcon.Core.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace Owlvey.Falcon.Data.SQLite.Context
//{
//    public class FalconDbContext : DbContext
//    {
//        public FalconDbContext(DbContextOptions options) :
//           base(options)
//        {

//        }
        
//        public DbSet<AppSettingEntity> AppSettings { get; set; }
//        public DbSet<SquadEntity> Squads { get; set; }
//        public DbSet<CustomerEntity> Customers { get; set; }
//        public DbSet<ProductEntity> Products { get; set; }
//        public DbSet<ServiceEntity> Services { get; set; }
//        public DbSet<FeatureEntity> Features { get; set; }
//        public DbSet<SourceEntity> Journals { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.RemovePluralizingTableNameConvention();

//            base.OnModelCreating(modelBuilder);
//        }
//    }
//}

using Owlvey.Falcon.Data.SQLite.Extensions;
using Owlvey.Falcon.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Owlvey.Falcon.Data.SQLite.Context
{
    public class FalconDbContext : DbContext
    {
        public FalconDbContext(DbContextOptions options) :
           base(options)
        {

        }
        
        public DbSet<AppSettingEntity> AppSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            base.OnModelCreating(modelBuilder);
        }
    }
}

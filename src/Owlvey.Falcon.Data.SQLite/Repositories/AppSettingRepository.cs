using Owlvey.Falcon.Data.SQLite.Context;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Data.SQLite.Repositories
{
    public class AppSettingRepository : Repository<AppSettingEntity>, IAppSettingRepository
    {
        public AppSettingRepository(FalconDbContext context) : base(context)
        {

        }

        public async Task<AppSettingEntity> GetAppSettingByKey(string key)
        {
            return await this.DbSet.FirstOrDefaultAsync(c => c.Key.Equals(key) );
        }
    }
}

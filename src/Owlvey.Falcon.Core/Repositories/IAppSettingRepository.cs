using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Core.Repositories
{
    public interface IAppSettingRepository : IRepository<AppSettingEntity>
    {
        Task<AppSettingEntity> GetAppSettingByKey(string key);
    }
}

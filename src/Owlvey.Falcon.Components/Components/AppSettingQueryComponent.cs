using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class AppSettingQueryComponent : BaseComponent, IAppSettingQueryComponent
    {
        private readonly IAppSettingRepository _appSettingRepository;
        public AppSettingQueryComponent(IAppSettingRepository appSettingRepository)
        {
            this._appSettingRepository = appSettingRepository;
        }

        /// <summary>
        /// Get setting by key
        /// </summary>
        /// <param name="key">App Setting key</param>
        /// <returns></returns>
        public async Task<AppSettingGetRp> GetAppSettingById(string key)
        {
            var entity = await this._appSettingRepository.GetAppSettingByKey(key);

            if (entity == null)
                return null;

            return new AppSettingGetRp {
                Key = entity.Key,
                Value = entity.Value,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }

        /// <summary>
        /// Get All settings
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AppSettingGetListRp>> GetSettings()
        {
            var entities = await this._appSettingRepository.GetAll();

            return entities.Select(entity => new AppSettingGetListRp {
                Key = entity.Key,
                Value = entity.Value,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}

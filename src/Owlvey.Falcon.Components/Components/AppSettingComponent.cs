using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class AppSettingComponent : BaseComponent, IAppSettingComponent
    {
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly IUserIdentityService _identityService;

        public AppSettingComponent(IAppSettingRepository appSettingRepository,
            IUserIdentityService identityService)
        {
            this._appSettingRepository = appSettingRepository;
            this._identityService = identityService;
        }

        /// <summary>
        /// Create a new appsetting
        /// </summary>
        /// <param name="model">AppSetting Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> CreateAppSetting(AppSettingPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var appSetting = AppSettingEntity.Factory.Create(model.Key, model.Value, true, DateTime.UtcNow, createdBy);

            var entity = await this._appSettingRepository.GetAppSettingByKey(model.Key);
            if (entity != null)
            {
                result.AddConflict($"The Key {model.Key} has already been taken.");
                return result;
            }

            this._appSettingRepository.Add(appSetting);

            await this._appSettingRepository.SaveChanges();

            result.AddResult("Key", appSetting.Key);

            return result;
        }

        /// <summary>
        /// Delete appsetting
        /// </summary>
        /// <param name="key">AppSetting Keg</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteAppSetting(string key)
        {
            var result = new BaseComponentResultRp();

            var appSetting = await this._appSettingRepository.GetAppSettingByKey(key);

            if (appSetting == null)
            {
                result.AddNotFound($"The Key {key} doesn't exists.");
                return result;
            }
            
            appSetting.ModifiedBy = this._identityService.GetIdentity();
            appSetting.ModifiedOn = DateTime.UtcNow;

            this._appSettingRepository.Remove(appSetting);

            await this._appSettingRepository.SaveChanges();

            return result;
        }
        
        /// <summary>
        /// Update appsetting
        /// </summary>
        /// <param name="model">AppSetting Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> UpdateAppSetting(string key, AppSettingPutRp model)
        {
            var result = new BaseComponentResultRp();
            var appSetting = await this._appSettingRepository.GetAppSettingByKey(key);

            if (appSetting == null)
            {
                result.AddNotFound($"The Key {key} doesn't exists.");
                return result;
            }

            appSetting.Value = model.Value;
            appSetting.ModifiedBy = this._identityService.GetIdentity();
            appSetting.ModifiedOn = DateTime.UtcNow;

            this._appSettingRepository.Update(appSetting);

            await this._appSettingRepository.SaveChanges();

            return result;
        }
    }
}

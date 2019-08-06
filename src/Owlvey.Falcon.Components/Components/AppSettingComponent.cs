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
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Owlvey.Falcon.Components
{
    public class AppSettingComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;        

        public AppSettingComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService,
            IDateTimeGateway dateTimeGateway,
            IMapper mapper): base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;            
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

            var entity = await this._dbContext.AppSettings.FirstOrDefaultAsync(c=> c.Key.Equals(model.Key));
            if (entity != null)
            {
                result.AddConflict($"The Key {model.Key} has already been taken.");
                return result;
            }

            await this._dbContext.AddAsync(appSetting);
            await this._dbContext.SaveChangesAsync();

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

            var appSetting = await this._dbContext.AppSettings.FirstOrDefaultAsync(c => c.Key.Equals(key));

            if (appSetting == null)
            {
                result.AddNotFound($"The Resource {key} doesn't exists.");
                return result;
            }
            
            this._dbContext.Remove(appSetting);

            await this._dbContext.SaveChangesAsync();

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
            var appSetting = await this._dbContext.AppSettings.FirstOrDefaultAsync(c => c.Key.Equals(key));

            if (appSetting == null)
            {
                result.AddNotFound($"The Resource {key} doesn't exists.");
                return result;
            }

            appSetting.Value = model.Value;
            appSetting.ModifiedBy = this._identityService.GetIdentity();
            appSetting.ModifiedOn = DateTime.UtcNow;

            this._dbContext.Update(appSetting);
            await this._dbContext.SaveChangesAsync();

            return result;
        }
    }
}

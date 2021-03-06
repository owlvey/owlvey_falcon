using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class AppSettingQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        public AppSettingQueryComponent(FalconDbContext dbContext,
            IDateTimeGateway dateTimeGateway, IMapper mapper, 
            IUserIdentityGateway identityGateway,
            ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityGateway, configuration)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Get setting by key
        /// </summary>
        /// <param name="key">App Setting key</param>
        /// <returns></returns>
        public async Task<AppSettingGetRp> GetAppSettingById(string key)
        {
            var entity = await this._dbContext.AppSettings.FirstOrDefaultAsync(c=> c.Key.Equals(key));

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
            var entities = await this._dbContext.AppSettings.ToListAsync();

            return entities.Select(entity => new AppSettingGetListRp {
                Key = entity.Key,
                Value = entity.Value,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}

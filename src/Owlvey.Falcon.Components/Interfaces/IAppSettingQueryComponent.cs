using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface IAppSettingQueryComponent
    {
        Task<IEnumerable<AppSettingGetListRp>> GetSettings();
        Task<AppSettingGetRp> GetAppSettingById(string key);
    }
}

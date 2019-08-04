using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface IServiceComponent
    {
        Task<BaseComponentResultRp> CreateService(ServicePostRp model);
        Task<BaseComponentResultRp> UpdateService(int id, ServicePutRp model);
        Task<BaseComponentResultRp> DeleteService(int id);
    }
}

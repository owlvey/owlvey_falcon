using Owlvey.Falcon.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Interfaces
{
    public interface IFeatureQueryComponent
    {
        Task<IEnumerable<FeatureGetListRp>> GetFeatures();
        Task<FeatureGetRp> GetFeatureById(int id);
    }
}

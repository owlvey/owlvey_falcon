using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface IFeatureQueryComponent
    {
        Task<IEnumerable<FeatureGetListRp>> GetFeatures();
        Task<FeatureGetRp> GetFeatureById(int id);
    }
}

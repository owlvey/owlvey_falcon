using Owlvey.Falcon.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Interfaces
{
    public interface IFeatureComponent
    {
        Task<BaseComponentResultRp> CreateFeature(FeaturePostRp model);
        Task<BaseComponentResultRp> UpdateFeature(int id, FeaturePutRp model);
        Task<BaseComponentResultRp> DeleteFeature(int id);
    }
}

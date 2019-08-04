using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface IFeatureComponent
    {
        Task<BaseComponentResultRp> CreateFeature(FeaturePostRp model);
        Task<BaseComponentResultRp> UpdateFeature(int id, FeaturePutRp model);
        Task<BaseComponentResultRp> DeleteFeature(int id);
    }
}

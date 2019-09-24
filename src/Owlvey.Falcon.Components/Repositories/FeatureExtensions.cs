using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Owlvey.Falcon.Repositories.Features
{
    public static class FeatureExtensions
    {
        public static async Task<IEnumerable<IncidentEntity>> GetIncidentsByFeature(this FalconDbContext context,int featureId)
        {
            return await context.IncidentMaps.Where(c=>c.FeatureId == featureId).Select(c=>c.Incident).ToListAsync();
        }
    }
}

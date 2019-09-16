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
        public static async Task<IEnumerable<IncidentEntity>> GetIncidentsByFeature(this FalconDbContext context,
            int featureId, DateTime start, DateTime end)
        {
            return await context.IncidentMaps.Where(c=>c.FeatureId == featureId && c.Incident.Start >= start && c.Incident.End <= end).Select(c=>c.Incident).ToListAsync();
        }
    }
}

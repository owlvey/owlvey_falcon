using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Owlvey.Falcon.Repositories.Services
{
    public static class ServicesExtensions
    {        

        public static async Task<IEnumerable<IncidentEntity>> GetIncidentsByService(
            this FalconDbContext context,
            int serviceId, DateTime start, DateTime end)
        {
            var features = await context.ServiceMaps.Where(c => c.ServiceId == serviceId).Select(c=>c.FeatureId).ToListAsync();

            var incidents =  await context.IncidentMaps
                .Where(c => features.Contains(c.FeatureId) && c.Incident.Start >= start && c.Incident.End <= end)
                .Select(c=>c.Incident).ToListAsync();

            return incidents;
        }
    }
}
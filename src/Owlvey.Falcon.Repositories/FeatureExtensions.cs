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

        public static async Task RemoveFeature(this FalconDbContext context, int id) {
            
            var feature = await context.Features
                .Include(c => c.IncidentMap)
                .Include(c => c.ServiceMaps)
                .Include(c => c.Squads).SingleAsync(c => c.Id == id);

            if (feature != null)
            {
                foreach (var map in feature.IncidentMap)
                {
                    context.IncidentMaps.Remove(map);
                }
                foreach (var map in feature.ServiceMaps)
                {
                    context.ServiceMaps.Remove(map);
                }
                foreach (var squad in feature.Squads)
                {
                    context.SquadFeatures.Remove(squad);
                }

                await context.SaveChangesAsync();

                feature = await context.Features
                .Include(c => c.ServiceMaps)
                .Include(c => c.Squads).SingleAsync(c => c.Id == id);

                context.Features.Remove(feature);

                await context.SaveChangesAsync();
            }
        }
    }
}

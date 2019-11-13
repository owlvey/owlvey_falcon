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
            this FalconDbContext context, int serviceId)
        {
            var features = await context.ServiceMaps.Where(c => c.ServiceId == serviceId).Select(c=>c.FeatureId).ToListAsync();

            var incidents =  await context.IncidentMaps
                .Where(c => features.Contains(c.FeatureId))
                .Select(c=>c.Incident).ToListAsync();

            return incidents;
        }

        public static async Task RemoveService(this FalconDbContext context, int serviceId) {            

            var service = await context.Services.SingleAsync(c => c.Id == serviceId);
            
            context.Services.Remove(service);

            await context.SaveChangesAsync();
            
        }
    }
}
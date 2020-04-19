﻿using Owlvey.Falcon.Core.Entities;
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

        public static async Task<ServiceEntity> GetService(this FalconDbContext context, int serviceId) {
            var service = await context.Services
                .Include(c => c.FeatureMap)                
                .Where(c => c.Id == serviceId).SingleOrDefaultAsync();

            if (service == null) {
                return null;
            }

            var featuresIds = service.FeatureMap.Select(c => c.FeatureId).ToList();

            var features = await context.Features
                .Include(c=>c.Indicators)
                .Where(c => featuresIds.Contains(c.Id.Value)).ToListAsync();

            var sourceIds = features.SelectMany(c => c.Indicators).Select(c => c.SourceId).ToList();

            var sources = await context.Sources.Where(c => sourceIds.Contains(c.Id.Value)).ToListAsync();

            foreach (var feature in features)
            {
                foreach (var indicator in feature.Indicators)
                {
                    indicator.Source = sources.Single(c => c.Id == indicator.SourceId);
                }
            }

            foreach (var map in service.FeatureMap)
            {
                map.Feature = features.Single(c => c.Id == map.FeatureId);
            }
            return service;
        }

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
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Values;


namespace Owlvey.Falcon.Repositories.Journeys
{
    public static class JourneysExtensions
    {

        public static async Task<ICollection<JourneyEntity>> GetJourneyByProduct(this FalconDbContext context, int productId) {
            var journeys = await context.Journeys
                .Include(c => c.FeatureMap)
                .Where(c => c.ProductId == productId).ToListAsync();

            var featuresIds = journeys.SelectMany(c=> c.FeatureMap).Select(c=>c.FeatureId).Distinct().ToList();

            var features = await context.Features
                .Include(c => c.Indicators)
                .Where(c => featuresIds.Contains(c.Id.Value)).ToListAsync();

            var sourceIds = features.SelectMany(c => c.Indicators).Select(c => c.SourceId).Distinct().ToList();

            var sources = await context.Sources.Where(c => sourceIds.Contains(c.Id.Value)).ToListAsync();

            foreach (var feature in features)
            {
                foreach (var indicator in feature.Indicators)
                {
                    indicator.Source = sources.Single(c => c.Id == indicator.SourceId);
                }
            }

            foreach (var journey in journeys)
            {
                foreach (var map in journey.FeatureMap)
                {
                    map.Feature = features.Single(c => c.Id == map.FeatureId);
                }
            }
            return journeys;
        }


        public static async Task<JourneyEntity> GetJourney(this FalconDbContext context, int journeyId) {
            var journey = await context.Journeys
                .Include(c => c.FeatureMap)                
                .Where(c => c.Id == journeyId).SingleOrDefaultAsync();

            if (journey == null) {
                return null;
            }

            var featuresIds = journey.FeatureMap.Select(c => c.FeatureId).Distinct().ToList();

            var features = await context.Features
                .Include(c=>c.Indicators)
                .Where(c => featuresIds.Contains(c.Id.Value)).ToListAsync();

            var sourceIds = features.SelectMany(c => c.Indicators).Select(c => c.SourceId).Distinct().ToList();

            var sources = await context.Sources.Where(c => sourceIds.Contains(c.Id.Value)).ToListAsync();

            foreach (var feature in features)
            {
                foreach (var indicator in feature.Indicators)
                {
                    indicator.Source = sources.Single(c => c.Id == indicator.SourceId);
                }
            }

            foreach (var map in journey.FeatureMap)
            {
                map.Feature = features.Single(c => c.Id == map.FeatureId);
            }
            return journey;
        }

        public static async Task<JourneyEntity> FullJourneyWithSourceItems(this FalconDbContext context, int journeyId,
            DateTime start, DateTime end) {
            var entity = await GetJourney(context, journeyId);
            var sources = entity.FeatureMap.SelectMany(c => c.Feature.Indicators).Select(c => c.SourceId).Distinct().ToList();
            var sourceItems = await context.GetSourceItems(sources, start, end);
            foreach (var map in entity.FeatureMap)
            {
                foreach (var indicator in map.Feature.Indicators)
                {
                    indicator.Source.SourceItems = sourceItems.Where(c => c.SourceId == indicator.SourceId).ToList();
                }                
            }
            return entity;
        }

        

        public static async Task<IEnumerable<IncidentEntity>> GetIncidentsByJourney(
            this FalconDbContext context, int journeyId)
        {
            var features = await context.JourneyMaps.Where(c => c.JourneyId == journeyId).Select(c=>c.FeatureId).ToListAsync();

            var incidents =  await context.IncidentMaps
                .Where(c => features.Contains(c.FeatureId))
                .Select(c=>c.Incident).ToListAsync();

            return incidents;
        }

        public static async Task RemoveJourney(this FalconDbContext context, int journeyId) {            

            var journey = await context.Journeys.SingleAsync(c => c.Id == journeyId);
            
            context.Journeys.Remove(journey);

            await context.SaveChangesAsync();
            
        }
    }
}
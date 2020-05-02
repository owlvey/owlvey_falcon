using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Owlvey.Falcon.Repositories.Squads
{
    public static class SquadExtensions
    {
        public static async Task<SquadEntity> GetSquad(this FalconDbContext context, int customerId, string name) {

            return await context.Squads.SingleOrDefaultAsync(c => c.CustomerId == customerId && c.Name == name);
        }
    }
}

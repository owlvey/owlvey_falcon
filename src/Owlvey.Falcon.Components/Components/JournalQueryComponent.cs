using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class JournalQueryComponent : BaseComponent, IJournalQueryComponent
    {
        private readonly FalconDbContext _dbContext;
        public JournalQueryComponent(FalconDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Get Journal by id
        /// </summary>
        /// <param name="key">Journal Id</param>
        /// <returns></returns>
        public async Task<JournalGetRp> GetJournalById(int id)
        {
            var entity = await this._dbContext.Journals.FirstOrDefaultAsync(c=> c.Id.Equals(id));

            if (entity == null)
                return null;

            return new JournalGetRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }

        /// <summary>
        /// Get All Journal
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<JournalGetListRp>> GetJournals()
        {
            var entities = await this._dbContext.Journals.ToListAsync();

            return entities.Select(entity => new JournalGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}

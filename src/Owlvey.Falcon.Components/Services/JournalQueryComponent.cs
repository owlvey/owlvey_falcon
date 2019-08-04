using Owlvey.Falcon.Components.Interfaces;
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Components.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Services
{
    public class JournalQueryComponent : BaseComponent, IJournalQueryComponent
    {
        private readonly IJournalRepository _journalRepository;
        public JournalQueryComponent(IJournalRepository journalRepository)
        {
            this._journalRepository = journalRepository;
        }

        /// <summary>
        /// Get Journal by id
        /// </summary>
        /// <param name="key">Journal Id</param>
        /// <returns></returns>
        public async Task<JournalGetRp> GetJournalById(int id)
        {
            var entity = await this._journalRepository.FindFirst(c=> c.Id.Equals(id));

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
            var entities = await this._journalRepository.GetAll();

            return entities.Select(entity => new JournalGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}

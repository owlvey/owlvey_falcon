using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface IJournalQueryComponent
    {
        Task<IEnumerable<JournalGetListRp>> GetJournals();
        Task<JournalGetRp> GetJournalById(int id);
    }
}

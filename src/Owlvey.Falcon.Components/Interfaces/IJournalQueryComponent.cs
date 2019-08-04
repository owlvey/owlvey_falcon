using Owlvey.Falcon.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Interfaces
{
    public interface IJournalQueryComponent
    {
        Task<IEnumerable<JournalGetListRp>> GetJournals();
        Task<JournalGetRp> GetJournalById(int id);
    }
}

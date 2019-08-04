using Owlvey.Falcon.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Interfaces
{
    public interface IJournalComponent
    {
        Task<BaseComponentResultRp> CreateJournal(JournalPostRp model);
        Task<BaseComponentResultRp> UpdateJournal(int id, JournalPutRp model);
        Task<BaseComponentResultRp> DeleteJournal(int id);
    }
}

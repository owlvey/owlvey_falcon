using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface ISquadComponent
    {
        Task<BaseComponentResultRp> CreateSquad(SquadPostRp model);
        Task<BaseComponentResultRp> UpdateSquad(int id, SquadPutRp model);
        Task<BaseComponentResultRp> DeleteSquad(int id);
    }
}

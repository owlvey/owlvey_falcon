using Owlvey.Falcon.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Interfaces
{
    public interface ISquadQueryComponent
    {
        Task<IEnumerable<SquadGetListRp>> GetSquads();
        Task<SquadGetRp> GetSquadById(int id);
    }
}

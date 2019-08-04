using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface ISquadQueryComponent
    {
        Task<IEnumerable<SquadGetListRp>> GetSquads();
        Task<SquadGetRp> GetSquadById(int id);
    }
}

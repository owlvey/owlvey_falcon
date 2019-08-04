using Owlvey.Falcon.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Interfaces
{
    public interface ICustomerQueryComponent
    {
        Task<IEnumerable<CustomerGetListRp>> GetCustomers();
        Task<CustomerGetRp> GetCustomerById(int id);
    }
}

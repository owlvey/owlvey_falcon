using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface ICustomerComponent
    {
        Task<BaseComponentResultRp> CreateCustomer(CustomerPostRp model);
        Task<BaseComponentResultRp> UpdateCustomer(int id, CustomerPutRp model);
        Task<BaseComponentResultRp> DeleteCustomer(int id);

   
    }
}   

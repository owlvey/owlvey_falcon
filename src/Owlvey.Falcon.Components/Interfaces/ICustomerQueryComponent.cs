using AutoMapper;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public interface ICustomerQueryComponent
    {
        Task<IEnumerable<CustomerGetListRp>> GetCustomers();
        Task<CustomerGetRp> GetCustomerById(int id);    
    }
   
}

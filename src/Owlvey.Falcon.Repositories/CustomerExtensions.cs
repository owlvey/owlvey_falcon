using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Owlvey.Falcon.Repositories.Customers
{
    public static class CustomerExtensions
    {
        public static async Task<CustomerEntity> GetCustomer(this FalconDbContext context, string name)
        {
            return await context.Customers.SingleOrDefaultAsync(c => c.Name == name);
        }
    }
}

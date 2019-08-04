using Owlvey.Falcon.Data.SQLite.Context;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Data.SQLite.Repositories
{
    public class FeatureRepository : Repository<FeatureEntity>, IFeatureRepository
    {
        public FeatureRepository(FalconDbContext context) : base(context)
        {

        }

    }
}

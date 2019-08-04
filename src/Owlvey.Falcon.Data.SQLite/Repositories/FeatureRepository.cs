using Owlvey.Falcon.Data.SQLite.Context;
using Owlvey.Falcon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owlvey.Falcon.Components.Repositories;

namespace Owlvey.Falcon.Data.SQLite.Repositories
{
    public class FeatureRepository : Repository<FeatureEntity>, IFeatureRepository
    {
        public FeatureRepository(FalconDbContext context) : base(context)
        {

        }

    }
}

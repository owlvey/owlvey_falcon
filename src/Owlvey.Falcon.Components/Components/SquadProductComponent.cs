using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Owlvey.Falcon.Core.Entities;
using System.Collections.Generic;

namespace Owlvey.Falcon.Components
{
    public class SquadProductComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public SquadProductComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<SquadProductGetRp> GetId(int id)
        {
            var entity = await this._dbContext.SquadProducts.SingleOrDefaultAsync(c => c.Id.Equals(id));

            if (entity == null)
                return null;

            return this._mapper.Map<SquadProductGetRp>(entity);
        }

        public async Task<IEnumerable<SquadProductGetListRp>> GetAll(){
            var entities = await this._dbContext.SquadProducts.ToListAsync();
            return this._mapper.Map<IEnumerable<SquadProductGetListRp>>(entities);
        }

        public async Task<IEnumerable<SquadProductGetListRp>> GetBySquad(int squadId)
        {
            var entities = await this._dbContext.SquadProducts.Include(c=> c.Product).Where(c => c.SquadId.Equals(squadId)).ToListAsync();
            return entities.Select(c => new SquadProductGetListRp {
                Id = c.Id.Value,
                Name = c.Product.Name,
                CreatedBy = c.CreatedBy,
                CreatedOn = c.CreatedOn
            }).ToList();
        }

        public async Task<BaseComponentResultRp> CreateSquadProduct(SquadProductPostRp model)
        {
            var result = new BaseComponentResultRp();

            var createdBy = this._identityService.GetIdentity();

            var squad = await this._dbContext.Squads.SingleAsync(c => c.Id == model.SquadId);
            var Product = await this._dbContext.Products.SingleAsync(c => c.Id == model.ProductId);            
            
            var entity = SquadProductEntity.Factory.Create(squad, Product, this._datetimeGateway.GetCurrentDateTime(), createdBy);

            this._dbContext.SquadProducts.Add(entity);

            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }
    }
}

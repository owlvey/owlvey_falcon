using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);
        Task<TEntity> FindFirst(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);
        Task<IEnumerable<TEntity>> GetAll();
        Task<int> SaveChanges();
    }
}

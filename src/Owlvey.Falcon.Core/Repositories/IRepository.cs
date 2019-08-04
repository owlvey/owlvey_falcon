using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Core.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
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

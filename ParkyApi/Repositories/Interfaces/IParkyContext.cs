using ParkyApi.Models;
using System.Data;
using System.Linq.Expressions;

namespace ParkyApi.Repositories.Interfaces
{
    public interface IParkyContext
    {
        Task<bool> CreateAsync<TEntity>(TEntity entity) where TEntity : IBaseEntity;
        Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : IBaseEntity;
        Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : class, IBaseEntity;
        Task<TEntity> GetByIdAsync<TEntity>(int id, params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IBaseEntity;
        Task<TEntity> FirstOrDefaultAsync<TEntity>(bool readOnly, Expression<Func<TEntity, bool>> where) where TEntity : class;
        Task<List<TEntity>> WhereAsync<TEntity>(bool readOnly, Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[]? includes) where TEntity : class;
        Task<List<TEntity>> AllAsync<TEntity>() where TEntity : class;
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable);
        Task CommitAsync();
        Task RollbackAsync();
    }
}

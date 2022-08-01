using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ParkyApi.Data;
using ParkyApi.Extensions;
using ParkyApi.Models;
using ParkyApi.Repositories.Extensions;
using ParkyApi.Repositories.Interfaces;
using System.Data;
using System.Linq.Expressions;

namespace ParkyApi.Repositories;

public class ParkyContext : IParkyContext
{
    private readonly ApplicationDbContext _applicationDbContext;
    private IDbContextTransaction _transaction;

    public IBaseEntity Entity { get; private set; }

    public ParkyContext(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> CreateAsync<TEntity>(TEntity entity) where TEntity : IBaseEntity
    {
        async Task<bool> Insert()
        {
            _applicationDbContext.Add(entity);
            Entity = entity;

            var result = await _applicationDbContext.SaveChangesAsync();
            return result > 0;
        }

        var result = await this.ExecuteStrategyAndRetry(_applicationDbContext, Insert);

        return result;
    }

    public async Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : IBaseEntity
    {
        async Task<bool> Update()
        {
            _applicationDbContext.Update(entity);
            Entity = entity;

            var result = await _applicationDbContext.SaveChangesAsync();
            return result > 0;
        }

        var result = await this.ExecuteStrategyAndRetry(_applicationDbContext, Update);

        return result;
    }

    public async Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : class, IBaseEntity
    {
        async Task<TEntity?> GetById()
        {
            var entity = await _applicationDbContext
                .GetDbSet<TEntity>()
                .FindAsync(id);

            return entity;
        }

        var result = await this.ExecuteRetry(GetById);

        return result;
    }

    public async Task<TEntity> GetByIdAsync<TEntity>(int id, params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IBaseEntity
    {
        async Task<TEntity?> GetById()
        {
            var query = _applicationDbContext
                .GetDbSet<TEntity>()
                .Where(x => x.Id == id);

            query = includes.Aggregate(query, (current, expressionProperty) => current.Include(expressionProperty));

            var entity = await query.FirstOrDefaultAsync();

            return entity;
        }

        var result = await this.ExecuteRetry(GetById);

        return result;
    }

    public async Task<TEntity> FirstOrDefaultAsync<TEntity>(bool readOnly, Expression<Func<TEntity, bool>> where) where TEntity : class
    {
        async Task<TEntity?> GetFirstOrDefault()
        {
            var entity = await _applicationDbContext.GetDbSet<TEntity>()
                .AsNoTracking(readOnly)
                .FirstOrDefaultAsync(@where);

            return entity;
        }

        var result = await this.ExecuteRetry(GetFirstOrDefault);

        return result;
    }

    public async Task<List<TEntity>> WhereAsync<TEntity>(bool readOnly, Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[]? includes) where TEntity : class
    {
        async Task<List<TEntity>> GetList()
        {
            var queryable = _applicationDbContext
                .GetDbSet<TEntity>()
                .AsNoTracking(readOnly)
                .Where(where);

            if (includes?.Length > 0)
            {
                queryable = includes.Aggregate(queryable, (current, expressionProperty) => current.Include(expressionProperty));
            }

            var entities = await queryable.ToListAsync();

            return entities;
        }

        var entitiesRetry = await this.ExecuteRetry(GetList);

        return entitiesRetry;
    }

    public async Task<List<TEntity>> AllAsync<TEntity>() where TEntity : class
    {
        async Task<List<TEntity>> GetAll()
        {
            var entities = await _applicationDbContext
                .GetDbSet<TEntity>()
                .AsNoTracking()
                .ToListAsync();

            return entities;
        }

        var result = await this.ExecuteRetry(GetAll);

        return result;
    }

    public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
    {
        async Task<bool> GetAny()
        {
            var any = await _applicationDbContext
                .GetDbSet<TEntity>()
                .AsNoTracking()
                .AnyAsync(where);

            return any;
        }

        var result = await this.ExecuteRetry(GetAny);

        return result;
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable)
    {
        await this.ExecuteStrategyAndRetry(_applicationDbContext, async () =>
        {
            _transaction = await _applicationDbContext.Database.BeginTransactionAsync(isolationLevel);
        });
    }

    public async Task CommitAsync()
    {
        async Task Commit()
        {
            await _transaction?.CommitAsync();
        }

        await this.ExecuteStrategyAndRetry(_applicationDbContext, Commit);
    }

    public async Task RollbackAsync()
    {
        async Task Rollback()
        {
            await _transaction?.RollbackAsync();
        }

        await this.ExecuteStrategyAndRetry(_applicationDbContext, Rollback);
    }
}
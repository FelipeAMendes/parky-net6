using Microsoft.EntityFrameworkCore;

namespace ParkyApi.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<TEntity> AsNoTracking<TEntity>(this IQueryable<TEntity> source, bool enable) where TEntity : class
    {
        return enable ? source.AsNoTracking() : source;
    }
}
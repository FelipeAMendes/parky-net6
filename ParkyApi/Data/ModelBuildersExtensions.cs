using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ParkyApi.Data;

public static class ExpressionExtensions
{
    public static Expression<Func<TTarget, bool>> Convert<TSource, TTarget>(this Expression<Func<TSource, bool>> root)
    {
        var visitor = new ParameterTypeVisitor<TSource, TTarget>();

        return (Expression<Func<TTarget, bool>>)visitor.Visit(root);
    }

    private class ParameterTypeVisitor<TSource, TTarget> : ExpressionVisitor
    {
        private ReadOnlyCollection<ParameterExpression>? _parameters;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameters?.FirstOrDefault(p => p.Name == node.Name) ?? (node.Type == typeof(TSource)
                ? Expression.Parameter(typeof(TTarget), node.Name)
                : node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            _parameters = VisitAndConvert(node.Parameters, "VisitLambda");

            return Expression.Lambda(Visit(node.Body), _parameters);
        }
    }
}

public static class ModelBuilderExtensions
{
    private static readonly MethodInfo SetQueryFilterMethod = typeof(ModelBuilderExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));

    public static void SetQueryFilter<TEntity, TEntityInterface>(this ModelBuilder builder, Expression<Func<TEntityInterface, bool>> filterExpression)
        where TEntityInterface : class
        where TEntity : class, TEntityInterface
    {
        var concreteExpression = filterExpression.Convert<TEntityInterface, TEntity>();

        builder.Entity<TEntity>().HasQueryFilter(concreteExpression);
    }

    public static void SetQueryFilterOnAllEntities<TEntityInterface>(this ModelBuilder builder, Expression<Func<TEntityInterface, bool>> filterExpression)
    {
        var types = builder.Model
            .GetEntityTypes()
            .Where(t => t.BaseType == null)
            .Select(t => t.ClrType)
            .Where(t => typeof(TEntityInterface).IsAssignableFrom(t));

        foreach (var type in types)
            builder.SetEntityQueryFilter(type, filterExpression);
    }

    private static void SetEntityQueryFilter<TEntityInterface>(this ModelBuilder builder, Type entityType, Expression<Func<TEntityInterface, bool>> filterExpression)
    {
        SetQueryFilterMethod.MakeGenericMethod(entityType, typeof(TEntityInterface))
            .Invoke(null, new object[] { builder, filterExpression });
    }
}

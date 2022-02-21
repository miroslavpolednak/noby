using System.Linq.Expressions;

namespace CIS.Infrastructure.Data;

public static class QueryableExtensions
{
    /// <summary>
    /// Podpora dynamicke OrderBy ze stringu
    /// query = query.ApplyOrderBy(new[] { Tuple.Create(pagination.Sorting.First().Field, pagination.Sorting.First().Descending) });
    /// </summary>
    public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> query, IEnumerable<Tuple<string, bool>> order)
    {
        var expr = ApplyOrderBy(typeof(T), query.Expression, order);
        return query.Provider.CreateQuery<T>(expr);
    }

    static Expression MakePropPath(Expression objExpression, string path)
    {
        return path.Split('.').Aggregate(objExpression, Expression.PropertyOrField);
    }

    static Expression ApplyOrderBy(Type entityType, Expression queryExpr, IEnumerable<Tuple<string, bool>> order)
    {
        var param = Expression.Parameter(entityType, "e");
        var isFirst = true;
        foreach (var tuple in order)
        {
            var lambda = Expression.Lambda(MakePropPath(param, tuple.Item1), param);
            var methodName =
                isFirst ? tuple.Item2 ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy)
                : tuple.Item2 ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy);

            queryExpr = Expression.Call(typeof(Queryable), methodName, new[] { entityType, lambda.Body.Type }, queryExpr, lambda);
            isFirst = false;
        }

        return queryExpr;
    }
}

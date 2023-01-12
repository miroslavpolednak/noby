using System.Data;
using CIS.Core.Data;
using Dapper;

namespace CIS.Infrastructure.Data;

public static class DapperExtensions
{
    public static async Task<List<T>> ExecuteDapperRawSqlToList<T>(this IConnectionProvider connectionProvider, string sqlQuery, CancellationToken cancellationToken = default(CancellationToken))
        => await connectionProvider.ExecuteDapperQuery<List<T>>(async c => (await c.QueryAsync<T>(sqlQuery)).AsList(), cancellationToken);
    
    public static async Task<List<T>> ExecuteDapperRawSqlToList<T>(this IConnectionProvider connectionProvider, string sqlQuery, object param, CancellationToken cancellationToken = default(CancellationToken))
        => await connectionProvider.ExecuteDapperQuery<List<T>>(async c => (await c.QueryAsync<T>(sqlQuery, param)).AsList(), cancellationToken);

    public static async Task<IEnumerable<dynamic>> ExecuteDapperRawSqlToDynamicList(this IConnectionProvider connectionProvider, string sqlQuery, object? param = null, CancellationToken cancellationToken = default(CancellationToken))
        => await connectionProvider.ExecuteDapperQuery<IEnumerable<dynamic>>(async c => (await c.QueryAsync(sqlQuery, param)), cancellationToken);

    public static async Task<T?> ExecuteDapperRawSqlFirstOrDefault<T>(this IConnectionProvider connectionProvider, string sqlQuery, CancellationToken cancellationToken = default(CancellationToken))
        => await connectionProvider.ExecuteDapperQuery<T>(async c => (await c.QueryFirstOrDefaultAsync<T>(sqlQuery)), cancellationToken);
    
    public static async Task<T?> ExecuteDapperRawSqlFirstOrDefault<T>(this IConnectionProvider connectionProvider, string sqlQuery, object param, CancellationToken cancellationToken = default(CancellationToken))
        => await connectionProvider.ExecuteDapperQuery<T>(async c => (await c.QueryFirstOrDefaultAsync<T>(sqlQuery, param)), cancellationToken);
    
    public static async Task<T> ExecuteDapperQuery<T>(this IConnectionProvider connectionProvider, Func<IDbConnection, Task<T>> getData, CancellationToken cancellationToken = default(CancellationToken))
    {
        await using var connection = connectionProvider.Create();
        await connection.OpenAsync(cancellationToken);
        return await getData(connection);
    }
    
    public static async Task ExecuteDapperQuery(this IConnectionProvider connectionProvider, Func<IDbConnection, Task> getData, CancellationToken cancellationToken = default(CancellationToken))
    {
        await using var connection = connectionProvider.Create();
        await connection.OpenAsync(cancellationToken);
        await getData(connection);
    }
    
    public static async Task<TResult> ExecuteDapperQuery<TRead, TResult>(this IConnectionProvider connectionProvider, Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process, CancellationToken cancellationToken = default(CancellationToken))
    {
        await using var connection = connectionProvider.Create();
        await connection.OpenAsync(cancellationToken);
        var data = await getData(connection);
        return await process(data);
    }
}
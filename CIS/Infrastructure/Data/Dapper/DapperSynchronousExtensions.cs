using System.Data;
using CIS.Core.Data;
using Dapper;

namespace CIS.Infrastructure.Data;

public static class DapperSynchronousExtensions
{
    public static List<T> ExecuteDapperRawSqlToList<T>(this IConnectionProvider connectionProvider, string sqlQuery)
        => connectionProvider.ExecuteDapperQuery<List<T>>(c => c.Query<T>(sqlQuery).AsList());
    
    public static List<T> ExecuteDapperRawSqlToList<T>(this IConnectionProvider connectionProvider, string sqlQuery, object param)
        => connectionProvider.ExecuteDapperQuery<List<T>>(c => c.Query<T>(sqlQuery, param).AsList());

    public static List<dynamic> ExecuteDapperRawSqlToDynamicList(this IConnectionProvider connectionProvider, string sqlQuery)
        => connectionProvider.ExecuteDapperQuery<List<dynamic>>(c => c.Query(sqlQuery).AsList());

    public static T ExecuteDapperFirstOrDefault<T>(this IConnectionProvider connectionProvider, string sqlQuery, object param)
        => connectionProvider.ExecuteDapperQuery<T>(c => c.QueryFirstOrDefault<T>(sqlQuery, param));

    public static T ExecuteDapperQuery<T>(this IConnectionProvider connectionProvider, Func<IDbConnection, T> getData)
    {
        using var connection = connectionProvider.Create();
        connection.Open();
        return getData(connection);
    }
    
    public static TResult ExecuteDapperQuery<TRead, TResult>(this IConnectionProvider connectionProvider, Func<IDbConnection, TRead> getData, Func<TRead, TResult> process)
    {
        using var connection = connectionProvider.Create();
        connection.Open();
        var data = getData(connection);
        return process(data);
    }
}
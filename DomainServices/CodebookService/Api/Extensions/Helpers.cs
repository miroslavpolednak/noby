using CIS.Core.Data;
using Dapper;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.Contracts.v1;
using System.Runtime.CompilerServices;

namespace DomainServices.CodebookService.Api;

internal static class Helpers
{
    public static Task<GenericCodebookItemResponse> GetGenericItems(this IConnectionProvider connectionProvider, ReadOnlySpan<char> sqlQuery, [CallerMemberName] string method = "")
    {
        GenericCodebookItemResponse response = new();
        response.Items.AddRange(connectionProvider.GetOrCreate<GenericCodebookItem>(sqlQuery.ToString(), method.AsSpan()));
        return Task.FromResult(response);
    }

    public static List<TResponse> GetOrCreate<TResponse>(this IConnectionProvider connectionProvider, string sqlQuery, ReadOnlySpan<char> method)
        where TResponse : class
    {
        return FastMemoryCache.GetOrCreate<TResponse>(method.ToString(), () =>
        {
            using var connection = connectionProvider.Create();
            connection.Open();
            return connection.Query<TResponse>(sqlQuery).AsList();
        });
    }
}

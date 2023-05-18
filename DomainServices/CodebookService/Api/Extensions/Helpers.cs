using CIS.Core.Data;
using Dapper;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.Contracts.v1;
using System.Runtime.CompilerServices;

namespace DomainServices.CodebookService.Api;

internal static class Helpers
{
    public static Task<TResponse> GetItems<TResponse, TItem>(TResponse response, Func<List<TItem>> createItems, [CallerMemberName] string method = "")
        where TResponse : Contracts.IItemsResponse<TItem>
        where TItem : class, Google.Protobuf.IMessage
    {
        var items = FastMemoryCache.GetOrCreate(method.ToString(), createItems);
        response.Items.AddRange(items);
        return Task.FromResult(response);
    }

    public static List<TResponse> GetOrCreateCachedResponse<TResponse>(this IConnectionProvider connectionProvider, string sqlQuery, ReadOnlySpan<char> method)
        where TResponse : class, Google.Protobuf.IMessage
    {
        return FastMemoryCache.GetOrCreate(method.ToString(), () =>
        {
            using var connection = connectionProvider.Create();
            connection.Open();
            return connection.Query<TResponse>(sqlQuery).AsList();
        });
    }

    public static Task<TResponse> GetItems<TResponse, TItem>(this IConnectionProvider connectionProvider, TResponse response, ReadOnlySpan<char> sqlQuery, [CallerMemberName] string method = "")
        where TResponse : Contracts.IItemsResponse<TItem>
        where TItem : class, Google.Protobuf.IMessage
    {
        var items = connectionProvider.GetOrCreateCachedResponse<TItem>(sqlQuery.ToString(), method.AsSpan());
        response.Items.Add(items);
        return Task.FromResult(response);
    }

    public static Task<TResponse> GetItems<TResponse, TItem>(TResponse response, Func<IEnumerable<TItem>> items)
        where TResponse : Contracts.IItemsResponse<TItem>
        where TItem : class, Google.Protobuf.IMessage
    {
        response.Items.Add(items());
        return Task.FromResult(response);
    }

    public static Task<GenericCodebookResponse> GetGenericItems(this IConnectionProvider connectionProvider, ReadOnlySpan<char> sqlQuery, [CallerMemberName] string method = "")
    {
        GenericCodebookResponse response = new();
        response.Items.AddRange(connectionProvider.GetOrCreateCachedResponse<GenericCodebookResponse.Types.GenericCodebookItem>(sqlQuery.ToString(), method.AsSpan()));
        return Task.FromResult(response);
    }

    public static Task<GenericCodebookWithCodeResponse> GetGenericItemsWithCode<TEnum>()
        where TEnum : struct, Enum
    {
#pragma warning disable CA1305 // Specify IFormatProvider
        var items = FastEnum.GetValues<TEnum>()
            .Where(t => Convert.ToInt32(t) > 0)
            .Select(t => new Contracts.v1.GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem
            {
                Id = Convert.ToInt32(t),
                Code = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? t.ToString(),
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                IsValid = true
            })
            .ToList()!;
#pragma warning restore CA1305 // Specify IFormatProvider

        GenericCodebookWithCodeResponse response = new();
        response.Items.AddRange(items);
        return Task.FromResult(response);
    }
}

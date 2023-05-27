using CIS.Core.Data;
using Dapper;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.Contracts.v1;
using System.Runtime.CompilerServices;

namespace DomainServices.CodebookService.Api;

internal static class Helpers
{
    /// <summary>
    /// Converts string with separated integers into list.
    /// </summary>
    public static List<int>? ParseIDs(this string value, string separator = ",")
    {
        if (String.IsNullOrWhiteSpace(value))
            return null;

        return value
                .Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => Convert.ToInt32(x, System.Globalization.CultureInfo.InvariantCulture))
                .ToList();
    }

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

    public static List<TResponse> GetOrCreateCachedResponse<TResponse>(this IConnectionProvider connectionProvider, string sqlQuery, object param, ReadOnlySpan<char> method)
        where TResponse : class, Google.Protobuf.IMessage
    {
        return FastMemoryCache.GetOrCreate(method.ToString(), () =>
        {
            using var connection = connectionProvider.Create();
            connection.Open();
            return connection.Query<TResponse>(sqlQuery, param).AsList();
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

    public static Task<TResponse> GetItems<TResponse, TItem>(this IConnectionProvider connectionProvider, TResponse response, ReadOnlySpan<char> sqlQuery, object param, [CallerMemberName] string method = "")
        where TResponse : Contracts.IItemsResponse<TItem>
        where TItem : class, Google.Protobuf.IMessage
    {
        var items = connectionProvider.GetOrCreateCachedResponse<TItem>(sqlQuery.ToString(), param, method.AsSpan());
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

    public static Task<GenericCodebookWithDefaultAndCodeResponse> GetGenericItemsWithDefaultAndCode(this IConnectionProvider connectionProvider, ReadOnlySpan<char> sqlQuery, [CallerMemberName] string method = "")
    {
        GenericCodebookWithDefaultAndCodeResponse response = new();
        response.Items.AddRange(connectionProvider.GetOrCreateCachedResponse<GenericCodebookWithDefaultAndCodeResponse.Types.GenericCodebookWithDefaultAndCodeItem>(sqlQuery.ToString(), method.AsSpan()));
        return Task.FromResult(response);
    }

    public static Task<GenericCodebookWithRdmCodeResponse> GetGenericItemsWithRdmCode(this IConnectionProvider connectionProvider, ReadOnlySpan<char> sqlQuery, [CallerMemberName] string method = "")
    {
        GenericCodebookWithRdmCodeResponse response = new();
        response.Items.AddRange(connectionProvider.GetOrCreateCachedResponse<GenericCodebookWithRdmCodeResponse.Types.GenericCodebookWithRdmCodeItem>(sqlQuery.ToString(), method.AsSpan()));
        return Task.FromResult(response);
    }

    public static Task<GenericCodebookFullResponse> GetGenericFullItems(this IConnectionProvider connectionProvider, ReadOnlySpan<char> sqlQuery, [CallerMemberName] string method = "")
    {
        GenericCodebookFullResponse response = new();
        response.Items.AddRange(connectionProvider.GetOrCreateCachedResponse<GenericCodebookFullResponse.Types.GenericCodebookFullItem>(sqlQuery.ToString(), method.AsSpan()));
        return Task.FromResult(response);
    }

    public static Task<GenericCodebookWithDefaultAndCodeResponse> GetGenericItemsWithDefaultAndCode<TEnum>()
    where TEnum : struct, Enum
    {
#pragma warning disable CA1305 // Specify IFormatProvider
        var items = FastEnum.GetValues<TEnum>()
            .Where(t => Convert.ToInt32(t) > 0)
            .Select(t => new GenericCodebookWithDefaultAndCodeResponse.Types.GenericCodebookWithDefaultAndCodeItem
            {
                Id = Convert.ToInt32(t),
                Code = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? t.ToString(),
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>()
            })
            .ToList()!;
#pragma warning restore CA1305 // Specify IFormatProvider

        GenericCodebookWithDefaultAndCodeResponse response = new();
        response.Items.AddRange(items);
        return Task.FromResult(response);
    }

    public static Task<GenericCodebookResponse> GetGenericItems<TEnum>()
        where TEnum : struct, Enum
    {
#pragma warning disable CA1305 // Specify IFormatProvider
        var items = FastEnum.GetValues<TEnum>()
            .Where(t => Convert.ToInt32(t) > 0)
            .Select(t => new GenericCodebookResponse.Types.GenericCodebookItem
            {
                Id = Convert.ToInt32(t),
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                IsValid = true
            })
            .ToList()!;
#pragma warning restore CA1305 // Specify IFormatProvider

        GenericCodebookResponse response = new();
        response.Items.AddRange(items);
        return Task.FromResult(response);
    }
}

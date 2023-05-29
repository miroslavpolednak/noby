using CIS.Core.Data;
using Dapper;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.v1;
using Google.Protobuf;
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

    public static TResponse AddItems<TResponse, TItem>(this TResponse response, IEnumerable<TItem> items)
        where TItem : class, IMessage
        where TResponse : class, IMessage, IItemsResponse<TItem>
    {
        response.Items.AddRange(items);
        return response;
    }

    public static TResponse GetOrCreateCachedResponse<TResponse, TItem>(this IConnectionProvider connectionProvider, ReadOnlySpan<char> sqlQuery, ReadOnlySpan<char> method)
        where TResponse : class, IMessage, IItemsResponse<TItem>
        where TItem : class, IMessage
    {
        return CodebookMemoryCache.GetOrCreate(method.ToString(), sqlQuery, (sql) =>
        {
            using var connection = connectionProvider.Create();
            connection.Open();
            var items = connection.Query<TItem>(sql).AsList();

            var response = Activator.CreateInstance<TResponse>();
            response.Items.AddRange(items);
            return response;
        });
    }

    public static Task<TResponse> GetItems<TResponse>(Func<TResponse> createItems, [CallerMemberName] string method = "")
        where TResponse : class
    {
        return Task.FromResult(CodebookMemoryCache.GetOrCreate(method.ToString(), createItems));
    }

    public static Task<GenericCodebookResponse> GetGenericItems<TEnum>(bool useCode = false)
        where TEnum : struct, Enum
    {
#pragma warning disable CA1305 // Specify IFormatProvider
        var items = FastEnum.GetValues<TEnum>()
            .Where(t => Convert.ToInt32(t) > 0)
            .Select(t => new GenericCodebookResponse.Types.GenericCodebookItem
            {
                Id = Convert.ToInt32(t),
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                Code = useCode ? t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? t.ToString() : null,
                Description = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Description,
                IsValid = true,
                IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>() ? true : null
            })
            .ToList()!;
#pragma warning restore CA1305 // Specify IFormatProvider

        return Task.FromResult((new GenericCodebookResponse()).AddItems(items));
    }
}

using CIS.Core.Data;
using DomainServices.CodebookService.Contracts.v1;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace DomainServices.CodebookService.Api.Database;

internal sealed class SqlQueryCollection
    : ReadOnlyDictionary<string, string>
{
    public SqlQueryCollection(IDictionary<string, string> values)
        : base(values)
    { }

    public ReadOnlySpan<char> Get(int? suffix = null, [CallerMemberName] string method = "")
    {
        return suffix.HasValue ? $"{this[method]}{suffix}" : this[method];
    }

    public Task<GenericCodebookResponse> GetGenericItems(IConnectionProvider connectionProvider, [CallerMemberName] string method = "")
    {
        return Task.FromResult(connectionProvider.GetOrCreateCachedResponse<GenericCodebookResponse, GenericCodebookResponse.Types.GenericCodebookItem>(this[method].AsSpan(), method.AsSpan()));
    }

    public Task<TResponse> GetItems<TResponse, TItem>(IConnectionProvider connectionProvider, [CallerMemberName] string method = "")
        where TResponse : class, Contracts.IItemsResponse<TItem>
        where TItem : class, Google.Protobuf.IMessage
    {
        return Task.FromResult(connectionProvider.GetOrCreateCachedResponse<TResponse, TItem>(this[method].AsSpan(), method.AsSpan()));
    }
}

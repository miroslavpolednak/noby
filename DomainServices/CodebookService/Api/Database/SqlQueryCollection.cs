using System.Collections.ObjectModel;

namespace DomainServices.CodebookService.Api.Database;

internal sealed class SqlQueryCollection(
    IDictionary<string, SqlQueryCollection.QueryItem> values)
        : ReadOnlyDictionary<string, SqlQueryCollection.QueryItem>(values)
{
    public enum DatabaseProviders : byte
    {
        Xxd = 1,
        XxdHf = 2,
        KonsDb = 3,
        Self = 4
    }

    public sealed class QueryItem
    {
        public string Query { get; set; } = null!;
        public DatabaseProviders Provider { get; set; }
    }
}

using System.Collections.ObjectModel;

namespace DomainServices.CodebookService.Api.Database;

internal sealed class SqlQueryCollection
    : ReadOnlyDictionary<string, string>
{
    public SqlQueryCollection(IDictionary<string, string> values)
        : base(values)
    { }
}

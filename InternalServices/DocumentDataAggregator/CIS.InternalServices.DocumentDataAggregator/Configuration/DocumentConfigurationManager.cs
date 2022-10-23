using System.Collections.Immutable;
using System.Collections.ObjectModel;
using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration;

[ScopedService, SelfService]
internal class DocumentConfigurationManager
{
    private readonly ConfigurationRepository _repository;

    public DocumentConfigurationManager(ConfigurationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ImmutableDictionary<DataSource, ReadOnlyCollection<DataSourceField>>> Load(CancellationToken cancellationToken)
    {
        var fields = _repository.LoadDocumentFields();

        return fields.GroupBy(f => f.DataSource).ToImmutableDictionary(g => g.Key, g => g.ToList().AsReadOnly());
    }
}
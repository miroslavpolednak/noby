using CIS.Core.Data;
using CIS.Infrastructure.Data;

namespace DomainServices.DocumentArchiveService.Api.Database.Repositories;


public interface IDocumentSequenceRepository
{
    Task<long> GetNextDocumentSeqValue(CancellationToken cancellation);
}

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.AsImplementedInterfacesService]
internal class DocumentSequenceRepository : IDocumentSequenceRepository
{
    private readonly IConnectionProvider<IXxvDapperConnectionProvider> _connectionProvider;

    public DocumentSequenceRepository(CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<long> GetNextDocumentSeqValue(CancellationToken cancellation)
    {
        return await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<long>("SELECT NEXT VALUE FOR dbo.GenerateDocumentIdSequence", cancellation);
    }
}

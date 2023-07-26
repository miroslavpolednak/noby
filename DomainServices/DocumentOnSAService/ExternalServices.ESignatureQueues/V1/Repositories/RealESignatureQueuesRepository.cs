using CIS.Core.Data;
using ExternalServices.ESignatureQueues.Abstraction;

namespace ExternalServices.ESignatureQueues.V1.Repositories;

public class RealESignatureQueuesRepository : IESignatureQueuesRepository
{
    private readonly IConnectionProvider<IESignatureQueuesDapperConnectionProvider> _connectionProvider;

    public RealESignatureQueuesRepository(IConnectionProvider<IESignatureQueuesDapperConnectionProvider> connectionProvider)
    {
        Dapper.SqlMapper.Settings.CommandTimeout = 10;
        _connectionProvider = connectionProvider;
    }

    public Task<string> GetAttachmentExternalId(string attachmentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
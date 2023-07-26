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

    public async Task<string> GetAttachmentExternalId(string attachmentId, CancellationToken cancellationToken)
    {
        // todo:
        await Task.Delay(0, cancellationToken);
        return Guid.NewGuid().ToString();
    }
}
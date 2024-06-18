using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Contracts;
using Google.Protobuf.WellKnownTypes;
using System.Data;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.Maintanance.DeleteDataFromArchiveQueue;

internal class DeleteBinaryDataFromArchiveQueueHandler(IConnectionProvider<IXxvDapperConnectionProvider> provider) : IRequestHandler<DeleteDataFromArchiveQueueRequest, Empty>
{
    private readonly IConnectionProvider<IXxvDapperConnectionProvider> _provider = provider;

    private const int _batchSize = 10000;
    private const int _olderThanDays = 30;

    private const string _sql = """
        WITH forDel AS (
        SELECT TOP (@BatchSize) d.DOCUMENT_ID from DocumentInterface d
        WHERE d.[STATUS] IN (302, 400) AND d.DOCUMENT_DATA IS NOT NULL AND d.CREATED_ON < @DateTime
        )
        UPDATE di
        SET di.DOCUMENT_DATA = NULL
        FROM DocumentInterface di
        INNER JOIN forDel ON forDel.DOCUMENT_ID = di.DOCUMENT_ID
        """;

    public async Task<Empty> Handle(DeleteDataFromArchiveQueueRequest request, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@BatchSize", _batchSize, DbType.Int32, ParameterDirection.Input);
        parameters.Add("@DateTime", DateTime.Now.AddDays(_olderThanDays), DbType.DateTime, ParameterDirection.Input);

        await _provider.ExecuteDapperQueryAsync(async c => await c.ExecuteAsync(_sql, parameters), cancellationToken);

        return new Empty();
    }
}

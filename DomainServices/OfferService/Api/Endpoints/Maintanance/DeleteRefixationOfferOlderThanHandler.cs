using Dapper;
using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DomainServices.OfferService.Api.Endpoints.Maintanance;

public class DeleteRefixationOfferOlderThanHandler(IConfiguration config, TimeProvider timeProvider) : IRequestHandler<DeleteRefixationOfferOlderThanRequest, Empty>
{
    private const int _technicalTimeout = 50; // [s]

    private readonly IConfiguration _config = config;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<Empty> Handle(DeleteRefixationOfferOlderThanRequest request, CancellationToken cancellationToken)
    {
        var connectionStr = _config.GetConnectionString("default") ?? throw new NotSupportedException("defaut connection string required");
        using var connection = new SqlConnection(connectionStr);
        await connection.OpenAsync(cancellationToken);

        await connection.QueryFirstOrDefaultAsync<int>(
        "[dbo].[DeleteRefixationOfferOlderThan]",
        new { Date = _timeProvider.GetLocalNow() },
          commandType: CommandType.StoredProcedure,
          commandTimeout: _technicalTimeout
          );
        
        return new Empty();
    }
}

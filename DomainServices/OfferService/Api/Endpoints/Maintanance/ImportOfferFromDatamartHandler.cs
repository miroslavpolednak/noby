using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using DomainServices.OfferService.Api.Extensions;

namespace DomainServices.OfferService.Api.Endpoints.Maintanance;

public class ImportOfferFromDatamartHandler(IConfiguration config, ILogger<ImportOfferFromDatamartHandler> logger) : IRequestHandler<ImportOfferFromDatamartRequest, Empty>
{
    private const int _technicalTimeout = 500; // [s]
    private const string _existBatchForProcessingSql = """SELECT COUNT(*) FROM dbo.D_CUST_RETENTION_BATCH b WHERE b.Load_Status='Complete' AND b.Was_Processed_By_Noby = 0""";
    private const string _oldestBatchIdForProcessing = """SELECT TOP (1) b.Batch_Id FROM dbo.D_CUST_RETENTION_BATCH b WHERE b.Load_Status='Complete' AND b.Was_Processed_By_Noby = 0 ORDER BY b.Batch_Id""";
    private const string _existDataForProcessingSql = """SELECT COUNT(*) FROM dbo.D_CUST_RETENTION_ACCOUNT a WHERE a.Was_Processed_By_Noby = 0 AND Batch_Id = @BatchId""";

    private readonly IConfiguration _config = config;
    private readonly ILogger<ImportOfferFromDatamartHandler> _logger = logger;

    public async Task<Empty> Handle(
        ImportOfferFromDatamartRequest request,
        CancellationToken cancellationToken)
    {
        var connectionStr = _config.GetConnectionString("default") ?? throw new NotSupportedException("defaut connection string required");
        using var connection = new SqlConnection(connectionStr);
        await connection.OpenAsync(cancellationToken);

        int maxAllowedBatchPerJob = 10;
        while (await connection.ExecuteScalarAsync<int>(_existBatchForProcessingSql) > 0 && maxAllowedBatchPerJob > 0)
        {
            var batchId = await connection.ExecuteScalarAsync<long>(_oldestBatchIdForProcessing);
            _logger.BatchIdForProcessing(batchId);

            //Delete all non Communicated refixation offer from datalake (only where is intersection of sets)
            await connection.QueryFirstOrDefaultAsync<int>(
                "dbo.DeleteRefixationOffer",
                new { FlagState = (int)OfferFlagTypes.Communicated, BatchId = batchId }, // Gonna delete all refixation offer which are not in FlagState
                commandType: CommandType.StoredProcedure,
                commandTimeout: _technicalTimeout
                );

            // Import Refixation offer from datalake
            while (await connection.ExecuteScalarAsync<int>(_existDataForProcessingSql, new { BatchId = batchId }) > 0)
            {
                await connection.QueryFirstOrDefaultAsync<int>(
                "dbo.ImportDataFromDatamart",
                new { BatchSize = request.BatchSize ?? 10000, BatchId = batchId },
                commandType: CommandType.StoredProcedure,
                commandTimeout: _technicalTimeout);
            }

            // Delete data from stage tables (datalake) after import (data for batchId) 
            await connection.QueryFirstOrDefaultAsync<int>(
                    "dbo.DeleteDatamartStageTables",
                    new { BatchId = batchId },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _technicalTimeout
                    );

            maxAllowedBatchPerJob--;
        }
        return new();
    }
}

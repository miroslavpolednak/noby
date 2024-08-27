using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using DomainServices.OfferService.Api.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace DomainServices.OfferService.Api.Endpoints.Maintanance;

public class ImportOfferFromDatamartHandler(
    IConfiguration config, 
    ILogger<ImportOfferFromDatamartHandler> logger) 
    : IRequestHandler<ImportOfferFromDatamartRequest, Empty>
{
    private const int _numberOfBatchsInHistory = 31;
    private const int _technicalTimeout = 3600; // [s]
    private int _maxAllowedBatchPerJob = 10;
    private const string _existBatchForProcessingSql = """SELECT COUNT(*) FROM bdp.D_CUST_RETENTION_BATCH b WHERE b.Load_Status='Complete' AND b.Was_Processed_By_Noby = 0""";
    private const string _oldestBatchIdForProcessing = """SELECT TOP (1) b.Batch_Id FROM bdp.D_CUST_RETENTION_BATCH b WHERE b.Load_Status='Complete' AND b.Was_Processed_By_Noby = 0 ORDER BY b.Batch_Id""";
    private const string _existDataForProcessingSql = """SELECT COUNT(*) FROM bdp.D_CUST_RETENTION_OFFER o WHERE o.Was_Processed_By_Noby = 0 AND o.Batch_Id = @BatchId""";
    private const string _getProcessedBatchs = """SELECT b.Batch_Id FROM bdp.D_CUST_RETENTION_BATCH b WHERE b.Was_Processed_By_Noby = 1""";

    private readonly IConfiguration _config = config;
    private readonly ILogger<ImportOfferFromDatamartHandler> _logger = logger;

    public async Task<Empty> Handle(
        [NotNull] ImportOfferFromDatamartRequest request,
        CancellationToken cancellationToken)
    {
        var connectionStr = _config.GetConnectionString("default") ?? throw new NotSupportedException("defaut connection string required");
        using var connection = new SqlConnection(connectionStr);
        await connection.OpenAsync(cancellationToken);

        // Import data for pairing (translate account nbr to CaseId)
        await connection.QueryFirstOrDefaultAsync<int>(
        "[dbo].[ImportKonstDbView]",
        commandType: CommandType.StoredProcedure,
        commandTimeout: _technicalTimeout);

        while (await connection.ExecuteScalarAsync<int>(_existBatchForProcessingSql) > 0 && _maxAllowedBatchPerJob > 0)
        {
            var batchId = await connection.ExecuteScalarAsync<long>(_oldestBatchIdForProcessing);
            _logger.BatchIdForProcessing(batchId);

            // Update customer information CustomerChurnRisk and CustomerPriceSensitivity on Case (CaseServiceDb)
            await connection.QueryFirstOrDefaultAsync<int>(
                 "dbo.UpdateCustomerInformation",
                 new { BatchId = batchId },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: _technicalTimeout
                 );

            //Delete all non Communicated refixation offer from datalake (only where is intersection of sets)
            await connection.QueryFirstOrDefaultAsync<int>(
                "dbo.DeleteRefixationOffer",
                new { FlagState = (int)EnumOfferFlagTypes.Communicated, BatchId = batchId }, // Gonna delete all refixation offer which are not in FlagState
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

            var processedBatches = (await connection.QueryAsync<long>(_getProcessedBatchs)).OrderBy(b => b).ToList();

            // For observation reason we keep history of processed batches in stage tables. 
            if (processedBatches.Count > _numberOfBatchsInHistory)
            {
                // Delete data from stage tables (datalake)  
                await connection.QueryFirstOrDefaultAsync<int>(
                        "dbo.DeleteDatamartStageTables",
                        new { BatchId = processedBatches[0] },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: _technicalTimeout
                        );
            }
            
            _maxAllowedBatchPerJob--;
        }
        return new();
    }
}

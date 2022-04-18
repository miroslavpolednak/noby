using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.WorkSectors;

namespace DomainServices.CodebookService.Endpoints.WorkSectors
{
    public class WorkSectorsHandler
        : IRequestHandler<WorkSectorsRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(WorkSectorsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(WorkSectorsHandler), async () =>
                    await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItem>(_sqlQuery, cancellationToken)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private const string _sqlQuery =
            "SELECT KOD 'Id', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_PRACOVNI_SEKTOR] ORDER BY KOD ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<WorkSectorsHandler> _logger;

        public WorkSectorsHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<WorkSectorsHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}

using DomainServices.CodebookService.Contracts.Endpoints.JobTypes;

namespace DomainServices.CodebookService.Endpoints.JobTypes
{
    public class JobTypesHandler
        : IRequestHandler<JobTypesRequest, List<JobTypeItem>>
    {
        public async Task<List<JobTypeItem>> Handle(JobTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<JobTypeItem>(nameof(JobTypesHandler), async () =>
                    await _connectionProvider.ExecuteDapperRawSqlToList<JobTypeItem>(_sqlQuery, cancellationToken)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private const string _sqlQuery =
            "SELECT KOD 'Id', TEXT 'Name', DEF 'IsDefault' FROM [SBR].[CIS_PRACOVNI_POZICE] ORDER BY KOD ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<JobTypesHandler> _logger;

        public JobTypesHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<JobTypesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}
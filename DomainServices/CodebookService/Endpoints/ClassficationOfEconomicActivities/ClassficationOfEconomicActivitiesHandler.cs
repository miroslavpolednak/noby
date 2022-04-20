using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ClassficationOfEconomicActivities;

namespace DomainServices.CodebookService.Endpoints.ClassficationOfEconomicActivities
{
    public class ClassficationOfEconomicActivitiesHandler
        : IRequestHandler<ClassficationOfEconomicActivitiesRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(ClassficationOfEconomicActivitiesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(ClassficationOfEconomicActivitiesHandler), async () =>
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
            "SELECT KOD 'Id', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_OKEC] ORDER BY KOD ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<ClassficationOfEconomicActivitiesHandler> _logger;

        public ClassficationOfEconomicActivitiesHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<ClassficationOfEconomicActivitiesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}
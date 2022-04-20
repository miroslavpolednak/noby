using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.Nationalities;

namespace DomainServices.CodebookService.Endpoints.Nationalities
{
    public class NationalitiesHandler
        : IRequestHandler<NationalitiesRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(NationalitiesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(NationalitiesHandler), async () =>
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
            "SELECT Id, NazevStatniPrislusnost 'Name' FROM [cis].[Zeme] WHERE Id>0 ORDER BY NazevStatniPrislusnost ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<NationalitiesHandler> _logger;

        public NationalitiesHandler(
            CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> connectionProvider,
            ILogger<NationalitiesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}

using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.AcademicDegreesAfter;

namespace DomainServices.CodebookService.Endpoints.AcademicDegreesAfter
{
    public class AcademicDegreesAfterHandler
        : IRequestHandler<AcademicDegreesAfterRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(AcademicDegreesAfterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(AcademicDegreesAfterHandler), async () =>
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
            "SELECT KOD 'Id', TEXT 'Name' FROM [SBR].[CIS_TITULY_ZA] ORDER BY TEXT ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<AcademicDegreesAfterHandler> _logger;

        public AcademicDegreesAfterHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<AcademicDegreesAfterHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}

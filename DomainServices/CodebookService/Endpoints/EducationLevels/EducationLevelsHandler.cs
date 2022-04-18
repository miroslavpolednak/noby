using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;

namespace DomainServices.CodebookService.Endpoints.EducationLevels
{
    public class EducationLevelsHandler
        : IRequestHandler<EducationLevelsRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(EducationLevelsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(EducationLevelsHandler), async () =>
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
            "SELECT ID_VZDELANI 'Id', NAZEV_VZDELANI 'Name' FROM [SBR].[CIS_VZDELANI] ORDER BY ID_VZDELANI ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<EducationLevelsHandler> _logger;

        public EducationLevelsHandler(

            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<EducationLevelsHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}

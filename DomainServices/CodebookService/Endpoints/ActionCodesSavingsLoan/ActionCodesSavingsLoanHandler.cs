using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ActionCodesSavings;

namespace DomainServices.CodebookService.Endpoints.ActionCodesSavings
{
    public class ActionCodesSavingsHandler
        : IRequestHandler<ActionCodesSavingsRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(ActionCodesSavingsRequest request, CancellationToken cancellationToken)
        {
            return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(ActionCodesSavingsHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItem>(_sqlQuery, cancellationToken)
            );
        }

        private const string _sqlQuery =
            "SELECT ID_AKCE_SPO 'Id', NAZEV_AKCE_SPO 'Name', CAST(CASE WHEN PLATNOST_DO_ES IS NULL THEN 1 ELSE 0 END as bit) 'IsActual' FROM SBR.AKCE_SPORENI ORDER BY NAZEV_AKCE_SPO ASC";
        
        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<ActionCodesSavingsHandler> _logger;

        public ActionCodesSavingsHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<ActionCodesSavingsHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}

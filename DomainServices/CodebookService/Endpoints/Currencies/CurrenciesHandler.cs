using DomainServices.CodebookService.Contracts.Endpoints.Currencies;

namespace DomainServices.CodebookService.Endpoints.Currencies;

public class CurrenciesHandler
    : IRequestHandler<CurrenciesRequest, List<CurrenciesItem>>
{
    public async Task<List<CurrenciesItem>> Handle(CurrenciesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<CurrenciesItem>(nameof(CurrenciesHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<CurrenciesItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = "SELECT DISTINCT MENA 'Code', POVOLENO_PRO_MENU_PRIJMU 'AllowedForIncomeCurrency', POVOLENO_PRO_MENU_BYDLISTE 'AllowedForResidencyCurrency', DEF 'IsDefault' FROM [SBR].[CIS_STATY] ORDER BY MENA ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<CurrenciesHandler> _logger;

    public CurrenciesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<CurrenciesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

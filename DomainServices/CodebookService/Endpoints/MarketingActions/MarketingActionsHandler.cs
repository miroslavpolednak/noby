using DomainServices.CodebookService.Contracts.Endpoints.MarketingActions;

namespace DomainServices.CodebookService.Endpoints.MarketingActions;

public class MarketingActionsHandler
    : IRequestHandler<MarketingActionsRequest, List<MarketingActionItem>>
{
    public async Task<List<MarketingActionItem>> Handle(MarketingActionsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<MarketingActionItem>(nameof(MarketingActionsHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<MarketingActionItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT KOD_MA_AKCIE 'Id', TYP_AKCIE 'Code', NULLIF(MANDANT, 0) 'MandantId', NAZOV 'Name', POPIS 'Description', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_MA_AKCIE] ORDER BY KOD_MA_AKCIE ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<MarketingActionsHandler> _logger;

    public MarketingActionsHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<MarketingActionsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

using DomainServices.CodebookService.Contracts.Endpoints.LoanKinds;

namespace DomainServices.CodebookService.Endpoints.LoanKinds;

public class LoanKindsHandler
    : IRequestHandler<LoanKindsRequest, List<LoanKindsItem>>
{
    public async Task<List<LoanKindsItem>> Handle(LoanKindsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<LoanKindsItem>(nameof(LoanKindsHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<LoanKindsItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT KOD 'Id', NULLIF(MANDANT, 0) 'MandantId', DRUH_UVERU_TEXT 'Name', CAST(DEFAULT_HODNOTA as bit) 'IsDefault', CASE WHEN SYSDATETIME() BETWEEN[DATUM_OD_ES] AND ISNULL([DATUM_DO_ES], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_DRUH_UVERU] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<LoanKindsHandler> _logger;

    public LoanKindsHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<LoanKindsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

using DomainServices.CodebookService.Contracts.Endpoints.Fees;

namespace DomainServices.CodebookService.Endpoints.Fees;

public class FeesHandler
    : IRequestHandler<FeesRequest, List<FeeItem>>
{
    public async Task<List<FeeItem>> Handle(FeesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<FeeItem>(nameof(FeesHandler), async () =>
            {
                return await _connectionProvider.ExecuteDapperRawSqlToList<FeeItem>(_sqlQuery, cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT POPLATEK_ID 'Id', POPLATEK_ID_KB 'IdKb', MANDANT 'MandantId', TEXT 'ShortName', TEXT_DOKUMENTACE 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].[CIS_POPLATKY_UV_DEF] ORDER BY POPLATEK_ID ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<FeesHandler> _logger;

    public FeesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<FeesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
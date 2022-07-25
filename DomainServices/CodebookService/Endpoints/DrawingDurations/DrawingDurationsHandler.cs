using DomainServices.CodebookService.Contracts.Endpoints.DrawingDurations;

namespace DomainServices.CodebookService.Endpoints.DrawingDurations;

public class DrawingDurationsHandler
    : IRequestHandler<DrawingDurationsRequest, List<DrawingDurationItem>>
{
    public async Task<List<DrawingDurationItem>> Handle(DrawingDurationsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<DrawingDurationItem>(nameof(DrawingDurationsHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<DrawingDurationItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT KOD 'Id', LHUTA_K_CERPANI 'DrawingDuration', DEF 'IsDefault', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].[CIS_LHUTA_K_CERPANI] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<DrawingDurationsHandler> _logger;

    public DrawingDurationsHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<DrawingDurationsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

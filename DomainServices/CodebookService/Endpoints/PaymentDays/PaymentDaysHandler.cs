using DomainServices.CodebookService.Contracts.Endpoints.PaymentDays;

namespace DomainServices.CodebookService.Endpoints.PaymentDays;

public class PaymentDaysHandler
    : IRequestHandler<PaymentDaysRequest, List<PaymentDayItem>>
{
    public async Task<List<PaymentDayItem>> Handle(PaymentDaysRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<PaymentDayItem>(nameof(PaymentDaysHandler), async () =>
        {
            return await _connectionProvider.ExecuteDapperRawSqlToList<PaymentDayItem>(_sqlQuery, cancellationToken);
        });
    }

    const string _sqlQuery = @"SELECT DEN_SPLACENI 'PaymentDay', DEN_ZAPOCTENI_SPLATKY 'PaymentAccountDay', NULLIF(MANDANT, 0) 'MandantId', DEF 'IsDefault', NABIZET_PORTAL 'ShowOnPortal'
                               FROM [SBR].[CIS_DEN_SPLACENI] ORDER BY DEN_SPLACENI ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<PaymentDaysHandler> _logger;

    public PaymentDaysHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<PaymentDaysHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

using DomainServices.CodebookService.Contracts.Endpoints.PaymentDays;

namespace DomainServices.CodebookService.Endpoints.PaymentDays;

public class PaymentDaysHandler
    : IRequestHandler<PaymentDaysRequest, List<PaymentDayItem>>
{
    public async Task<List<PaymentDayItem>> Handle(PaymentDaysRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<PaymentDayItem>(nameof(PaymentDaysHandler), async () =>
            {
                var items = await _connectionProvider.ExecuteDapperRawSqlToList<PaymentDayItem>(_sqlQuery, cancellationToken);

                var result = items.Select(i => new PaymentDayItem
                {
                    PaymentDay = i.PaymentDay,
                    PaymentAccountDay = i.PaymentAccountDay,
                    MandantId = i.MandantId,
                    IsDefault = i.IsDefault,
                }).ToList();

                return result;
            });
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT DEN_SPLACENI 'PaymentDay', DEN_ZAPOCTENI_SPLATKY 'PaymentAccountDay', MANDANT 'MandantId', DEF 'IsDefault'
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

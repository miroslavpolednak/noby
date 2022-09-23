using DomainServices.CodebookService.Contracts.Endpoints.PropertySettlements;

namespace DomainServices.CodebookService.Endpoints.PropertySettlements;

public class PropertySettlementsHandler
    : IRequestHandler<PropertySettlementsRequest, List<PropertySettlementItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<PropertySettlementsHandler> _logger;

    public PropertySettlementsHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<PropertySettlementsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    #endregion

    const string _sqlQuery = @"SELECT KOD 'Id', TEXT_CZE 'Name', TEXT_ENG 'NameEnglish', ROD_STAV 'MaritalStateId', PORADI 'Order', 
                                CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                                FROM [SBR].[CIS_VYPORADANI_MAJETKU] ORDER BY KOD ASC";

    private class PropertySettlementItemExt : PropertySettlementItem
    {
        public string? MaritalStateId { get; set; }
    }

    public async Task<List<PropertySettlementItem>> Handle(PropertySettlementsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<PropertySettlementItem>(nameof(PropertySettlementsHandler), async () =>
            {
                var items = await _connectionProvider.ExecuteDapperRawSqlToList<PropertySettlementItemExt>(_sqlQuery, cancellationToken);

                return items.Select(i => new PropertySettlementItem
                {
                    Id = i.Id,
                    Name = i.Name,
                    NameEnglish = i.NameEnglish,
                    MaritalStateIds = i.MaritalStateId?.ParseIDs(),
                    Order = i.Order,
                    IsValid = i.IsValid,

                }).ToList();
            });
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }
}

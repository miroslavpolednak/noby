using DomainServices.CodebookService.Contracts.Endpoints.NetMonthEarnings;

namespace DomainServices.CodebookService.Endpoints.NetMonthEarnings;

public class NetMonthEarningsHandler
    : IRequestHandler<NetMonthEarningsRequest, List<NetMonthEarningItem>>
{
    public async Task<List<NetMonthEarningItem>> Handle(NetMonthEarningsRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<NetMonthEarningItem>(nameof(NetMonthEarningsHandler), async () =>
        {
            // load extensions
            var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);
            var extMapperById = extMapper.ToDictionary(i => i.NetMonthEarningId);

            // load items
            var result = await _connectionProviderXxd.ExecuteDapperRawSqlToList<NetMonthEarningItem>(_sqlQuery, cancellationToken);

            // map RdmCode from extension table
            result.ForEach(t =>
            {
                t.RdmCode = extMapperById[t.Id].RdmCode;
            });

            return result;
        });
    }

    private class ExtensionMapper
    {
        public int NetMonthEarningId { get; set; }
        public string RdmCode { get; set; } = string.Empty;
    }
    
    // dotaz na rozsirene vlastnosti codebooku mimo SB
    const string _sqlQueryExtension = "SELECT NetMonthEarningId, RdmCode FROM dbo.NetMonthEarningsExtension";

    // dotaz na codebook do SB
    const string _sqlQuery = @"SELECT KOD 'Id', NAZEV 'Name' FROM [SBR].[CIS_AML_IDENTIFIKACE_PRIJMU] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProviderXxd;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<NetMonthEarningsHandler> _logger;
    private readonly IMediator _mediator;
    
    public NetMonthEarningsHandler(
        IMediator mediator,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProviderXxd,
        ILogger<NetMonthEarningsHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
        _connectionProviderXxd = connectionProviderXxd;
    }
}

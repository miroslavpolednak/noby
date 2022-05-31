using DomainServices.CodebookService.Contracts.Endpoints.MaritalStatuses;

namespace DomainServices.CodebookService.Endpoints.MaritalStatuses;

public class MaritalStatusesHandler
    : IRequestHandler<MaritalStatusesRequest, List<MaritalStatusItem>>
{
    public async Task<List<MaritalStatusItem>> Handle(MaritalStatusesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate(nameof(MaritalStatusesHandler), async () =>
            {
                var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);

                var result = await _connectionProvider.ExecuteDapperRawSqlToList<MaritalStatusItem>(_sqlQuery, cancellationToken);

                result.ForEach(t => {
                    t.RdmMaritalStatusCode = extMapper.FirstOrDefault(s => s.MaritalStatusId == t.Id)?.RDMCode;
                });

                return result;
            });
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    private class ExtensionMapper
    {
        public int MaritalStatusId { get; set; }
        public string? RDMCode { get; set; }
    }

    const string _sqlQuery = "SELECT KOD 'Id', TEXT 'Name', DEF 'IsDefault' FROM [SBR].[CIS_RODINNE_STAVY] ORDER BY KOD";
    const string _sqlQueryExtension = "Select * From MaritalStatusExtension";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<MaritalStatusesHandler> _logger;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public MaritalStatusesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<MaritalStatusesHandler> logger,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }
}
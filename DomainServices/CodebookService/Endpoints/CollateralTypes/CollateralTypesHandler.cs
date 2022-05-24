using DomainServices.CodebookService.Contracts.Endpoints.CollateralTypes;

namespace DomainServices.CodebookService.Endpoints.CollateralTypes;

public class CollateralTypesHandler
    : IRequestHandler<CollateralTypesRequest, List<CollateralTypeItem>>
{
    public async Task<List<CollateralTypeItem>> Handle(CollateralTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<CollateralTypeItem>(nameof(CollateralTypesHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<CollateralTypeItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT TYP_ZABEZPECENIA 'CollateralType', MANDANT 'MandantId', MANDANT 'Mandant', KOD_BGM 'CodeBgm', TEXT_BGM 'TextBgm', TEXT_K_TYPU 'NameType'
                               FROM [SBR].[CIS_VAHY_ZABEZPECENI] ORDER BY TYP_ZABEZPECENIA ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<CollateralTypesHandler> _logger;

    public CollateralTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<CollateralTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

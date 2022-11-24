using DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes;

namespace DomainServices.CodebookService.Endpoints.ObligationTypes;

public class ObligationTypesHandler
    : IRequestHandler<ObligationTypesRequest, List<ObligationTypesItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<ObligationTypesHandler> _logger;

    public ObligationTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        ILogger<ObligationTypesHandler> logger)
    {
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
        _connectionProvider = connectionProvider;
    }

    #endregion

    #region Extension

    private class ObligationTypeExtension
    {
        public int ObligationTypeId { get; set; }
        public string ObligationProperty { get; set; } = null!;
    }

    #endregion

    // dotaz na codebook do SB
    const string _sql = @"SELECT KOD 'Id', CODE 'Code', TEXT 'Name', KOREKCE_ZAVAZKU 'ObligationCorrectionTypeId', PORADIE 'Order', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].CIS_DRUH_ZAVAZKU ORDER BY KOD ASC";

    // dotaz na extenstion
    const string _sqlExtension = "SELECT [ObligationTypeId], [ObligationProperty] FROM [dbo].[ObligationTypeExtension]";


    public async Task<List<ObligationTypesItem>> Handle(ObligationTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<ObligationTypesItem>(nameof(ObligationTypesHandler), async () =>
        {
            // load codebook items
            var items = await _connectionProvider.ExecuteDapperRawSqlToList<ObligationTypesItem>(_sql, cancellationToken);

            // load extensions
            var extensions = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ObligationTypeExtension>(_sqlExtension, cancellationToken);
            var dictExtensions = extensions.ToDictionary(i => i.ObligationTypeId);

            // assign ObligationProperty mapping to items
            items.ForEach(item =>
            {
                var itemExt = dictExtensions.ContainsKey(item.Id) ? dictExtensions[item.Id] : null;
                item.ObligationProperty = itemExt?.ObligationProperty;
            });

            return items;
        });
    }
}

using DomainServices.CodebookService.Contracts.Endpoints.EaCodesMain;

namespace DomainServices.CodebookService.Endpoints.EaCodesMain;

public class EaCodesMainHandler
    : IRequestHandler<EaCodesMainRequest, List<EaCodeMainItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    
    private readonly ILogger<EaCodesMainHandler> _logger;

    public EaCodesMainHandler(
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        ILogger<EaCodesMainHandler> logger)
    {
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    #endregion


    // dotaz na codebook do SB
    const string _sql = @"SELECT KOD 'Id', POPIS 'Name', popis_klient 'DescriptionForClient', KATEGORIE 'Category', DRUH_KB 'KindKb', viditelnost_ps_kb_prodejni_sit_kb 'IsVisibleForKb', viditelnost_pro_vlozeni_noby 'IsInsertingAllowedNoby'
                          , CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                          FROM [dbo].[EA_CIS_EACODEMAIN] ORDER BY kod ASC";

    const string _sqlExtension = "SELECT EaCodesMainId, IsFormIdRequested FROM dbo.EaCodesMainExtension";

    private class ExtensionMapper
    {
        public int EaCodesMainId { get; set; }
        public bool IsFormIdRequested { get; set; }
    }

    public async Task<List<EaCodeMainItem>> Handle(EaCodesMainRequest request, CancellationToken cancellationToken)
    {
        //return await FastMemoryCache.GetOrCreate<EaCodeMainItem>(nameof(EaCodesMainHandler), async () => { return await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<EaCodeMainItem>(_sql, cancellationToken); });

        return await FastMemoryCache.GetOrCreate<EaCodeMainItem>(nameof(EaCodesMainHandler), async () =>
        {

            // load codebook items
            var items = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<EaCodeMainItem>(_sql, cancellationToken);

            // load extensions
            var extensions = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlExtension, cancellationToken);
            var dictExtensions = extensions.ToDictionary(i => i.EaCodesMainId);

            // assign extension data to items
            items.ForEach(item =>
            {
                var itemExt = dictExtensions.ContainsKey(item.Id) ? dictExtensions[item.Id] : null;
                item.IsFormIdRequested = itemExt == null ? false : itemExt.IsFormIdRequested;
            });

            return items;
        });
    }
}
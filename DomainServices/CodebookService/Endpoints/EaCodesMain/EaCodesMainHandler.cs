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

    public async Task<List<EaCodeMainItem>> Handle(EaCodesMainRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<EaCodeMainItem>(nameof(EaCodesMainHandler), async () =>
            {
                // load codebook items
                return await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<EaCodeMainItem>(_sql, cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }
}


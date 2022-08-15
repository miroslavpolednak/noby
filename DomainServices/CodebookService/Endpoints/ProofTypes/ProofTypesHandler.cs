using DomainServices.CodebookService.Contracts.Endpoints.ProofTypes;

namespace DomainServices.CodebookService.Endpoints.ProofTypes;

public class ProofTypesHandler
    : IRequestHandler<ProofTypesRequest, List<ProofTypeItem>>
{
    public async Task<List<ProofTypeItem>> Handle(ProofTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<ProofTypeItem>(nameof(ProofTypesHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<ProofTypeItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT KOD 'Id', CODE 'Code', TEXT_CZE 'Name', TEXT_ENG 'NameEnglish', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].[CIS_TYP_POTVRDENIE_PRIJMU] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<ProofTypesHandler> _logger;

    public ProofTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<ProofTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

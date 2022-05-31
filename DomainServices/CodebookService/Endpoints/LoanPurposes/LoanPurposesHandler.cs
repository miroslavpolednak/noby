using DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes;

namespace DomainServices.CodebookService.Endpoints.LoanPurposes;

public class LoanPurposesHandler
    : IRequestHandler<LoanPurposesRequest, List<LoanPurposesItem>>
{
    public async Task<List<LoanPurposesItem>> Handle(LoanPurposesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<LoanPurposesItem>(nameof(LoanPurposesHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<LoanPurposesItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT KOD 'Id', TEXT 'Name', MANDANT 'MandantId', MANDANT 'Mandant', KOD_UVER 'ProductTypeId', PORADI 'Order', CASE WHEN SYSDATETIME() BETWEEN [DATUM_PLATNOSTI_OD] AND ISNULL([DATUM_PLATNOSTI_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid'
                                FROM SBR.CIS_UCEL_UVERU_INT1 ORDER BY KOD";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<LoanPurposesHandler> _logger;

    public LoanPurposesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<LoanPurposesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
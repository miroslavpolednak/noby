using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ObligationCorrectionTypes;

namespace DomainServices.CodebookService.Endpoints.ObligationCorrectionTypes;

public class ObligationCorrectionTypesHandler
    : IRequestHandler<ObligationCorrectionTypesRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(ObligationCorrectionTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(ObligationCorrectionTypesHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    private const string _sqlQuery =
            "SELECT KOD 'Id', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN PLATNOST_OD AND ISNULL(PLATNOST_DO, '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM SBR.CIS_KOREKCE_ZAVAZKU ORDER BY KOD";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<ObligationCorrectionTypesHandler> _logger;

    public ObligationCorrectionTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<ObligationCorrectionTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

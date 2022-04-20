using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ActionCodesSavingsLoan;

namespace DomainServices.CodebookService.Endpoints.ActionCodesSavingsLoan;

public class ActionCodesSavingsLoanHandler
    : IRequestHandler<ActionCodesSavingsLoanRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(ActionCodesSavingsLoanRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(ActionCodesSavingsLoanHandler), async () =>
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
        "SELECT ID_AKCE_UV 'Id', NAZEV_AKCE_UV 'Name', CAST(CASE WHEN PLATNOST_DO_ES IS NULL THEN 1 ELSE 0 END as bit) 'IsActual' FROM [SBR].[AKCE_UV] ORDER BY NAZEV_AKCE_UV ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<ActionCodesSavingsLoanHandler> _logger;

    public ActionCodesSavingsLoanHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<ActionCodesSavingsLoanHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

using CIS.Core.Data;
using DomainServices.CodebookService.Contracts.Endpoints.TinNoFillReasonsByCountry;

namespace DomainServices.CodebookService.Endpoints.TinNoFillReasonsByCountry;

public class TinNoFillReasonsByCountryHandler
    : IRequestHandler<TinNoFillReasonsByCountryRequest, List<TinNoFillReasonItem>>
{
    #region Construction

    private readonly IConnectionProvider _connectionProvider;

    private const string _sqlQuery =
        @"SELECT Id, IsTinMandatory, ReasonForBlankTin,
          CASE WHEN SYSDATETIME() BETWEEN[ValidFrom] AND ISNULL([ValidTo], '9999-12-31') THEN 1 ELSE 0 END 'IsValid'
          FROM [dbo].[TinNoFillReasonsByCountry]";

    public TinNoFillReasonsByCountryHandler(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    #endregion

    public async Task<List<TinNoFillReasonItem>> Handle(TinNoFillReasonsByCountryRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<TinNoFillReasonItem>(nameof(TinNoFillReasonsByCountryHandler), async () =>
            await _connectionProvider.ExecuteDapperRawSqlToList<TinNoFillReasonItem>(_sqlQuery, cancellationToken)
        );
    }
}

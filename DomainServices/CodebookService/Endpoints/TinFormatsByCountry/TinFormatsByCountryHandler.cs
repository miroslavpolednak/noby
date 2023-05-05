using CIS.Core.Data;
using DomainServices.CodebookService.Contracts.Endpoints.TinFormatsByCountry;

namespace DomainServices.CodebookService.Endpoints.TinFormatsByCountry;

public class TinFormatsByCountryHandler
    : IRequestHandler<TinFormatsByCountryRequest, List<TinFormatItem>>
{
    #region Construction

    private readonly IConnectionProvider _connectionProvider;

    private const string _sqlQuery =
        @"SELECT Id, CountryCode, RegularExpression, IsForFo, Tooltip,
          CASE WHEN SYSDATETIME() BETWEEN[ValidFrom] AND ISNULL([ValidTo], '9999-12-31') THEN 1 ELSE 0 END 'IsValid'
          FROM [dbo].[TinFormatsByCountry]";

    public TinFormatsByCountryHandler(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    #endregion

    public async Task<List<TinFormatItem>> Handle(TinFormatsByCountryRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<TinFormatItem>(nameof(TinFormatsByCountryHandler), async () =>
            await _connectionProvider.ExecuteDapperRawSqlToList<TinFormatItem>(_sqlQuery, cancellationToken)
        );
    }
}

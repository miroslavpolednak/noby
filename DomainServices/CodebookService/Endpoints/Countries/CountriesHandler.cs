using DomainServices.CodebookService.Contracts.Endpoints.Countries;

namespace DomainServices.CodebookService.Endpoints.Countries
{
    public class CountriesHandler
        : IRequestHandler<CountriesRequest, List<CountriesItem>>
    {
        public async Task<List<CountriesItem>> Handle(CountriesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<CountriesItem>(nameof(CountriesHandler), async () =>
                    await _connectionProvider.ExecuteDapperRawSqlToList<CountriesItem>(_sqlQuery, cancellationToken)
                );
            }
            catch (Exception ex)
            {
                _logger.GeneralException(ex);
                throw;
            }
        }

        const string _sqlQuery = "SELECT KOD 'Id', SKRATKA 'ShortName', TEXT 'Name', TEXT_CELY 'LongName', DEF 'IsDefault', RIZIKOVOST 'Risk', CLEN_EU 'EuMember', EUROZONA 'Eurozone' FROM [SBR].[CIS_STATY] WHERE KOD != -1 ORDER BY KOD ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<CountriesHandler> _logger;

        public CountriesHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<CountriesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}

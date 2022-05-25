using DomainServices.CodebookService.Contracts.Endpoints.StatementTypes;

namespace DomainServices.CodebookService.Endpoints.StatementTypes
{
    public class StatementTypesHandler
        : IRequestHandler<StatementTypesRequest, List<StatementTypeItem>>
    {
        public async Task<List<StatementTypeItem>> Handle(StatementTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<StatementTypeItem>(nameof(StatementTypesHandler), async () =>
                    await _connectionProvider.ExecuteDapperRawSqlToList<StatementTypeItem>(_sqlQuery, cancellationToken)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private const string _sqlQuery =
            "SELECT KOD 'Id', C_POPIS_R_CZ 'Name', C_POPIS_CZ 'ShortName', N_SORT_ORDER 'Order', CASE WHEN SYSDATETIME() BETWEEN[VALID_FROM] AND ISNULL([VALID_TO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_HU_TYP_VYPIS] ORDER BY KOD ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<StatementTypesHandler> _logger;

        public StatementTypesHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<StatementTypesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}
using DomainServices.CodebookService.Contracts.Endpoints.HousingConditions;

namespace DomainServices.CodebookService.Endpoints.HousingConditions
{
    public class HousingConditionsHandler
        : IRequestHandler<HousingConditionsRequest, List<HousingConditionItem>>
    {

        #region Construction

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<HousingConditionsHandler> _logger;

        public HousingConditionsHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<HousingConditionsHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        #endregion

        // dotaz na codebook do SB
        const string _sql = @"SELECT KOD 'Id', TEXT 'Name', CODE 'Code', CODE 'RdmCode', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                                FROM [SBR].[CIS_FORMA_BYVANIA] ORDER BY KOD ASC";

        public async Task<List<HousingConditionItem>> Handle(HousingConditionsRequest request, CancellationToken cancellationToken)
        {
            return await FastMemoryCache.GetOrCreate(nameof(HousingConditionsHandler), async () =>
            {
                // load codebook items
                return await _connectionProvider.ExecuteDapperRawSqlToList<HousingConditionItem>(_sql, cancellationToken);
            });
        }
    }
}
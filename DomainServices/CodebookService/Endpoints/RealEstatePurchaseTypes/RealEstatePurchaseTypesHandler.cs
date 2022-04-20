using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.RealEstatePurchaseTypes;

namespace DomainServices.CodebookService.Endpoints.RealEstatePurchaseTypes
{
    public class RealEstatePurchaseTypesHandler
        : IRequestHandler<RealEstatePurchaseTypesRequest, List<RealEstatePurchaseTypeItem>>
    {
        public async Task<List<RealEstatePurchaseTypeItem>> Handle(RealEstatePurchaseTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<RealEstatePurchaseTypeItem>(nameof(RealEstatePurchaseTypesHandler), async () =>
                    await _connectionProvider.ExecuteDapperRawSqlToList<RealEstatePurchaseTypeItem>(_sqlQuery, cancellationToken)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private const string _sqlQuery =
            "SELECT KOD 'Id', POPIS 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid', DEF 'IsDefault', PORADI 'ORDER' FROM [SBR].[CIS_UCEL_PORIZENI_UV] ORDER BY PORADI ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<RealEstatePurchaseTypesHandler> _logger;

        public RealEstatePurchaseTypesHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<RealEstatePurchaseTypesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;

namespace DomainServices.CodebookService.Endpoints.SalesArrangementTypes
{
    public class SalesArrangementTypesHandler
        : IRequestHandler<SalesArrangementTypesRequest, List<SalesArrangementTypeItem>>
    {
        public async Task<List<SalesArrangementTypeItem>> Handle(SalesArrangementTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<SalesArrangementTypeItem>(nameof(SalesArrangementTypesHandler), async () =>
                    await _connectionProvider.ExecuteDapperRawSqlToList<SalesArrangementTypeItem>(_sqlQuery, cancellationToken)
                );
            }
            catch (Exception ex)
            {
                _logger.GeneralException(ex);
                throw;
            }
        }

        private const string _sqlQuery = "SELECT Id, Name, ProductTypeId, SalesArrangementCategory FROM [dbo].[SalesArrangementType]";

        private readonly CIS.Core.Data.IConnectionProvider _connectionProvider;
        private readonly ILogger<SalesArrangementTypesHandler> _logger;

        public SalesArrangementTypesHandler(
            CIS.Core.Data.IConnectionProvider connectionProvider,
            ILogger<SalesArrangementTypesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}

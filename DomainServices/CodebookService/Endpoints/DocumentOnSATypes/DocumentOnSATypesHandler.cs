using DomainServices.CodebookService.Contracts.Endpoints.DocumentOnSATypes;

namespace DomainServices.CodebookService.Endpoints.DocumentOnSATypes
{
    public class DocumentOnSATypesHandler
        : IRequestHandler<DocumentOnSATypesRequest, List<DocumentOnSATypeItem>>
    {
        public async Task<List<DocumentOnSATypeItem>> Handle(DocumentOnSATypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<DocumentOnSATypeItem>(nameof(DocumentOnSATypesHandler), async () =>
                    await _connectionProvider.ExecuteDapperRawSqlToList<DocumentOnSATypeItem>(_sqlQuery, cancellationToken)
                );
            }
            catch (Exception ex)
            {
                _logger.GeneralException(ex);
                throw;
            }
        }

        private const string _sqlQuery = "SELECT Id, Name, SalesArrangementTypeId, FormTypeId FROM [dbo].[DocumentOnSAType]";

        private readonly CIS.Core.Data.IConnectionProvider _connectionProvider;
        private readonly ILogger<DocumentOnSATypesHandler> _logger;

        public DocumentOnSATypesHandler(
            CIS.Core.Data.IConnectionProvider connectionProvider,
            ILogger<DocumentOnSATypesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}

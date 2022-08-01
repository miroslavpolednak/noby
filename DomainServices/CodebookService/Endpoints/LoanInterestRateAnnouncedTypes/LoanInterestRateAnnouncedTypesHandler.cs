using DomainServices.CodebookService.Contracts.Endpoints.LoanInterestRateAnnouncedTypes;

namespace DomainServices.CodebookService.Endpoints.LoanInterestRateAnnouncedTypes
{
    public class LoanInterestRateAnnouncedTypesHandler
        : IRequestHandler<LoanInterestRateAnnouncedTypesRequest, List<LoanInterestRateAnnouncedTypeItem>>
    {
        public async Task<List<LoanInterestRateAnnouncedTypeItem>> Handle(LoanInterestRateAnnouncedTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<LoanInterestRateAnnouncedTypeItem>(nameof(LoanInterestRateAnnouncedTypesHandler), async () =>
                    await _connectionProvider.ExecuteDapperRawSqlToList<LoanInterestRateAnnouncedTypeItem>(_sqlQuery, cancellationToken)
                );
            }
            catch (Exception ex)
            {
                _logger.GeneralException(ex);
                throw;
            }
        }

        private const string _sqlQuery = "SELECT Id, Code, Name FROM [dbo].[LoanInterestRateAnnouncedType] ORDER BY Id ASC";

        private readonly CIS.Core.Data.IConnectionProvider _connectionProvider;
        private readonly ILogger<LoanInterestRateAnnouncedTypesHandler> _logger;

        public LoanInterestRateAnnouncedTypesHandler(
            CIS.Core.Data.IConnectionProvider connectionProvider,
            ILogger<LoanInterestRateAnnouncedTypesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}

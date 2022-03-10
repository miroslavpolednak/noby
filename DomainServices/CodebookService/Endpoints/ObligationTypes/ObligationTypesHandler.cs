using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes;

namespace DomainServices.CodebookService.Endpoints.ObligationTypes
{
    public class ObligationTypesHandler
        : IRequestHandler<ObligationTypesRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(ObligationTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found ObligationTypes in cache");

                    return await _cache.GetAllAsync<GenericCodebookItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading ObligationTypes from database");

                    return GetMockData(); // TODO: Redirect to real data source!                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private List<GenericCodebookItem> GetMockData()
        {
            return new List<GenericCodebookItem>
            {
                new GenericCodebookItem() { Id = 1, Name = "Hypotéka" },            // code MORTGAGE
                new GenericCodebookItem() { Id = 2, Name = "Spotřební úvěr" },      // code UTILITY_LOAN
                new GenericCodebookItem() { Id = 3, Name = "Kreditní karta" },      // code CREDIT_CARD
                new GenericCodebookItem() { Id = 4, Name = "Debet / Kontokorent" }, // code DEBIT
                new GenericCodebookItem() { Id = 5, Name = "Nebankovní půjčka" },   // code NON_BANK_LOAN
            };
        }

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<ObligationTypesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public ObligationTypesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<ObligationTypesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "ObligationTypes";
    }
}
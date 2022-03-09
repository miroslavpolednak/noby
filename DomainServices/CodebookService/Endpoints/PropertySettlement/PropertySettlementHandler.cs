using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.PropertySettlement;

namespace DomainServices.CodebookService.Endpoints.PropertySettlement
{
    public class PropertySettlementHandler
        : IRequestHandler<PropertySettlementRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(PropertySettlementRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found PropertySettlement in cache");

                    return await _cache.GetAllAsync<GenericCodebookItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading PropertySettlement from database");

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
                new GenericCodebookItem() { Id = 1, Name = "Unknown" },
                new GenericCodebookItem() { Id = 2, Name = "Společné jmění manželů" },
            };
        }

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<PropertySettlementHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public PropertySettlementHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<PropertySettlementHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "PropertySettlement";
    }
}
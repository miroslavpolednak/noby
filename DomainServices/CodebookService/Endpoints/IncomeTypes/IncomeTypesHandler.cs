using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.IncomeTypes;

namespace DomainServices.CodebookService.Endpoints.IncomeTypes
{
    public class IncomeTypesHandler
        : IRequestHandler<IncomeTypesRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(IncomeTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found IncomeTypes in cache");

                    return await _cache.GetAllAsync<GenericCodebookItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading IncomeTypes from database");

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
                new GenericCodebookItem() { Id = 1, Name = "Ze zaměstnání" },
                new GenericCodebookItem() { Id = 2, Name = "Z podnikání" },
                new GenericCodebookItem() { Id = 3, Name = "Z pronájmu" },
                new GenericCodebookItem() { Id = 4, Name = "Ostatní" },
            };
        }

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<IncomeTypesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public IncomeTypesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<IncomeTypesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "IncomeTypes";
    }
}
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.IncomeAbroadTypes;

namespace DomainServices.CodebookService.Endpoints.IncomeAbroadTypes;

public class IncomeAbroadTypesHandler
    : IRequestHandler<IncomeAbroadTypesRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(IncomeAbroadTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.LogDebug("Found IncomeAbroadTypes in cache");

                return await _cache.GetAllAsync<GenericCodebookItem>(_cacheKey);
            }
            else
            {
                _logger.LogDebug("Reading IncomeAbroadTypes from database");

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
            new GenericCodebookItem() { Id = 1, Name = "expat" },
            new GenericCodebookItem() { Id = 2, Name = "pendler" },
            new GenericCodebookItem() { Id = 3, Name = "cizinec s příjmem v zahraničí" },
            new GenericCodebookItem() { Id = 4, Name = "příjem ze zahr. S výkonem zam. V ČR" },
        };
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<IncomeAbroadTypesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public IncomeAbroadTypesHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<IncomeAbroadTypesHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "IncomeAbroadTypes";
}

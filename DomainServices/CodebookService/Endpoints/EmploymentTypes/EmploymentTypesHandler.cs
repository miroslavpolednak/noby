using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.EmploymentTypes;

namespace DomainServices.CodebookService.Endpoints.EmploymentTypes;

public class EmploymentTypesHandler
    : IRequestHandler<EmploymentTypesRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(EmploymentTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.LogDebug("Found EmploymentTypes in cache");

                return await _cache.GetAllAsync<GenericCodebookItem>(_cacheKey);
            }
            else
            {
                _logger.LogDebug("Reading EmploymentTypes from database");

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
            new GenericCodebookItem() { Id = 1, Name = "pronájem existující" },
            new GenericCodebookItem() { Id = 2, Name = "pronájem budoucí" },
            new GenericCodebookItem() { Id = 3, Name = "prac.poměr - doba určitá" },
            new GenericCodebookItem() { Id = 4, Name = "prac.poměr - doba neurčitá" },
            new GenericCodebookItem() { Id = 5, Name = "prac.poměr - dpp" },
            new GenericCodebookItem() { Id = 6, Name = "prac.poměr - dpc" },
        };
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<EmploymentTypesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public EmploymentTypesHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<EmploymentTypesHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "EmploymentTypes";
}

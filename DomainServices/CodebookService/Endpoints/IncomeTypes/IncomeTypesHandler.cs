using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.IncomeTypes;

namespace DomainServices.CodebookService.Endpoints.IncomeTypes;

public class IncomeTypesHandler
    : IRequestHandler<IncomeTypesRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(IncomeTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: Redirect to real data source!  
            return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(IncomeTypesHandler), async () => GetMockData());
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

    private readonly ILogger<IncomeTypesHandler> _logger;

    public IncomeTypesHandler(
        ILogger<IncomeTypesHandler> logger)
    {
        _logger = logger;
    }
}

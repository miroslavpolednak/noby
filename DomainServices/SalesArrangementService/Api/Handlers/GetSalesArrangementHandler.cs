using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementHandler
    : IRequestHandler<Dto.GetSalesArrangementMediatrRequest, SalesArrangement>
{
    public async Task<SalesArrangement> Handle(Dto.GetSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Get SA #{id}", request.SalesArrangementId);

        var saInstance = await _repository.GetSalesArrangement(request.SalesArrangementId, cancellation);
        
        return saInstance;
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementHandler> _logger;

    public GetSalesArrangementHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

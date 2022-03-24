using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementHandler
    : IRequestHandler<Dto.GetSalesArrangementMediatrRequest, _SA.SalesArrangement>
{
    public async Task<_SA.SalesArrangement> Handle(Dto.GetSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangementHandler), request.SalesArrangementId);

        return await _repository.GetSalesArrangement(request.SalesArrangementId, cancellation);
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

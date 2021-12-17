namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class LinkModelationToSalesArrangementHandler
    : IRequestHandler<Dto.LinkModelationToSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkModelationToSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Link offer {offerInstanceId} to {salesArrangementId}", request.OfferInstanceId, request.SalesArrangementId);

        await _repository.UpdateOfferInstanceId(request.SalesArrangementId, request.OfferInstanceId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<LinkModelationToSalesArrangementHandler> _logger;

    public LinkModelationToSalesArrangementHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<LinkModelationToSalesArrangementHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

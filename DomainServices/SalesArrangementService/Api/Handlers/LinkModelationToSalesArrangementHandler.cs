namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class LinkModelationToSalesArrangementHandler
    : IRequestHandler<Dto.LinkModelationToSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkModelationToSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LinkToModelationStarted(request.OfferInstanceId, request.SalesArrangementId);

        // overit existenci SA
        await _repository.EnsureExistingSalesArrangement(request.SalesArrangementId, cancellation);

        // validace na existenci offerInstance
        /*var offerInstance = CIS.Core.Results.ServiceCallResult.ResolveToDefault<OfferService.Contracts>(await _offerService.(request.OfferInstanceId, cancellation))
            ?? throw new CisNotFoundException(16001, $"OfferInstance ID #{request.Request.OfferInstanceId} does not exist.");*/

        // update linku v DB
        await _repository.UpdateOfferInstanceId(request.SalesArrangementId, request.OfferInstanceId, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<LinkModelationToSalesArrangementHandler> _logger;

    public LinkModelationToSalesArrangementHandler(
        OfferService.Abstraction.IOfferServiceAbstraction offerService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<LinkModelationToSalesArrangementHandler> logger)
    {
        _offerService = offerService;
        _repository = repository;
        _logger = logger;
    }
}

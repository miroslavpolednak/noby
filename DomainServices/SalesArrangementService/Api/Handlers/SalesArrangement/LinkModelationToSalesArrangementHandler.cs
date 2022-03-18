namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class LinkModelationToSalesArrangementHandler
    : IRequestHandler<Dto.LinkModelationToSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkModelationToSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LinkToModelationStarted(request.OfferId, request.SalesArrangementId);

        // overit existenci SA
        var salesArrangementInstance = await _repository.GetSalesArrangement(request.SalesArrangementId, cancellation);

        // kontrola zda SA uz neni nalinkovan na stejnou Offer na kterou je request
        if (salesArrangementInstance.OfferId == request.OfferId)
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.InvalidArgument, $"SalesArrangement {request.SalesArrangementId} is already linked to Offer {request.OfferId}", 16012);

        // validace na existenci offer
        var offerInstance = ServiceCallResult.ResolveToDefault<DomainServices.OfferService.Contracts.GetOfferResponse>(await _offerService.GetOffer(request.OfferId, cancellation))
            ?? throw new CisNotFoundException(16001, $"Offer ID #{request.OfferId} does not exist.");

        // update linku v DB
        await _repository.UpdateOfferId(request.SalesArrangementId, request.OfferId, Guid.Parse(offerInstance.ResourceProcessId), cancellation);

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

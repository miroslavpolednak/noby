using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using _Offer = DomainServices.OfferService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class LinkModelationToSalesArrangementHandler
    : IRequestHandler<Dto.LinkModelationToSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkModelationToSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LinkToModelationStarted(request.OfferId, request.SalesArrangementId);

        // overit existenci SA
        var salesArrangementInstance = await _dbContext.SalesArrangements.FindAsync(new object[] { request.SalesArrangementId }, cancellation) 
            ?? throw new CisNotFoundException(16000, $"Sales arrangement ID {request.SalesArrangementId} does not exist.");
        
        // kontrola zda SA uz neni nalinkovan na stejnou Offer na kterou je request
        if (salesArrangementInstance.OfferId == request.OfferId)
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"SalesArrangement {request.SalesArrangementId} is already linked to Offer {request.OfferId}", 16012);

        // validace na existenci offer
        var offerInstance = ServiceCallResult.ResolveToDefault<_Offer.GetOfferResponse>(await _offerService.GetOffer(request.OfferId, cancellation))
            ?? throw new CisNotFoundException(16001, $"Offer ID #{request.OfferId} does not exist.");

        // kontrola, zda simulace neni nalinkovana na jiny SA
        if (await _dbContext.SalesArrangements.AnyAsync(t => t.OfferId == request.OfferId, cancellation))
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"Offer {request.OfferId} is already linked to another SA", 16012);

        // update linku v DB
        salesArrangementInstance.OfferId = request.OfferId;
        salesArrangementInstance.ResourceProcessId = Guid.Parse(offerInstance.ResourceProcessId);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<LinkModelationToSalesArrangementHandler> _logger;

    public LinkModelationToSalesArrangementHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        OfferService.Abstraction.IOfferServiceAbstraction offerService,
        ILogger<LinkModelationToSalesArrangementHandler> logger)
    {
        _dbContext = dbContext;
        _offerService = offerService;
        _logger = logger;
    }
}

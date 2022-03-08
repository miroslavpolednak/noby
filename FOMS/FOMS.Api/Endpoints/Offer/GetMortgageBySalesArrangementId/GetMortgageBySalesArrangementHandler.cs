using Abstraction = DomainServices.SalesArrangementService.Abstraction;
using DSContracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal class GetMortgageBySalesArrangementHandler
    : IRequestHandler<GetMortgageBySalesArrangementRequest, Dto.GetMortgageResponse>
{
    public async Task<Dto.GetMortgageResponse> Handle(GetMortgageBySalesArrangementRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetMortgageBySalesArrangementHandler), request.SalesArrangementId);

        // ziskat offerId z SA
        var salesArrangementInstance = ServiceCallResult.Resolve<DSContracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
        
        // kontrola, zda ma SA OfferId
        if (!salesArrangementInstance.OfferId.HasValue)
            throw new CisArgumentNullException(ErrorCodes.SalesArrangementNotLinkedToOffer, "SalesArrangement is not linked to any Offer", nameof(request));
        
        return await _mediator.Send(new GetMortgageByOfferId.GetMortgageByOfferIdRequest(salesArrangementInstance.OfferId.Value), cancellationToken);
    }

    private readonly Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly IMediator _mediator;
    private readonly ILogger<GetMortgageBySalesArrangementHandler> _logger;
    
    public GetMortgageBySalesArrangementHandler(
        IMediator mediator, 
        ILogger<GetMortgageBySalesArrangementHandler> logger, 
        Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _logger = logger;
        _mediator = mediator;
    }
}
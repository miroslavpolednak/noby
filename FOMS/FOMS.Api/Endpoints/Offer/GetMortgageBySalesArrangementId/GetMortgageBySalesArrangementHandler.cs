using CIS.Infrastructure.gRPC;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class GetMortgageBySalesArrangementHandler
    : IRequestHandler<Dto.GetMortgageBySalesArrangementRequest, Dto.GetMortgageResponse>
{
    public async Task<Dto.GetMortgageResponse> Handle(Dto.GetMortgageBySalesArrangementRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetMortgageBySalesArrangementHandler), request.SalesArrangementId);

        // ziskat offerId z SA
        var salesArrangementInstance = ServiceCallResult.Resolve<DomainServices.SalesArrangementService.Contracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
        
        // kontrola, zda ma SA OfferId
        if (!salesArrangementInstance.OfferId.HasValue)
            throw new ArgumentNullException(nameof(request.SalesArrangementId), "SalesArrangement is not linked to any Offer");
        
        return await _mediator.Send(new Dto.GetMortgageRequest(salesArrangementInstance.OfferId.Value));
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly IMediator _mediator;
    private readonly ILogger<GetMortgageBySalesArrangementHandler> _logger;
    
    public GetMortgageBySalesArrangementHandler(IMediator mediator, ILogger<GetMortgageBySalesArrangementHandler> logger, DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _logger = logger;
        _mediator = mediator;
    }
}
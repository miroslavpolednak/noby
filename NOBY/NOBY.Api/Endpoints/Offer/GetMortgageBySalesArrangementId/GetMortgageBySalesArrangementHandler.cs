﻿using Abstraction = DomainServices.SalesArrangementService.Clients;
using DSContracts = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal sealed class GetMortgageBySalesArrangementHandler
    : IRequestHandler<GetMortgageBySalesArrangementRequest, Dto.GetMortgageResponse>
{
    public async Task<Dto.GetMortgageResponse> Handle(GetMortgageBySalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // ziskat offerId z SA
        var salesArrangementInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        
        // kontrola, zda ma SA OfferId
        if (!salesArrangementInstance.OfferId.HasValue)
            throw new NobyValidationException("SalesArrangement is not linked to any Offer");
         
        return await _mediator.Send(new GetMortgageByOfferId.GetMortgageByOfferIdRequest(salesArrangementInstance.OfferId.Value), cancellationToken);
    }

    private readonly Abstraction.ISalesArrangementServiceClient _salesArrangementService;
    private readonly IMediator _mediator;
    
    public GetMortgageBySalesArrangementHandler(
        IMediator mediator,
        Abstraction.ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _mediator = mediator;
    }
}
﻿using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuation;

internal sealed class CreateRealEstateValuationHandler
    : IRequestHandler<CreateRealEstateValuationRequest>
{
    public async Task Handle(CreateRealEstateValuationRequest request, CancellationToken cancellationToken)
    {

        await _realEstateValuationService.CreateRealEstateValuation(new DomainServices.RealEstateValuationService.Contracts.CreateRealEstateValuationRequest
        {
            CaseId = request.CaseId
        }, cancellationToken);
    }

    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public CreateRealEstateValuationHandler(
        IRealEstateValuationServiceClient realEstateValuationService, 
        ICaseServiceClient caseService)
    {
        _realEstateValuationService = realEstateValuationService;
        _caseService = caseService;
    }
}
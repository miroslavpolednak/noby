﻿using DomainServices.HouseholdService.Clients;

namespace FOMS.Api.Endpoints.CustomerObligation.DeleteObligation;

internal class DeleteObligationHandler
    : AsyncRequestHandler<DeleteObligationRequest>
{
    protected override async Task Handle(DeleteObligationRequest request, CancellationToken cancellationToken)
    {
        ServiceCallResult.Resolve(await _customerService.DeleteObligation(request.ObligationId, cancellationToken));
    }

    private readonly ICustomerOnSAServiceClient _customerService;
    
    public DeleteObligationHandler(ICustomerOnSAServiceClient customerService)
    {
        _customerService = customerService;
    }
}

﻿using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerObligation.UpdateObligation;

internal class UpdateObligationHandler
    : AsyncRequestHandler<UpdateObligationRequest>
{
    protected override async Task Handle(UpdateObligationRequest request, CancellationToken cancellationToken)
    {
        var obligationInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.SalesArrangementService.Contracts.Obligation>(await _customerService.GetObligation(request.ObligationId, cancellationToken));

        var model = new DomainServices.SalesArrangementService.Contracts.Obligation
        {
            ObligationId = request.ObligationId,
            CustomerOnSAId = obligationInstance.CustomerOnSAId,
            ObligationState = request.ObligationState,
            CreditCardLimit = request.CreditCardLimit,
            InstallmentAmount = request.InstallmentAmount,
            ObligationTypeId = request.ObligationTypeId,
            LoanPrincipalAmount = request.LoanPrincipalAmount
        };
        if (request.Creditor is not null)
            model.Creditor = new DomainServices.SalesArrangementService.Contracts.ObligationCreditor
            {
                CreditorId = request.Creditor.CreditorId ?? "",
                IsExternal = request.Creditor.IsExternal,
                Name = request.Creditor.Name ?? ""
            };
        if (request.Correction is not null)
            model.Correction = new DomainServices.SalesArrangementService.Contracts.ObligationCorrection
            {
                CorrectionTypeId = request.Correction.CorrectionTypeId,
                CreditCardLimitCorrection = request.Correction.CreditCardLimitCorrection,
                InstallmentAmountCorrection = request.Correction.InstallmentAmountCorrection,
                LoanPrincipalAmountCorrection = request.Correction.LoanPrincipalAmountCorrection
            };

        ServiceCallResult.Resolve(await _customerService.UpdateObligation(model, cancellationToken));
    }

    private readonly ICustomerOnSAServiceAbstraction _customerService;
    
    public UpdateObligationHandler(ICustomerOnSAServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}

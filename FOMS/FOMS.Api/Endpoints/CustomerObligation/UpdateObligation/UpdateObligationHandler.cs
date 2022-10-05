using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace FOMS.Api.Endpoints.CustomerObligation.UpdateObligation;

internal class UpdateObligationHandler
    : AsyncRequestHandler<UpdateObligationRequest>
{
    protected override async Task Handle(UpdateObligationRequest request, CancellationToken cancellationToken)
    {
        var obligationInstance = ServiceCallResult.ResolveAndThrowIfError<_HO.Obligation>(await _customerService.GetObligation(request.ObligationId, cancellationToken));

        var model = new _HO.Obligation
        {
            ObligationId = request.ObligationId,
            CustomerOnSAId = obligationInstance.CustomerOnSAId,
            ObligationState = request.ObligationState,
            CreditCardLimit = request.CreditCardLimit,
            InstallmentAmount = request.InstallmentAmount,
            ObligationTypeId = request.ObligationTypeId,
            LoanPrincipalAmount = request.LoanPrincipalAmount,
            LoanPrincipalAmountConsolidated = request.LoanPrincipalAmountConsolidated,
        };
        if (request.Creditor is not null)
            model.Creditor = new _HO.ObligationCreditor
            {
                CreditorId = request.Creditor.CreditorId ?? "",
                IsExternal = request.Creditor.IsExternal,
                Name = request.Creditor.Name ?? ""
            };
        if (request.Correction is not null)
            model.Correction = new _HO.ObligationCorrection
            {
                CorrectionTypeId = request.Correction.CorrectionTypeId,
                CreditCardLimitCorrection = request.Correction.CreditCardLimitCorrection,
                InstallmentAmountCorrection = request.Correction.InstallmentAmountCorrection,
                LoanPrincipalAmountCorrection = request.Correction.LoanPrincipalAmountCorrection
            };

        ServiceCallResult.Resolve(await _customerService.UpdateObligation(model, cancellationToken));
    }

    private readonly ICustomerOnSAServiceClient _customerService;
    
    public UpdateObligationHandler(ICustomerOnSAServiceClient customerService)
    {
        _customerService = customerService;
    }
}

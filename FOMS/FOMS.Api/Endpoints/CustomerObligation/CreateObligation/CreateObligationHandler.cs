using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerObligation.CreateObligation;

internal sealed class CreateObligationHandler
    : IRequestHandler<CreateObligationRequest, int>
{
    public async Task<int> Handle(CreateObligationRequest request, CancellationToken cancellationToken)
    {
        var model = new DomainServices.SalesArrangementService.Contracts.CreateObligationRequest
        {
            CustomerOnSAId = request.CustomerOnSAId!.Value,
            ObligationTypeId = request.ObligationTypeId!.Value,
            ObligationState = request.ObligationState,
            InstallmentAmount = request.InstallmentAmount,
            LoanPrincipalAmount = request.LoanPrincipalAmount,
            LoanPrincipalAmountConsolidated = request.LoanPrincipalAmountConsolidated,
            CreditCardLimit = request.CreditCardLimit
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

        int obligationId = ServiceCallResult.ResolveAndThrowIfError<int>(await _customerService.CreateObligation(model, cancellationToken));
        return obligationId;
    }

    private readonly ICustomerOnSAServiceAbstraction _customerService;

    public CreateObligationHandler(ICustomerOnSAServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}

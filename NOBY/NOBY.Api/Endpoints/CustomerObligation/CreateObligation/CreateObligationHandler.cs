using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerObligation.CreateObligation;

internal sealed class CreateObligationHandler
    : IRequestHandler<CreateObligationRequest, int>
{
    public async Task<int> Handle(CreateObligationRequest request, CancellationToken cancellationToken)
    {
        var model = new _HO.CreateObligationRequest
        {
            CustomerOnSAId = request.CustomerOnSAId!.Value,
            ObligationTypeId = request.ObligationTypeId!.Value,
            ObligationState = request.ObligationState,
            InstallmentAmount = request.InstallmentAmount,
            LoanPrincipalAmount = request.LoanPrincipalAmount,
            AmountConsolidated = request.AmountConsolidated,
            CreditCardLimit = request.CreditCardLimit
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

        int obligationId = ServiceCallResult.ResolveAndThrowIfError<int>(await _customerService.CreateObligation(model, cancellationToken));
        return obligationId;
    }

    private readonly ICustomerOnSAServiceClient _customerService;

    public CreateObligationHandler(ICustomerOnSAServiceClient customerService)
    {
        _customerService = customerService;
    }
}

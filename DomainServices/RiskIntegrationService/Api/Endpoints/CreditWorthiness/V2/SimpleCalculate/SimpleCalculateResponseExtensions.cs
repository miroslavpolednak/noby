using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.SimpleCalculate;

internal static class SimpleCalculateResponseExtensions
{
    public static _V2.CreditWorthinessSimpleCalculateResponse ToServiceResponse(this _C4M.CreditWorthinessCalculationResponse response, int loanPaymentAmount)
        => new()
        {
            InstallmentLimit = (long)(response.InstallmentLimit?.Value ?? 0),
            MaxAmount = (long)(response.MaxAmount?.Value ?? 0),
            RemainsLivingAnnuity = (long?)response.RemainingAnnuityLivingAmount?.Value,
            RemainsLivingInst = (long?)response.RemainingAnnuityLivingMaxInstallmentAmount?.Value,
            ResultReason = response.ResultReason is null ? null : new Contracts.Shared.ResultReasonDetail
            {
                Code = response.ResultReason.Code,
                Description = response.ResultReason.Description
            },
            WorthinessResult = (response.InstallmentLimit?.Value ?? 0) > loanPaymentAmount
                ? _V2.CreditWorthinessResults.Success
                : _V2.CreditWorthinessResults.Failed
        };
}

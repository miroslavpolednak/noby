using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate;

internal static class CalculateResponseExtensions
{
    public static _V2.CreditWorthinessCalculateResponse ToServiceResponse(this _C4M.CreditWorthinessCalculation response, decimal? dti, decimal? dsti, int loanPaymentAmount)
        => new()
        {
            InstallmentLimit = response.InstallmentLimit,
            MaxAmount = response.MaxAmount,
            RemainsLivingAnnuity = response.RemainsLivingAnnuity,
            RemainsLivingInst = response.RemainsLivingInst,
            Dti = dti,
            Dsti = dsti,
            ResultReason = response.ResultReason is null ? null : new Contracts.Shared.ResultReasonDetail
            {
                Code = response.ResultReason.Code,
                Description = response.ResultReason.Description
            },
            WorthinessResult = response.InstallmentLimit > loanPaymentAmount
                ? _V2.CreditWorthinessResults.Success 
                : _V2.CreditWorthinessResults.Failed
        };
}

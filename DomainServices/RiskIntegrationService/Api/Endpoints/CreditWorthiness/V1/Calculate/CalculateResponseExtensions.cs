using _V1 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V1;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V1.Calculate;

internal static class CalculateResponseExtensions
{
    public static _V1.CreditWorthinessCalculateResponse ToServiceResponse(this _C4M.CreditWorthinessCalculation response)
        => new()
        {
            InstallmentLimit = response.InstallmentLimit,
            MaxAmount = response.MaxAmount,
            RemainsLivingAnnuity = response.RemainsLivingAnnuity,
            RemainsLivingInst = response.RemainsLivingInst,
            ResultReason = response.ResultReason is null ? null : new _V1.CreditWorthinessResultReason
            {
                Code = response.ResultReason.Code,
                Description = response.ResultReason.Description
            },
            WorthinessResult = response.InstallmentLimit > response.RemainsLivingAnnuity 
                ? _V1.CreditWorthinessResults.Success 
                : _V1.CreditWorthinessResults.Failed
        };
}

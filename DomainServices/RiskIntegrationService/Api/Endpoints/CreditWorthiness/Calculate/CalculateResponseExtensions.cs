using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.Calculate;

internal static class CalculateResponseExtensions
{
    public static Contracts.CreditWorthiness.CalculateResponse ToServiceResponse(this _C4M.CreditWorthinessCalculation response)
        => new()
        {
            InstallmentLimit = response.InstallmentLimit,
            MaxAmount = response.MaxAmount,
            RemainsLivingAnnuity = response.RemainsLivingAnnuity,
            RemainsLivingInst = response.RemainsLivingInst,
            ResultReason = response.ResultReason is null ? null : new Contracts.CreditWorthiness.ResultReason
            {
                Code = response.ResultReason.Code,
                Description = response.ResultReason.Description
            },
            WorthinessResult = response.InstallmentLimit > response.RemainsLivingAnnuity 
                ? Contracts.CreditWorthiness.CreditWorthinessResults.Success 
                : Contracts.CreditWorthiness.CreditWorthinessResults.Failed
        };
}

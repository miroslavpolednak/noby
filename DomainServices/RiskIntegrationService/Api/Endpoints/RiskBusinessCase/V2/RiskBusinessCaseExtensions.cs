using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;
using DomainServices.RiskIntegrationService.Contracts.Shared;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2;

internal static class RiskBusinessCaseExtensions
{
    public static AmountDetail? ToAmountDetail(this _C4M.Amount amount)
        => amount.Value != null ? new()
        {
            Amount = amount.Value.Value,
            CurrencyCode = amount.CurrencyCode
        } : null;
}

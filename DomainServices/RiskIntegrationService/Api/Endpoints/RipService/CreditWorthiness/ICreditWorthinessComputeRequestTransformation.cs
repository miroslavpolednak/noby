namespace DomainServices.RiskIntegrationService.Api.Endpoints.RipService.CreditWorthiness;

public interface ICreditWorthinessComputeRequestTransformation
{
    Task<Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness.CreditWorthinessCalculationArguments> Transform(Contracts.CreditWorthinessRequest ripRequest);
}

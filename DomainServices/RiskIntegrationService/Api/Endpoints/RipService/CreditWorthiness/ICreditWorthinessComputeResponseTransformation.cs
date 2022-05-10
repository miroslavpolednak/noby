namespace DomainServices.RiskIntegrationService.Api.Endpoints.RipService.CreditWorthiness;

public interface ICreditWorthinessComputeResponseTransformation
{
    Contracts.CreditWorthinessResponse Transform(Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness.CreditWorthinessCalculation c4mResult, Contracts.CreditWorthinessRequest ripRequest);
}
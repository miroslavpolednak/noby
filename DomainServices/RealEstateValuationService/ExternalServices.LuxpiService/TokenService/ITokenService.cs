using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.TokenService;

internal interface ITokenService
    : IExternalServiceClient
{
    Task<string> GetToken(string apiKey, CancellationToken cancellationToken);
}

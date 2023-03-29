using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.Kyc.V1;

public interface IKycClient
    : IExternalServiceClient
{
    Task SetSocialCharacteristics(long customerId, Contracts.SocialCharacteristics request, CancellationToken cancellationToken = default(CancellationToken));

    Task SetKyc(long customerId, Contracts.Kyc request, CancellationToken cancellationToken = default(CancellationToken));

    Task SetFinancialProfile(long customerId, Contracts.EmploymentFinancialProfile request, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}
